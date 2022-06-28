using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSRTT_Launcher
{
    public partial class Heatmaps : UserControl
    {
        public List<ProcessData.processedResult> averageData = new List<ProcessData.processedResult>();
        public List<int> RGBArr = new List<int> { 0, 51, 102, 153, 204, 255 };
        public int rtIndex = 4;
        public class colourKey
        {
            public int keyType { get; set; }
            public double min { get; set; }
            public double middle { get; set; }
            public double max { get; set; }
        }
        List<colourKey> colourKeys = new List<colourKey>();
        public ProcessData.rtMethods rtMethod;
        public ProcessData.osMethods osMethod;
        public ProcessData.runSettings runSettings;
        public void setAverageData(List<ProcessData.processedResult> ad)
        {
            if (ad != null)
            {
                averageData.Clear();
                averageData.AddRange(ad);
            }
        }
        public void setRGBArr(List<int> RGB)
        {
            if (RGB != null)
            {
                RGBArr.Clear();
                RGBArr.AddRange(RGB);
            }
        }
        public void setRtIndex(int i)
        {
            rtIndex = i;
        }
        public void setRtMethod(ProcessData.rtMethods rt)
        {
            if (rt != null)
            {
                rtMethod = rt;
            }
        }
        public void setOsMethod(ProcessData.osMethods os)
        {
            if (os != null)
            {
                osMethod = os;
            }
        }
        public void setRunSettings(ProcessData.runSettings run)
        {
            if (run != null)
            {
                runSettings = run;
            }
        }
        public void hideText(bool state)
        {
            from1.Visible = state;
            from2.Visible = state;
            from3.Visible = state;
            label12.Visible = state;
            label10.Visible = state;
            label9.Visible = state;
            rtTitle.Visible = state;
            rtSubTitle.Visible = state;
            osTitle.Visible = state;
            osSubTitle.Visible = state;
            vrrTitle.Visible = state;
            vrrSubTitle.Visible = state;
            //label1.Visible = state;
            //label5.Visible = state;
            //label11.Visible = state;
        }
        public Heatmaps()
        {
            Graphics g = this.CreateGraphics();
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            string[] rtKey = Properties.Settings.Default.rtKey.Split(',');
            if (rtKey.Length == 3)
            {
                colourKeys.Add(new colourKey { keyType = 0, min = double.Parse(rtKey[0]), middle = double.Parse(rtKey[1]), max = double.Parse(rtKey[2]) });
                rtGreenLbl.Text = rtKey[0];
                rtOrangeLbl.Text = rtKey[1];
                rtRedLbl.Text = rtKey[2];
            }
            string[] osKey = Properties.Settings.Default.osKey.Split(',');
            if (osKey.Length == 3)
            {
                colourKeys.Add(new colourKey { keyType = 1, min = double.Parse(osKey[0]), middle = double.Parse(osKey[1]), max = double.Parse(osKey[2]) });
                osGreenLbl.Text = osKey[0];
                osOrangeLbl.Text = osKey[1];
                osRedLbl.Text = osKey[2];
            }
            string[] vrrKey = Properties.Settings.Default.vrrKey.Split(',');
            if (vrrKey.Length == 3)
            {
                colourKeys.Add(new colourKey { keyType = 2, min = double.Parse(vrrKey[0]), middle = double.Parse(vrrKey[1]), max = double.Parse(vrrKey[2]) });
                vrrGreenLbl.Text = vrrKey[2];
                vrrOrangeLbl.Text = vrrKey[1];
                vrrRedLbl.Text = vrrKey[0];
            }
        }

        private void Heatmaps_Load(object sender, EventArgs e)
        {

        }

        private void drawTable(DataGridView dgv, string type)
        {
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
                    if (averageData[k].StartingRGB == RGBArr[current])
                    {
                        if (type == "perceived") { data[current][next] = averageData[k].perTime.ToString(); }
                        else if (type == "initial") { data[current][next] = averageData[k].initTime.ToString(); }
                        else if (type == "complete") { data[current][next] = averageData[k].compTime.ToString(); }
                        else if (type == "overshoot") { data[current][next] = averageData[k].Overshoot.ToString(); }
                        else if (type == "vrr") { data[current][next] = averageData[k].visualResponseRating.ToString(); }
                    }
                    else
                    {
                        if (type == "perceived") { data[next][current] = averageData[k].perTime.ToString(); }
                        else if (type == "initial") { data[next][current] = averageData[k].initTime.ToString(); }
                        else if (type == "complete") { data[next][current] = averageData[k].compTime.ToString(); }
                        else if (type == "overshoot") { data[next][current] = averageData[k].Overshoot.ToString(); }
                        else if (type == "vrr") { data[next][current] = averageData[k].visualResponseRating.ToString(); }
                        next++;
                    }
                }
                else
                {
                    current++;
                    next = current + 1;
                    if (averageData[k].StartingRGB == RGBArr[current])
                    {
                        if (type == "perceived") { data[current][next] = averageData[k].perTime.ToString(); }
                        else if (type == "initial") { data[current][next] = averageData[k].initTime.ToString(); }
                        else if (type == "complete") { data[current][next] = averageData[k].compTime.ToString(); }
                        else if (type == "overshoot") { data[current][next] = averageData[k].Overshoot.ToString(); }
                        else if (type == "vrr") { data[current][next] = averageData[k].visualResponseRating.ToString(); }
                    }
                    else
                    {
                        if (type == "perceived") { data[next][current] = averageData[k].perTime.ToString(); }
                        else if (type == "initial") { data[next][current] = averageData[k].initTime.ToString(); }
                        else if (type == "complete") { data[next][current] = averageData[k].compTime.ToString(); }
                        else if (type == "overshoot") { data[next][current] = averageData[k].Overshoot.ToString(); }
                        else if (type == "vrr") { data[next][current] = averageData[k].visualResponseRating.ToString(); }
                        next++;
                    }
                }
            }
            // Add string array to rows
            foreach (var item in data)
            {
                dgv.Rows.Add(item);
            }
            for (int l = 0; l < dgv.Rows.Count; l++)
            {
                dgv.Rows[l].Height += 5;
                dgv.Rows[l].HeaderCell.Value = RGBArr[l].ToString();
                Console.WriteLine(dgv.Rows[l].HeaderCell.Value);
            }
        }

        private void drawStatsTable(DataGridView dgv, string type)
        {
            // Average function

            // part average (rise or fall) - bundle into main average with if (start < end) += riseAvg

            // Best / worst

            // 0 - 255 - 0 - again roll through main loop. 
            Console.WriteLine(type);
            // Basically just loop through the list, add value to any relevant sums then divide/round at the end.
            if (type == "overshoot")
            {

                double averageError = 0;
                double errorOver10 = 0;
                double worstRT = 0;
                for (int l = 0; l < averageData.Count; l++)
                {
                    averageError += averageData[l].Overshoot;
                    if (averageData[l].Overshoot > worstRT)
                    {
                        worstRT = averageData[l].Overshoot;
                    }
                    if (averageData[l].Overshoot > 10)
                    {
                        errorOver10++;
                    }
                }
                errorOver10 = (errorOver10 / averageData.Count) * 100;
                errorOver10 = Math.Round(errorOver10, 2);
                averageError = averageError / averageData.Count;
                averageError = Math.Round(averageError, 2);

                // Create string arrays for rows
                List<string[]> data = new List<string[]>();
                for (int i = 0; i < 3; i++)
                {
                    // Fill list with sized empty string arrays to address later
                    string[] line = new string[2];
                    data.Add(line);
                }
                data[0][0] = "Average Error";
                data[0][1] = averageError.ToString();
                data[1][0] = "Worst Error";
                data[1][1] = worstRT.ToString();
                string ostype = "Percent Above 10";
                if (osMethod.endPercent || osMethod.rangePercent)
                {
                    ostype += "%";
                }
                else
                {
                    ostype += " RGB";
                }
                data[2][0] = ostype;
                data[2][1] = errorOver10.ToString();

                foreach (var item in data)
                {
                    dgv.Rows.Add(item);
                }
            }
            else if (type == "vrr")
            {
                double averageRating = 0;
                double averageFall = 0;
                double averageRise = 0;
                double worst = averageData[0].visualResponseRating;
                double best = averageData[0].visualResponseRating;
                for (int l = 0; l < averageData.Count; l++)
                {
                    averageRating += averageData[l].visualResponseRating;
                    if (averageData[l].visualResponseRating < worst)
                    {
                        worst = averageData[l].visualResponseRating;
                    }
                    if (averageData[l].visualResponseRating > best)
                    {
                        best = averageData[l].visualResponseRating;
                    }
                    if (averageData[l].StartingRGB < averageData[l].EndRGB)
                    {
                        averageRise += averageData[l].visualResponseRating;
                    }
                    else
                    {
                        averageFall += averageData[l].visualResponseRating;
                    }
                }
                averageRating = (averageRating / averageData.Count);
                averageRating = Math.Round(averageRating, 2);
                double div2 = averageData.Count / 2;
                averageFall = (averageFall / div2);
                averageFall = Math.Round(averageFall, 2);
                averageRise = (averageRise / div2);
                averageRise = Math.Round(averageRise, 2);
                List<string[]> data = new List<string[]>();
                for (int i = 0; i < 5; i++)
                {
                    // Fill list with sized empty string arrays to address later
                    string[] line = new string[2];
                    data.Add(line);
                }
                data[0][0] = "Average Rating";
                data[0][1] = averageRating.ToString();
                data[1][0] = "Average Rise";
                data[1][1] = averageRise.ToString();
                data[2][0] = "Average Fall";
                data[2][1] = averageFall.ToString();
                data[3][0] = "Best";
                data[3][1] = best.ToString();
                data[4][0] = "Worst";
                data[4][1] = worst.ToString();
                foreach (var item in data)
                {
                    dgv.Rows.Add(item);
                }
            }
            else if (dgv.Name.Contains("Refresh"))
            {
                int indexToUse = 0;
                if (type == "perceived") { indexToUse = 0; }
                else if (type == "initial") { indexToUse = 1; }
                else if (type == "complete") { indexToUse = 2; }
                double refreshRate = 60;
                if (runSettings != null)
                {
                    refreshRate = runSettings.RefreshRate;
                }
                double refreshWindow = Math.Round((1000 / refreshRate), 2);
                double percentInWindow = 0;
                for (int l = 0; l < averageData.Count; l++)
                {
                    double[] rtTimes = { averageData[l].perTime, averageData[l].initTime, averageData[l].compTime };
                    if (rtTimes[indexToUse] < refreshWindow)
                    {
                        percentInWindow++;
                    }
                }
                percentInWindow = (percentInWindow / averageData.Count) * 100;
                percentInWindow = Math.Round(percentInWindow, 2);
                // Create string arrays for rows
                List<string[]> data = new List<string[]>();
                for (int i = 0; i < 3; i++)
                {
                    // Fill list with sized empty string arrays to address later
                    string[] line = new string[2];
                    data.Add(line);
                }
                data[0][0] = "Refresh Rate";
                data[0][1] = refreshRate.ToString();
                data[1][0] = "Refresh Window";
                data[1][1] = refreshWindow.ToString();
                data[2][0] = "Percent in Window";
                data[2][1] = percentInWindow.ToString();
                foreach (var item in data)
                {
                    dgv.Rows.Add(item);
                }
            }
            else if (dgv.Name.Contains("Settings"))
            {
                List<string[]> data = new List<string[]>();
                if (runSettings.OverdriveMode == null)
                {
                    runSettingsView.Size = new Size(450, 105);
                    for (int i = 0; i < 3; i++)
                    {
                        // Fill list with sized empty string arrays to address later
                        string[] line = new string[2];
                        data.Add(line);
                    }
                    string monitorName = runSettings.MonitorName;
                    if (runSettings.MonitorName[3] == ' ')
                    {
                        monitorName = runSettings.MonitorName.Remove(0, 4);
                    }
                    else if (runSettings.MonitorName[0] == ' ')
                    {
                        monitorName = runSettings.MonitorName.Remove(0, 1);
                    }
                    data[0][0] = "Monitor Name";
                    data[0][1] = monitorName;
                    data[1][0] = "FPS Limit";
                    data[1][1] = runSettings.FPSLimit.ToString();
                    data[2][0] = "V-Sync";
                    data[2][1] = runSettings.Vsync.ToString();
                }
                else
                {
                    runSettingsView.Size = new Size(450, 140);
                    for (int i = 0; i < 4; i++)
                    {
                        // Fill list with sized empty string arrays to address later
                        string[] line = new string[2];
                        data.Add(line);
                    }
                    string monitorName = runSettings.MonitorName;
                    if (runSettings.MonitorName[3] == ' ')
                    {
                        monitorName = runSettings.MonitorName.Remove(0, 4);
                    }
                    else if (runSettings.MonitorName[0] == ' ')
                    {
                        monitorName = runSettings.MonitorName.Remove(0, 1);
                    }
                    data[0][0] = "Monitor Name";
                    data[0][1] = monitorName;
                    data[1][0] = "Overdrive";
                    data[1][1] = runSettings.OverdriveMode;
                    data[2][0] = "FPS Limit";
                    data[2][1] = runSettings.FPSLimit.ToString();
                    data[3][0] = "V-Sync";
                    data[3][1] = runSettings.Vsync.ToString();
                }
                
                foreach (var item in data)
                {
                    dgv.Rows.Add(item);
                }
            }
            else
            {
                int indexToUse = 0;
                if (type == "perceived") { indexToUse = 0; }
                else if (type == "initial") { indexToUse = 1; }
                else if (type == "complete") { indexToUse = 2; }
                double averageInitialRT = 0;
                double averageCompleteRT = 0;
                double averagePerceivedRT = 0;
                double averageRise = 0;
                double averageFall = 0;
                double roundTrip = 0;
                double bestRT = averageData[0].perTime;
                double worstRT = averageData[0].perTime;
                for (int l = 0; l < averageData.Count; l++)
                {
                    double[] rtTimes = { averageData[l].perTime, averageData[l].initTime, averageData[l].compTime };
                    averageCompleteRT += averageData[l].compTime;
                    averageInitialRT += averageData[l].initTime;
                    averagePerceivedRT += averageData[l].perTime;
                    if (averageData[l].StartingRGB < averageData[l].EndRGB)
                    {
                        averageRise += rtTimes[indexToUse];
                    }
                    else
                    {
                        averageFall += rtTimes[indexToUse];
                    }
                    if ((averageData[l].StartingRGB == 0 && averageData[l].EndRGB == 255) || (averageData[l].StartingRGB == 255 && averageData[l].EndRGB == 0))
                    {
                        roundTrip += rtTimes[indexToUse];
                    }
                    if (rtTimes[indexToUse] < bestRT)
                    {
                        bestRT = rtTimes[indexToUse];
                    }
                    if (rtTimes[indexToUse] > worstRT)
                    {
                        worstRT = rtTimes[indexToUse];
                    }
                    
                }
                averageInitialRT = averageInitialRT / averageData.Count;
                averageInitialRT = Math.Round(averageInitialRT, 2);
                averageCompleteRT = averageCompleteRT / averageData.Count;
                averageCompleteRT = Math.Round(averageCompleteRT, 2);
                averagePerceivedRT = averagePerceivedRT / averageData.Count;
                averagePerceivedRT = Math.Round(averagePerceivedRT, 2);
                averageRise = averageRise / (averageData.Count / 2);
                averageRise = Math.Round(averageRise, 2);
                averageFall = averageFall / (averageData.Count / 2);
                averageFall = Math.Round(averageFall, 2);
                // Create string arrays for rows
                List<string[]> data = new List<string[]>();
                for (int i = 0; i < 8; i++)
                {
                    // Fill list with sized empty string arrays to address later
                    string[] line = new string[2];
                    data.Add(line);
                }
                data[0][0] = "Average Initial Time";
                data[0][1] = averageInitialRT.ToString();
                data[1][0] = "Average Complete Time";
                data[1][1] = averageCompleteRT.ToString();
                data[2][0] = "Average Perceived Time";
                data[2][1] = averagePerceivedRT.ToString();
                data[3][0] = "Average Rise Time";
                data[3][1] = averageRise.ToString();
                data[4][0] = "Average Fall Time";
                data[4][1] = averageFall.ToString();
                data[5][0] = "0-255-0";
                data[5][1] = roundTrip.ToString();
                data[6][0] = "Best";
                data[6][1] = bestRT.ToString();
                data[7][0] = "Worst";
                data[7][1] = worstRT.ToString();
                foreach (var item in data)
                {
                    dgv.Rows.Add(item);
                }
            }
        }

        void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == -1 && e.RowIndex > -1)
            {
                e.PaintBackground(e.CellBounds, true);
                using (SolidBrush br = new SolidBrush(Color.White))
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    e.Graphics.DrawString(e.Value.ToString(),
                    e.CellStyle.Font, br, e.CellBounds, sf);
                }
                e.Handled = true;
            }
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
            dgv.CellPainting += dataGridView_CellPainting;
            dgv.ColumnCount = 6;
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#ACAEAE");
            dgv.RowHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#ACAEAE");
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 22, FontStyle.Bold);
            dgv.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 22, FontStyle.Bold);
            dgv.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedRowHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(84, 157, 178, 189);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(255, 255, 255, 255);
            dgv.RowHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            dgv.RowsDefaultCellStyle.Font = new Font("Calibri", 21);
            dgv.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.RowsDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            if (dgv.Name.Contains("vrr"))
            {
                dgv.CellFormatting += new DataGridViewCellFormattingEventHandler(dgvVRR_CellFormatting);
            }
            else
            {
                dgv.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
            }
            for (int i = 0; i < RGBArr.Count; i++)
            {
                dgv.Columns[i].Name = RGBArr[i].ToString();
            }

            for (int k = 0; k < dgv.Columns.Count; k++)
            {
                dgv.Columns[k].Width = 75;
                dgv.Columns[k].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }


        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Compare the value of second Column i.e. Column with Index 1.
            DataGridView dgv = sender as DataGridView;
            Color cellColour;
            double min = colourKeys[0].min;
            double middle = colourKeys[0].middle;
            double max = colourKeys[0].max;
            if (dgv.Name.Contains("os"))
            {
                min = colourKeys[1].min;
                middle = colourKeys[1].middle;
                max = colourKeys[1].max;
            }
            if (e.Value != null)
            {
                if (dgv.Name.Contains("Refresh"))
                {
                    if ((e.RowIndex == 0 || e.RowIndex == 1) && dgv.Name.Contains("Refresh"))
                    {
                        cellColour = Color.SteelBlue;
                    }
                    else if (e.ColumnIndex == 0)
                    {
                        cellColour = Color.Gray;
                    }
                    else
                    {
                        double val = Convert.ToDouble(e.Value);
                        if (val <= 60)
                        {
                            int r = 213;
                            int g = 73;
                            int b = 70;
                            cellColour = ColorTranslator.FromHtml("#" + r.ToString("X") + g.ToString("X") + b.ToString("X"));
                        }
                        else if (val > 60 && val <= 80)
                        {
                            int r = 213;
                            int g = 183;
                            int b = 98;
                            cellColour = ColorTranslator.FromHtml("#" + r.ToString("X") + g.ToString("X") + b.ToString("X"));
                        }
                        else
                        {
                            int r = 58;
                            int g = 186;
                            int b = 92;
                            cellColour = ColorTranslator.FromHtml("#" + r.ToString("X") + g.ToString("X") + b.ToString("X"));
                        }
                    }
                }
                else if (dgv.Name.Contains("Stats") && e.ColumnIndex == 0 )
                {
                    cellColour = Color.Gray;
                }
                else if (dgv.Name.Contains("Settings"))
                {
                    cellColour = Color.Gray;
                    e.CellStyle.ForeColor = Color.White;
                }
                else
                {
                    double val = Convert.ToDouble(e.Value);
                    if (dgv.Name.Contains("Stats") && e.RowIndex == 5)
                    {
                        val *= 2;
                    }
                    if (val == middle)
                    {
                        int r = 213;
                        int g = 183;
                        int b = 98;
                        cellColour = ColorTranslator.FromHtml("#" + r.ToString("X") + g.ToString("X") + b.ToString("X"));
                    }
                    else if (val < middle)
                    {
                        if (val < min)
                        {
                            // set min colour
                            int r = 58;
                            int g = 186;
                            int b = 92;
                            cellColour = ColorTranslator.FromHtml("#" + r.ToString("X") + g.ToString("X") + b.ToString("X"));
                        }
                        else
                        {
                            double range = middle - min;
                            double time = val - min;
                            double result = time / range;
                            int greenRGB = 205;
                            int redRGB = Convert.ToInt32((160 * result) + 55);
                            int blueRGB = 80;
                            cellColour = ColorTranslator.FromHtml("#" + redRGB.ToString("X") + greenRGB.ToString("X") + blueRGB.ToString("X"));
                        }
                    }
                    else
                    {
                        if (val > max)
                        {
                            // set max colour
                            int r = 213;
                            int g = 73;
                            int b = 70;
                            cellColour = ColorTranslator.FromHtml("#" + r.ToString("X") + g.ToString("X") + b.ToString("X"));
                        }
                        else
                        {
                            double range = max - middle;
                            double time = max - val;
                            double result = time / range;
                            int greenRGB = Convert.ToInt32((135 * result) + 75);
                            int redRGB = 213;
                            int blueRGB = 70;
                            cellColour = ColorTranslator.FromHtml("#" + redRGB.ToString("X") + greenRGB.ToString("X") + blueRGB.ToString("X"));
                        }
                    }
                }
            }
            else
            {
                cellColour = Color.LightGray;
            }
            e.CellStyle.BackColor = cellColour;
        }

        private void dgvVRR_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Compare the value of second Column i.e. Column with Index 1.
            DataGridView dgv = sender as DataGridView;
            int colourKeyIndex = 0;
            Color cellColour = Color.LightGray;
            double min = colourKeys[2].min;
            double middle = colourKeys[2].middle;
            double max = colourKeys[2].max;
            if (e.Value != null)
            {
                if (dgv.Name.Contains("Stats") && e.ColumnIndex == 0)
                {
                    cellColour = Color.Gray;
                }
                else
                {
                    double val = Convert.ToDouble(e.Value);
                    if (dgv.Name.Contains("Stats") && e.RowIndex == 5)
                    {
                        val /= 2;
                    }
                    if (val == colourKeys[colourKeyIndex].middle)
                    {
                        int r = 213;
                        int g = 183;
                        int b = 98;
                        cellColour = ColorTranslator.FromHtml("#" + r.ToString("X") + g.ToString("X") + b.ToString("X"));
                    }
                    else if (val < middle)
                    {
                        if (val < min)
                        {
                            int r = 213;
                            int g = 73;
                            int b = 70;
                            cellColour = ColorTranslator.FromHtml("#" + r.ToString("X") + g.ToString("X") + b.ToString("X"));
                        }
                        else
                        {
                            double range = middle - min;
                            double time = val - min;
                            double result = time / range;
                            
                            int greenRGB = Convert.ToInt32((135 * result) + 75);
                            int redRGB = 213;
                            int blueRGB = 70;
                            cellColour = ColorTranslator.FromHtml("#" + redRGB.ToString("X") + greenRGB.ToString("X") + blueRGB.ToString("X"));
                        }
                    }
                    else
                    {
                        if (val > max)
                        {
                            int r = 58;
                            int g = 186;
                            int b = 92;
                            cellColour = ColorTranslator.FromHtml("#" + r.ToString("X") + g.ToString("X") + b.ToString("X"));
                        }
                        else
                        {
                            double range = max - middle;
                            double time = max - val;
                            double result = time / range;
                            
                            int greenRGB = 205;
                            int redRGB = Convert.ToInt32((160 * result) + 55);
                            int blueRGB = 80;
                            cellColour = ColorTranslator.FromHtml("#" + redRGB.ToString("X") + greenRGB.ToString("X") + blueRGB.ToString("X"));
                        }
                    }
                }
            }
            else    
            {
                cellColour = Color.LightGray;
            }
            e.CellStyle.BackColor = cellColour;
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
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersVisible = false;
            dgv.RowHeadersVisible = false;
            dgv.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            dgv.RowsDefaultCellStyle.ForeColor = Color.White;
            dgv.RowsDefaultCellStyle.BackColor = Color.Gray;
            dgv.RowsDefaultCellStyle.Font = new Font("Calibri", 22);
            if (dgv.Name.Contains("vrr"))
            {
                dgv.CellFormatting += new DataGridViewCellFormattingEventHandler(dgvVRR_CellFormatting);
            }
            else
            {
                dgv.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);
            }

            // rtGridView.RowHeadersDefaultCellStyle.Padding = new Padding(rtGridView.RowHeadersWidth / 2 );
            for (int k = 0; k < dgv.Columns.Count; k++)
            {
                if (k == 0)
                {
                    if (dgv.Name.Contains("Settings"))
                    {
                        dgv.Columns[k].Width = 200;
                    }
                    else
                    {
                        dgv.Columns[k].Width = 325;
                    }
                    dgv.Columns[k].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else
                {
                    if (dgv.Name.Contains("Settings"))
                    {
                        dgv.Columns[k].Width = 250;
                    }
                    else
                    {
                        dgv.Columns[k].Width = 125;
                    }
                }
                dgv.Columns[k].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        private void gridView_SelectionChanged(Object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            dgv.ClearSelection();
            dgv.CurrentRow.Selected = false;
        }

        public void standardView()
        {
            vrrTitle.Text = "Visual Response Rating";
            vrrSubTitle.Text = "Score out of 100 of visible performance";
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
            string rtType = "perceived";
            rtTitle.Text = "Perceived Response Time";
            if (rtIndex == 3)
            {
                rtType = "initial";
                rtTitle.Text = "Initial Response Time";
            }
            else if (rtIndex == 2)
            {
                rtType = "complete";
                rtTitle.Text = "Complete Response Time";
            }
            rtSubTitle.Text = rtMethod.Name;
            if (osMethod.gammaCorrected && (!osMethod.endPercent || !osMethod.rangePercent))
            {
                osTitle.Text = "RGB Overshoot";
                osSubTitle.Text = "RGB values over/under target";
            }
            else if (osMethod.gammaCorrected && (osMethod.endPercent || osMethod.rangePercent))
            {
                osTitle.Text = "Percent RGB Overshoot";
                if (osMethod.rangePercent)
                {
                    osSubTitle.Text = "Percentage of RGB values over/under transition range";
                }
                else
                {
                    osSubTitle.Text = "Percentage of RGB values over/under target";
                }
            }
            else if (!osMethod.gammaCorrected && (osMethod.endPercent || osMethod.rangePercent))
            {
                osTitle.Text = "Percent Overshoot";
                if (osMethod.rangePercent)
                {
                    osSubTitle.Text = "Percent of light level over/under transition range";
                }
                else
                {
                    osSubTitle.Text = "Percent of light level over/under target";
                }
            }
            try
            {
                drawTable(rtGridView, rtType);
                drawTable(osGridView, "overshoot");
                drawTable(vrrGridView, "vrr");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            try
            {
                initStatsGridViewColumns(rtStatsRefreshGridView);
                initStatsGridViewColumns(rtStatsGridView);
                initStatsGridViewColumns(osStatsGridView);
                initStatsGridViewColumns(vrrStatsGridView);
                initStatsGridViewColumns(runSettingsView);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
            try
            {
                drawStatsTable(rtStatsRefreshGridView, rtType);
                drawStatsTable(rtStatsGridView, rtType);
                drawStatsTable(osStatsGridView, "overshoot");
                drawStatsTable(vrrStatsGridView, "vrr");
                drawStatsTable(runSettingsView, "runSettings");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            // Set location, size, enabled/clickable/editable
            // Draw averages 
        }
    }
}
