using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestConsole
{
    public static class DPerfLogger                 // 168 lines
    {
        // CONVERT THIS TO JUST LIST FRAME TIMES THEN CALL MAIN() FUNCITON WITH AVERAGE FRAME TIME


        // Static variables
        public static StringBuilder sb;
        public static int TestTimeInSeconds = 8, estimatedFPS = 5000;

        // static properties
        public static bool IsTimedTest { get; set; }
        public static int TotalSamples { get; set; }
        public static List<float> SampleFloatSet { get; set; }
        public static string ComputerName { get { return Environment.MachineName; } }

        // Static Methods
        public static void Initialize(string TestTitleType, int seconds, int width, int height)
        {
            IsTimedTest = (seconds > 0);
            TestTimeInSeconds = seconds;
            CalculateEstimatedFrames(width, height);
            Initialize(TestTitleType);
        }
        private static void Initialize(string TestTitleType)
        {
            sb = new StringBuilder();
            sb.AppendLine(ComputerName + " " + DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToShortTimeString() + "  " + TestTitleType + "--");
            SampleFloatSet = new List<float>();
        }
        private static void CalculateEstimatedFrames(int width,  int height)
        {
            // Create a List<float> with the approximate number of needed slots in the list so that we are not creating nad inserting each entry. As well as an estimated number of pixels that can be processed as 2 billion per second.
            estimatedFPS = 2000000000 / (width * height);
        }
        public static void Frame(float frameTime)
        {
            SampleFloatSet.Add(1000.0f / frameTime);
        }
        public static void WriteFPSTest()
        {
            File.AppendAllText("Test.txt", sb.ToString());

            // Reset Static class variables for next execution now that this test has been completed/
            TestTimeInSeconds = 8;
            estimatedFPS = 5000;
        }
        public static double CalcualteFPSMetrics(List<float> sampleFloatSet, bool writeToFile, bool cleanDirtyData, double percentageAllowance)
        {
            double fPSAverage = 0.0, FPSMinimum = 0.0, FPSMaximum = 0.0, standardDeviation2 = 0.0, newfPSAverage2 = 0.0;

            // Time Test
            double timeInBeforeDirtyDataRemoved = sampleFloatSet.Select(e => 1000.0 / e).Sum() / 1000.0;

            if (sampleFloatSet.Any())
            {
                fPSAverage = sampleFloatSet.Average();
                FPSMinimum = sampleFloatSet.Min();
                FPSMaximum = sampleFloatSet.Max();

                // List<float> sameplSet = sampleFloatSet.ToList();
                if (cleanDirtyData && sampleFloatSet.Count > 0)
                {
                    // Record the count of SDamples before reducing Samples that represent Dirty Data.
                    TotalSamples = sampleFloatSet.Count;
                    // Perform Dirty Data cleaning algorythum
                    newfPSAverage2 = RemoveUpperLowerLimits(FPSMinimum, FPSMaximum, sampleFloatSet, percentageAllowance);

                    // If in the case that all samples get trimmed, revert back to the previous calculated average minimum NS Mxumum.
                    if (sampleFloatSet.Count > 0)
                    {
                        // Update the fpsMin & Max values since Dirty data has now been removed.
                        FPSMinimum = sampleFloatSet.Min();
                        FPSMaximum = sampleFloatSet.Max();
                    }
                    else
                    { }
                }
                else
                    newfPSAverage2 = fPSAverage;
                
                double sumOfSquaresOfDifferences2 = sampleFloatSet.Select(val => (val - newfPSAverage2) * (val - newfPSAverage2)).Sum();
                standardDeviation2 = Math.Sqrt(sumOfSquaresOfDifferences2 / sampleFloatSet.Count);
            }

            if (writeToFile)
                sb.AppendLine(string.Format("FPS Stats: # Samples: {0}\tTime: {8}\tAve: {1}\tMin: {2}\tMax: {3}\tStdDev: {4}/{5:p2}\tDirtyData: {6}/{7:p2}", sampleFloatSet.Count, newfPSAverage2.ToString("f4"), (FPSMinimum < 100) ? FPSMinimum.ToString("f0") + "  " : FPSMinimum.ToString("f0"), FPSMaximum.ToString("f0"), ((standardDeviation2 / newfPSAverage2) * newfPSAverage2).ToString("f2"), standardDeviation2 / newfPSAverage2, (TotalSamples == 0) ? " - " : (TotalSamples - sampleFloatSet.Count).ToString(), (TotalSamples == 0) ? " - " : ((double)(TotalSamples - sampleFloatSet.Count) / (double)TotalSamples).ToString("p2"), (timeInBeforeDirtyDataRemoved < 10.0) ? timeInBeforeDirtyDataRemoved.ToString("f3") : timeInBeforeDirtyDataRemoved.ToString("f2")));

            return standardDeviation2;
        }
        private static double RemoveUpperLowerLimits(double fPSMinimum, double fPSMaximum, List<float> sameplSet, double percentageAllowance)
        {
            double doubleAverage = 0.0;
            int previousCount = sameplSet.Count;
            var tempfPSAverage = sameplSet.Average();

            // Ensure we only Trim Dirty Data that needs to be Max or min values.
            if (fPSMinimum < (tempfPSAverage * (1.0 - percentageAllowance))) // .7
                sameplSet.RemoveAll((e => e <= (fPSMinimum * 1.1)));
            if (fPSMaximum > (tempfPSAverage * (1.0 + percentageAllowance))) // 1.3
                sameplSet.RemoveAll((e => e >= (fPSMaximum * 0.9)));

            // After removing superfluious samples, ensure that we have any samples left to re-Calculate, otherwise return the first passed in average back since trimming removed all samples.
            if (sameplSet.Count == 0 || previousCount == sameplSet.Count)
                return tempfPSAverage;

            // Re-Evaluate the Minimum and Max values.
            var newfPSAverage = sameplSet.Average();
            var newFPSMinimum = sameplSet.Min();
            var newFPSMaximum = sameplSet.Max();

            // .70 to 1.3 default values.
            if (newFPSMinimum < (newfPSAverage * (1.0 - percentageAllowance)) || newFPSMaximum > newfPSAverage * (1.0 + percentageAllowance))
                doubleAverage = RemoveUpperLowerLimits(newFPSMinimum, newFPSMaximum, sameplSet, percentageAllowance);
            else
                return newfPSAverage;

            return doubleAverage;
        }
        public static void ShutDown()
        {
            int rampupIndexEnd = 1;
            float percentAllowance = 0.25f;
            double standardDeveiation, startDeviation;

            standardDeveiation = CalcualteFPSMetrics(SampleFloatSet.Skip(SampleFloatSet.Count / 2).Take((SampleFloatSet.Count / 2) - (int)(SampleFloatSet.Count * 0.1)).ToList(), false, true, 0.3);
          
            // Now loop through the SamepleSet and when the standard deviation is grater then the above, its still rampming up.
            for (int i = 1; i < SampleFloatSet.Count; i++)
            {
                // Isolate a sliding second for ramp analysis
                startDeviation = CalcualteFPSMetrics(SampleFloatSet.Skip(i - 1).Take(100).ToList(), false, true, percentAllowance);
               
                if (startDeviation <= (standardDeveiation * (1.0f + 0.05f)))  // was 1.1  then  1.3  then  percentAllowance.
                {
                    rampupIndexEnd = i + 99;// - 1;
                    break;
                }
            }

            // Reset the Total Count before writting any results.
            TotalSamples = 0;
            startDeviation = CalcualteFPSMetrics(SampleFloatSet.Take(rampupIndexEnd + 1).ToList(), true, false, 0.3);
            standardDeveiation = CalcualteFPSMetrics(SampleFloatSet.Skip(rampupIndexEnd).ToList(), true, true, 0.45); // was .5

            if (IsTimedTest)
                WriteFPSTest();

            SampleFloatSet.Clear();
            SampleFloatSet = null;
            sb.Clear();
            sb = null;
        }
    }
}