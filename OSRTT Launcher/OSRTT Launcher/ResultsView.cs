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
using BrightIdeasSoftware;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Resources;

namespace OSRTT_Launcher
{
    // TODO List //
    // Graph View
    //  - Make response time options work
    //  - Fix overshoot gamma corrected percentage options
    //  - Save graph as PNG
    // Results view
    //  - Style headers
    //  - Style/colour cells based on type/key (user changable setting?)
    // Handle multiple runs (dropdown of Averaged + run 1 + run 2 ...)
    // Save graph as (optionally transparent) image
    // Save results view as (optionally transparent) image - full page minus headers.
    // Make import functions generic (pass expected file name in, return object back)

    public partial class ResultsView : Form
    {
        string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        // Raw sensor data, list of lists to hold multiple runs worth of results at once
        public List<List<int[]>> rawData = new List<List<int[]>>();
        // Moving average smoothed sensor data, list of lists to hold multiple runs worth of results at once
        public List<List<int[]>> smoothedData = new List<List<int[]>>();
        // Processed results, list of lists to hold multiple runs worth of results at once
        public List<List<int[]>> multipleRunsData = new List<List<int[]>>();
        // Single averaged results list from multiple run data
        public List<double[]> averageData = new List<double[]>();
        // Raw gamma sensor data
        public List<int[]> gamma = new List<int[]>();
        // Processed gamma results (list of 256 RGB & correlating light values
        public List<int[]> processedGamma = new List<int[]>();
        // Single 0-255 transition to gauge distance to start point (initial latency)
        public List<int> testLatency = new List<int>();
        // RGB array to use
        public List<int> RGBArr = new List<int> { 0, 51, 102, 153, 204, 255 };
        // Noise level per transition/RGB level, used for smoothing and processing.
        public List<int[]> noiseLevel = new List<int[]>();
        // How many samples to build the initial min/max from
        public int startDelay = 150;

        private class rtMethods
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Tolerance { get; set; }
            public bool gammaCorrected { get; set; }
            public bool percentage { get; set; }
        }

        private List<rtMethods> rtMethodologies = new List<rtMethods>
        {
            new rtMethods { Type = "RGB5", Name = "RGB 5 Tolerance", Tolerance = 5, gammaCorrected = true, percentage = false },
            new rtMethods { Type = "RGB10", Name = "RGB 10 Tolerance", Tolerance = 10, gammaCorrected = true, percentage = false },
            new rtMethods { Type = "3PerGamCor", Name = "3% of RGB Tolerance", Tolerance = 3, gammaCorrected = true, percentage = true },
            new rtMethods { Type = "10PerGamCor", Name = "10% of RGB Tolerance", Tolerance = 10, gammaCorrected = true, percentage = true },
            new rtMethods { Type = "3Per", Name = "3% of Light Level Tolerance", Tolerance = 3, gammaCorrected = false, percentage = true},
            new rtMethods { Type = "10Per", Name = "10% of Light Level Tolerance", Tolerance = 10, gammaCorrected = false, percentage = true }
        };
        private class osMethods
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public bool endPercent { get; set; }
            public bool rangePercent { get; set; }
            public bool gammaCorrected { get; set; }
        }
        private List<osMethods> osMethodologies = new List<osMethods>
        {
            new osMethods { Type = "GamCor", Name = "RGB Values", endPercent = false, rangePercent = false, gammaCorrected = true },
            new osMethods { Type = "GamCorrEndPer", Name = "Percent over end RGB Value", endPercent = true, rangePercent = false, gammaCorrected = true },
            new osMethods { Type = "GamCorrRangePer", Name = "Percent above RGB range", endPercent = false, rangePercent = true, gammaCorrected = true },
            new osMethods { Type = "EndPer", Name = "Percent over end light level", endPercent = true, rangePercent = false, gammaCorrected = false },
            new osMethods { Type = "RangePer", Name = "Percent over light level range", endPercent = false, rangePercent = true, gammaCorrected = false }
        };
        private class resultSelection
        {
            public int arrayIndex { get; set; }
            public int resultIndex { get; set; }
            public rtMethods rtStyle { get; set; }
            public osMethods osStyle { get; set; }
        }
        private resultSelection selectedResult;
        public class processedResult
        {
            public double Time { get; set; }
            public int startIndex { get; set; }
            public int endIndex { get; set; }
            public double Overshoot { get; set; }
        }
        public class colourKey
        {
            public int keyType { get; set; }
            public double min { get; set; }
            public double middle { get; set; }
            public double max { get; set; }
        }
        List<colourKey> colourKeys = new List<colourKey>();

        private ResourceManager rm = OSRTT_Launcher.Properties.Resources.ResourceManager;

        public void setRawData (List<List<int[]>> rd)
        {
            if (rd != null)
            {
                rawData.AddRange(rd);
            }
        }

        public void setMultiRunData(List<List<int[]>> mrd)
        {
            if (mrd != null)
            {
                multipleRunsData.AddRange(mrd);
            }
        }

        public void setAverageData(List<double[]> ad)
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

        public ResultsView()
        {
            InitializeComponent();
            this.Icon = (Icon)rm.GetObject("osrttIcon");
            path = new Uri(System.IO.Path.GetDirectoryName(path)).LocalPath;
            path += @"\Results";
            Size = new Size(1400, 800);
            initResultsMethodList();
            initOvershootStyleList();
            string[] rtKey = Properties.Settings.Default.rtKey.Split(',');
            if (rtKey.Length == 3)
            {
                colourKeys.Add( new colourKey { keyType = 0, min = double.Parse(rtKey[0]), middle = double.Parse(rtKey[1]), max = double.Parse(rtKey[2]) });
            }
            string[] osKey = Properties.Settings.Default.osKey.Split(',');
            if (osKey.Length == 3)
            {
                colourKeys.Add(new colourKey { keyType = 1, min = double.Parse(osKey[0]), middle = double.Parse(osKey[1]), max = double.Parse(osKey[2]) });
            }
            string[] vrrKey = Properties.Settings.Default.vrrKey.Split(',');
            if (vrrKey.Length == 3)
            {
                colourKeys.Add(new colourKey { keyType = 2, min = double.Parse(vrrKey[0]), middle = double.Parse(vrrKey[1]), max = double.Parse(vrrKey[2]) });
            }
        }

        private void initResultsMethodList()
        {
            processTypeListBox.Items.Clear();
            foreach(var item in rtMethodologies)
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

        private void ResultsView_Load(object sender, EventArgs e)
        {
            if (rawData.Count == 0 && averageData.Count == 0)
            {
                graphViewMenuBtn.Visible = false;
                // Import View
            }
            if (multipleRunsData.Count == 0)
            {
                allResultsMenuBtn.Visible = false;
                stdResultsMenuBtn.Visible = true;
                // Import View
            }

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
                    if (filePath.Contains("RAW-OSRTT"))
                    {
                        //Read the contents of the file into a stream
                        try
                        {
                            List<int[]> tempRes = new List<int[]>();
                            List<int[]> tempGamma = new List<int[]>();
                            var fileStream = openFileDialog.OpenFile();
                            using (StreamReader reader = new StreamReader(fileStream))
                            {
                                while (!reader.EndOfStream)
                                {
                                    // This can probably be done better
                                    string[] line = reader.ReadLine().Split(',');
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
                                        tempRes.Add(intLine);
                                    }
                                }
                            }
                            rawData.AddRange(new List<List<int[]>> { tempRes });
                            gamma.AddRange(tempGamma);
                            processedGamma.AddRange(processGammaTable());
                            processTestLatency();
                            Console.WriteLine(rawData.Count);
                            // Draw graph
                            graphViewMenuBtn_Click(null, null);
                            handleRunsList();
                            handleResultsList(runSelectBox.SelectedIndex);
                        }
                        catch
                        {
                            DialogResult d = MessageBox.Show("File may be in use by another program, please make sure it's not open elsewhere and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, only 'RAW' files can be imported. Please select a 'RAW-OSRTT.csv' file instead.", "Importer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void drawTable(DataGridView dgv, string type)
        {
            //      0           1       2        3         4          5       6      7
            // StartingRGB, EndRGB, Complete, Initial, Perceived, Overshoot, VRR, Latency
            int indexToUse = 4;
            switch (type)
            {
                case "perceived":
                    indexToUse = 4;
                    break;
                case "initial":
                    indexToUse = 3;
                    break;
                case "complete":
                    indexToUse = 2;
                    break;
                case "overshoot":
                    indexToUse = 5;
                    break;
                case "vrr":
                    indexToUse = 6;
                    break;
                default:
                    indexToUse = 4;
                    break;
            }
            // Create string arrays for rows
            List<string[]> data = new List<string[]>();
            for (int i = 0; i < RGBArr.Count; i++)
            {
                // Fill list with sized empty string arrays to address later
                string[] line = new string[6];
                data.Add(line);
            }
            int current = 0;
            int next = 1;
            for (int k = 0; k < averageData.Count; k++)
            {
                if (next < RGBArr.Count)
                {
                    if (averageData[k][0] == RGBArr[current])
                    {
                        // current base
                        data[current][next] = averageData[k][indexToUse].ToString();
                    }
                    else 
                    {
                        // next 
                        data[next][current] = averageData[k][indexToUse].ToString();
                        next++;
                    }
                }
                else
                {
                    current++;
                    next = current + 1;
                    if (averageData[k][0] == RGBArr[current])
                    {
                        // current base
                        data[current][next] = averageData[k][indexToUse].ToString();
                    }
                    else
                    {
                        // next 
                        data[next][current] = averageData[k][indexToUse].ToString();
                        next++;
                    }
                }
                Console.WriteLine("Current: " + current + ", Next: " + next + ", StartingRGB: " + averageData[k][0] + ", EndRGB: " + averageData[k][1]);
            }
            // Add string array to rows
            foreach (var item in data)
            {
                dgv.Rows.Add(item);
            }
            int colourKeyIndex = 0;
            if (indexToUse == 5)
            {
                colourKeyIndex = 1;
            }
            else if (indexToUse == 6)
            {
                colourKeyIndex = 2;
            }
            for (int l = 0; l < dgv.Rows.Count; l++)
            {
                dgv.Rows[l].HeaderCell.Value = RGBArr[l].ToString();
                for (int p = 0; p < dgv.Columns.Count; p++)
                {
                    if (dgv.Rows[l].Cells[p].Value != null)
                    {
                        // ARGB (153, 0, 255, 0) Green -> (153, 255, 255, 0) Yellow -> (153,255,0,0) Red
                        double cellValue = double.Parse(dgv.Rows[l].Cells[p].Value.ToString());
                        Console.WriteLine(cellValue);
                        if (cellValue < colourKeys[colourKeyIndex].middle)
                        {
                            if (cellValue < colourKeys[colourKeyIndex].min)
                            {
                                // set min colour
                                DataGridViewCellStyle dv = dgv.Rows[l].Cells[p].InheritedStyle.Clone();
                                dv.BackColor = Color.FromArgb(153, 0, 255, 0);
                                dgv.Rows[l].Cells[p].Style.ApplyStyle(dv);
                            }
                            else
                            {
                                // divide time (minus min) by range (average - min)
                                // times 255 by result of above
                                double range = colourKeys[colourKeyIndex].middle - colourKeys[colourKeyIndex].min;
                                double time = cellValue - colourKeys[colourKeyIndex].min;
                                double result = time / range;
                                double greenRGB = 255 * result;
                                greenRGB = Math.Round(greenRGB, 0);
                                DataGridViewCellStyle dv = dgv.Rows[l].Cells[p].InheritedStyle.Clone();
                                dv.BackColor = Color.FromArgb(153, 0, Convert.ToInt32(greenRGB), 0);
                                dgv.Rows[l].Cells[p].Style.ApplyStyle(dv);
                            }
                        }
                        else
                        {
                            if (cellValue > colourKeys[colourKeyIndex].max)
                            {
                                // set max colour
                                DataGridViewCellStyle dv = dgv.Rows[l].Cells[p].InheritedStyle.Clone();
                                dv.BackColor = Color.FromArgb(153, 255, 0, 0); 
                                dgv.Rows[l].Cells[p].Style.ApplyStyle(dv);
                            }
                            else
                            {
                                // divide time (minus max) by range (max - average)
                                // times 255 by result of above
                                double range = colourKeys[colourKeyIndex].max - colourKeys[colourKeyIndex].middle;
                                double time = colourKeys[colourKeyIndex].max - cellValue;
                                double result = time / range;
                                double redRGB = 255 * result;
                                redRGB = Math.Round(redRGB, 0);
                                dgv.Rows[l].Cells[p].Style.BackColor = Color.FromArgb(153, Convert.ToInt32(redRGB), 0, 0);
                                DataGridViewCellStyle dv = dgv.Rows[l].Cells[p].InheritedStyle.Clone();
                                dv.BackColor = Color.FromArgb(153, Convert.ToInt32(redRGB), 0, 0);
                                dgv.Rows[l].Cells[p].Style.ApplyStyle(dv);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cell value null");
                        dgv.Rows[l].Cells[p].Style.BackColor = Color.Gray;
                    }
                }
            }
            //SubItems.AddRange(data.ToArray());
            // Style cells? Probs not here.
            // You can set the ListViewItem.BackColor property
        }

        private void initGridViewColumns(DataGridView dgv)
        {
            if (dgv.Columns.Count != 0)
            {
                dgv.Columns.Clear();
            }
            if (dgv.Rows.Count != 0)
            {
                dgv.Rows.Clear();
            }
            dgv.SelectionChanged += gridView_SelectionChanged;
            dgv.ColumnCount = 6;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(84, 157, 178, 189);
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(84, 157, 178, 189);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 14, FontStyle.Bold);
            dgv.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 14, FontStyle.Bold);
            dgv.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedRowHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.RowsDefaultCellStyle.Font = new Font("Calibri", 14);
            dgv.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.RowsDefaultCellStyle.ForeColor = Color.White;

            dgv.CellFormatting += dgv_CellFormatting;
            //dgv.RowsDefaultCellStyle.BackColor = Color.Gray;
            // rtGridView.RowHeadersDefaultCellStyle.Padding = new Padding(rtGridView.RowHeadersWidth / 2 );
            for (int i = 0; i < RGBArr.Count; i++)
            {
                dgv.Columns[i].Name = RGBArr[i].ToString();
            }

            for (int k = 0; k < dgv.Columns.Count; k++)
            {
                dgv.Columns[k].Width = 55;
            }
        }


        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Compare the value of second Column i.e. Column with Index 1.
            DataGridView dgv = sender as DataGridView;
            Console.WriteLine("Name: "+ dgv.Name);
            if (dgv.Name.Contains("rt") && e.Value != null)
            {
                //Fetch the value of the second Column.
                double quantity = Convert.ToDouble(e.Value);

                //Apply Background color based on value.
                if (quantity == 0)
                {
                    e.CellStyle.BackColor = Color.Red;
                }
                if (quantity > 0 && quantity <= 50)
                {
                    e.CellStyle.BackColor = Color.Yellow;
                }
                if (quantity > 50 && quantity <= 100)
                {
                    e.CellStyle.BackColor = Color.Orange;
                }
            }
        }


        private void initStatsGridViewColumns(DataGridView dgv)
        {
            if (dgv.Columns.Count != 0)
            {
                dgv.Columns.Clear();
            }
            if (dgv.Rows.Count != 0)
            {
                dgv.Rows.Clear();
            }
            dgv.SelectionChanged += gridView_SelectionChanged;
            dgv.ColumnCount = 2;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.ColumnHeadersVisible = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(84, 157, 178, 189);
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(84, 157, 178, 189);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 14, FontStyle.Bold);
            dgv.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 14, FontStyle.Bold);
            dgv.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedRowHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.RowsDefaultCellStyle.Font = new Font("Calibri", 14);
            dgv.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.RowsDefaultCellStyle.ForeColor = Color.White;
            //dgv.RowsDefaultCellStyle.BackColor = Color.Gray;
            // rtGridView.RowHeadersDefaultCellStyle.Padding = new Padding(rtGridView.RowHeadersWidth / 2 );
            for (int i = 0; i < RGBArr.Count; i++)
            {
                dgv.Columns[i].Name = RGBArr[i].ToString();
            }

            for (int k = 0; k < dgv.Columns.Count; k++)
            {
                dgv.Columns[k].Width = 55;
            }
        }

        private void gridView_SelectionChanged(Object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            dgv.ClearSelection();
            Console.WriteLine(dgv.Name);
            DataGridViewCellEventArgs cellEv = e as DataGridViewCellEventArgs;
            // dgv.Rows[cellEv.RowIndex].Selected = false;
            //sender.ClearSelection();
        }

        private void standardView()
        {
            try
            {
                initGridViewColumns(rtGridView);
                initGridViewColumns(osGridView);
                initGridViewColumns(vrrGridView);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);  
            }

            try
            {
                drawTable(rtGridView, "perceived");
                drawTable(osGridView, "overshoot");
                drawTable(vrrGridView, "vrr");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            // Set location, size, enabled/clickable/editable
            // Draw averages 
        }

        private void completeView()
        {
            /*ListView perceivedResponseTime = new ListView();
            ListView overshoot = new ListView();
            ListView vrr = new ListView();
            ListView initialResponseTime = new ListView();
            ListView completeResponseTime = new ListView();
            // Set location, size, enabled/clickable/editable
            drawTable(perceivedResponseTime, "perceived");
            drawTable(overshoot, "overshoot");
            drawTable(vrr, "vrr");
            drawTable(initialResponseTime, "initial");
            drawTable(completeResponseTime, "complete");
            */
            // Draw averages
        }

        private void graphView()
        {
            // Clear view/import panel
            // Create graph
            // Set position, size, etc
            // Draw graph
            if (rawData.Count != 0)
            {
                Size = new Size(1400, 800);
                graphViewPanel.Location = new Point(5, 52);
                drawGraph(0, 0);
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

        private void drawGraph(int arrayIndex, int resultIndex)
        {
            if (resultIndex < 0)
            {
                resultIndex = 0;
            }
            int startingRGB = rawData[arrayIndex][resultIndex][0];
            int endRGB = rawData[arrayIndex][resultIndex][1];
            int sampleTime = rawData[arrayIndex][resultIndex][2];
            int sampleCount = rawData[arrayIndex][resultIndex][3];
            double averageTime = sampleTime / sampleCount;
            double[] resultData = rawData[arrayIndex][resultIndex].Skip(4).Select(x => (double)x).ToArray();
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
                foreach (var i in rawData[arrayIndex])
                {
                    transSelect1.Items.Add("RGB " + i[0] + " to RGB " + i[1]);
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
            Size = new Size(1400, 800);
            importPanel.Location = new Point(5, 52);
        }

        private void stdResultsMenuBtn_Click(object sender, EventArgs e)
        {
            standardView();
            BackColor = Color.White;
            importPanel.Location = new Point(1500, 52);
            graphViewPanel.Location = new Point(1439, 762);
            standardResultsPanel.Location = new Point(5, 52);
            allResultsMenuBtn.Checked = false;
            graphViewMenuBtn.Checked = false;
            importViewMenuButton.Checked = false;
        }
        private void allResultsMenuBtn_Click(object sender, EventArgs e)
        {
            if (allResultsMenuBtn.Checked)
            {
                stdResultsMenuBtn.Checked = false;
                allResultsMenuBtn.Checked = true;
                graphViewMenuBtn.Checked = false;
                importViewMenuButton.Checked = false;
            }
            else
            {
                completeView();
                Size = new Size(1400, 800);
                stdResultsMenuBtn.Checked = false;
                allResultsMenuBtn.Checked = true;
                graphViewMenuBtn.Checked = false;
                importViewMenuButton.Checked = false;
            }
        }

        private void graphViewMenuBtn_Click(object sender, EventArgs e)
        {
            if (graphViewMenuBtn.Checked)
            {
                stdResultsMenuBtn.Checked = false;
                allResultsMenuBtn.Checked = false;
                graphViewMenuBtn.Checked = true;
                graphViewMenuBtn.Visible = true;
                importViewMenuButton.Checked = false;
            }
            else
            {
                graphView();
                importPanel.Location = new Point(1439, 762);
                graphViewPanel.Location = new Point(5, 52);
                standardResultsPanel.Location = new Point(1439, 52);
                BackColor = System.Drawing.ColorTranslator.FromHtml("#2e3440");
                stdResultsMenuBtn.Checked = false;
                allResultsMenuBtn.Checked = false;
                graphViewMenuBtn.Checked = true;
                graphViewMenuBtn.Visible = true;
                importViewMenuButton.Checked = false;
            }
        }

        private void importViewMenuButton_Click(object sender, EventArgs e)
        {
            if (importViewMenuButton.Checked)
            {
                stdResultsMenuBtn.Checked = false;
                allResultsMenuBtn.Checked = false;
                graphViewMenuBtn.Checked = false;
                importViewMenuButton.Checked = true;
            }   
            else
            {
                importView();
                Size = new Size(1400, 800);
                importPanel.Location = new Point(5, 52);
                graphViewPanel.Location = new Point(1439, 762);
                standardResultsPanel.Location = new Point(1439, 52);
                stdResultsMenuBtn.Checked = false;
                allResultsMenuBtn.Checked = false;
                graphViewMenuBtn.Checked = false;
                importViewMenuButton.Checked = true;
            }
        }

        private void transSelect1_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
            showProcessedData();
        }

        private void importGraphBtn_Click(object sender, EventArgs e)
        {
            importRawData();
        }

        private void resetGraphBtn_Click(object sender, EventArgs e)
        {
            drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
            showProcessedData();
        }

        private List<int[]> processGammaTable()
        {
            if (gamma.Count == 0)
            {
                if (!Properties.Settings.Default.SuppressDiagBox)
                {
                    MessageBox.Show("No Gamma data is stored in the program.", "Gamma Table Processing Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return null;
            }
            else
            {
                noiseLevel.Clear();
                double[] rgbVals = new double[gamma.Count];
                double[] lightLevelVals = new double[gamma.Count];
                for (int i = 0; i < gamma.Count; i++)
                {
                    int[] dataLine = gamma[i].Skip(300).ToArray();
                    int lineAverage = 0;
                    for (int j = 0; j < (dataLine.Length - 100); j++)
                    {
                        lineAverage += dataLine[j];
                    }
                    noiseLevel.Add(new int[] { gamma[i][0], (dataLine.Max() - dataLine.Min()) });
                    lineAverage /= (dataLine.Length - 100);
                    rgbVals[i] = gamma[i][0];
                    lightLevelVals[i] = lineAverage;
                }
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
                List<int[]> xy = new List<int[]>();
                for (int k = 0; k < x.Count; k++)
                {
                    xy.Add(new int[] { x[k], y[k] });
                }
                return xy;
            }
        }

        private void processTestLatency()
        {

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
        }

        private processedResult ProcessResponseTimeData(resultSelection res)
        {
            //This is a long one. This is the code that builds the gamma curve, finds the start/end points and calculates response times and overshoot % (gamma corrected)
            List<double[]> processedData = new List<double[]>();

            // First, create gamma array from the data
            List<int[]> localGamma = new List<int[]>();
            List<int[]> fullGammaTable = new List<int[]>();
            List<int[]> smoothedDataTable = new List<int[]>();
            int noise = 0;

            try //Wrapped whole thing in try just in case
            {
                // Save start, end, time and sample count then clear the values from the array
                int StartingRGB = rawData[res.arrayIndex][res.resultIndex][0];
                int EndRGB = rawData[res.arrayIndex][res.resultIndex][1];
                int TimeTaken = rawData[res.arrayIndex][res.resultIndex][2];
                int SampleCount = rawData[res.arrayIndex][res.resultIndex][3];
                int[] samples = rawData[res.arrayIndex][res.resultIndex].Skip(4).ToArray();

                double SampleTime = ((double)TimeTaken / (double)SampleCount); // Get the time taken between samples

                // Clean up noisy data using moving average function
                int period = 10;
                foreach (var n in noiseLevel)
                {
                    if (n[0] == StartingRGB || n[0] == EndRGB)
                    {
                        noise = n[1];
                        break;
                    }
                }
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
                for (int m = samples.Length - 5; m > samples.Length - 150; m--)
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
                    if (maxValue > (endAverage + 100) && maxValue > (processedGamma[EndRGB][1] + 100))
                    {
                        // undershoot may have occurred
                        Console.WriteLine("Overshoot found");
                        // convert maxValue to RGB using gamma table
                        for (int i = 0; i < processedGamma.Count; i++)
                        {
                            // Find what RGB value matches or exceeds the peak light reading for this run
                            if (maxValue <= processedGamma[i][1])
                            {
                                // Check if peak light reading is closer to upper or lower bound value
                                int diff1 = processedGamma[i][1] - maxValue;
                                int diff2 = maxValue - processedGamma[i - 1][1];
                                if (diff1 < diff2)
                                {
                                    overUnderRGB = processedGamma[i][0];
                                }
                                else
                                {
                                    overUnderRGB = processedGamma[i - 1][0];
                                }
                                break;
                            }
                            else if (maxValue > processedGamma.Last()[1])
                            {
                                if (maxValue > 65500)
                                {
                                    overUnderRGB = 260;
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
                    if (minValue < (endAverage - 100) && minValue < (processedGamma[EndRGB][1] - 100))
                    {
                        // overshoot may have occurred
                        // convert minValue to RGB using gamma table
                        Console.WriteLine("Undershoot found");
                        for (int i = 0; i < processedGamma.Count; i++)
                        {
                            // Find what RGB value matches or exceeds the peak light reading for this run
                            if (minValue <= processedGamma[i][1])
                            {
                                if (i == 0)
                                {
                                    overUnderRGB = 0;
                                    break;
                                }
                                else
                                {
                                    // Check if peak light reading is closer to upper or lower bound value
                                    int diff1 = processedGamma[i][1] - minValue;
                                    int diff2 = minValue - processedGamma[i - 1][1];
                                    if (diff1 < diff2)
                                    {
                                        overUnderRGB = processedGamma[i][0];
                                    }
                                    else
                                    {
                                        overUnderRGB = processedGamma[i - 1][0];
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
                    double tol = res.rtStyle.Tolerance / 100;
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
                        start3 = processedGamma[Convert.ToInt32(StartingRGB + RGBTolerance)][1];
                        end3 = processedGamma[Convert.ToInt32(EndRGB - RGBTolerance)][1];
                        if (overUnderRGB > (EndRGB + RGBTolerance) && overUnderRGB != 0)
                        { endOffsetRGB = EndRGB + RGBTolerance; }
                        else if (overUnderRGB == -1)
                        { endOffsetRGB = EndRGB; }
                        endPer3 = processedGamma[Convert.ToInt32(endOffsetRGB)][1];
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
                    double tol = res.rtStyle.Tolerance / 100;
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
                        start3 = processedGamma[Convert.ToInt32(StartingRGB - RGBTolerance)][1];
                        end3 = processedGamma[Convert.ToInt32(EndRGB + RGBTolerance)][1];
                        if (overUnderRGB < (EndRGB - RGBTolerance) && overUnderRGB != 0)
                        {
                            endOffsetRGB = EndRGB - RGBTolerance;
                        }
                        endPer3 = processedGamma[Convert.ToInt32(endOffsetRGB)][1];
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

                double visualResponseRating = 100 - (initialResponseTime + perceivedResponseTime);

                double inputLag = Math.Round(inputLagTime, 1);
            

                if (res.osStyle.gammaCorrected && (!res.osStyle.endPercent || !res.osStyle.rangePercent))
                {
                    // Standard output with total transition time & gamma corrected overshoot value
                    if (overUnderRGB == -1)
                    {
                        overshootRGBDiff = 100;
                    }
                    double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, initialResponseTime, perceivedResponseTime, overshootRGBDiff, visualResponseRating, inputLag };
                    processedData.Add(completeResult);

                    return new processedResult { Time = perceivedResponseTime, startIndex = perceivedTransStart, endIndex = perceivedTransEnd, Overshoot = overshootRGBDiff };
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
                            if (peakValue > (endAverage + 100))
                            {
                                os = (peakValue - endAverage) / endAverage;
                                os *= 100;
                                os = Math.Round(os, 1);
                            }
                        }
                        else
                        {
                            if (peakValue < (endAverage - 100))
                            {
                                os = (endAverage - peakValue) / endAverage;
                                // os *= -1;
                                os *= 100;
                                os = Math.Round(os, 1);
                            }
                        }
                    }
                    else
                    {
                        if (StartingRGB < EndRGB)
                        {
                            if (peakValue > (endAverage + 100))
                            {
                                double range = endAverage - startAverage;
                                double peakRange = peakValue - endAverage;
                                os = peakRange / range;
                                os *= 100;
                                os = Math.Round(os, 1);
                            }
                        }
                        else
                        {
                            if (peakValue < (endAverage - 100))
                            {
                                double range = startAverage - endAverage;
                                double peakRange = endAverage - peakValue;
                                os = peakRange / range;
                                // os *= -1;
                                os *= 100;
                                os = Math.Round(os, 1);
                            }
                        }
                    }
                    double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, initialResponseTime, perceivedResponseTime, os, visualResponseRating, inputLag };
                    processedData.Add(completeResult);
                    return new processedResult { Time = perceivedResponseTime, startIndex = perceivedTransStart, endIndex = perceivedTransEnd, Overshoot = os };
                }
                else
                {
                    // Standard output with total transition time & gamma corrected overshoot percentage
                    double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, initialResponseTime, perceivedResponseTime, overshootPercent, visualResponseRating, inputLag };
                    processedData.Add(completeResult);

                    return new processedResult { Time = perceivedResponseTime, startIndex = perceivedTransStart, endIndex = perceivedTransEnd, Overshoot = overshootPercent };
                }
            }
            catch (Exception procEx)
            {
                Console.WriteLine(procEx.Message + procEx.StackTrace);
                return null;
            }
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
                    processedResult proc = ProcessResponseTimeData(new resultSelection
                    {
                        arrayIndex = runSelectBox.SelectedIndex,
                        resultIndex = resInd,
                        rtStyle = rtMethodologies[processTypeListBox.SelectedIndex],
                        osStyle = osMethodologies[overshootStyleListBox.SelectedIndex]
                    });
                    rtLabel.Text = proc.Time.ToString() + " ms";
                    rtTypeLabel.Text = "Perceived";
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
                    double start = proc.startIndex * sampleTime;
                    start /= 1000;
                    start = Math.Round(start, 2);
                    double end = proc.endIndex * sampleTime;
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
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
            }
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

        private void processTypeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            showProcessedData();
        }

        private void overshootStyleListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            showProcessedData();
        }

        private void runSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawGraph(runSelectBox.SelectedIndex, transSelect1.SelectedIndex);
            showProcessedData();
        }

        private void importRawFolder_Click(object sender, EventArgs e)
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
                        //results.Clear();
                        string[] files = Directory.GetFiles(filePath);
                        bool valid = false;
                        foreach (var f in files)
                        {
                            if (f.Contains("-RAW-OSRTT"))
                            {
                                valid = true;
                                try
                                {
                                    List<int[]> tempRes = new List<int[]>();
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
                                                string[] line = reader.ReadLine().Split(',');
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
                                                    tempRes.Add(intLine);
                                                }
                                            }
                                        }
                                    }
                                    rawData.AddRange(new List<List<int[]>> { tempRes });
                                    gamma.AddRange(tempGamma);
                                    processedGamma.AddRange(processGammaTable());
                                    processTestLatency();
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
                                        Console.WriteLine(iex.Message + iex.StackTrace);
                                    }
                                }
                            }
                        }
                        if (valid)
                        {
                            // Draw graph
                            graphViewMenuBtn_Click(null, null);
                            handleRunsList();
                            handleResultsList(runSelectBox.SelectedIndex);
                        }
                        else
                        {
                            MessageBox.Show("Please select a results folder with one or more raw data files", "Unable to load files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a results folder with one or more raw data files", "Unable to load files", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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
                    averageData.Clear();
                    if (filePath.Contains("FULL-OSRTT"))
                    {
                        //Read the contents of the file into a stream
                        try
                        {
                            List<double[]> tempRes = new List<double[]>();
                            var fileStream = openFileDialog.OpenFile();
                            using (StreamReader reader = new StreamReader(fileStream))
                            {
                                while (!reader.EndOfStream)
                                {
                                    // This can probably be done better
                                    string[] line = reader.ReadLine().Split(',');
                                    if (line[0].Contains("RGB"))
                                    {
                                        continue;
                                    }
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
                                    
                                    tempRes.Add(intLine);
                                    
                                }
                            }
                            averageData.AddRange( tempRes );
                            stdResultsMenuBtn_Click(null, null);
                        }
                        catch (Exception ex)
                        {
                            DialogResult d = MessageBox.Show("File may be in use by another program, please make sure it's not open elsewhere and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
