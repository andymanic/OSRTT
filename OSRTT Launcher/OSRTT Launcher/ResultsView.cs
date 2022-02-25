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

namespace OSRTT_Launcher
{
    public partial class ResultsView : Form
    {
        string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;

        public List<List<int[]>> rawData = new List<List<int[]>>();
        public List<List<int[]>> multipleRunsData = new List<List<int[]>>();
        public List<double[]> averageData = new List<double[]>();
        public List<int[]> gamma = new List<int[]>();
        public List<int> testLatency = new List<int>();
        public List<int> RGBArr = new List<int> { 0, 51, 102, 153, 204, 255 };

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
            path = new Uri(System.IO.Path.GetDirectoryName(path)).LocalPath;
            path += @"\Results";
            listView1.OwnerDraw = true;
            listView1.DrawColumnHeader += ListView_DrawColumnHeader;
            listView1.DrawItem += ListView_DrawItem;
        }

        private void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (StringFormat sf = new StringFormat())
            {
                // Store the column text alignment, letting it default
                // to Left if it has not been set to Center or Right.
                
                sf.Alignment = StringAlignment.Center;

                // Draw the standard header background.
                //e.DrawBackground();
                Rectangle ch = e.Bounds;
                
                e.Graphics.FillRectangle(new SolidBrush(Color.Gray), ch);
                //e.Graphics.DrawLines(Pens.White, e.Bounds.Left)

                // Draw the header text.
                using (Font headerFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold))
                {
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
            return;
        }

        private void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
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
                stdResultsMenuBtn.Visible = false;
                // Import View
            }

            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Product ID";
            dataGridView1.Columns[1].Name = "Product Name";
            dataGridView1.Columns[2].Name = "Product Price";

            string[] row = new string[] { "1", "Product 1", "1000" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "2", "Product 2", "2000" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "3", "Product 3", "3000" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "4", "Product 4", "4000" };
            dataGridView1.Rows.Add(row);

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("", 55);
            listView1.Columns.Add("0", 45);
            listView1.Columns.Add("51", 45);
            listView1.Columns.Add("102", 45);
            listView1.Columns.Add("153", 45);
            listView1.Columns.Add("204", 45);
            listView1.Columns.Add("255", 45);
            listView1.Items.Add("0");

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
                            // Draw graph

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

        private void drawTable(ListView lv, string type)
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
            // Add Column Headers
            lv.Columns.Add("", 55);
            foreach (var i in RGBArr)
            {
                lv.Columns.Add(i.ToString(), 45);
            }
            // Create string arrays for rows
            List<string[]> data = new List<string[]>();
            for (int i = 0; i < RGBArr.Count; i++)
            {
                // Fill list with sized empty string arrays to address later
                string[] line = new string[RGBArr.Count];
                data.Add(line);
            }
            int current = 0;
            int next = 1;
            for (int k = 0; k < averageData.Count; k++)
            {
                if (next < RGBArr.Count)
                {
                    if (averageData[k][indexToUse] == RGBArr[current])
                    {
                        // current base
                        
                    }
                    else
                    {
                        // next 

                    }
                    next++;
                }
                else
                {
                    current++;
                    next = current + 1;
                    // fill data

                }
            }
            // Add string array to rows

            // Style cells? Probs not here.
        }

        private void standardView()
        {
            ListView responseTimes = new ListView();
            ListView overshoot = new ListView();
            ListView vrr = new ListView();
            responseTimes.Size = new Size (350,250);
            responseTimes.Location = new Point(55, 55);
            responseTimes.DrawColumnHeader += ListView_DrawColumnHeader;
            overshoot.DrawColumnHeader += ListView_DrawColumnHeader;
            vrr.DrawColumnHeader += ListView_DrawColumnHeader;
            responseTimes.DrawItem += ListView_DrawItem;
            overshoot.DrawItem += ListView_DrawItem;
            vrr.DrawItem += ListView_DrawItem;
            // Set location, size, enabled/clickable/editable
            drawTable(responseTimes, "perceived");
            drawTable(overshoot, "overshoot");
            drawTable(vrr, "vrr");
            // Draw averages 
        }

        private void completeView()
        {
            ListView perceivedResponseTime = new ListView();
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
            // Draw averages
        }

        private void graphView()
        {
            // Clear view/import panel
            // Create graph
            // Set position, size, etc
            // Draw graph
        }

        private void importView()
        {
            // Clear view/graph panel
            // Move import panel in view
        }

        private void stdResultsMenuBtn_Click(object sender, EventArgs e)
        {
            if (stdResultsMenuBtn.Checked)
            {
                stdResultsMenuBtn.Checked = true;
                allResultsMenuBtn.Checked = false;
                graphViewMenuBtn.Checked = false;
                importViewMenuButton.Checked = false;
            }
            else
            {
                standardView();
                allResultsMenuBtn.Checked = false;
                graphViewMenuBtn.Checked = false;
                importViewMenuButton.Checked = false;
            }

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
                importViewMenuButton.Checked = false;
            }
            else
            {
                graphView();
                stdResultsMenuBtn.Checked = false;
                allResultsMenuBtn.Checked = false;
                graphViewMenuBtn.Checked = true;
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
                stdResultsMenuBtn.Checked = false;
                allResultsMenuBtn.Checked = false;
                graphViewMenuBtn.Checked = false;
                importViewMenuButton.Checked = true;
            }
        }


    }
}
