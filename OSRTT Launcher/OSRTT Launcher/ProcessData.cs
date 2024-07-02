using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRTT_Launcher
{
    public class ProcessData
    {
        // scratch this shit. 
        // move all the processing code currently in resultsview to here and make resultsview work with that
        // then copy the business logic of averaging the data to a function here
        // then migrate main.cs processing & averaging functionality to here with just the primary logic. Handle saving etc in-file for now.
        public class rtMethods
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Tolerance { get; set; }
            public bool gammaCorrected { get; set; }
            public bool percentage { get; set; }
        }
        public class osMethods
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public bool endPercent { get; set; }
            public bool rangePercent { get; set; }
            public bool gammaCorrected { get; set; }
        }
        public class resultSelection
        {
            public int arrayIndex { get; set; }
            public int resultIndex { get; set; }
            public rtMethods rtStyle { get; set; }
            public osMethods osStyle { get; set; }
        }
        public class graphResult
        {
            public double Time { get; set; }
            public int startIndex { get; set; }
            public int endIndex { get; set; }
            public double Overshoot { get; set; }
            public int offset { get; set; }
        }
        public class processedResult
        {
            public int StartingRGB { get; set; }
            public int EndRGB { get; set; }
            public double SampleTime { get; set; }
            public double initTime { get; set; }
            public int initStartIndex { get; set; }
            public int initEndIndex { get; set; }
            public double perTime { get; set; }
            public int perStartIndex { get; set; }
            public int perEndIndex { get; set; }
            public double compTime { get; set; }
            public int compStartIndex { get; set; }
            public int compEndIndex { get; set; }
            public double Overshoot { get; set; }
            public double overshootRGB { get; set; }
            public double visualResponseRating { get; set; }
            public double inputLag { get; set; }
            public int offset { get; set; }
        }
        public class rawResultData
        {
            public int StartingRGB { get; set; }
            public int EndRGB { get; set; }
            public int TimeTaken { get; set; }
            public int SampleCount { get; set; }
            public double SampleTime { get; set; }
            public List<int> Samples { get; set; }
            public int noiseLevel { get; set; }
        }
        public class gammaResult
        {
            public int RGB { get; set; }
            public int LightLevel { get; set; }
        }
        public class runSettings
        {
            public string RunName { get; set; }
            public string DateAndTime { get; set; }
            public string MonitorName { get; set; }
            public string EDIDProductcode { get; set; }
            public int RefreshRate { get; set; }
            public string Resolution { get; set; }
            public int FPSLimit { get; set; }
            public bool Vsync { get; set; }
            public string OverdriveMode { get; set; } = "";
            public int Mode { get; set; }
            public rtMethods rtMethod { get; set; }
            public osMethods osMethod { get; set; }
            public string ExtraInfo { get; set; }
            public int MovingAverageSize { get; set; }
        }

        public class normalisedGamma
        {
            public double gammaTitle { get; set; }
            public List<double> normalisedData { get; set; }
        }

        public static List<normalisedGamma> NormalGamma = new List<normalisedGamma>
        {
            new normalisedGamma
            {
                gammaTitle = 1.6,
                normalisedData = new List<double> {0,0.013,0.040,0.076,0.121,0.172,0.231,0.295,0.366,0.442,0.523,0.609,0.700,0.795,0.895,1,1.109,1.222,1.339}
            },
            new normalisedGamma
            {
                gammaTitle = 1.8,
                normalisedData = new List<double> {0,0.008,0.027,0.055,0.093,0.138,0.192,0.254,0.323,0.399,0.482,0.572,0.669,0.773,0.883,1,1.123,1.253,1.388}
            },
            new normalisedGamma
            {
                gammaTitle = 2.0,
                normalisedData = new List<double> {0,0.004,0.018,0.040,0.071,0.111,0.160,0.218,0.284,0.360,0.444,0.538,0.640,0.751,0.871,1,1.138,1.284,1.440}
            },
            new normalisedGamma
            {
                gammaTitle = 2.2,
                normalisedData = new List<double> {0,0.003,0.012,0.029,0.055,0.089,0.133,0.187,0.251,0.325,0.410,0.505,0.612,0.730,0.859,1,1.153,1.317,1.493}
            },
            new normalisedGamma
            {
                gammaTitle = 2.4,
                normalisedData = new List<double> {0,0.002,0.008,0.021,0.042,0.072,0.111,0.161,0.221,0.293,0.378,0.475,0.585,0.709,0.847,1,1.168,1.350,1.549}
            },
            new normalisedGamma
            {
                gammaTitle = 2.6,
                normalisedData = new List<double> {0,0.001,0.005,0.015,0.032,0.057,0.092,0.138,0.195,0.265,0.348,0.446,0.560,0.689,0.836,1,1.183,1.385,1.606}
            }
        };
        public class VRRKey
        {
            public string Name { get; set; }
            public int Type { get; set; }
            public double Best { get; set; }
            public double Middle { get; set; }
            public double Worst { get; set; }
        }

        public List<VRRKey> VRRTable = new List<VRRKey>
        {
            new VRRKey { Name = "IRT", Type=0, Best=2, Middle=14, Worst=28 },
            new VRRKey { Name = "PRT", Type=1, Best=2, Middle=8, Worst=16 },
            new VRRKey { Name = "OS", Type=2, Best=5, Middle=15, Worst=25 }
        };

        public graphResult processGraphResult(List<List<rawResultData>> data, resultSelection res, int startDelay, List<gammaResult> processedGamma, string rtType, runSettings runSetting)
        {
            try
            {
                processedResult proc = ProcessResponseTimeData(data, res, startDelay, processedGamma, runSetting);
                graphResult gProc = new graphResult();
                if (rtType == "complete")
                {
                    gProc.Time = proc.compTime;
                    gProc.startIndex = proc.compStartIndex;
                    gProc.endIndex = proc.compEndIndex;
                }    
                else if (rtType == "perceived")
                {
                    gProc.Time = proc.perTime;
                    gProc.startIndex = proc.perStartIndex;
                    gProc.endIndex = proc.perEndIndex;
                }
                else if (rtType == "initial")
                {
                    gProc.Time = proc.initTime;
                    gProc.startIndex = proc.initStartIndex;
                    gProc.endIndex = proc.initEndIndex;
                }
                gProc.Overshoot = proc.Overshoot;
                gProc.offset = proc.offset;
                return gProc;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return null;
            }
        }

        public List<gammaResult> processGammaTable(List<int[]> gamma, List<rawResultData> data)
        {
            if (gamma.Count != 0)
            {
                double[] rgbVals = new double[gamma.Count + 3];
                double[] lightLevelVals = new double[gamma.Count + 3];
                for (int i = 0; i < gamma.Count; i++)
                { 
                    int[] dataLine = gamma[i].Skip(300).ToArray();
                    int lineAverage = 0;
                    for (int j = 0; j < (dataLine.Length - 100); j++)
                    {
                        lineAverage += dataLine[j];
                    }
                    foreach (var result in data)
                    {
                        if (result.StartingRGB == gamma[i][0])
                        {
                            result.noiseLevel = (dataLine.Max() - dataLine.Min());
                        }
                    }
                    lineAverage /= (dataLine.Length - 100);
                    rgbVals[i] = gamma[i][0];
                    lightLevelVals[i] = lineAverage;
                }
                // Extrapolate upwards to catch overshoot above RGB 255
                double[] normalRGB = new double[rgbVals.Length];
                double[] normalLight = new double[lightLevelVals.Length];
                double peakLight = lightLevelVals[gamma.Count - 1];
                for (int i = 0; i < normalLight.Length - 3; i++)
                {
                    normalRGB[i] = 17 * i;
                    normalRGB[i] /= 255;
                    normalLight[i] = lightLevelVals[i];
                    normalLight[i] = normalLight[i] / peakLight;
                }
                List<double> closestMatchGamma = NormalGamma[0].normalisedData;
                double gammaDifference = 100;
                foreach (var g in NormalGamma)
                {
                    double diff = 0;
                    for (int a = 0; a < normalLight.Length; a++)
                    {
                        double big = normalLight[a];
                        double little = g.normalisedData[a];
                        if (big < little)
                        {
                            little = normalLight[a];
                            big = g.normalisedData[a];
                        }
                        diff += big - little;
                    }
                    if (diff < gammaDifference)
                    {
                        gammaDifference = diff;
                        closestMatchGamma = g.normalisedData;
                    }
                }
                for (int k = gamma.Count; k < normalLight.Length; k++)
                {
                    double val = closestMatchGamma[k] * peakLight;
                    double rgb = 17 * k;
                    rgbVals[k] = rgb;
                    lightLevelVals[k] = Math.Round(val,0);
                }
                Console.WriteLine();



                int pointsBetween = 51;
                if (gamma.Count == 16)
                {
                    pointsBetween = 17;
                }
                var interpPoints = new ScottPlot.Statistics.Interpolation.NaturalSpline(rgbVals, lightLevelVals, pointsBetween);
                List<int> x = new List<int>();
                List<int> y = new List<int>();
                foreach (var p in interpPoints.interpolatedXs)
                {
                    x.Add(Convert.ToInt32(p));
                }
                foreach (var p in interpPoints.interpolatedYs)
                {
                    y.Add(Convert.ToInt32(p));
                }
                List<gammaResult> xy = new List<gammaResult>();
                for (int k = 0; k < x.Count; k++)
                {
                    xy.Add(new gammaResult { RGB = x[k], LightLevel = y[k] });
                }
                return xy;    
            }
            else
            {
                return null;
            }
        }

        PointF[] InterpolatePoints(PointF[] original, int numberRequired)
        {
            // The new array, ready to return.
            PointF[] interpolated = new PointF[numberRequired];

            // The number of interpolated points in between each pair of existing points.
            int between = ((numberRequired - original.Length) / (original.Length - 1)) + 1;

            // Loop through the original list.
            int index = 0;
            for (int i = 0; i < original.Length - 1; i++)
            {
                // Add each original point to the interpolated points.
                interpolated[index++] = original[i];

                // The step distances in x and y directions between this original point and the next one.
                float stepX = (original[i + 1].X - original[i].X) / ((float)between + 1);
                float stepY = (original[i + 1].Y - original[i].Y) / ((float)between + 1);

                // Add the interpolated points at the given steps.
                for (int j = 0; j < between; j++)
                {
                    float x = original[i].X + stepX * (float)(j + 1);
                    float y = original[i].Y + stepY * (float)(j + 1);

                    if (index < numberRequired)
                    {
                        interpolated[index++] = new PointF(x, y);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return interpolated;
        }

        public int processTestLatency(List<int> testLatency)
        {
            int startDelay = 150;
            if (testLatency.Count != 0)
            {
                int[] tl = testLatency.Skip(5).ToArray();
                for (int n = 0; n < tl.Length; n++)
                {
                    if (tl[n] > 8000)
                    {
                        if (n <= 150 && n > 30)
                        {
                            startDelay = n - 30;
                        }
                        else if (n < 30)
                        {
                            n /= 2;
                            startDelay = n;
                        }
                        else if (n > 400)
                        {
                            startDelay = 250;
                        }
                        break;
                    }
                }
            }
            return startDelay;
        }

        public int[] smoothData(int[] samples, int period)
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

        public processedResult ProcessResponseTimeData(List<List<rawResultData>> data, resultSelection res, int startDelay, List<gammaResult> processedGamma, runSettings runSetting)
        {
            //This is a long one. This is the code that builds the gamma curve, finds the start/end points and calculates response times and overshoot % (gamma corrected)
            List<double[]> processedData = new List<double[]>();

            // First, create gamma array from the data
            List<int[]> localGamma = new List<int[]>();
            List<int[]> fullGammaTable = new List<int[]>();
            List<int[]> smoothedDataTable = new List<int[]>();
            

            try //Wrapped whole thing in try just in case
            {
                // Save start, end, time and sample count then clear the values from the array
                int StartingRGB = data[res.arrayIndex][res.resultIndex].StartingRGB;
                int EndRGB = data[res.arrayIndex][res.resultIndex].EndRGB;
                int TimeTaken = data[res.arrayIndex][res.resultIndex].TimeTaken;
                int SampleCount = data[res.arrayIndex][res.resultIndex].SampleCount;
                int[] samples = data[res.arrayIndex][res.resultIndex].Samples.ToArray();

                double SampleTime = ((double)TimeTaken / (double)SampleCount); // Get the time taken between samples

                // Clean up noisy data using moving average function
                int period = 10;
                int noise = data[res.arrayIndex][res.resultIndex].noiseLevel;
                if (noise < 250)
                {
                    period = 20;
                }
                else if (noise < 500)
                {
                    period = 30;
                }
                else if (noise < 750)
                {
                    period = 40;
                }
                else
                {
                    period = 50;
                }
                if (Properties.Settings.Default.movingAverageSize > period)
                {
                    period = Properties.Settings.Default.movingAverageSize;
                }
                Console.WriteLine("Moving Average Window Size: " + period);
                samples = smoothData(samples, period); //Moving average spoils the first 10 samples so currently removing them.

                List<int> fullSmoothedLine = new List<int> { StartingRGB, EndRGB, TimeTaken, SampleCount };
                fullSmoothedLine.AddRange(samples);
                smoothedDataTable.Add(fullSmoothedLine.ToArray());

                int maxValue = samples.Max(); // Find the maximum value for overshoot
                int minValue = samples.Min(); // Find the minimum value for undershoot
                                              // Initialise in-use variables
                int transStart = 0;
                int transEnd = 0;
                int initialTransStart = 0;
                int initialTransEnd = 0;
                int perceivedTransStart = 0;
                int perceivedTransEnd = 0;

                double overUnderRGB = 0.0;

                int startMax = samples[5]; // Initialise these variables with a real value 
                int startMin = samples[5]; // Initialise these variables with a real value 
                int endMax = samples[samples.Length - 10]; // Initialise these variables with a real value 
                int endMin = samples[samples.Length - 10]; // Initialise these variables with a real value 

                // Build start min/max to compare against
                for (int l = 0; l < startDelay; l++) //CHANGE TO 180 FOR RUN 2 DATA
                {
                    if (samples[l] < startMin)
                    {
                        startMin = samples[l];
                    }
                    else if (samples[l] > startMax)
                    {
                        startMax = samples[l];
                    }
                }

                // Build end min/max to compare against
                for (int m = samples.Length - 5; m > samples.Length - 300; m--)
                {
                    if (samples[m] < endMin)
                    {
                        endMin = samples[m];
                    }
                    else if (samples[m] > endMax)
                    {
                        endMax = samples[m];
                    }
                }

                // Search for where the result starts transitioning - start is almost always less sensitive
                for (int j = 0; j < samples.Length; j++)
                {
                    if (StartingRGB < EndRGB)
                    {
                        if (samples[j] > (startMax))
                        {
                            if (StartingRGB == 0 && EndRGB == 26)
                            {
                                if ((samples[j + 50] > (samples[j] + 25) || samples[j + 56] > (samples[j] + 25))
                                    && (samples[j + 100] > (samples[j] + 50) || samples[j + 106] > (samples[j] + 50))
                                    && (samples[j + 125] > (samples[j] + 75) || samples[j + 131] > (samples[j] + 75))
                                    && (samples[j + 150] > (samples[j] + 100) || samples[j + 156] > (samples[j] + 100))) // check the trigger point is actually the trigger and not noise
                                {
                                    transStart = j;
                                    break;
                                }
                                else
                                {
                                    if (samples[j] > startMax)
                                    {
                                        startMax = samples[j];
                                    }
                                }
                            }
                            else
                            {
                                if ((samples[j + 50] > (samples[j] + 50) || samples[j + 56] > (samples[j] + 50))
                                    && (samples[j + 100] > (samples[j] + 100) || samples[j + 106] > (samples[j] + 100))
                                    && (samples[j + 125] > (samples[j] + 100) || samples[j + 131] > (samples[j] + 100))
                                    && (samples[j + 150] > (samples[j] + 100) || samples[j + 156] > (samples[j] + 100))) // check the trigger point is actually the trigger and not noise
                                {
                                    transStart = j;
                                    break;
                                }
                                else
                                {
                                    if (samples[j] > startMax)
                                    {
                                        startMax = samples[j];
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (samples[j] < (startMin))
                        {
                            if (StartingRGB == 26 && EndRGB == 0)
                            {
                                if ((samples[j + 50] < (samples[j] - 25) || samples[j + 56] < (samples[j] - 25))
                                && (samples[j + 100] < (samples[j] - 50) || samples[j + 106] < (samples[j] - 50))
                                && (samples[j + 125] < (samples[j] - 75) || samples[j + 131] < (samples[j] - 75))
                                && (samples[j + 150] < (samples[j] - 100) || samples[j + 156] < (samples[j] - 100))) // check the trigger point is actually the trigger and not noise
                                {
                                    transStart = j;
                                    break;
                                }
                                else
                                {
                                    if (samples[j] < startMin)
                                    {
                                        startMin = samples[j];
                                    }
                                }
                            }
                            else
                            {
                                if ((samples[j + 50] < (samples[j] - 50) || samples[j + 56] < (samples[j] - 50))
                                    && (samples[j + 100] < (samples[j] - 100) || samples[j + 106] < (samples[j] - 100))
                                    && (samples[j + 125] < (samples[j] - 100) || samples[j + 131] < (samples[j] - 100))
                                    && (samples[j + 150] < (samples[j] - 100) || samples[j + 156] < (samples[j] - 100))) // check the trigger point is actually the trigger and not noise
                                {
                                    transStart = j;
                                    break;
                                }
                                else
                                {
                                    if (samples[j] < startMin)
                                    {
                                        startMin = samples[j];
                                    }
                                }
                            }
                        }
                    }
                }

                // Search for where the result stops transitioning (from the end) - end position is almost always more sensitive hence lower values - also must account for over/undershoot
                for (int j = samples.Length - 1; j > 0; j--)
                {
                    if (StartingRGB < EndRGB)
                    {
                        if (maxValue > (endMax + 100)) //Check for overshoot
                        {
                            if (samples[j] > endMax)
                            {
                                if (samples[j - 100] > (samples[j] + 50) && samples[j - 125] > (samples[j] + 50)) // check the trigger point is actually the trigger and not noise
                                {
                                    transEnd = j;
                                    break;
                                }
                                else
                                {
                                    if (samples[j] > endMax)
                                    {
                                        endMax = samples[j];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (samples[j] <= (endMin + 20)) //Check for regular finish point
                            {
                                if (StartingRGB == 0 && EndRGB == 26)
                                {
                                    if ((samples[j - 100] < (samples[j] - 25) || samples[j - 106] < (samples[j] - 25))
                                    && (samples[j - 125] < (samples[j] - 50) || samples[j - 131] < (samples[j] - 50))
                                    && (samples[j - 150] < (samples[j] - 75) || samples[j - 156] < (samples[j] - 75))) // check the trigger point is actually the trigger and not noise
                                    {
                                        transEnd = j;
                                        break;
                                    }
                                    else
                                    {
                                        if (samples[j] < endMin)
                                        {
                                            endMin = samples[j];
                                        }
                                    }
                                }
                                else
                                {
                                    if ((samples[j - 100] < (samples[j] - 50) || samples[j - 106] < (samples[j] - 50))
                                    && (samples[j - 125] < (samples[j] - 75) || samples[j - 131] < (samples[j] - 75))
                                    && (samples[j - 150] < (samples[j] - 100) || samples[j - 156] < (samples[j] - 100))) // check the trigger point is actually the trigger and not noise
                                    {
                                        transEnd = j;
                                        break;
                                    }
                                    else
                                    {
                                        if (samples[j] < endMin)
                                        {
                                            endMin = samples[j];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (minValue < (endMin - 100)) //Check for undershoot
                        {
                            if (samples[j] < endMin) //Check for under-shot finish point
                            {
                                if (samples[j - 100] < (samples[j] - 50) && samples[j - 125] < (samples[j] - 50)) // check the trigger point is actually the trigger and not noise
                                {
                                    transEnd = j;
                                    break;
                                }
                                else
                                {
                                    if (samples[j] < endMin)
                                    {
                                        endMin = samples[j];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (samples[j] > endMax) //Check for regular finish point
                            {
                                if (StartingRGB == 26 && EndRGB == 0)
                                {
                                    if ((samples[j - 100] > (samples[j] + 25) || samples[j - 106] > (samples[j] + 25))
                                    && (samples[j - 125] > (samples[j] + 50) || samples[j - 131] > (samples[j] + 50))
                                    && (samples[j - 150] > (samples[j] + 75) || samples[j - 156] > (samples[j] + 75)))
                                    {
                                        transEnd = j;
                                        break;
                                    }
                                    else
                                    {
                                        if (samples[j] > endMax)
                                        {
                                            endMax = samples[j];
                                        }
                                    }
                                }
                                else
                                {
                                    if ((samples[j - 100] > (samples[j] + 50) || samples[j - 106] > (samples[j] + 50))
                                    && (samples[j - 125] > (samples[j] + 75) || samples[j - 131] > (samples[j] + 75))
                                    && (samples[j - 150] > (samples[j] + 100) || samples[j - 156] > (samples[j] + 100)))
                                    {
                                        transEnd = j;
                                        break;
                                    }
                                    else
                                    {
                                        if (samples[j] > endMax)
                                        {
                                            endMax = samples[j];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                double startAverage = 0;
                double endAverage = 0;
                int avgStart = transStart - 200;
                int avgEnd = transEnd + 400;
                if (transStart < 200)
                {
                    int t = transStart / 5;
                    avgStart = transStart - t;
                }
                if ((samples.Length - transEnd) < 400)
                {
                    int t = (samples.Length - transEnd) / 5;
                    avgEnd = transEnd + t;
                }
                for (int q = 0; q < avgStart; q++)
                {
                    startAverage += samples[q];
                }
                startAverage /= avgStart;
                startAverage = Math.Round(startAverage, 0);
                for (int q = avgEnd; q < samples.Length; q++)
                {
                    endAverage += samples[q];
                }
                endAverage /= (samples.Length - avgEnd);
                endAverage = Math.Round(endAverage, 0);
                int arrSize = (transEnd - transStart + 100);
                if (samples.Length < (transEnd + 100))
                {
                    arrSize = samples.Length - transStart;
                }
                if (arrSize < 110)
                {
                    arrSize = 200;
                }
                int[] transitionSamples = new int[arrSize];
                // Getting min/max from INSIDE the transition window
                if ((transEnd - transStart) != 0)
                {
                    Array.Copy(samples, transStart, transitionSamples, 0, arrSize);
                    maxValue = transitionSamples.Max();
                    minValue = transitionSamples.Min();
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Overshoot calculations
                double overshootPercent = 0;
                double overshootRGBDiff = 0;
                double peakValue = 0;
                if (StartingRGB < EndRGB)
                {
                    peakValue = maxValue;
                    // Dark to light transition
                    if (maxValue > (endAverage + 100) && maxValue > (processedGamma[EndRGB].LightLevel + 100))
                    {
                        // undershoot may have occurred
                        Console.WriteLine("Overshoot found");
                        // convert maxValue to RGB using gamma table
                        for (int i = 0; i < processedGamma.Count; i++)
                        {
                            // Find what RGB value matches or exceeds the peak light reading for this run
                            if (maxValue <= processedGamma[i].LightLevel)
                            {
                                // Check if peak light reading is closer to upper or lower bound value
                                int diff1 = processedGamma[i].LightLevel - maxValue;
                                int diff2 = maxValue - processedGamma[i - 1].LightLevel;
                                if (diff1 < diff2)
                                {
                                    overUnderRGB = processedGamma[i].RGB;
                                }
                                else
                                {
                                    overUnderRGB = processedGamma[i - 1].RGB;
                                }
                                break;
                            }
                            else if (maxValue > processedGamma.Last().LightLevel)
                            {
                                if (maxValue > 65500)
                                {
                                    overUnderRGB = 261;
                                    break;
                                }
                                else
                                {
                                    overUnderRGB = 256;
                                    break;
                                }
                            }
                        }
                        if (overUnderRGB == -1)
                        {
                            //overshootPercent = 100;
                        }
                        else
                        {
                            overshootRGBDiff = overUnderRGB - EndRGB;
                            double os = 0;
                            if (res.osStyle.endPercent)
                            {
                                os = (overUnderRGB - EndRGB) / EndRGB;
                            }
                            else
                            {
                                double range = EndRGB - StartingRGB;
                                os = overshootRGBDiff / range;
                            }
                            os *= 100;
                            overshootPercent = Math.Round(os, 1);
                        }
                    }
                }
                else
                {
                    peakValue = minValue;
                    // Light to dark transistion
                    if (minValue < (endAverage - 100) && minValue < (processedGamma[EndRGB].LightLevel - 100))
                    {
                        // overshoot may have occurred
                        // convert minValue to RGB using gamma table
                        Console.WriteLine("Undershoot found");
                        for (int i = 0; i < processedGamma.Count; i++)
                        {
                            // Find what RGB value matches or exceeds the peak light reading for this run
                            if (minValue <= processedGamma[i].LightLevel)
                            {
                                if (i == 0)
                                {
                                    overUnderRGB = 0;
                                    break;
                                }
                                else
                                {
                                    // Check if peak light reading is closer to upper or lower bound value
                                    int diff1 = processedGamma[i].LightLevel - minValue;
                                    int diff2 = minValue - processedGamma[i - 1].LightLevel;
                                    if (diff1 < diff2)
                                    {
                                        overUnderRGB = processedGamma[i].RGB;
                                    }
                                    else
                                    {
                                        overUnderRGB = processedGamma[i - 1].RGB;
                                    }
                                    break;
                                }
                            }
                        }
                        overshootRGBDiff = EndRGB - overUnderRGB;
                        double os = 0;
                        if (res.osStyle.endPercent)
                        {
                            os = (EndRGB - overUnderRGB) / EndRGB;
                        }
                        else
                        {
                            double range = StartingRGB - EndRGB;
                            os = overshootRGBDiff / range;
                        }
                        // os *= -1;
                        os *= 100;
                        overshootPercent = Math.Round(os, 1);
                        if (overshootPercent != 0 && overshootPercent < 1)
                        {
                            overshootPercent = 0;
                        }
                    }
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // INITIAL AND PERCEIVED RESPONSE TIME MEASUREMENTS
                if (StartingRGB < EndRGB)
                {
                    // Setup variables for start/end trigger points
                    double start3 = 0;
                    double endOffsetRGB = 0;
                    double end3 = 0;
                    double endPer3 = 0;
                    double RGBTolerance = res.rtStyle.Tolerance;
                    double tol = (RGBTolerance / 100);
                    if (!res.rtStyle.gammaCorrected)
                    {
                        double range3 = (endAverage - startAverage) * tol; // Subtract low value from high value to get light level range
                        start3 = startAverage + range3; // Start trigger value
                        end3 = endAverage - range3;
                        if (peakValue > (endAverage + range3))
                        { endPer3 = endAverage + range3; } // End trigger value
                        else
                        { endPer3 = endAverage - range3; } // End trigger value
                    }
                    else
                    {
                        if (res.rtStyle.percentage)
                        {
                            RGBTolerance = (EndRGB - StartingRGB) * tol;
                            RGBTolerance = Math.Round(RGBTolerance, 0);
                        }
                        endOffsetRGB = EndRGB - RGBTolerance;
                        start3 = processedGamma[Convert.ToInt32(StartingRGB + RGBTolerance)].LightLevel;
                        end3 = processedGamma[Convert.ToInt32(EndRGB - RGBTolerance)].LightLevel;
                        if (overUnderRGB > (EndRGB + RGBTolerance) && overUnderRGB != 0)
                        { endOffsetRGB = EndRGB + RGBTolerance; }
                        else if (overUnderRGB == -1)
                        { endOffsetRGB = EndRGB; }
                        endPer3 = processedGamma[Convert.ToInt32(endOffsetRGB)].LightLevel;
                        if (overUnderRGB == -1)
                        { endPer3 *= 1.02; }

                    }
                    if (endPer3 >= 65520)
                    { endPer3 = 65500; }

                    // Actually find the start/end points
                    for (int j = (transStart - 20); j < (transEnd + 20); j++) // search samples for start & end trigger points
                    {
                        if (samples[j] >= start3 && initialTransStart == 0) // save the FIRST time value exceeds start trigger
                        {
                            if ((samples[j + 50] > (start3 + 25) || samples[j + 60] > (start3 + 25))
                                && (samples[j + 100] > (start3 + 50) || samples[j + 110] > (start3 + 50))
                                && (samples[j + 150] > (start3 + 75) || samples[j + 160] > (start3 + 75)))
                            {
                                initialTransStart = j;
                                perceivedTransStart = j;
                            }
                            else if (j == transEnd)
                            {
                                initialTransStart = transStart;
                                perceivedTransStart = transStart;
                            }
                        }
                        else if (samples[j] >= end3) // Save when value exceeds end trigger then break.
                        {
                            if ((samples[j + 20] > (end3 + 25) || samples[j + 25] > (end3 + 25))
                                && (samples[j + 30] > (end3 + 50) || samples[j + 35] > (end3 + 50))
                                && (samples[j + 50] > (end3 + 75) || samples[j + 55] > (end3 + 75)))
                            {
                                initialTransEnd = j;
                                break;
                            }
                            else if (j == transEnd)
                            {
                                initialTransEnd = transEnd;
                                break;
                            }
                        }
                        else if (j == transEnd)
                        {
                            initialTransEnd = transEnd;
                            break;
                        }
                    }
                    for (int j = (transEnd + 20); j > (transStart - 20); j--) // search samples for end point
                    {
                        if (endOffsetRGB > EndRGB || overUnderRGB == -1 || (endOffsetRGB == 0 && endPer3 > endAverage && overshootPercent > 1)) // Including overshoot in the curve
                        {
                            if (samples[j] >= endPer3)  // add the same sort of more detailed check like complete for finding this
                            {
                                if ((samples[j - 25] > (endPer3 + 25) || samples[j - 30] > (endPer3 + 25))
                                && (samples[j - 35] > (endPer3 + 50) || samples[j - 40] > (endPer3 + 50)))
                                {
                                    perceivedTransEnd = j;
                                    break;
                                }
                            }
                            else if (j == transStart)
                            {
                                perceivedTransEnd = j;
                                break;
                            }
                        }
                        else // No overshoot found within RGB tolerance
                        {
                            if (samples[j] <= endPer3)
                            {
                                if ((samples[j - 50] < (endPer3 - 25) || samples[j - 60] < (endPer3 - 25))
                                && (samples[j - 100] < (endPer3 - 50) || samples[j - 110] < (endPer3 - 50))
                                && (samples[j - 150] < (endPer3 - 75) || samples[j - 160] < (endPer3 - 75)))
                                {
                                    perceivedTransEnd = j;
                                    break;
                                }
                            }
                            else if (j == transStart)
                            {
                                perceivedTransEnd = j;
                                break;
                            }
                        }
                    }
                    if (perceivedTransEnd < initialTransEnd)
                    { // just in case the two methods differ slightly and perceived would come out as shorter.
                        perceivedTransEnd = initialTransEnd;
                    }
                }
                else
                {
                    // Setup variables for start/end trigger points
                    double start3 = 0;
                    double endOffsetRGB = 0;
                    double end3 = 0;
                    double endPer3 = 0;
                    double RGBTolerance = res.rtStyle.Tolerance;
                    double tol = (RGBTolerance / 100);
                    if (!res.rtStyle.gammaCorrected)
                    {
                        double range3 = (startAverage - endAverage) * tol; // Subtract low value from high value to get light level range
                        start3 = startAverage - range3; // Start trigger value
                        end3 = endAverage + range3;
                        if (peakValue < (endAverage - range3))
                        { endPer3 = endAverage - range3; } // End trigger value 
                        else
                        { endPer3 = endAverage + range3; } // End trigger value
                    }
                    else
                    {
                        if (res.rtStyle.percentage)
                        {
                            RGBTolerance = (StartingRGB - EndRGB) * tol;
                            RGBTolerance = Math.Round(RGBTolerance, 0);
                        }
                        endOffsetRGB = EndRGB + RGBTolerance;
                        start3 = processedGamma[Convert.ToInt32(StartingRGB - RGBTolerance)].LightLevel;
                        end3 = processedGamma[Convert.ToInt32(EndRGB + RGBTolerance)].LightLevel;
                        if (overUnderRGB < (EndRGB - RGBTolerance) && overUnderRGB != 0)
                        {
                            endOffsetRGB = EndRGB - RGBTolerance;
                        }
                        endPer3 = processedGamma[Convert.ToInt32(endOffsetRGB)].LightLevel;
                    }

                    for (int j = (transStart - 20); j < (transEnd + 20); j++) // search samples for start point
                    {
                        if (samples[j] <= start3 && initialTransStart == 0) // save the FIRST time value exceeds start trigger
                        {
                            if ((samples[j + 50] < (start3 - 25) || samples[j + 60] < (start3 - 25))
                                && (samples[j + 100] < (start3 - 50) || samples[j + 110] < (start3 - 50))
                                && (samples[j + 150] < (start3 - 75) || samples[j + 160] < (start3 - 75)))
                            {
                                initialTransStart = j;
                                perceivedTransStart = j;
                            }
                            else if (j == transEnd)
                            {
                                initialTransStart = transStart;
                                perceivedTransStart = transStart;
                            }
                        }
                        else if (samples[j] <= end3) // Save when value exceeds end trigger then break.
                        {
                            if ((samples[j + 50] < (end3 - 25) || samples[j + 60] < (end3 - 25))
                                && (samples[j + 100] < (end3 - 50) || samples[j + 110] < (end3 - 50))
                                && (samples[j + 150] < (end3 - 75) || samples[j + 160] < (end3 - 75)))
                            {
                                initialTransEnd = j;
                                break;
                            }
                            else if (j == transEnd)
                            {
                                initialTransEnd = transEnd;
                                break;
                            }
                        }
                        else if (j == transEnd)
                        {
                            initialTransEnd = transEnd;
                            break;
                        }
                    }
                    for (int j = (transEnd + 20); j > (transStart - 20); j--) // search samples for end point
                    {
                        if ((endOffsetRGB < EndRGB && endOffsetRGB != 0) || (endPer3 < endAverage && endOffsetRGB == 0 && overshootPercent > 1)) // Including undershoot in the curve
                        {
                            if (samples[j] <= endPer3)
                            {
                                if ((samples[j - 20] < (endPer3 - 25) || samples[j - 25] < (endPer3 - 25))
                                    && (samples[j - 30] < (endPer3 - 50) || samples[j - 35] < (endPer3 - 50)))
                                {
                                    perceivedTransEnd = j;
                                    break;
                                }
                            }
                            else if (j == transStart)
                            {
                                perceivedTransEnd = j;
                                break;
                            }
                        }
                        else // No overshoot found within RGB tolerance
                        {
                            if (samples[j] >= endPer3)
                            {

                                if ((samples[j - 50] > (endPer3 + 25) || samples[j - 60] > (endPer3 + 25))
                                && (samples[j - 100] > (endPer3 + 50) || samples[j - 110] > (endPer3 + 50))
                                && (samples[j - 150] > (endPer3 + 75) || samples[j - 160] > (endPer3 + 75)))
                                {
                                    perceivedTransEnd = j;
                                    break;
                                }
                            }
                            else if (j == transStart)
                            {
                                perceivedTransEnd = j;
                                break;
                            }
                        }
                    }
                    if (perceivedTransEnd < initialTransEnd)
                    { // just in case the two methods differ slightly and perceived would come out as shorter.
                        perceivedTransEnd = initialTransEnd;
                    }
                }

                double transCount = transEnd - transStart;
                double transTime = (transCount * SampleTime) / 1000;

                double initialTransCount = initialTransEnd - initialTransStart;
                double initialTransTime = (initialTransCount * SampleTime) / 1000;

                double perceivedTransCount = perceivedTransEnd - perceivedTransStart;
                double perceivedTransTime = (perceivedTransCount * SampleTime) / 1000;

                double inputLagTime = (transStart * SampleTime) / 1000;

                double responseTime = Math.Round(transTime, 1);
                double initialResponseTime = Math.Round(initialTransTime, 1);
                double perceivedResponseTime = Math.Round(perceivedTransTime, 1);

                double frameTimeScore = 1000 / Convert.ToDouble(runSetting.RefreshRate);
                frameTimeScore = (10 - frameTimeScore)*10;
                double initialScore = 0;
                
                double perceivedScore = 0;
                double overshootScore = 0;
                foreach (VRRKey v in VRRTable)
                {
                    double valueToScore = 0;
                    double range = 0;
                    double distanceToWorst = 0;
                    double Best = v.Best;
                    double Middle = v.Middle;
                    double Worst = v.Worst;
                    double result = 0;
                    if (v.Type == 0)
                    {
                        valueToScore = initialResponseTime;
                    }
                    else if (v.Type == 1)
                    {
                        valueToScore = perceivedResponseTime - initialResponseTime; 
                    }
                    else if (v.Type == 2)
                    {
                        valueToScore = overshootRGBDiff;
                    }
                    if (valueToScore <= Best)
                    {
                        result = 100;
                    }
                    else if (valueToScore >= Worst)
                    {
                        result = 0;
                    }
                    else
                    {
                        // shifting results to a base of 0 to be able to compute percentage
                        valueToScore -= Best;
                        Worst -= Best;
                        Best -= Best;
                        distanceToWorst = valueToScore / Worst;
                        result = 1 - distanceToWorst;
                        result *= 100;
                    }
                    if (v.Type == 0) // I know this can be done without checking this again.... but I can't engage my brain enough to find out how.
                    {
                        initialScore = result;
                    }
                    else if (v.Type == 1)
                    {
                        perceivedScore = result;
                    }
                    else if (v.Type == 2)
                    {
                        overshootScore = result;
                    }
                }
                Console.WriteLine("FT: " + frameTimeScore + ", OS: " + overshootScore + ", PRT: " + perceivedScore + ", IRT: " + initialScore);
                double visualResponseRating = frameTimeScore * 0.1;
                visualResponseRating += overshootScore * 0.1;
                visualResponseRating += perceivedScore * 0.3;
                visualResponseRating += initialScore * 0.5;
                visualResponseRating = Math.Round(visualResponseRating, 1);
                // Old style
                //double visualResponseRating = 100 - (initialResponseTime + perceivedResponseTime);

                double inputLag = Math.Round(inputLagTime, 1);


                if (res.osStyle.gammaCorrected && !res.osStyle.endPercent && !res.osStyle.rangePercent)
                {
                    // Standard output with total transition time & gamma corrected overshoot value
                    if (overUnderRGB == -1)
                    {
                        overshootRGBDiff = 100;
                    }
                    double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, initialResponseTime, perceivedResponseTime, overshootRGBDiff, visualResponseRating, inputLag };
                    processedData.Add(completeResult);

                    return new processedResult {
                        StartingRGB = StartingRGB,
                        EndRGB = EndRGB,
                        SampleTime = SampleTime,
                        perTime = perceivedResponseTime,
                        perStartIndex = perceivedTransStart,
                        perEndIndex = perceivedTransEnd,
                        compTime = responseTime,
                        compStartIndex = transStart,
                        compEndIndex = transEnd,
                        initTime = initialResponseTime,
                        initStartIndex = initialTransStart,
                        initEndIndex = initialTransEnd,
                        Overshoot = overshootRGBDiff,
                        overshootRGB = overshootRGBDiff,
                        visualResponseRating = visualResponseRating,
                        inputLag = inputLag,
                        offset = period
                    };
                }
                else if (res.osStyle.endPercent || res.osStyle.rangePercent)
                {
                    // Standard output with total transition time & overshoot light level percentage
                    double os = 0;
                    if (res.osStyle.gammaCorrected)
                    {
                        peakValue = overUnderRGB;
                        endAverage = EndRGB;
                        startAverage = StartingRGB;
                    }
                    if (res.osStyle.endPercent)
                    {
                        if (StartingRGB < EndRGB)
                        {
                            /*if (peakValue > (endAverage + 100) || (res.osStyle.gammaCorrected && peakValue > endAverage))
                            {
                                os = (peakValue - endAverage) / endAverage;
                                os *= 100;
                                os = Math.Round(os, 1);
                            }*/
                            os = overshootPercent;
                        }
                        else
                        {
                            /*if (peakValue < (endAverage - 100) || (res.osStyle.gammaCorrected && peakValue < endAverage))
                            {
                                os = (endAverage - peakValue) / endAverage;
                                // os *= -1;
                                os *= 100;
                                os = Math.Round(os, 1);
                            }*/
                            os = overshootPercent;
                        }
                    }
                    else
                    {
                        if (StartingRGB < EndRGB)
                        {
                            /*if (peakValue > (endAverage + 100) || (res.osStyle.gammaCorrected && peakValue > endAverage))
                            {
                                double range = endAverage - startAverage;
                                double peakRange = peakValue - endAverage;
                                os = peakRange / range;
                                os *= 100;
                                os = Math.Round(os, 1);
                            }*/
                            os = overshootPercent;
                        }
                        else
                        {
                            /*
                            if (peakValue < (endAverage - 100) || (res.osStyle.gammaCorrected && peakValue < endAverage))
                            {
                                double range = startAverage - endAverage;
                                double peakRange = endAverage - peakValue;
                                os = peakRange / range;
                                // os *= -1;
                                os *= 100;
                                os = Math.Round(os, 1);
                            }*/
                            os = overshootPercent;
                        }
                    }
                    double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, initialResponseTime, perceivedResponseTime, os, visualResponseRating, inputLag };
                    processedData.Add(completeResult);
                    return new processedResult
                    {
                        StartingRGB = StartingRGB,
                        EndRGB = EndRGB,
                        SampleTime = SampleTime,
                        perTime = perceivedResponseTime,
                        perStartIndex = perceivedTransStart,
                        perEndIndex = perceivedTransEnd,
                        compTime = responseTime,
                        compStartIndex = transStart,
                        compEndIndex = transEnd,
                        initTime = initialResponseTime,
                        initStartIndex = initialTransStart,
                        initEndIndex = initialTransEnd,
                        Overshoot = os,
                        overshootRGB = overshootRGBDiff,
                        visualResponseRating = visualResponseRating,
                        inputLag = inputLag,
                        offset = period
                    };
                }
                else
                {
                    // Standard output with total transition time & gamma corrected overshoot percentage
                    double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, initialResponseTime, perceivedResponseTime, overshootPercent, visualResponseRating, inputLag };
                    processedData.Add(completeResult);

                    return new processedResult
                    {
                        StartingRGB = StartingRGB,
                        EndRGB = EndRGB,
                        SampleTime = SampleTime,
                        perTime = perceivedResponseTime,
                        perStartIndex = perceivedTransStart,
                        perEndIndex = perceivedTransEnd,
                        compTime = responseTime,
                        compStartIndex = transStart,
                        compEndIndex = transEnd,
                        initTime = initialResponseTime,
                        initStartIndex = initialTransStart,
                        initEndIndex = initialTransEnd,
                        Overshoot = overshootPercent,
                        overshootRGB = overshootRGBDiff,
                        visualResponseRating = visualResponseRating,
                        inputLag = inputLag,
                        offset = period
                    };
                }
            }
            catch (Exception procEx)
            {
                Console.WriteLine(procEx.Message + procEx.StackTrace);
                return null;
            }
        }

        
        public List<List<processedResult>> ProcessAllResults(List<List<rawResultData>> data, resultSelection res, int startDelay, List<gammaResult> processedGamma, runSettings runSetting)
        {
            List<List<processedResult>> multipleProcessedResults = new List<List<processedResult>>();
            for (int k = 0; k < data.Count; k++)
            {
                List<processedResult> processedResults = new List<processedResult>();
                for (int i = 0; i < data[k].Count; i++)
                {
                    resultSelection resSel = new resultSelection
                    {
                        arrayIndex = k,
                        resultIndex = i,
                        rtStyle = res.rtStyle,
                        osStyle = res.osStyle,
                    };
                    processedResult procRes = ProcessResponseTimeData(data, resSel, startDelay, processedGamma, runSetting);
                    if (procRes == null)
                    {
                        procRes = new processedResult
                        {
                            StartingRGB = data[k][i].StartingRGB,
                            EndRGB = data[k][i].EndRGB,
                            SampleTime = 0,
                            perTime = 0,
                            perStartIndex = 0,
                            perEndIndex = 0,
                            compTime = 0,
                            compStartIndex = 0,
                            compEndIndex = 0,
                            initTime = 0,
                            initStartIndex = 0,
                            initEndIndex = 0,
                            Overshoot = 0,
                            overshootRGB = 0,
                            visualResponseRating = 0,
                            inputLag = 0,
                            offset = 0
                        };
                    }
                    processedResults.Add(procRes);
                }
                multipleProcessedResults.Add(processedResults);
            }
            return multipleProcessedResults;
        }

        public static double GetMedian(double[] sourceNumbers)
        {
            //Framework 2.0 version of this method. there is an easier way in F4        
            if (sourceNumbers == null || sourceNumbers.Length == 0)
                throw new System.Exception("Median of empty array not defined.");

            //make sure the list is sorted, but use a new array
            double[] sortedPNumbers = (double[])sourceNumbers.Clone();
            Array.Sort(sortedPNumbers);

            //get the median
            int size = sortedPNumbers.Length;
            int mid = size / 2;
            double median = (size % 2 != 0) ? (double)sortedPNumbers[mid] : ((double)sortedPNumbers[mid] + (double)sortedPNumbers[mid - 1]) / 2;
            return median;
        }
        public List<processedResult> AverageMultipleRuns(List<List<processedResult>> multipleRunData, osMethods os)
        {
            int resultCount = multipleRunData[0].Count();            
            List<processedResult> averageData = new List<processedResult>();

            // Average the data, excluding outliers
            for (int k = 0; k < resultCount; k++)
            {
                processedResult res = new processedResult();
                List<double> rTLine = new List<double>();
                List<double> initRTLine = new List<double>();
                List<double> perRTLine = new List<double>();
                List<double> oSLine = new List<double>();
                List<double> vrrLine = new List<double>();
                List<double> iLLine = new List<double>();
                foreach (var list in multipleRunData)
                {
                    rTLine.Add(list[k].compTime);
                    initRTLine.Add(list[k].initTime);
                    perRTLine.Add(list[k].perTime);
                    oSLine.Add(list[k].Overshoot);
                    vrrLine.Add(list[k].visualResponseRating);
                    iLLine.Add(list[k].inputLag);
                }
                double rtMedian = GetMedian(rTLine.ToArray());
                double initRtMedian = GetMedian(initRTLine.ToArray());
                double perRtMedian = GetMedian(perRTLine.ToArray());
                double osMedian = GetMedian(oSLine.ToArray());
                double vrrMedian = GetMedian(vrrLine.ToArray());
                double ilMedian = GetMedian(iLLine.ToArray());
                int validTimeResults = 0;
                int validInitialTimeResults = 0;
                int validPerceivedTimeResults = 0;
                int validOvershootResults = 0;
                int validVRRResults = 0;
                int validILResults = 0;
                foreach (var o in multipleRunData)
                {
                    if ((o[k].compTime < (rtMedian * 1.2) && o[k].compTime > (rtMedian * 0.8)) || multipleRunData.Count < 3)
                    {
                        res.compTime += o[k].compTime;
                        validTimeResults++;
                    }
                    if ((o[k].initTime < (initRtMedian * 1.2) && o[k].initTime > (initRtMedian * 0.8)) || multipleRunData.Count < 3)
                    {
                        res.initTime += o[k].initTime;
                        validInitialTimeResults++;
                    }
                    if ((o[k].perTime < (perRtMedian * 1.2) && o[k].perTime > (perRtMedian * 0.8)) || multipleRunData.Count < 3)
                    {
                        res.perTime += o[k].perTime;
                        validPerceivedTimeResults++;
                    }
                    if ((o[k].Overshoot < (osMedian * 1.2) && o[k].Overshoot > (osMedian * 0.8) && o[k].Overshoot != 0) || multipleRunData.Count < 3)
                    {
                        res.Overshoot += o[k].Overshoot;
                        validOvershootResults++;
                    }
                    if ((o[k].visualResponseRating < (vrrMedian * 1.2) && o[k].visualResponseRating > (vrrMedian * 0.8)) || multipleRunData.Count < 3)
                    {
                        res.visualResponseRating += o[k].visualResponseRating;
                        validVRRResults++;
                    }
                    if ((o[k].inputLag < (ilMedian * 1.2) && o[k].inputLag > (ilMedian * 0.8)) || multipleRunData.Count < 3)
                    {
                        res.inputLag += o[k].inputLag;
                        validILResults++;
                    }
                }
                res.StartingRGB = multipleRunData[0][k].StartingRGB;
                res.EndRGB = multipleRunData[0][k].EndRGB;
                res.compTime = res.compTime / validTimeResults;
                res.compTime = Math.Round(res.compTime, 1);
                res.initTime = res.initTime / validInitialTimeResults;
                res.initTime = Math.Round(res.initTime, 1);
                res.perTime = res.perTime / validPerceivedTimeResults;
                res.perTime = Math.Round(res.perTime, 1);
                if (res.Overshoot != 0)
                {
                    res.Overshoot = res.Overshoot / validOvershootResults;
                    if (os.gammaCorrected && (!os.endPercent || !os.rangePercent))
                    {
                        res.Overshoot = Math.Round(res.Overshoot, 0);
                    }
                    else
                    {
                        res.Overshoot = Math.Round(res.Overshoot, 1);
                    }
                }
                res.visualResponseRating = res.visualResponseRating / validVRRResults;
                res.visualResponseRating = Math.Round(res.visualResponseRating, 1);
                res.inputLag = res.inputLag / validILResults;
                res.inputLag = Math.Round(res.inputLag, 1);
                averageData.Add(res);
            }
            return averageData;
        }

        public List<List<rawResultData>> SmoothAllData(List<List<rawResultData>> rawData)
        {
            List<List<rawResultData>> smoothedData = new List<List<rawResultData>>();
            foreach (List<ProcessData.rawResultData> res in rawData)
            {
                List<ProcessData.rawResultData> tempSmoothed = new List<ProcessData.rawResultData>();
                foreach (ProcessData.rawResultData raw in res)
                {
                    int[] samples = raw.Samples.ToArray();
                    int period = 10;
                    int noise = raw.noiseLevel;
                    if (noise < 250)
                    {
                        period = 20;
                    }
                    else if (noise < 500)
                    {
                        period = 30;
                    }
                    else if (noise < 750)
                    {
                        period = 40;
                    }
                    else
                    {
                        period = 50;
                    }
                    if (Properties.Settings.Default.movingAverageSize > period)
                    {
                        period = Properties.Settings.Default.movingAverageSize;
                    }
                    int[] smoothedSamples = smoothData(samples, period);
                    ProcessData.rawResultData d = new ProcessData.rawResultData
                    {
                        StartingRGB = raw.StartingRGB,
                        EndRGB = raw.EndRGB,
                        SampleCount = raw.SampleCount,
                        TimeTaken = raw.TimeTaken,
                        SampleTime = raw.SampleTime,
                        Samples = smoothedSamples.ToList()
                    };
                    tempSmoothed.Add(d);
                }
                smoothedData.Add(tempSmoothed);

            }
            return smoothedData;
        }


        public rawResultData InterpolateClippedGraph(rawResultData data)
        {
            List<int> Samples = data.Samples;
            List<double> doubleSamples = new List<double>();
            int counter = 0;
            for (int c = 0; c < Samples.Count; c++)
            {
                if (Samples[c] < 64500)
                {
                    doubleSamples.Add((double)Samples[c]);
                }
                else
                {
                    counter++;
                }
            }

            double[] rawX = new double[doubleSamples.Count];
            double[] rawY = doubleSamples.ToArray();
            for (int i = 0; i < rawX.Length; i++)
            {
                rawX[i] = data.SampleTime * 1;
            }
            var line = ScottPlot.Statistics.Interpolation.Cubic.InterpolateXY(rawX, rawY, counter);
            List<int> newLine = new List<int>();
            for (int p = 0; p < line.ys.Length; p++)
            {
                newLine.Add((int)line.ys[p]);
            }
            data.Samples = newLine;

            return data;
        }


/////////////////////////////////////////////////////////////////////////////
//              Input Lag
////////////////////////////////////////////////////////////////////////////


        public class rawInputLagResult
        {
            public double ClickTime { get; set; }
            public float FrameTime { get; set; }
            public int TimeTaken { get; set; }
            public int SampleCount { get; set; }
            public double SampleTime { get; set; }
            public List<int> Samples { get; set; }
        }

        public class inputLagResult
        {
            public int shotNumber { get; set; }
            public double clickTimeMs { get; set; }
            public double frameTimeMs { get; set; }
            public double inputLag { get; set; }
            public double totalInputLag { get; set; }
            public double onDisplayLatency { get; set; }
        }

        public class averageInputLagResult
        {
            public double AVG { get; set; }
            public double MIN { get; set; }
            public double MAX { get; set; }
        }

        public class averagedInputLag
        {
            public List<inputLagResult> inputLagResults { get; set; }
            public averageInputLagResult ClickTime { get; set; }
            public averageInputLagResult FrameTime { get; set; }
            public averageInputLagResult onDisplayLatency { get; set; }
            public averageInputLagResult totalInputLag { get; set; }
        }

        public static List<inputLagResult> processInputLagData(List<rawInputLagResult> inputLagRawData)
        {
            List<inputLagResult> inputLagProcessed = new List<inputLagResult>();

            int shotNumber = 1;
            foreach (rawInputLagResult item in inputLagRawData)
            {
                // Save start, end, time and sample count then clear the values from the array
                double ClickTime = item.ClickTime;
                float FrameTime = item.FrameTime;
                int TimeTaken = item.TimeTaken;
                int SampleCount = item.SampleCount;
                int[] samples = item.Samples.ToArray();

                double SampleTime = ((double)TimeTaken / (double)SampleCount); // Get the time taken between samples

                // Clean up noisy data using moving average function
                /*int period = 20;
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

                samples = averagedSamples.Skip(period).ToArray(); //Moving average spoils the first 10 samples so currently removing them.
                */
                // removed smoothing to not spoil data/accuracy and it's just not needed.

                // Initialise in-use variables
                int transStart = 0;

                int startMax = samples[5]; // Initialise these variables with a real value 
                int startMin = samples[5]; // Initialise these variables with a real value 
                int endMax = samples[samples.Length - 10]; // Initialise these variables with a real value 
                int endMin = samples[samples.Length - 10]; // Initialise these variables with a real value 

                // Build start min/max to compare against
                for (int l = 0; l < 50; l++) //CHANGE TO 180 FOR RUN 2 DATA
                {
                    if (samples[l] < startMin)
                    {
                        startMin = samples[l];
                    }
                    else if (samples[l] > startMax)
                    {
                        startMax = samples[l];
                    }
                }

                

                // Search for where the result starts transitioning - start is almost always less sensitive
                for (int j = 0; j < samples.Length; j++)
                {
                    if (samples[j] > (startMax))
                    {
                        if ((samples[j + 50] > (samples[j] + 50) || samples[j + 56] > (samples[j] + 50))
                             && (samples[j + 100] > (samples[j] + 100) || samples[j + 106] > (samples[j] + 100))
                             && (samples[j + 125] > (samples[j] + 100) || samples[j + 131] > (samples[j] + 100))
                             && (samples[j + 150] > (samples[j] + 100) || samples[j + 156] > (samples[j] + 100))) // check the trigger point is actually the trigger and not noise
                        {
                            transStart = j;
                            break;
                        }
                        else
                        {
                            if (samples[j] > startMax)
                            {
                                startMax = samples[j];
                            }
                        }
                    }
                }

                Console.WriteLine("ClickTime: " + ClickTime);
                double clickTimeMs = ClickTime;
                clickTimeMs /= 1000;
                Console.WriteLine("ClickTimems: " + clickTimeMs);
                double transTime = (transStart * SampleTime) / 1000;
                double inputLag = Math.Round(transTime, 3);

                double totalInputLag = (ClickTime + (transStart * SampleTime)) / 1000;
                totalInputLag = Math.Round(totalInputLag, 3);

                double onDisplayLag = inputLag - FrameTime;

                if (clickTimeMs == totalInputLag || onDisplayLag < 0)
                {
                    clickTimeMs = 0;
                    FrameTime = 0;
                    inputLag = 0;
                    totalInputLag = 0;
                    onDisplayLag = 0;
                }

                inputLagResult completeResult = new inputLagResult { shotNumber = shotNumber, clickTimeMs = clickTimeMs, frameTimeMs = Convert.ToDouble(FrameTime), inputLag = inputLag, totalInputLag = totalInputLag, onDisplayLatency = onDisplayLag};
                inputLagProcessed.Add(completeResult);
                shotNumber++;
            }
            Console.WriteLine("Finished processing");
            return inputLagProcessed;
        }

        public static averagedInputLag AverageInputLagResults(List<rawInputLagResult> inputLagData)
        {
            averagedInputLag inputLagProcessed = new averagedInputLag();

            List<inputLagResult> processedResults = processInputLagData(inputLagData);
            if (processedResults.Count == 0)
            {
                throw new Exception("Processing Failed");
            }

            List<inputLagResult> clearedResults = inputLagOutlierRejection(processedResults);

            inputLagProcessed.inputLagResults = clearedResults;

            // convert to double array for each type of average
            inputLagProcessed.ClickTime = new averageInputLagResult { AVG=0, MIN=100, MAX=0};
            inputLagProcessed.FrameTime = new averageInputLagResult { AVG = 0, MIN = 100, MAX = 0 };
            inputLagProcessed.onDisplayLatency = new averageInputLagResult { AVG = 0, MIN = 100, MAX = 0 };
            inputLagProcessed.totalInputLag = new averageInputLagResult { AVG = 0, MIN = 100, MAX = 0 };
            for (int i = 0; i < inputLagProcessed.inputLagResults.Count; i++)
            {
                inputLagProcessed.ClickTime.AVG += inputLagProcessed.inputLagResults[i].clickTimeMs;
                inputLagProcessed.FrameTime.AVG += inputLagProcessed.inputLagResults[i].frameTimeMs;
                inputLagProcessed.onDisplayLatency.AVG += inputLagProcessed.inputLagResults[i].onDisplayLatency;
                inputLagProcessed.totalInputLag.AVG += inputLagProcessed.inputLagResults[i].totalInputLag;
                if (inputLagProcessed.inputLagResults[i].clickTimeMs < inputLagProcessed.ClickTime.MIN)
                {
                    inputLagProcessed.ClickTime.MIN = inputLagProcessed.inputLagResults[i].clickTimeMs;
                }
                else if (inputLagProcessed.inputLagResults[i].clickTimeMs > inputLagProcessed.ClickTime.MAX)
                {
                    inputLagProcessed.ClickTime.MAX = inputLagProcessed.inputLagResults[i].clickTimeMs;
                }
                if (inputLagProcessed.inputLagResults[i].frameTimeMs < inputLagProcessed.FrameTime.MIN)
                {
                    inputLagProcessed.FrameTime.MIN = inputLagProcessed.inputLagResults[i].frameTimeMs;
                }
                else if (inputLagProcessed.inputLagResults[i].frameTimeMs > inputLagProcessed.FrameTime.MAX)
                {
                    inputLagProcessed.FrameTime.MAX = inputLagProcessed.inputLagResults[i].frameTimeMs;
                }
                if (inputLagProcessed.inputLagResults[i].onDisplayLatency < inputLagProcessed.onDisplayLatency.MIN)
                {
                    inputLagProcessed.onDisplayLatency.MIN = inputLagProcessed.inputLagResults[i].onDisplayLatency;
                }
                else if (inputLagProcessed.inputLagResults[i].onDisplayLatency > inputLagProcessed.onDisplayLatency.MAX)
                {
                    inputLagProcessed.onDisplayLatency.MAX = inputLagProcessed.inputLagResults[i].onDisplayLatency;
                }
                if (inputLagProcessed.inputLagResults[i].totalInputLag < inputLagProcessed.totalInputLag.MIN)
                {
                    inputLagProcessed.totalInputLag.MIN = inputLagProcessed.inputLagResults[i].totalInputLag;
                }
                else if (inputLagProcessed.inputLagResults[i].totalInputLag > inputLagProcessed.totalInputLag.MAX)
                {
                    inputLagProcessed.totalInputLag.MAX = inputLagProcessed.inputLagResults[i].totalInputLag;
                }
            }
            inputLagProcessed.ClickTime.AVG /= inputLagProcessed.inputLagResults.Count;
            inputLagProcessed.ClickTime.AVG = Math.Round(inputLagProcessed.ClickTime.AVG, 3);
            inputLagProcessed.FrameTime.AVG /= inputLagProcessed.inputLagResults.Count;
            inputLagProcessed.FrameTime.AVG = Math.Round(inputLagProcessed.FrameTime.AVG, 3);
            inputLagProcessed.onDisplayLatency.AVG /= inputLagProcessed.inputLagResults.Count;
            inputLagProcessed.onDisplayLatency.AVG = Math.Round(inputLagProcessed.onDisplayLatency.AVG, 3);
            inputLagProcessed.totalInputLag.AVG /= inputLagProcessed.inputLagResults.Count;
            inputLagProcessed.totalInputLag.AVG = Math.Round(inputLagProcessed.totalInputLag.AVG, 3);
            return inputLagProcessed;
        }

        public static List<inputLagResult> inputLagOutlierRejection(List<inputLagResult> res)
        {
            // Consider adding actual outlier rejection like response time averaging...
            List<inputLagResult> newRes = new List<inputLagResult>();
            newRes.AddRange(res);
            foreach (var i in res)
            {
                if (i.onDisplayLatency == 0)
                {
                    newRes.Remove(i);
                }
            }
            return newRes;
        }

    }
}
