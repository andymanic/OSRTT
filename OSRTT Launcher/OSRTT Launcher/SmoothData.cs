using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRTT_Launcher
{
    static class SmoothData
    {
        // Existing function (moving average)
        public static int[] smoothData(int[] samples, int period)
        {
            int[] buffer = new int[period];
            int[] averagedSamples = new int[samples.Length];
            int current_index = 0;
            for (int a = 0; a < samples.Length; a++)
            {
                buffer[current_index] = samples[a] / period;
                int movAvg = 0;
                for (int b = 0; b < period; b++)
                {
                    movAvg += buffer[b];
                }
                averagedSamples[a] = movAvg;
                current_index = (current_index + 1) % period;
            }
            return averagedSamples.Skip(period).ToArray();
        }


        // Existing backlight flicker culling function
        public class CulledSamples
        {
            public int[] sampleArray { get; set; }
            public int NoiseLevel { get; set; } = 0;
        }

        public static CulledSamples CullBacklightFlicker(int[] samplesIn)
        {
            int[] samples = new int[samplesIn.Length];
            Array.Copy(samplesIn, samples, samplesIn.Length);
            // Search for flicker drops
            List<int> flickerLocations = new List<int>();
            int lookBehind = 3;
            int lookAhead = 100;
            for (int i = lookBehind; i < samples.Length; i++) // this doesn't currently account for a flicker at the START.
            {
                // check if current sample has dropped dramatically
                if (samples[i] < (samples[i - lookBehind] * 0.5))
                {
                    List<int> potentialLocations = new List<int>();
                    potentialLocations.AddRange(new List<int> { i - 2, i - 1, i });
                    for (int k = i + 1; k < i + lookAhead; k++)
                    {
                        if (k >= samples.Length) { break; }
                        if (samples[k] < samples[i] || samples[k] < (samples[i] * 1.1) || samples[k] < (samples[i - lookBehind] * 0.5))
                        {
                            potentialLocations.Add(k);
                        }
                        else if (samples[k] > (samples[i - lookBehind] * 0.9))
                        {
                            // samples DO go back up, stop searching, add drops to list, skip ahead
                            flickerLocations.AddRange(potentialLocations);
                            // add another 5 samples to be removed to ensure endpoint accuracy
                            int lastLocation = potentialLocations.Last();
                            flickerLocations.AddRange(new List<int> { lastLocation + 1, lastLocation + 2, lastLocation + 3, lastLocation + 4, lastLocation + 5 });
                            i = k + 5;
                            break;
                        }
                    }
                }
            }
            Console.WriteLine(flickerLocations.Count);
            Console.WriteLine();

            if (flickerLocations.Count != 0 && Properties.Settings.Default.flickerCulling)
            {
                if (flickerLocations.Last() >= samples.Length - 1)
                {
                    for (int m = flickerLocations.Count - 1; m > 0; m--)
                    {
                        if (flickerLocations[m] >= samples.Length - 1)
                        {
                            flickerLocations.RemoveAt(m);
                        }
                        else { break; }
                    }
                }
                for (int p = 0; p < flickerLocations.Count; p++)
                {
                    // Find current continuous group to replace
                    int startLocation = flickerLocations[p];
                    int endLocation = flickerLocations[p];
                    for (int m = p + 1; m < flickerLocations.Count; m++)
                    {
                        if ((flickerLocations[m] - 1) == endLocation)
                        {
                            endLocation = flickerLocations[m];
                        }
                        else
                        {
                            p = m;
                            break;
                        }
                    }

                    // Replace the group
                    // Optional upgrade - include more results then only take the middle ones
                    double[] rawX = new double[2];
                    double[] rawY = new double[2];
                    rawX[0] = 0;
                    rawX[1] = 1;
                    rawY[0] = samples[startLocation - 1];
                    rawY[1] = samples[endLocation + 1];
                    var line = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(rawX, rawY, endLocation - startLocation);
                    for (int q = 0; q < line.ys.Length - 1; q++)
                    {
                        samples[startLocation + q] = (int)line.ys[q + 1];
                    }
                }
                int max = 0;
                int min = 65536;
                for (int i = 20; i < 300; i++)
                {
                    if (samples[i] < min) { min = samples[i]; }
                    else if (samples[i] > max) { max = samples[i]; }
                }

                return new CulledSamples { sampleArray = samples, NoiseLevel = max - min };
            }
            else { return new CulledSamples { sampleArray = samples, NoiseLevel = 0 }; }
        }

        //Hampel Filter for outlier and spike removal
        public static int[] ApplyHampelFilter(int[] input, int windowRadius = 3, double nSigmas = 3.0)
        {
            int[] output = (int[])input.Clone();
            double scaleFactor = 1.4826; // Scale factor for normal distribution

            for (int i = windowRadius; i < input.Length - windowRadius; i++)
            {
                int windowSize = 2 * windowRadius + 1;
                int[] window = new int[windowSize];
                Array.Copy(input, i - windowRadius, window, 0, windowSize);

                // Calculate Median
                int[] sorted = (int[])window.Clone();
                Array.Sort(sorted);
                int median = sorted[windowRadius];

                // Calculate Median Absolute Deviation (MAD)
                int[] deviations = new int[windowSize];
                for (int j = 0; j < windowSize; j++)
                {
                    deviations[j] = Math.Abs(window[j] - median);
                }
                Array.Sort(deviations);
                double mad = scaleFactor * deviations[windowRadius];

                // Replace outlier with median if deviation exceeds threshold
                if (Math.Abs(input[i] - median) > nSigmas * mad)
                {
                    output[i] = median;
                }
            }

            return output;
        }

        // Median Filtering for edge-preserving smoothing
        public static int[] ApplyMedianFilter(int[] input, int windowSize = 5)
        {
            int radius = windowSize / 2;
            int[] output = new int[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                int start = Math.Max(0, i - radius);
                int end = Math.Min(input.Length - 1, i + radius);
                int count = end - start + 1;

                int[] temp = new int[count];
                Array.Copy(input, start, temp, 0, count);
                Array.Sort(temp);

                output[i] = temp[count / 2];
            }

            return output;
        }

        //zero phase forward backwards filtering to eliminate time delay
        public static int[] ApplyZeroPhaseMovingAverage(int[] input, int windowSize = 5)
        {
            // Step 1: Forward Pass
            int[] forward = ApplyMovingAverage(input, windowSize);

            // Step 2: Reverse Array
            Array.Reverse(forward);

            // Step 3: Backward Pass
            int[] backward = ApplyMovingAverage(forward, windowSize);

            // Step 4: Reverse back to original order
            Array.Reverse(backward);

            return backward;
        }

        private static int[] ApplyMovingAverage(int[] input, int windowSize)
        {
            int[] output = new int[input.Length];
            int radius = windowSize / 2;

            for (int i = 0; i < input.Length; i++)
            {
                int start = Math.Max(0, i - radius);
                int end = Math.Min(input.Length - 1, i + radius);
                int count = end - start + 1;

                long currentSum = 0;
                for (int j = start; j <= end; j++)
                {
                    currentSum += input[j];
                }
                output[i] = (int)Math.Round((double)currentSum / count);
            }

            return output;
        }


        // 1D bilateral filter
        public static int[] ApplyBilateralFilter1D(int[] input, int windowRadius = 3, double spatialSigma = 2.0, double rangeSigma = 10.0)
        {
            int[] output = new int[input.Length];
            double twoSpatialSigmaSq = 2 * spatialSigma * spatialSigma;
            double twoRangeSigmaSq = 2 * rangeSigma * rangeSigma;

            for (int i = 0; i < input.Length; i++)
            {
                double normFactor = 0;
                double filteredValue = 0;

                int start = Math.Max(0, i - windowRadius);
                int end = Math.Min(input.Length - 1, i + windowRadius);

                for (int j = start; j <= end; j++)
                {
                    double spatialDiff = i - j;
                    double rangeDiff = input[i] - input[j];

                    double spatialWeight = Math.Exp(-(spatialDiff * spatialDiff) / twoSpatialSigmaSq);
                    double rangeWeight = Math.Exp(-(rangeDiff * rangeDiff) / twoRangeSigmaSq);

                    double weight = spatialWeight * rangeWeight;

                    filteredValue += input[j] * weight;
                    normFactor += weight;
                }

                output[i] = (int)Math.Round(filteredValue / normFactor);
            }

            return output;
        }




    }
}
