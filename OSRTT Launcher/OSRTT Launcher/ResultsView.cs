using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Resources;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace OSRTT_Launcher
{
    // TODO List //
    // Graph View
    //  - Limit scrolling/zoom to only within the bounds of the data

    public partial class ResultsView : Form
    {
        string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        string resultsFolderPath = "";
        public ProcessData.runSettings runSettings;
        // Raw sensor data, list of lists to hold multiple runs worth of results at once
        public List<List<ProcessData.rawResultData>> rawData = new List<List<ProcessData.rawResultData>>();
        // Moving average smoothed sensor data, list of lists to hold multiple runs worth of results at once
        public List<List<ProcessData.rawResultData>> smoothedData = new List<List<ProcessData.rawResultData>>();
        // Processed results, list of lists to hold multiple runs worth of results at once
        public List<List<ProcessData.processedResult>> multipleRunsData = new List<List<ProcessData.processedResult>>();
        // Single averaged results list from multiple run data
        public List<ProcessData.processedResult> averageData = new List<ProcessData.processedResult>();
        // Raw gamma sensor data
        public List<int[]> gamma = new List<int[]>();
        // Processed gamma results (list of 256 RGB & correlating light values
        public List<ProcessData.gammaResult> processedGamma = new List<ProcessData.gammaResult>();
        // Single 0-255 transition to gauge distance to start point (initial latency)
        public List<int> testLatency = new List<int>();
        // RGB array to use
        public List<int> RGBArr = new List<int> { 0, 51, 102, 153, 204, 255 };
        // Noise level per transition/RGB level, used for smoothing and processing.
        public List<int[]> noiseLevel = new List<int[]>();
        // How many samples to build the initial min/max from
        public int startDelay = 150;

        public List<ProcessData.rtMethods> rtMethodologies = new List<ProcessData.rtMethods>
        {
            new ProcessData.rtMethods { Type = "RGB5", Name = "RGB 5 Tolerance", Tolerance = 5, gammaCorrected = true, percentage = false },
            new ProcessData.rtMethods { Type = "RGB10", Name = "RGB 10 Tolerance", Tolerance = 10, gammaCorrected = true, percentage = false },
            new ProcessData.rtMethods { Type = "3PerGamCor", Name = "3% of RGB Tolerance", Tolerance = 3, gammaCorrected = true, percentage = true },
            new ProcessData.rtMethods { Type = "10PerGamCor", Name = "10% of RGB Tolerance", Tolerance = 10, gammaCorrected = true, percentage = true },
            new ProcessData.rtMethods { Type = "3Per", Name = "3% of Light Level Tolerance", Tolerance = 3, gammaCorrected = false, percentage = true},
            new ProcessData.rtMethods { Type = "10Per", Name = "10% of Light Level Tolerance", Tolerance = 10, gammaCorrected = false, percentage = true }
        };
        public List<ProcessData.osMethods> osMethodologies = new List<ProcessData.osMethods>
        {
            new ProcessData.osMethods { Type = "GamCor", Name = "RGB Values", endPercent = false, rangePercent = false, gammaCorrected = true },
            new ProcessData.osMethods { Type = "GamCorrEndPer", Name = "Percent over end RGB Value", endPercent = true, rangePercent = false, gammaCorrected = true },
            new ProcessData.osMethods { Type = "GamCorrRangePer", Name = "Percent above RGB range", endPercent = false, rangePercent = true, gammaCorrected = true },
            new ProcessData.osMethods { Type = "EndPer", Name = "Percent over end light level", endPercent = true, rangePercent = false, gammaCorrected = false },
            new ProcessData.osMethods { Type = "RangePer", Name = "Percent over light level range", endPercent = false, rangePercent = true, gammaCorrected = false }
        };
        public ProcessData.rtMethods rtMethod;
        public ProcessData.osMethods osMethod;

        private ResourceManager rm = OSRTT_Launcher.Properties.Resources.ResourceManager;

        public void setRawData(List<List<ProcessData.rawResultData>> rd)
        {
            if (rd != null)
            {
                rawData.AddRange(rd);
            }
        }
        public void setGammaData(List<ProcessData.gammaResult> gd)
        {
            if (gd != null)
            {
                processedGamma.AddRange(gd);
            }
        }

        public void setTestLatency(List<int> tl)
        {
            if (tl != null)
            {
                testLatency.AddRange(tl);
            }
        }

        public void setMultiRunData(List<List<ProcessData.processedResult>> mrd)
        {
            if (mrd != null)
            {
                multipleRunsData.AddRange(mrd);
            }
        }

        public void setAverageData(List<ProcessData.processedResult> ad)
        {
            if (ad != null)
            {
                averageData.AddRange(ad);
            }
        }

        public void setRGBArr(List<int> RGB)
        {
            if (RGB != null)
            {
                RGBArr.AddRange(RGB);
            }
        }

        public void setResultsFolder(string rfp)
        {
            resultsFolderPath = rfp;
        }

        public void setStandardView()
        {
            stdResultsMenuBtn_Click(null, null);
        }

        public void setGraphView()
        {
            graphViewMenuBtn_Click(null, null);
        }
        public void setRtMethod(ProcessData.rtMethods rt)
        {
            rtMethod = rt;
        }
        public void setOsMethod(ProcessData.osMethods os)
        {
            osMethod = os;
        }
        public void setRunSettings(ProcessData.runSettings run)
        {
            if (run != null)
            {
                runSettings = run;
            }
        }

        public ResultsView()
        {
            InitializeComponent();
            this.Icon = (Icon)rm.GetObject("osrttIcon");
            path = new Uri(System.IO.Path.GetDirectoryName(path)).LocalPath;
            path += @"\Results";
            Size = new Size(1100, 500);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            saveHeatmapsToolStripMenuItem.Visible = false;
            viewToolStripMenuItem.Visible = false;
            runSelectToolStrip.Visible = false;
            toolStripSeparator1.Visible = false;
            toolStripSeparator3.Visible = false;
            rtViewMenuList.Visible = false;
            denoiseToolStripBtn.Visible = false;
            deNoisedRawDataToolStripMenuItem.Checked = Properties.Settings.Default.smoothGraph;
            denoiseToolStripBtn.Checked = Properties.Settings.Default.smoothGraph;
            initResultsMethodList();
            initOvershootStyleList();
            initRtStyleList();
        }
        private void ResultsView_Load(object sender, EventArgs e)
        {
            bool graph = false;
            if (multipleRunsData.Count == 0 && averageData.Count == 0)
            {
                stdResultsMenuBtn.Visible = false;
                graph = true;
            }
            else
            {
                stdResultsMenuBtn.Visible = true;
                toolStripSeparator1.Visible = true;
                stdResultsMenuBtn_Click(null, null);
            }
            if (rawData.Count == 0)
            {
                graphViewMenuBtn.Visible = false;
            }
            else
            {
                graphViewMenuBtn.Visible = true;
                
                handleRunsList();
                handleResultsList(runSelectBox.SelectedIndex);
                Thread smoothDataThread = new Thread(new ThreadStart(this.smoothDataThreaded));
                smoothDataThread.Start();
                if (graph)
                {
                    graphViewMenuBtn_Click(null, null);
                }
            }
        }

        private void initResultsMethodList()
        {
            processTypeListBox.Items.Clear();
            foreach (var item in rtMethodologies)
            {
                processTypeListBox.Items.Add(item.Name);
            }
            processTypeListBox.SelectedIndex = 0;
        }
        private void initOvershootStyleList()
        {
            overshootStyleListBox.Items.Clear();
            foreach (var item in osMethodologies)
            {
                overshootStyleListBox.Items.Add(item.Name);
            }
            overshootStyleListBox.SelectedIndex = 0;
        }

        private void initRtStyleList()
        {
            rtViewMenuList.Items.Clear();
            rtViewMenuList.Items.Add("Perceived Response Time");
            rtViewMenuList.Items.Add("Initial Response Time");
            rtViewMenuList.Items.Add("Complete Response Time");
            rtViewMenuList.SelectedIndex = 0;
        }
        private void importRawData()
        {
            // Open file picker dialogue
            var filePath = string.Empty;
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    rawData.Clear();
                    gamma.Clear();
                    //processedGamma.Clear();
                    //averageData.Clear();
                    //multipleRunsData.Clear();
                    if (filePath.Contains("RAW-OSRTT"))
                    {
                        //Read the contents of the file into a stream
                        try
                        {
                            List<ProcessData.rawResultData> tempRes = new List<ProcessData.rawResultData>();
                            List<int[]> tempGamma = new List<int[]>();
                            var fileStream = openFileDialog.OpenFile();
                            using (StreamReader reader = new StreamReader(fileStream))
                            {
                                while (!reader.EndOfStream)
                                {
                                    // This can probably be done better
                                    string fullLine = reader.ReadLine();
                                    if (fullLine.Contains("{"))
                                    {
                                        ProcessData.runSettings runs = JsonConvert.DeserializeObject<ProcessData.runSettings>(fullLine);
                                        runSettings = runs;
                                        continue;
                                    }
                                    else
                                    {
                                        string[] line = fullLine.Split(',');
                                        int[] intLine = new int[line.Length];
                                        for (int i = 0; i < line.Length; i++)
                                        {
                                            if (line[i] == "0")
                                            {
                                                intLine[i] = 0;
                                            }
                                            else if (line[i] != "")
                                            {
                                                intLine[i] = int.Parse(line[i]);
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        Array.Resize(ref intLine, intLine.Length - 1);
                                        if (intLine[0] == 1000)
                                        {
                                            testLatency.AddRange(intLine);
                                        }
                                        else if (intLine[0] == intLine[1])
                                        {
                                            tempGamma.Add(intLine);
                                        }
                                        else
                                        {
                                            ProcessData.rawResultData rawResult = new ProcessData.rawResultData
                                            {
                                                StartingRGB = intLine[0],
                                                EndRGB = intLine[1],
                                                TimeTaken = intLine[2],
                                                SampleCount = intLine[3],
                                                SampleTime = ((double)intLine[2] / (double)intLine[3]),
                                                Samples = intLine.Skip(4).ToList()
                                            };
                                            tempRes.Add(rawResult);
                                        }
                                    }
                                }
                            }
                            resultsFolderPath = filePath.Substring(0, filePath.LastIndexOf('\\'));
                            rawData.AddRange(new List<List<ProcessData.rawResultData>> { tempRes });
                            gamma.AddRange(tempGamma);
                            ProcessData pd = new ProcessData();
                            processedGamma.AddRange(pd.processGammaTable(gamma, rawData[0]));
                            // save gamma data to file
                            if (Properties.Settings.Default.saveGammaTable)
                            {
                                string gammaName = CFuncs.createFileName(resultsFolderPath, "-GAMMA-OSRTT.csv");
                                StringBuilder gammaCsv = new StringBuilder();
                                gammaCsv.AppendLine("RGB,Light Level");
                                foreach (ProcessData.gammaResult g in processedGamma)
                                {
                                    gammaCsv.AppendLine(g.RGB.ToString() + "," + g.LightLevel.ToString());
                                }
                                File.WriteAllText(resultsFolderPath + "\\" + gammaName, gammaCsv.ToString());
                            }
                            Thread smoothDataThread = new Thread(new ThreadStart(this.smoothDataThreaded));
                            smoothDataThread.Start();
                            
                            //processTestLatency();
                            Console.WriteLine(rawData.Count);
                            // Draw graph
                            
                        }
                        catch (Exception ex)
                        {
                            DialogResult d = MessageBox.Show("File may be in use by another program, please make sure it's not open elsewhere and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Console.WriteLine(ex.Message + ex.StackTrace);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, only 'RAW' files can be imported. Please select a 'RAW-OSRTT.csv' file instead.", "Importer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception("Importer Error");
                    }
                }
            }
        }

        private void smoothDataThreaded()
        {
            ProcessData pd = new ProcessData();
            smoothedData.Clear();
            smoothedData.AddRange(pd.SmoothAllData(rawData));
        }

        private void standardView()
        {
            viewToolStripMenuItem.Visible = true;
            runSelectToolStrip.Visible = true;
            runSelectToolStrip.Items.Clear();
            if (averageData.Count != 0)
            {
                runSelectToolStrip.Items.Add("Averaged Data");
                heatmaps1.setAverageData(averageData);
            }
            if (multipleRunsData.Count != 0)
            {
                for (int i = 0; i < multipleRunsData.Count; i++)
                {
                    runSelectToolStrip.Items.Add("Run " + (i + 1).ToString());
                }
            }
            runSelectToolStrip.SelectedIndex = 0;
            if (averageData.Count == 0 && multipleRunsData.Count != 0)
            {
                heatmaps1.setAverageData(multipleRunsData[0]);
            }
            else if (averageData.Count == 0 && multipleRunsData.Count == 0)
            {
                throw new Exception("No Data");
            }
            runSelectToolStrip.Visible = true;
            if (initialResponseTimeToolStripMenuItem.Checked)
            {
                heatmaps1.setRtIndex(3);
            }
            else if (completeResponseTimeToolStripMenuItem.Checked)
            {
                heatmaps1.setRtIndex(2);
            }
            else
            {
                heatmaps1.setRtIndex(4);
            }
            if (runSettings != null)
            {
                heatmaps1.setRtMethod(runSettings.rtMethod);
                heatmaps1.setOsMethod(runSettings.osMethod);
                heatmaps1.setRunSettings(runSettings);
            }
            else
            {
                heatmaps1.setRtMethod(rtMethodologies[0]); // FALLBACK OPTION FOR NOW
                heatmaps1.setOsMethod(osMethodologies[0]); // FALLBACK OPTION FOR NOW
            }
            if (runSettings != null)
            {
                heatmaps1.setRunSettings(runSettings);
            }
            heatmaps1.standardView();
            rtViewMenuList.Visible = false;
            denoiseToolStripBtn.Visible = false;
            string[] existingFiles = Directory.GetFiles(resultsFolderPath, "*.png");
            if (existingFiles.Length == 0 && Properties.Settings.Default.autoSavePNG != 0)
            {
                if (Properties.Settings.Default.autoSavePNG == 1)
                {
                    asTransparentPNGToolStripMenuItem_Click(null, null);
                }
                else
                {
                    asPNGToolStripMenuItem_Click(null, null);
                }
            }
        }
        private void graphView()
        {
            // Clear view/import panel
            // Create graph
            // Set position, size, etc
            // Draw graph
            if (rawData.Count != 0)
            {
                Size = new Size(Size.Width, (Size.Height + 20));
                graphViewPanel.Location = new Point(5, 52);
                drawGraph();
                showProcessedData();
            }
            else
            {
                MessageBox.Show("Error: No data loaded in window to graph. Please import a raw file.", "No Data To Graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        // You need to change ComboBox.DrawMode to OwnerDrawFixed/OwnerDrawVariable to fire the comboBox_DrawItem
        /*private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background 
            e.DrawBackground();

            // Get the item text    
            string text = ((ComboBox)sender).Items[e.Index].ToString();

            // Determine the forecolor based on whether or not the item is selected    
            Brush brush;
            if (YourListOfDates[e.Index] < DateTime.Now)// compare  date with your list.  
            {
                brush = Brushes.Red;
            }
            else
            {
                brush = Brushes.Green;
            }

            // Draw the text    
            e.Graphics.DrawString(text, ((Control)sender).Font, brush, e.Bounds.X, e.Bounds.Y);
        }*/

        private void drawGraph(int arrayIndex = 0, int resultIndex = 0)
        {
            if (resultIndex < 0)
            {
                resultIndex = 0;
            }
            int startingRGB = rawData[arrayIndex][resultIndex].StartingRGB;
            int endRGB = rawData[arrayIndex][resultIndex].EndRGB;
            int sampleTime = rawData[arrayIndex][resultIndex].TimeTaken;
            int sampleCount = rawData[arrayIndex][resultIndex].SampleCount;
            double averageTime = rawData[arrayIndex][resultIndex].SampleTime;
            double[] resultData;
            if (Properties.Settings.Default.smoothGraph)
            {
                resultData = smoothedData[arrayIndex][resultIndex].Samples.Select(x => (double)x).ToArray();
            }
            else
            {
                resultData = rawData[arrayIndex][resultIndex].Samples.Select(x => (double)x).ToArray();
            }
            double[] timeData = new double[resultData.Length];
            for (int i = 0; i < timeData.Length; i++)
            {
                timeData[i] = averageTime * i;
                timeData[i] /= 1000;
                timeData[i] = Math.Round(timeData[i], 2);
            }
            graphedData.Plot.Clear();
            graphedData.Plot.Palette = ScottPlot.Palette.OneHalfDark;
            graphedData.Plot.AxisAuto(0, 0.1);
            double xMax = timeData.Max();
            double yMin = resultData.Min();
            yMin *= 0.75;
            double yMax = resultData.Max();
            yMax *= 1.04;
            graphedData.Plot.SetOuterViewLimits(0, xMax, yMin, yMax);
            graphedData.Plot.Style(ScottPlot.Style.Gray1);
            var bnColor = System.Drawing.ColorTranslator.FromHtml("#2e3440");
            graphedData.Plot.Style(figureBackground: bnColor, dataBackground: bnColor);
            graphedData.Plot.Title("RGB " + startingRGB.ToString() + " to RGB " + endRGB.ToString());
            graphedData.Plot.YLabel("Light level (16 bit integer)");
            graphedData.Plot.XLabel("Time (ms)");
            graphedData.Plot.AddScatter(timeData, resultData, lineWidth: 3, markerSize: 4);
            //graphedData.Plot.SetAxisLimits(0, (timeData.Length + 100), 0, (resultData.Max() + 100));
            graphedData.Plot.Render();
            graphedData.Refresh();
            //showProcessedData();
        }
        private void onSpanDrag(object sender, EventArgs e)
        {
            var hSpan = sender as ScottPlot.Plottable.HSpan;
            double newTime = hSpan.X2 - hSpan.X1;
            newTime = Math.Round(newTime, 1);
            rtLabel.Text = newTime.ToString() + " ms";
            rtTypeLabel.Text = "Manual";
            Console.WriteLine(hSpan.X1 + "" + hSpan.X2);
        }
        static Coordinate MoveBetweenAdjacent(List<double> xs, List<double> ys, int index, Coordinate requested)
        {
            int leftIndex = Math.Max(index - 1, 0);
            int rightIndex = Math.Min(index + 1, xs.Count - 1);

            double newX = requested.X;
            newX = Math.Max(newX, xs[leftIndex]);
            newX = Math.Min(newX, xs[rightIndex]);

            return new Coordinate(newX, requested.Y);
        }

        private void handleResultsList(int arrayIndex)
        {
            if (rawData.Count != 0)
            {
                transSelect1.Items.Clear();
                foreach (var i in rawData[arrayIndex])
                {
                    transSelect1.Items.Add("RGB " + i.StartingRGB + " to RGB " + i.EndRGB);
                }
                transSelect1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Error: No data loaded in window to graph. Please import a raw file.", "No Data To Graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void handleRunsList()
        {
            if (rawData.Count != 0)
            {
                runSelectBox.Items.Clear();
                for (int i = 0; i < rawData.Count; i++)
                {
                    runSelectBox.Items.Add("Run " + (i + 1).ToString());
                }
                runSelectBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Error: No data loaded in window to graph. Please import a raw file or folder.", "No Data To Graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void importView()
        {
            // Clear view/graph panel
            // Move import panel in view
            Size = new Size(1100, 500);
            importPanel.Location = new Point(5, 52);
            importViewMenuButton.Checked = true;
        }

        private void stdResultsMenuBtn_Click(object sender, EventArgs e)
        {
            BackColor = Color.White;
            Size = new Size(1800, 1050);
            saveHeatmapsToolStripMenuItem.Visible = true;
            importPanel.Location = new Point(1500, 52);
            graphViewPanel.Location = new Point(1439, 762);
            stdResultsMenuBtn.Visible = true;
            stdResultsMenuBtn.Checked = true;
            heatmaps1.Location = new Point(5, 52);
            heatmaps1.Visible = true;
            inputLagProcRV1.Location = new Point(1500, 52);
            inputLagProcRV1.Visible = false;
            importPanel.Visible = false;
            graphViewPanel.Visible = false;
            if (rawData.Count != 0)
            {
                graphViewMenuBtn.Visible = true;
            }
            else
            {
                graphViewMenuBtn.Visible = false;
            }
            graphViewMenuBtn.Checked = false;
            importViewMenuButton.Checked = false;
            viewToolStripMenuItem.Visible = true;
            deNoisedRawDataToolStripMenuItem.Visible = false;
            toolStripSeparator2.Visible = false;
            toolStripSeparator1.Visible = true;
            rtViewMenuList.Visible = false;
            denoiseToolStripBtn.Visible = false;
            try
            {
                standardView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace, "Heatmaps View Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void graphViewMenuBtn_Click(object sender, EventArgs e)
        {
            try
            {
                handleRunsList();
                handleResultsList(0);
                graphView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace, "Graph View Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            importPanel.Location = new Point(1439, 762);
            graphViewPanel.Location = new Point(5, 52);
            heatmaps1.Location = new Point(1395, 55);
            inputLagProcRV1.Location = new Point(1500, 52);
            inputLagProcRV1.Visible = false;
            heatmaps1.Visible = false;
            graphViewPanel.Visible = true;
            importPanel.Visible = false;
            saveHeatmapsToolStripMenuItem.Visible = false;
            Size = new Size(1400, 820);
            //Size = new Size(Size.Width, (Size.Height + 15));
            BackColor = System.Drawing.ColorTranslator.FromHtml("#2e3440");
            stdResultsMenuBtn.Checked = false;
            graphViewMenuBtn.Checked = true;
            graphViewMenuBtn.Visible = true;
            importViewMenuButton.Checked = false;
            viewToolStripMenuItem.Visible = true;
            deNoisedRawDataToolStripMenuItem.Visible = true;
            toolStripSeparator2.Visible = true;
            runSelectToolStrip.Visible = false;
            toolStripSeparator3.Visible = true;
            rtViewMenuList.Visible = true;
            denoiseToolStripBtn.Visible = true;
            
        }

        private void importViewMenuButton_Click(object sender, EventArgs e)
        {
            importView();
            Size = new Size(1100, 500);
            BackColor = SystemColors.ControlDark;
            importPanel.Location = new Point(5, 52);
            importPanel.Visible = true;
            graphViewPanel.Location = new Point(1439, 762);
            graphViewPanel.Visible = false;
            heatmaps1.Location = new Point(1395, 55);
            heatmaps1.Visible = false;
            inputLagProcRV1.Location = new Point(1500, 52);
            inputLagProcRV1.Visible = false;
            saveHeatmapsToolStripMenuItem.Visible = false;
            stdResultsMenuBtn.Checked = false;
            graphViewMenuBtn.Checked = false;
            importViewMenuButton.Checked = true;
            viewToolStripMenuItem.Visible = false;
            runSelectToolStrip.Visible = false;
        }

        private void transSelect1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
                showProcessedData();
            }
        }

        private void importGraphBtn_Click(object sender, EventArgs e)
        {
            importRawData();
            graphViewMenuBtn_Click(null, null);
            handleRunsList();
            handleResultsList(runSelectBox.SelectedIndex);
        }

        private void resetGraphBtn_Click(object sender, EventArgs e)
        {
            drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
            showProcessedData();
        }
        private void showProcessedData()
        {
            if (rawData.Count != 0)
            {
                try
                {
                    int resInd = transSelect1.SelectedIndex;
                    if (resInd < 0)
                    {
                        resInd = 0;
                    }
                    int arrInd = runSelectBox.SelectedIndex;
                    if (arrInd < 0)
                    {
                        arrInd = 0;
                    }
                    string rtType = "perceived";
                    if (perceivedResponseTimeToolStripMenuItem.Checked)
                    {
                        rtType = "perceived";
                        rtTypeLabel.Text = "Perceived";
                    }
                    else if (initialResponseTimeToolStripMenuItem.Checked)
                    {
                        rtType = "initial";
                        rtTypeLabel.Text = "Initial";
                    }
                    else if (completeResponseTimeToolStripMenuItem.Checked)
                    {
                        rtType = "complete";
                        rtTypeLabel.Text = "Complete";
                    }
                    // make view button change this
                    ProcessData pd = new ProcessData();
                    int startDelay = pd.processTestLatency(testLatency);
                    ProcessData.graphResult proc = pd.processGraphResult(rawData, new ProcessData.resultSelection
                    {
                        arrayIndex = arrInd,
                        resultIndex = resInd,
                        rtStyle = rtMethodologies[processTypeListBox.SelectedIndex],
                        osStyle = osMethodologies[overshootStyleListBox.SelectedIndex]
                    }, startDelay, processedGamma, rtType, runSettings);
                    try
                    {
                        rtLabel.Text = proc.Time.ToString() + " ms";
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message + err.StackTrace);
                    }
                    if (osMethodologies[overshootStyleListBox.SelectedIndex].endPercent || osMethodologies[overshootStyleListBox.SelectedIndex].rangePercent)
                    {
                        osLabel.Text = proc.Overshoot.ToString() + "%";
                    }
                    else
                    {
                        osLabel.Text = proc.Overshoot.ToString() + " RGB";
                    }
                    // clear existing spans
                    graphedData.Plot.Clear(typeof(ScottPlot.Plottable.HSpan));
                    double resTime = Convert.ToDouble(testLatency[2]);
                    double sampleCount = Convert.ToDouble(testLatency[3]);
                    double sampleTime = resTime / sampleCount;
                    double start = ((proc.startIndex + proc.offset) * sampleTime);
                    double end = ((proc.endIndex + proc.offset) * sampleTime);
                    if (Properties.Settings.Default.smoothGraph)
                    {
                        start = (proc.startIndex * sampleTime);
                        end = (proc.endIndex * sampleTime);
                    }
                    start /= 1000;
                    start = Math.Round(start, 2);
                    end /= 1000;
                    end = Math.Round(end, 2);
                    var hSpan = graphedData.Plot.AddHorizontalSpan(start, end);
                    // graphedData.Plot.AddHorizontalSpan(start, end);
                    hSpan.DragEnabled = true;
                    hSpan.Dragged += new EventHandler(onSpanDrag);
                    graphedData.Plot.Render();
                    graphedData.Refresh();
                    latencyLabel.Text = start.ToString() + " ms";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace, "Failed to Process Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
            }
        }

        private void processTypeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                showProcessedData();
            }
        }

        private void overshootStyleListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                showProcessedData();
            }
        }

        private void runSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
                showProcessedData();
            }
        }

        private bool importRawFolderData()
        {
            // Open folder picker dialogue
            var filePath = string.Empty;
            using (FolderBrowserDialog folderDiag = new FolderBrowserDialog())
            {
                folderDiag.SelectedPath = path;
                if (folderDiag.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = folderDiag.SelectedPath;
                    if (filePath != path)
                    {
                        rawData.Clear();
                        gamma.Clear();
                        //processedGamma.Clear();
                        //averageData.Clear();
                        //multipleRunsData.Clear();
                        //results.Clear();
                        string[] files = Directory.GetFiles(filePath);
                        bool valid = false;
                        foreach (var f in files)
                        {
                            if (f.Contains("-RAW-OSRTT") && !f.Contains("INPUT"))
                            {
                                valid = true;
                                gamma.Clear();
                                testLatency.Clear();
                                try
                                {
                                    List<ProcessData.rawResultData> tempRes = new List<ProcessData.rawResultData>();
                                    List<int[]> tempGamma = new List<int[]>();
                                    using (System.Windows.Forms.OpenFileDialog OFD = new System.Windows.Forms.OpenFileDialog())
                                    {
                                        OFD.FileName = f;
                                        //Read the contents of the file into a stream

                                        var fileStream = OFD.OpenFile();
                                        using (StreamReader reader = new StreamReader(fileStream))
                                        {
                                            while (!reader.EndOfStream)
                                            {
                                                // This can probably be done better
                                                string fullLine = reader.ReadLine();
                                                if (fullLine.Contains("{"))
                                                {
                                                    ProcessData.runSettings runs = JsonConvert.DeserializeObject<ProcessData.runSettings>(fullLine);
                                                    runSettings = runs;
                                                    continue;
                                                }
                                                else
                                                {
                                                    string[] line = fullLine.Split(',');
                                                    int[] intLine = new int[line.Length];
                                                    for (int i = 0; i < line.Length; i++)
                                                    {
                                                        if (line[i] == "0")
                                                        {
                                                            intLine[i] = 0;
                                                        }
                                                        else if (line[i] != "" && !line[i].Contains("."))
                                                        {
                                                            intLine[i] = int.Parse(line[i]);
                                                        }
                                                        else
                                                        {
                                                            continue;
                                                        }
                                                    }
                                                    Array.Resize(ref intLine, intLine.Length - 1);
                                                    if (intLine[0] == 1000)
                                                    {
                                                        testLatency.AddRange(intLine);
                                                    }
                                                    else if (intLine[0] == intLine[1])
                                                    {
                                                        tempGamma.Add(intLine);
                                                    }
                                                    else
                                                    {
                                                        ProcessData.rawResultData rawResult = new ProcessData.rawResultData
                                                        {
                                                            StartingRGB = intLine[0],
                                                            EndRGB = intLine[1],
                                                            TimeTaken = intLine[2],
                                                            SampleCount = intLine[3],
                                                            SampleTime = ((double)intLine[2] / (double)intLine[3]),
                                                            Samples = intLine.Skip(4).ToList()
                                                        };
                                                        tempRes.Add(rawResult);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    rawData.AddRange(new List<List<ProcessData.rawResultData>> { tempRes });
                                    gamma.AddRange(tempGamma);

                                    //processTestLatency();
                                    Console.WriteLine(rawData.Count);
                                }
                                catch (IOException iex)
                                {
                                    if (!iex.Message.Contains(".xlsx"))
                                    {
                                        MessageBox.Show("Unable to open file - it may be in use in another program. Please close it out and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        MessageBox.Show(iex.Message + iex.StackTrace, "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            /*else if (f.Contains("data.json"))
                            {
                                valid = true;
                                gamma.Clear();
                                testLatency.Clear();
                                string json = "";
                                using (System.Windows.Forms.OpenFileDialog OFD = new System.Windows.Forms.OpenFileDialog())
                                {
                                    OFD.FileName = f;
                                    //Read the contents of the file into a stream

                                    var fileStream = OFD.OpenFile();
                                    using (StreamReader reader = new StreamReader(fileStream))
                                    {
                                        while (!reader.EndOfStream)
                                        {
                                            // This can probably be done better
                                            json = reader.ReadToEnd();
                                            
                                        }
                                        DataUpload.ShareData[] results = JsonConvert.DeserializeObject<DataUpload.ShareData[]>(json);
                                        if (results != null)
                                        {
                                            List<DataUpload.ShareData> resultsList = results.ToList();
                                            foreach (DataUpload.ShareData share in resultsList)
                                            {
                                                gamma.Clear();
                                                rawData.Add(share.rawData);
                                                gamma.AddRange(share.gammaData);
                                                testLatency.AddRange(share.testLatency);
                                                runSettings = share.runSettings;
                                            }
                                        }
                                    }
                                }
                            }*/
                        }
                        if (valid)
                        {
                            resultsFolderPath = filePath;
                            Console.WriteLine(filePath);
                            Console.WriteLine(resultsFolderPath);
                            ProcessData pd = new ProcessData();
                            foreach (var i in rawData)
                            {
                                processedGamma.Clear();
                                processedGamma.AddRange(pd.processGammaTable(gamma, i));
                            }
                            // save gamma data to file
                            if (Properties.Settings.Default.saveGammaTable)
                            {
                                string gammaName = CFuncs.createFileName(resultsFolderPath, "-GAMMA-OSRTT.csv");
                                StringBuilder gammaCsv = new StringBuilder();
                                gammaCsv.AppendLine("RGB,Light Level");
                                foreach (ProcessData.gammaResult g in processedGamma)
                                {
                                    gammaCsv.AppendLine(g.RGB.ToString() + "," + g.LightLevel.ToString());
                                }
                                File.WriteAllText(resultsFolderPath + "\\" + gammaName, gammaCsv.ToString());
                            }
                            Thread smoothDataThread = new Thread(new ThreadStart(this.smoothDataThreaded));
                            smoothDataThread.Start();
                        }
                        else
                        {
                            MessageBox.Show("Please select a results folder with one or more raw data files", "Unable to load files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return valid;
                    }
                    else
                    {
                        MessageBox.Show("Please select a results folder with one or more raw data files", "Unable to load files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return false;
        }

        private void importRawFolder_Click(object sender, EventArgs e)
        {
            importRawFolderData();
            graphViewMenuBtn_Click(null, null);
            handleRunsList();
            handleResultsList(runSelectBox.SelectedIndex);
        }

        private void importResultsViewBtn_Click(object sender, EventArgs e)
        {
            // Open file picker dialogue
            var filePath = string.Empty;

            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    //rawData.Clear();
                    //gamma.Clear();
                    //processedGamma.Clear();
                    averageData.Clear();
                    multipleRunsData.Clear();
                    if (filePath.Contains("FULL-OSRTT") || (!filePath.Contains("RAW") && !filePath.Contains("GAMMA") && !filePath.Contains("INPUT")))
                    {
                        //Read the contents of the file into a stream
                        try
                        {
                            List<ProcessData.processedResult> tempRes = new List<ProcessData.processedResult>();
                            var fileStream = openFileDialog.OpenFile();
                            using (StreamReader reader = new StreamReader(fileStream))
                            {
                                while (!reader.EndOfStream)
                                {
                                    // This can probably be done better
                                    string fullLine = reader.ReadLine();
                                    
                                    if (fullLine.Contains("{"))
                                    {
                                        ProcessData.runSettings runs = JsonConvert.DeserializeObject<ProcessData.runSettings>(fullLine);
                                        runSettings = runs;
                                        continue;
                                    }
                                    else if (fullLine.Contains("RGB"))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        string[] line = fullLine.Split(',');
                                        double[] intLine = new double[line.Length];
                                        for (int i = 0; i < line.Length; i++)
                                        {
                                            if (line[i] == "0")
                                            {
                                                intLine[i] = 0;
                                            }
                                            else if (line[i] != "")
                                            {
                                                intLine[i] = double.Parse(line[i]);
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        Array.Resize(ref intLine, intLine.Length - 1);
                                        ProcessData.processedResult proc = new ProcessData.processedResult
                                        {
                                            StartingRGB = (int)intLine[0],
                                            EndRGB = (int)intLine[1],
                                            compTime = intLine[2],
                                            initTime = intLine[3],
                                            perTime = intLine[4],
                                            Overshoot = intLine[5],
                                            visualResponseRating = intLine[6],
                                        };
                                        tempRes.Add(proc);
                                    }
                                }
                            }
                            resultsFolderPath = filePath.Substring(0, filePath.LastIndexOf('\\'));
                            averageData.AddRange(tempRes);
                            stdResultsMenuBtn_Click(null, null);
                        }
                        catch (Exception ex)
                        {
                            DialogResult d = MessageBox.Show("File may be in use by another program, please make sure it's not open elsewhere and try again. See more details on the error?", "Unable to open file", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            if (d == DialogResult.Yes)
                            {
                                MessageBox.Show(ex.Message + ex.StackTrace, "Full Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            Console.WriteLine(ex.Message + ex.StackTrace);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, only 'FULL' (processed) files can be imported. Please select a 'FULL-OSRTT.csv' file instead.", "Importer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void saveAsPNGBtn_Click(object sender, EventArgs e)
        {
            string run = "Run 1";
            string result = "RGB 0 to RGB 51";
            if (runSelectBox.SelectedIndex != null)
            {
                run = runSelectBox.Items[runSelectBox.SelectedIndex].ToString();
            }
            if (transSelect1.SelectedIndex != null)
            {
                result = transSelect1.Items[transSelect1.SelectedIndex].ToString();
            }
            Color bnColor = BackColor;
            graphedData.Plot.Style(figureBackground: Color.Transparent, dataBackground: Color.Transparent);
            graphedData.Plot.SaveFig(resultsFolderPath + "\\" + run + "-" + result + ".png", 1920, 1080, false);
            graphedData.Plot.Style(figureBackground: bnColor, dataBackground: bnColor);
            Process.Start("explorer.exe", resultsFolderPath);
        }

        private void saveGraphNoHSpanBtn_Click(object sender, EventArgs e)
        {
            string run = "Run 1";
            string result = "RGB 0 to RGB 51";
            if (runSelectBox.SelectedIndex != null)
            {
                run = runSelectBox.Items[runSelectBox.SelectedIndex].ToString();
            }
            if (transSelect1.SelectedIndex != null)
            {
                result = transSelect1.Items[transSelect1.SelectedIndex].ToString();
            }
            var plots = graphedData.Plot.GetPlottables();
            foreach (var i in plots)
            {
                if (i.ToString().Contains("span"))
                {
                    i.IsVisible = false;
                }
            }
            Color bnColor = BackColor;
            graphedData.Plot.Style(figureBackground: Color.Transparent, dataBackground: Color.Transparent);
            graphedData.Plot.SaveFig(resultsFolderPath + "\\" + run + "-" + result + ".png", 1920, 1080, false);
            graphedData.Plot.Style(figureBackground: bnColor, dataBackground: bnColor);
            Process.Start("explorer.exe", resultsFolderPath);

            foreach (var i in plots)
            {
                if (i.ToString().Contains("span"))
                {   
                    i.IsVisible = true;
                }
            }
        }

        private void asTransparentPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = "OSRTT Heatmaps.png";
            string monitorInfo = "";
            if (resultsFolderPath != "")
            {
                string[] existingFiles = Directory.GetFiles(resultsFolderPath, "*.png");
                int fileNumber = 0;
                //search files for number
                foreach (var s in existingFiles)
                {
                    try
                    {
                        int num = int.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                        if (num >= fileNumber)
                        {
                            fileNumber = num + 1;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Input string error");
                    }
                }
                string[] folders = resultsFolderPath.Split('\\');
                monitorInfo = folders.Last();
                fileName = monitorInfo + ".png";
            }
            else
            {
                resultsFolderPath = path;
            }
            Bitmap heatmaps = new Bitmap(this.Width, this.Height);
            BackColor = Color.FromArgb(255,240,240,240);
            heatmaps1.hideText(false);
            this.DrawToBitmap(heatmaps, new Rectangle(0, 0, heatmaps.Width, heatmaps.Height));
            
            // crop sides
            // 80px top, 8px every other side
            // width - 16, height - 88, x = 8, y = 80
            var rect = new Rectangle(new Point(8, 80), new Size((this.Width - 16), (this.Height - 88)));
            //Bitmap scaledHeatmap = CropImage(heatmaps, rect);
            //Bitmap finalHeatmaps = ScaleImage(scaledHeatmap, 1920, 1080);
            Bitmap finalHeatmaps = CropImage(heatmaps, rect);
            finalHeatmaps.MakeTransparent(BackColor);
            // draw text back...
            string rtTitle = "Perceived Response Time";
            string rtSubTitle = runSettings.rtMethod.Name;
            string osTitle = "RGB Overshoot";
            string osSubTitle = "RGB values over/under target";
            string vrrTitle = "Visual Response Rating";
            string vrrSubTitle = "Score out of 100 of visible performance";
            if (initialResponseTimeToolStripMenuItem.Checked)
            {
                rtTitle = "Initial Response Time";
            }
            else if (completeResponseTimeToolStripMenuItem.Checked)
            {
                rtTitle = "Complete Response Time";
            }
            if (runSettings.osMethod.gammaCorrected && (!runSettings.osMethod.endPercent || !runSettings.osMethod.rangePercent))
            {
                osTitle = "RGB Overshoot";
                osSubTitle = "RGB values over/under target";
            }
            else if (osMethod.gammaCorrected && (osMethod.endPercent || osMethod.rangePercent))
            {
                osTitle = "Percent RGB Overshoot";
                if (osMethod.rangePercent)
                {
                    osSubTitle = "Percentage of RGB values over/under transition range";
                }
                else
                {
                    osSubTitle = "Percentage of RGB values over/under target";
                }
            }
            else if (!osMethod.gammaCorrected && (osMethod.endPercent || osMethod.rangePercent))
            {
                osTitle = "Percent Overshoot";
                if (osMethod.rangePercent)
                {
                    osSubTitle = "Percent of light level over/under transition range";
                }
                else
                {
                    osSubTitle = "Percent of light level over/under target";
                }
            }
            using (Graphics g = Graphics.FromImage(finalHeatmaps))
            {
                Brush b = new SolidBrush(Properties.Settings.Default.heatmapTextColour);
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                Rectangle rt = new Rectangle(100, 36, 377, 36);
                Rectangle rtSub = new Rectangle(100, 65, 377, 36);
                Rectangle os = new Rectangle(676, 36, 377, 36);
                Rectangle osSub = new Rectangle(676, 65, 377, 36);
                Rectangle vrr = new Rectangle(1264, 36, 377, 36);
                Rectangle vrrSub = new Rectangle(1264, 65, 377, 36);
                FontFamily ff = new FontFamily("Calibri");
                Font f = new Font(ff, 20f, FontStyle.Bold);
                Font fi = new Font(ff, 16f, FontStyle.Italic);
                Font fk = new Font(ff, 17f, FontStyle.Bold);
                g.DrawString(rtTitle, f, b, rt, sf);
                g.DrawString(rtSubTitle, fi, b, rtSub, sf);
                g.DrawString(osTitle, f, b, os, sf);
                g.DrawString(osSubTitle, fi, b, osSub, sf);
                g.DrawString(vrrTitle, f, b, vrr, sf);
                g.DrawString(vrrSubTitle, fi, b, vrrSub, sf);
                g.DrawString("From", fi, b, new Point(29, 393));
                g.DrawString("From", fi, b, new Point(606, 393));
                g.DrawString("From", fi, b, new Point(1190, 393));
                g.DrawString("To", fi, b, new Point(556, 106));
                g.DrawString("To", fi, b, new Point(1132, 106));
                g.DrawString("To", fi, b, new Point(1717, 106));
                // DRAW key text too
                //g.DrawString("Response Time Key", fk, Brushes.Black, new Point(644, 820));
                //g.DrawString("Overshoot Key", fk, Brushes.Black, new Point(980, 821));
                //g.DrawString("Response Rating Key", fk, Brushes.Black, new Point(1260, 819));
            }
             
            finalHeatmaps.Save(resultsFolderPath + "\\" + fileName);
            //Process.Start("explorer.exe", resultsFolderPath);
            BackColor = Color.White;
            heatmaps1.hideText(true);
        }

        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        public Bitmap ScaleImage(Bitmap source, float width, float height) // Depreciated, resized window instead!
        {
            float scale = Math.Min(width / source.Width, height / source.Height);
            var scaleWidth = (int)(source.Width * scale);
            var scaleHeight = (int)(source.Height * scale);
            var bitmap = new Bitmap(scaleWidth, scaleHeight);
            using (Graphics graph = Graphics.FromImage(bitmap))
            {
                var brush = new SolidBrush(Color.Transparent);
                graph.InterpolationMode = InterpolationMode.High;
                graph.CompositingQuality = CompositingQuality.HighQuality;
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                graph.FillRectangle(brush, new RectangleF(0, 0, 1920, 1080));
                graph.DrawImage(source, new Rectangle(0, 0, scaleWidth, scaleHeight));
                return bitmap;
            }
        }

        private void asPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = "OSRTT Heatmaps.png";
            string monitorInfo = "";
            
            if (resultsFolderPath != "")
            {
                string[] existingFiles = Directory.GetFiles(resultsFolderPath, "*.png");
                int fileNumber = 0;
                //search files for number
                foreach (var s in existingFiles)
                {
                    int num = int.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                    if (num >= fileNumber)
                    {
                        fileNumber = num + 1;
                    }
                }
                string[] folders = resultsFolderPath.Split('\\');
                monitorInfo = folders.Last();
                fileName = fileNumber.ToString() + "-" + monitorInfo + ".png";
            }
            else
            {
                resultsFolderPath = path;
            }
            Bitmap heatmaps = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(heatmaps, new Rectangle(0, 0, heatmaps.Width, heatmaps.Height));
            //heatmaps.MakeTransparent(BackColor);

            // crop sides
            // 80px top, 8px every other side
            // width - 16, height - 88, x = 8, y = 80
            var rect = new Rectangle(new Point(8, 80), new Size((this.Width - 16), (this.Height - 88)));
            //Bitmap scaledHeatmap = CropImage(heatmaps, rect);
            //Bitmap finalHeatmaps = ScaleImage(scaledHeatmap, 1920, 1080);
            Bitmap finalHeatmaps = CropImage(heatmaps, rect);
            finalHeatmaps.Save(resultsFolderPath + "\\" + fileName);
            //Process.Start("explorer.exe", resultsFolderPath);
        }

        private void perceivedResponseTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            perceivedResponseTimeToolStripMenuItem.Checked = true;
            initialResponseTimeToolStripMenuItem.Checked = false;
            completeResponseTimeToolStripMenuItem.Checked = false;
            rtViewMenuList.SelectedIndex = 0;
            if (stdResultsMenuBtn.Checked)
            {
                standardView();
            }
            else if (graphViewMenuBtn.Checked)
            {
                drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
                showProcessedData();
            }
        }

        private void initialResponseTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            perceivedResponseTimeToolStripMenuItem.Checked = false;
            initialResponseTimeToolStripMenuItem.Checked = true;
            completeResponseTimeToolStripMenuItem.Checked = false;
            rtViewMenuList.SelectedIndex = 1;
            if (stdResultsMenuBtn.Checked)
            {
                standardView();
            }
            else if (graphViewMenuBtn.Checked)
            {
                drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
                showProcessedData();
            }
        }

        private void completeResponseTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            perceivedResponseTimeToolStripMenuItem.Checked = false;
            initialResponseTimeToolStripMenuItem.Checked = false;
            completeResponseTimeToolStripMenuItem.Checked = true;
            rtViewMenuList.SelectedIndex = 2;
            if (stdResultsMenuBtn.Checked)
            {
                standardView();
            }
            else if (graphViewMenuBtn.Checked)
            {
                drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
                showProcessedData();
            }
        }

        private void processAllRuns(ProcessData.rtMethods rtMethod, ProcessData.osMethods osMethod, bool single)
        {
            averageData.Clear();
            multipleRunsData.Clear();
            ProcessData pd = new ProcessData();
            int startDelay = pd.processTestLatency(testLatency);
            List<List<ProcessData.processedResult>> processedData = new List<List<ProcessData.processedResult>>();
            processedData.AddRange(pd.ProcessAllResults(rawData, new ProcessData.resultSelection
            {
                rtStyle = rtMethod,
                osStyle = osMethod
            }, startDelay, processedGamma, runSettings));
            CFuncs cf = new CFuncs();
            
            foreach (var res in processedData)
            {
                StringBuilder csvString = new StringBuilder();
                string rtType = "Initial Response Time - 3% (ms)";
                string osType = "Overshoot";
                string osSign = "(%)";
                string perType = "Perceived Response Time - 3% (ms)";
                if (rtMethod.Tolerance == 10 && !rtMethod.gammaCorrected)
                {
                    rtType = "Initial Response Time - 10% (ms)";
                    perType = "Perceived Response Time - 10% (ms)";
                }
                else if (rtMethod.Tolerance == 10 && rtMethod.gammaCorrected)
                {
                    rtType = "Initial Response Time - RGB10 (ms)";
                    perType = "Perceived Response Time - RGB10 (ms)";
                }
                else if (rtMethod.Tolerance == 5 && rtMethod.gammaCorrected)
                {
                    rtType = "Initial Response Time - RGB5 (ms)";
                    perType = "Perceived Response Time - RGB5 (ms)";
                }
                if (osMethod.gammaCorrected)
                {
                    osSign = "(RGB)";
                }
                if (osMethod.gammaCorrected && (osMethod.endPercent || osMethod.rangePercent))
                {
                    osSign = "(RGB %)";
                }
                string fullFileName = CFuncs.createFileName(resultsFolderPath, "-FULL-OSRTT.csv");
                csvString.AppendLine("Starting RGB,End RGB,Complete Response Time (ms)," + rtType + "," + perType + "," + osType + " " + osSign + ",Visual Response Rating,Input Lag (ms)");
                bool failed = false;
                foreach (ProcessData.processedResult i in res)
                {
                    if (i != null)
                    {
                        // save each run to file
                        csvString.AppendLine(
                            i.StartingRGB.ToString() + "," +
                            i.EndRGB.ToString() + "," +
                            i.compTime.ToString() + "," +
                            i.initTime.ToString() + "," +
                            i.perTime.ToString() + "," +
                            i.Overshoot.ToString() + "," +
                            i.visualResponseRating.ToString() + "," +
                            i.inputLag.ToString()
                            );
                    }
                    else
                    {
                        failed = true;
                    }
                }
                if (failed)
                {
                    cf.showMessageBox("Failed to Process", "One or more of the results failed to process and has been left blank.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (runSettings != null)
                {
                    csvString.AppendLine(JsonConvert.SerializeObject(runSettings));
                }
                string fullFilePath = resultsFolderPath + "\\" + fullFileName;
                File.WriteAllText(fullFilePath, csvString.ToString());
                multipleRunsData.Add(res);
            }
            if (!single)
            {
                averageData.AddRange(pd.AverageMultipleRuns(processedData, osMethod));
                // save averaged data to file
                string[] folders = resultsFolderPath.Split('\\');
                string monitorInfo = folders.Last();
                StringBuilder avgCsvString = new StringBuilder();
                string avgRtType = "Initial Response Time - 3% (ms)";
                string avgOsType = "Overshoot";
                string avgOsSign = "(%)";
                string avgPerType = "Perceived Response Time - 3% (ms)";
                if (rtMethod.Tolerance == 10 && !rtMethod.gammaCorrected)
                {
                    avgRtType = "Initial Response Time - 10% (ms)";
                    avgPerType = "Perceived Response Time - 10% (ms)";
                }
                else if (rtMethod.Tolerance == 10 && rtMethod.gammaCorrected)
                {
                    avgRtType = "Initial Response Time - RGB10 (ms)";
                    avgPerType = "Perceived Response Time - RGB10 (ms)";
                }
                else if (rtMethod.Tolerance == 5 && rtMethod.gammaCorrected)
                {
                    avgRtType = "Initial Response Time - RGB5 (ms)";
                    avgPerType = "Perceived Response Time - RGB5 (ms)";
                }
                if (osMethod.gammaCorrected)
                {
                    avgOsSign = "(RGB)";
                }
                if (osMethod.gammaCorrected && (osMethod.endPercent || osMethod.rangePercent))
                {
                    avgOsSign = "(RGB %)";
                }
                string fileName = monitorInfo + ".csv";
                avgCsvString.AppendLine("Starting RGB,End RGB,Complete Response Time (ms)," + avgRtType + "," + avgPerType + "," + avgOsType + " " + avgOsSign + ",Visual Response Rating,Input Lag (ms)");
                foreach (ProcessData.processedResult i in averageData)
                {
                    // save each run to file
                    avgCsvString.AppendLine(
                        i.StartingRGB.ToString() + "," +
                        i.EndRGB.ToString() + "," +
                        i.compTime.ToString() + "," +
                        i.initTime.ToString() + "," +
                        i.perTime.ToString() + "," +
                        i.Overshoot.ToString() + "," +
                        i.visualResponseRating.ToString() + "," +
                        i.inputLag.ToString()
                        );
                }
                if (runSettings != null)
                {
                    avgCsvString.AppendLine(JsonConvert.SerializeObject(runSettings));
                }
                string filePath = resultsFolderPath + "\\" + fileName;
                File.WriteAllText(filePath, avgCsvString.ToString());
                if (Properties.Settings.Default.saveXLSX)
                {
                    string[] headers = { "Starting RGB", "End RGB", "Complete Response Time (ms)", avgRtType, avgPerType, avgOsType + " " + avgOsSign, "Visual Response Rating", "Input Lag (ms)" };
                    SaveToExcel excel = new SaveToExcel();
                    excel.SaveDataToHeatmap(averageData, runSettings, path, resultsFolderPath + "\\" + monitorInfo + ".XLSX", headers);
                }
            }
        }

        private void importRawFileBtn_Click(object sender, EventArgs e)
        {
            bool success;
            try
            {
                importRawData();
                if (rawData.Count() == 0)
                {
                    success = false;
                    throw new Exception();
                }
                success = true;
                setProgressBar(true);
            }
            catch (Exception er)
            {
                success = false;
                Console.WriteLine(er.Message + er.StackTrace);
            }
            if (success)
            {
                ProcessData.rtMethods rt = new ProcessData.rtMethods
                {
                    Name = Properties.Settings.Default.rtName,
                    Tolerance = Properties.Settings.Default.rtTolerance,
                    gammaCorrected = Properties.Settings.Default.rtGammaCorrected,
                    percentage = Properties.Settings.Default.rtPercentage
                };
                ProcessData.osMethods os = new ProcessData.osMethods
                {
                    Name = Properties.Settings.Default.osName,
                    gammaCorrected = Properties.Settings.Default.osGammaCorrected,
                    endPercent = Properties.Settings.Default.osEndPercent,
                    rangePercent = Properties.Settings.Default.osRangePercent
                };
                if (runSettings == null)
                {
                    runSettings = new ProcessData.runSettings       // REMOVE THIS 
                    {
                        RunName = "001-DISPLAY-165-DP",
                        RefreshRate = 165,
                        FPSLimit = 1000,
                        DateAndTime = DateTime.Now.ToString(),
                        MonitorName = "DISPLAY",
                        Vsync = true,
                        osMethod = os,
                        rtMethod = rt
                    };
                }
                else
                {
                    runSettings.rtMethod = rt;
                    runSettings.osMethod = os;
                }
                if (success)
                {
                    try
                    {
                        processAllRuns(rt, os, true);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        MessageBox.Show(ex.Message + ex.StackTrace, "Error importing files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ex.Message + ex.StackTrace);
                    }
                }
                if (success)
                {
                    handleRunsList();
                    handleResultsList(runSelectBox.SelectedIndex);
                    stdResultsMenuBtn_Click(null, null);
                    Process.Start("explorer.exe", resultsFolderPath);
                }
                setProgressBar(false);
            }
        }

        private void importRawFolderBtn_Click(object sender, EventArgs e)
        {
            bool success = false;
            try
            {
                importRawFolderData();
                if (rawData.Count() == 0)
                {
                    success = false;
                    throw new Exception();
                }
                success = true;
                setProgressBar(true);
            }
            catch (Exception er)
            {
                success = false;
                Console.WriteLine(er.Message + er.StackTrace);
            }
            if (success)
            {
                ProcessData.rtMethods rt = new ProcessData.rtMethods
                {
                    Name = Properties.Settings.Default.rtName,
                    Tolerance = Properties.Settings.Default.rtTolerance,
                    gammaCorrected = Properties.Settings.Default.rtGammaCorrected,
                    percentage = Properties.Settings.Default.rtPercentage
                };
                ProcessData.osMethods os = new ProcessData.osMethods
                {
                    Name = Properties.Settings.Default.osName,
                    gammaCorrected = Properties.Settings.Default.osGammaCorrected,
                    endPercent = Properties.Settings.Default.osEndPercent,
                    rangePercent = Properties.Settings.Default.osRangePercent
                };
                if (runSettings == null)
                {
                    runSettings = new ProcessData.runSettings       // REMOVE THIS 
                    {
                        RunName = "001-DISPLAY-165-DP",
                        RefreshRate = 165,
                        FPSLimit = 1000,
                        DateAndTime = DateTime.Now.ToString(),
                        MonitorName = "DISPLAY",
                        Vsync = true,
                        osMethod = os,
                        rtMethod = rt
                    };
                }
                else
                {
                    runSettings.rtMethod = rt;
                    runSettings.osMethod = os;
                }
                if (success)
                {
                    try
                    {
                        processAllRuns(rt, os, false);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        MessageBox.Show(ex.Message + ex.StackTrace, "Error importing files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine(ex.Message + ex.StackTrace);
                    }
                }
                if (success)
                {
                    handleRunsList();
                    handleResultsList(runSelectBox.SelectedIndex);
                    stdResultsMenuBtn_Click(null, null);
                    Process.Start("explorer.exe", resultsFolderPath);
                }
                setProgressBar(false);
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int resSet = -1;
            FormCollection fc = Application.OpenForms;
            for (int i = 0; i < fc.Count; i++)
            {
                if (fc[i].Name == "ResultsSettings")
                {
                    resSet = i;
                }
            }
            if (resSet != -1)
            {
                fc[resSet].Close();
            }
            ResultsSettings rs = new ResultsSettings();
            rs.Show();
        }

        private void runSelectToolStrip_SelectedIndexChanged(object sender, EventArgs e)
        {        // draw different run
            var ctrl = sender as ToolStripComboBox;
            if (ctrl.Focused)
            {
                if (runSelectToolStrip.Items[0].ToString().Contains("Average"))
                {
                    if (runSelectToolStrip.SelectedIndex == 0)
                    {
                        heatmaps1.setAverageData(averageData);
                    }
                    else
                    {
                        int sel = runSelectToolStrip.SelectedIndex - 1;
                        heatmaps1.setAverageData(multipleRunsData[sel]);
                    }
                }
                else
                {
                    heatmaps1.setAverageData(multipleRunsData[runSelectToolStrip.SelectedIndex]);
                }
                heatmaps1.standardView();
            }
        }

        private void setProgressBar(bool on)
        {
            if (on)
            {
                
                if (progressBar1.InvokeRequired)
                {
                    this.progressBar1.Invoke((MethodInvoker)(() => { 
                        this.progressBar1.Style = ProgressBarStyle.Marquee; 
                        this.progressBar1.MarqueeAnimationSpeed = 30;
                        this.progressBar1.Visible = true;
                    }));
                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    progressBar1.MarqueeAnimationSpeed = 30;
                    this.progressBar1.Visible = true;
                }
            }
            else
            {
                if (progressBar1.InvokeRequired)
                {
                    this.progressBar1.Invoke((MethodInvoker)(() => { 
                        this.progressBar1.Style = ProgressBarStyle.Continuous; 
                        this.progressBar1.MarqueeAnimationSpeed = 0;
                        this.progressBar1.Visible = false;
                    }));
                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Continuous;
                    progressBar1.MarqueeAnimationSpeed = 0;
                    progressBar1.Visible = false;
                }
            }
        }

        private void deNoisedRawDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.smoothGraph = deNoisedRawDataToolStripMenuItem.Checked;
            denoiseToolStripBtn.Checked = deNoisedRawDataToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();

            try
            {
                drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
                showProcessedData();
            }
            catch (Exception ex)
            {
                CFuncs cf = new CFuncs();
                cf.showMessageBox(ex.Message + ex.StackTrace, "Error Drawing graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void viewGammaBtn_Click(object sender, EventArgs e)
        {
            if (processedGamma.Count != 0)
            {
                GammaView gv = new GammaView();
                gv.setGammaData(processedGamma);
                gv.Show();
            }
            else
            {
                MessageBox.Show("No Gamma Data!");
            }    
        }

        private void rtViewMenuList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ToolStripComboBox;
            if (ctrl.Focused)
            {
                if (rtViewMenuList.SelectedItem.ToString().Contains("Perceived"))
                {
                    perceivedResponseTimeToolStripMenuItem.Checked = true;
                    initialResponseTimeToolStripMenuItem.Checked = false;
                    completeResponseTimeToolStripMenuItem.Checked = false;
                    drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
                    showProcessedData();
                }
                else if (rtViewMenuList.SelectedItem.ToString().Contains("Initial"))
                {
                    perceivedResponseTimeToolStripMenuItem.Checked = false;
                    initialResponseTimeToolStripMenuItem.Checked = true;
                    completeResponseTimeToolStripMenuItem.Checked = false;
                    drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
                    showProcessedData();
                }
                else if (rtViewMenuList.SelectedItem.ToString().Contains("Complete"))
                {
                    perceivedResponseTimeToolStripMenuItem.Checked = false;
                    initialResponseTimeToolStripMenuItem.Checked = false;
                    completeResponseTimeToolStripMenuItem.Checked = true;
                    drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
                    showProcessedData();
                }
            }
        }

        private void denoiseToolStripBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.smoothGraph = denoiseToolStripBtn.Checked;
            deNoisedRawDataToolStripMenuItem.Checked = denoiseToolStripBtn.Checked;
            Properties.Settings.Default.Save();

            try
            {
                drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
                showProcessedData();
            }
            catch (Exception ex)
            {
                CFuncs cf = new CFuncs();
                cf.showMessageBox(ex.Message + ex.StackTrace, "Error Drawing graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessData pd = new ProcessData();
            ProcessData.rawResultData test = pd.InterpolateClippedGraph(rawData[runSelectBox.SelectedIndex][transSelect1.SelectedIndex]);
            rawData[runSelectBox.SelectedIndex][transSelect1.SelectedIndex] = test;
            drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
        }

        private void processRawInputLagBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var rawData = importRawInputLagData();
                if (rawData.Count != 0)
                {
                    var data = ProcessData.AverageInputLagResults(rawData);
                    // Write results to csv using new name
                    decimal fileNumber = 001;
                    // search /Results folder for existing file names, pick new name
                    string[] existingFiles = Directory.GetFiles(resultsFolderPath, "*-INPUT-LATENCY-OSRTT.csv");
                    // Search \Results folder for existing results to not overwrite existing or have save conflict errors
                    foreach (var s in existingFiles)
                    {
                        decimal num = 0;
                        Console.WriteLine(Path.GetFileNameWithoutExtension(s).Remove(3));
                        try
                        { num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3)); }
                        catch
                        { Console.WriteLine("Non-standard file name found"); }
                        if (num >= fileNumber)
                        {
                            fileNumber = num + 1;
                        }
                    }
                    string[] folders = resultsFolderPath.Split('\\');
                    string monitorInfo = folders.Last();
                    string filePath = resultsFolderPath + "\\" + monitorInfo + "-INPUT-LATENCY-OSRTT.csv";
                    //string filePath = resultsFolderPath + "\\" + fileNumber.ToString("000") + "-INPUT-LAG-OSRTT.csv";

                    string strSeparator = ",";
                    StringBuilder csvString = new StringBuilder();
                    csvString.AppendLine("Shot Number,Click Time (ms),Processing Latency (ms),Display Latency(ms),Total System Input Lag (ms)");

                    foreach (var res in data.inputLagResults)
                    {
                        csvString.AppendLine(
                            res.shotNumber.ToString() + "," +
                            res.clickTimeMs.ToString() + "," +
                            res.frameTimeMs.ToString() + "," +
                            res.onDisplayLatency.ToString() + "," +
                            res.totalInputLag.ToString()
                            );
                    }
                    csvString.AppendLine("AVERAGE," + data.ClickTime.AVG.ToString() + "," + data.FrameTime.AVG.ToString() + "," + data.onDisplayLatency.AVG.ToString() + "," + data.totalInputLag.AVG.ToString());
                    csvString.AppendLine("MINIMUM," + data.ClickTime.MIN.ToString() + "," + data.FrameTime.MIN.ToString() + "," + data.onDisplayLatency.MIN.ToString() + "," + data.totalInputLag.MIN.ToString());
                    csvString.AppendLine("MAXIMUM," + data.ClickTime.MAX.ToString() + "," + data.FrameTime.MAX.ToString() + "," + data.onDisplayLatency.MAX.ToString() + "," + data.totalInputLag.MAX.ToString());
                    Console.WriteLine(filePath);
                    File.WriteAllText(filePath, csvString.ToString());
                    inputLagMode(data);
                }
                else
                {
                    MessageBox.Show("Failed to import data", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }

        private void importProcILBtn_Click_1(object sender, EventArgs e)
        {
            var data = importInputLagData();
            if (data.inputLagResults.Count != 0)
            {
                inputLagMode(data);
            }
            else
            {
                MessageBox.Show("Failed to import data", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void inputLagMode(ProcessData.averagedInputLag data)
        {
            inputLagProcRV1.inputLagResults = data;
            inputLagProcRV1.resultsFolderPath = resultsFolderPath;
            Size = new Size(1257, 830);
            inputLagProcRV1.Location = new Point(5, 52);
            inputLagProcRV1.Visible = true;
            importPanel.Location = new Point(1500, 52);
            importPanel.Visible = false;
            graphViewPanel.Location = new Point(1439, 762);
            graphViewPanel.Visible = false;
            heatmaps1.Location = new Point(1395, 55);
            heatmaps1.Visible = false;
            saveHeatmapsToolStripMenuItem.Visible = false;
            stdResultsMenuBtn.Checked = false;
            graphViewMenuBtn.Checked = false;
            importViewMenuButton.Checked = true;
            viewToolStripMenuItem.Visible = false;
            runSelectToolStrip.Visible = false;
            inputLagProcRV1.drawBarGraph();
        }

        private ProcessData.averagedInputLag importInputLagData()
        {
            // Open file picker dialogue
            var filePath = string.Empty;
            ProcessData.averagedInputLag averagedInputLag = new ProcessData.averagedInputLag();
            averagedInputLag.inputLagResults = new List<ProcessData.inputLagResult>();
            averagedInputLag.ClickTime = new ProcessData.averageInputLagResult();
            averagedInputLag.FrameTime = new ProcessData.averageInputLagResult();
            averagedInputLag.onDisplayLatency = new ProcessData.averageInputLagResult();
            averagedInputLag.totalInputLag = new ProcessData.averageInputLagResult();
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    if (filePath.Contains("LATENCY-OSRTT"))
                    {
                        //Read the contents of the file into a stream
                        try
                        {
                            var fileStream = openFileDialog.OpenFile();
                            using (StreamReader reader = new StreamReader(fileStream))
                            {
                                while (!reader.EndOfStream)
                                {
                                    // This can probably be done better
                                    string fullLine = reader.ReadLine();
                                    if (fullLine.Contains("{"))
                                    {
                                        ProcessData.runSettings runs = JsonConvert.DeserializeObject<ProcessData.runSettings>(fullLine);
                                        runSettings = runs;
                                        continue;
                                    }
                                    else if (fullLine.Contains("Shot"))
                                    {
                                        //skip headers
                                    }
                                    else
                                    {
                                        string[] line = fullLine.Split(',');
                                        double[] intLine = new double[line.Length];
                                        string azpattern = "[a-z]+";
                                        if (line[0].Contains("A") || line[0].Contains("M"))
                                        {
                                            for (int i = 1; i < line.Length; i++)
                                            {
                                                if (line[i] == "0")
                                                {
                                                    intLine[i] = 0;
                                                }
                                                else if (line[i] != "")
                                                {
                                                    intLine[i] = double.Parse(line[i]);
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                            if (line[0].Contains("AV"))
                                            {
                                                averagedInputLag.ClickTime.AVG = intLine[1];
                                                averagedInputLag.FrameTime.AVG = intLine[2];
                                                averagedInputLag.onDisplayLatency.AVG = intLine[3];
                                                averagedInputLag.totalInputLag.AVG = intLine[4];
                                            }
                                            else if (line[0].Contains("MIN"))
                                            {
                                                averagedInputLag.ClickTime.MIN = intLine[1];
                                                averagedInputLag.FrameTime.MIN = intLine[2];
                                                averagedInputLag.onDisplayLatency.MIN = intLine[3];
                                                averagedInputLag.totalInputLag.MIN = intLine[4];
                                            }
                                            else if (line[0].Contains("MAX"))
                                            {
                                                averagedInputLag.ClickTime.MAX = intLine[1];
                                                averagedInputLag.FrameTime.MAX = intLine[2];
                                                averagedInputLag.onDisplayLatency.MAX = intLine[3];
                                                averagedInputLag.totalInputLag.MAX = intLine[4];
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 0; i < line.Length; i++)
                                            {
                                                if (line[i] == "0")
                                                {
                                                    intLine[i] = 0;
                                                }
                                                else if (line[i] != "")
                                                {
                                                    intLine[i] = double.Parse(line[i]);
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                            //Array.Resize(ref intLine, intLine.Length - 1);
                                            ProcessData.inputLagResult rawResult = new ProcessData.inputLagResult
                                            {
                                               shotNumber = Convert.ToInt32(intLine[0]),
                                               clickTimeMs = intLine[1],
                                               frameTimeMs = intLine[2],
                                               onDisplayLatency = intLine[3],
                                               totalInputLag = intLine[4]
                                            };
                                            averagedInputLag.inputLagResults.Add(rawResult);
                                        }
                                    }
                                }
                            }
                            resultsFolderPath = filePath.Substring(0, filePath.LastIndexOf('\\'));
                        }
                        catch (Exception ex)
                        {
                            DialogResult d = MessageBox.Show("File may be in use by another program, please make sure it's not open elsewhere and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Console.WriteLine(ex.Message + ex.StackTrace);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, only 'RAW' files can be imported. Please select a 'LATENCY-OSRTT.csv' file instead.", "Importer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception("Importer Error");
                    }
                }
            }
            return averagedInputLag;
        }

        private List<ProcessData.rawInputLagResult> importRawInputLagData()
        {
            // Open file picker dialogue
            var filePath = string.Empty;
            List<ProcessData.rawInputLagResult> rawILData = new List<ProcessData.rawInputLagResult>();
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    if (filePath.Contains("LATENCY-RAW"))
                    {
                        //Read the contents of the file into a stream
                        try
                        {
                            var fileStream = openFileDialog.OpenFile();
                            using (StreamReader reader = new StreamReader(fileStream))
                            {
                                while (!reader.EndOfStream)
                                {
                                    // This can probably be done better
                                    string fullLine = reader.ReadLine();
                                    if (fullLine.Contains("{"))
                                    {
                                        ProcessData.runSettings runs = JsonConvert.DeserializeObject<ProcessData.runSettings>(fullLine);
                                        runSettings = runs;
                                        continue;
                                    }
                                    else
                                    {
                                        string[] line = fullLine.Split(',');
                                        int[] intLine = new int[line.Length];
                                        float frameTime = 0;
                                        for (int i = 0; i < line.Length; i++)
                                        {
                                            if (line[i] == "0")
                                            {
                                                intLine[i] = 0;
                                            }
                                            else if (line[i].Contains("."))
                                            {
                                                frameTime = float.Parse(line[i]);
                                            }
                                            else if (line[i] != "")
                                            {
                                                intLine[i] = int.Parse(line[i]);
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                        Array.Resize(ref intLine, intLine.Length - 1);
                                        List<int> samples = intLine.ToList();
                                        samples.RemoveRange(0, 4);
                                        ProcessData.rawInputLagResult rawResult = new ProcessData.rawInputLagResult
                                        {
                                            ClickTime = intLine[0],
                                            FrameTime = frameTime,
                                            TimeTaken = intLine[2],
                                            SampleCount = intLine[3],
                                            Samples = samples
                                        };
                                        rawILData.Add(rawResult);
                                    }
                                }
                            }
                            resultsFolderPath = filePath.Substring(0, filePath.LastIndexOf('\\'));
                        }
                        catch (Exception ex)
                        {
                            DialogResult d = MessageBox.Show("File may be in use by another program, please make sure it's not open elsewhere and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Console.WriteLine(ex.Message + ex.StackTrace);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, only 'RAW' files can be imported. Please select a 'LATENCY-OSRTT.csv' file instead.", "Importer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //throw new Exception("Importer Error");
                    }
                }
            }
            return rawILData;
        }

    }
}
