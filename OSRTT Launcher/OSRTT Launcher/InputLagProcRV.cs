using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSRTT_Launcher
{
    public partial class InputLagProcRV : UserControl
    {
        // Bar chart with average, min, max for USB, processing, display latency
        // Scatter plot (with labels?) for each run

        // Add ability to import other monitor results to graph/compare
        public ProcessData.averagedInputLag inputLagResults { get; set; }

        public string resultsFolderPath = "";

        public int type = 2;

        private bool ScatterOption = false;

        public InputLagProcRV()
        {
            InitializeComponent();
        }
        public void drawScatterGraph()
        {
            rtTitle.Text = "On Display Latency";
            graphedData.Plot.Clear();
            double[] xs = new double[inputLagResults.inputLagResults.Count];
            double[] ys = new double[inputLagResults.inputLagResults.Count];
            for (int i = 0; i < inputLagResults.inputLagResults.Count; i++)
            {
                xs[i] = inputLagResults.inputLagResults[i].shotNumber;
                if (type == 0)
                {
                    ys[i] = inputLagResults.inputLagResults[i].clickTimeMs;
                }
                else if (type == 1)
                {
                    ys[i] = inputLagResults.inputLagResults[i].frameTimeMs;
                }
                else if (type == 2)
                {
                    ys[i] = inputLagResults.inputLagResults[i].onDisplayLatency;
                }
                else if (type == 3)
                {
                    ys[i] = inputLagResults.inputLagResults[i].totalInputLag;
                }
            }
            graphedData.Plot.AddScatter(xs, ys, null, 3, 10);
            //graphedData.Plot.Title("");
            graphedData.Plot.Legend(false);
            graphedData.Plot.Style(null, SystemColors.ControlDark, Color.LightGray);
            
            graphedData.Plot.XAxis.TickLabelStyle(Color.Black, "Calibri", 20, false);
            graphedData.Plot.YAxis.TickLabelStyle(Color.Black, "Calibri", 20, false);
            //graphedData.Plot.SetAxisLimitsY(0, inputLagResults.totalInputLag.MAX + 1);

            graphedData.Plot.AddHorizontalLine(inputLagResults.onDisplayLatency.AVG, Color.DarkGreen, 5);

            graphedData.Plot.Render();
            graphedData.Plot.RenderLegend();
            graphedData.Render();
            graphedData.Refresh();
        }
        public void drawBarGraph()
        {
            rtTitle.Text = "Latency Results";
            barPlot.Plot.Clear();
            double[][] values = new double[3][ ];
            values[0] = new double[4];
            values[1] = new double[4];
            values[2] = new double[4];
            string[] titles = { "USB Polling Delay", "Render Time", "On Display Lag", "Total Input Lag" };
            string[] labels = { "AVG", "MIN", "MAX" };
            values[0][0] = inputLagResults.ClickTime.AVG;
            values[1][0] = inputLagResults.ClickTime.MIN;
            values[2][0] = inputLagResults.ClickTime.MAX;
            values[0][1] = inputLagResults.FrameTime.AVG;
            values[1][1] = inputLagResults.FrameTime.MIN;
            values[2][1] = inputLagResults.FrameTime.MAX;
            values[0][2] = inputLagResults.onDisplayLatency.AVG;
            values[1][2] = inputLagResults.onDisplayLatency.MIN;
            values[2][2] = inputLagResults.onDisplayLatency.MAX;
            values[0][3] = inputLagResults.totalInputLag.AVG;
            values[1][3] = inputLagResults.totalInputLag.MIN;
            values[2][3] = inputLagResults.totalInputLag.MAX;

            barPlot.Plot.Style(null, SystemColors.ControlDark);
            barPlot.Plot.AddBarGroups(titles, labels, values, null);
            barPlot.Plot.Legend(location: ScottPlot.Alignment.UpperLeft);
            barPlot.Plot.XAxis.Grid(false);
            barPlot.Plot.XAxis.TickLabelStyle(Color.Black, "Calibri", 24, true);
            barPlot.Plot.YAxis.TickLabelStyle(Color.Black, "Calibri", 20, false);
            barPlot.Plot.SetAxisLimitsY(0, inputLagResults.totalInputLag.MAX + 1);
            barPlot.Plot.Render();
            barPlot.Refresh();
        }
        private void switchGraphTypeBtn_Click(object sender, EventArgs e)
        {
            ScatterOption = !ScatterOption;
            
            if (ScatterOption)
            {
                switchGraphTypeBtn.Text = "Switch to Averaged Results";
                barPlot.Visible = false;
                barPlot.Enabled = false;
                barPlot.SendToBack();
                graphedData.Visible = true;
                graphedData.Enabled = true;
                graphedData.BringToFront();
                drawScatterGraph();
            }
            else
            {
                switchGraphTypeBtn.Text = "Switch to Individual Results";
                graphedData.Visible = false;
                graphedData.Enabled = false;
                graphedData.SendToBack();
                barPlot.Visible = true;
                barPlot.Enabled = true;
                barPlot.BringToFront();
                drawBarGraph();
            }
        }

        private void saveIMGBtn_Click(object sender, EventArgs e)
        {
            if (ScatterOption)
            {
                string run = CFuncs.createFileName(resultsFolderPath, "OSRTT-INPUT-LAG.png");
                Color bnColor = BackColor;
                graphedData.Plot.Style(figureBackground: Color.Transparent, dataBackground: Color.Transparent);
                graphedData.Plot.SaveFig(resultsFolderPath + "\\" + run, 1920, 1080, false);
                graphedData.Plot.Style(figureBackground: bnColor, dataBackground: bnColor);
                Process.Start("explorer.exe", resultsFolderPath);
            }
            else
            {
                string run = CFuncs.createFileName(resultsFolderPath, "OSRTT-INPUT-LAG.png");
                Color bnColor = BackColor;
                barPlot.Plot.Style(figureBackground: Color.Transparent, dataBackground: Color.Transparent);
                barPlot.Plot.SaveFig(resultsFolderPath + "\\" + run, 1920, 1080, false);
                barPlot.Plot.Style(figureBackground: bnColor, dataBackground: bnColor);
                Process.Start("explorer.exe", resultsFolderPath);
            }
        }
    }
}
