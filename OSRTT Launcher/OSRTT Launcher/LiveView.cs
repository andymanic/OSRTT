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
using GlobalHotKey;
using System.Windows.Input;

namespace OSRTT_Launcher
{
    // TODO List //
    // Fix graph length, fill data by popping 

    public partial class LiveView : Form
    {
        public class LiveData
        {
            public double result { get; set; }
            public double time { get; set; }
        }
        public List<LiveData> liveData = new List<LiveData>();
        string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        string resultsFolderPath = "";
        public ProcessData.runSettings runSettings;
        public List<int> graphData = new List<int>();
        private bool running = false;
        private ResourceManager rm = OSRTT_Launcher.Properties.Resources.ResourceManager;
        double[] xData = new double[16000];
        double[] yData = new double[16000];
        List<double> xDataList = new List<double>();
        List<double> yDataList = new List<double>();
        int counter = 0;
        public Main m;
        HotKeyManager hotKeys = new HotKeyManager();
        List<HotKey> hotKeyList = new List<HotKey>();

        public void addData(LiveData d)
        {
            xDataList.Add(d.time);
            yDataList.Add(d.result);
        }
        public void copyListToArray()
        {
            try
            {
                //changeArraySize(xDataList.Count);
                 xData = xDataList.ToArray();
                yData = yDataList.ToArray();
                //Array.Copy(xDataList.ToArray(), xData, xDataList.Count);
                //Array.Copy(yDataList.ToArray(), yData, yDataList.Count);
                //yData[yData.Length] = 1000;
            }
            catch
            {
                Console.WriteLine("Array length probably exceeded.");
            }
        }
        delegate void renderGraphCallback();
        public void renderGraph()
        {
            if (this.InvokeRequired)
            {
                var del = new renderGraphCallback(renderGraph);
                this.Invoke(del);
            }
            drawGraph();
            //graphedData.Plot.AxisAuto(0,0.1);
            //graphedData.Render(false, true);
            //graphedData.Refresh();
            //mainLabel.Visible = false;
        }

        public void clearData()
        {
            xDataList.Clear();
            yDataList.Clear();
            for (int i = 0; i < xData.Length; i++)
            {
                xData[i] = 0;
                yData[i] = 0;
            }
            graphedData.Plot.Clear(typeof(ScottPlot.Plottable.HSpan));
            graphedData.Plot.Clear(typeof(ScottPlot.Plottable.VSpan));
            graphedData.Render();
            counter = 0;
        }
        delegate void changeStateCallback(bool state);
        
        public void changeState(bool state)
        {
            if (this.InvokeRequired)
            {
                var del = new changeStateCallback(changeState);
                this.Invoke(del, state);
            }
            rtPanel.Enabled = state;
            osPanel.Enabled = state;
            potValPanel.Enabled = state;
            saveAsPNGBtn.Enabled = state;
            saveGraphNoHSpanBtn.Enabled = state;
            startStopBtn.Enabled = state;
            graphedData.Enabled = state;
        }

        public LiveView()
        {
            InitializeComponent();
            this.FormClosing += LiveView_Closing;
            this.Icon = (Icon)rm.GetObject("osrttIcon");
            path = new Uri(System.IO.Path.GetDirectoryName(path)).LocalPath;
            path += @"\Results\Live Data";
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            Size = new Size(1662, 883);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#2e3440");
        }
        private void ResultsView_Load(object sender, EventArgs e)
        {
            xDataList.Add(0);
            yDataList.Add(0);
            drawGraph();
            //m.sendText("X");
            hotKeys.KeyPressed += HotKeyPressed;
            var k = hotKeys.Register(Key.F10, System.Windows.Input.ModifierKeys.None);
            hotKeyList.Add(k);
        }
        
        
        private void changeArraySize(int s)
        {
            Array.Resize(ref xData, s);
            Array.Resize(ref yData, s);
        }
        private void drawGraph()
        {
            graphedData.Plot.Clear();
            graphedData.Plot.Palette = ScottPlot.Palette.OneHalfDark;
            graphedData.Plot.AxisAuto(0, 0.1);
            
            //graphedData.Plot.SetOuterViewLimits(0, xMax, yMin, yMax);
            graphedData.Plot.Style(ScottPlot.Style.Gray1);
            //var bnColor = System.Drawing.ColorTranslator.FromHtml("#2e3440");
            var bnColor = Color.Transparent;
            graphedData.Plot.Style(figureBackground: bnColor, dataBackground: bnColor);
            graphedData.Plot.Title("Live Data View");
            graphedData.Plot.YLabel("Light level (16 bit integer)");
            graphedData.Plot.XLabel("Time (ms)");
            graphedData.Plot.AddScatter(xData, yData, lineWidth: 3, markerSize: 4);
            //graphedData.Plot.SetAxisLimits(0, (timeData.Length + 100), 0, (resultData.Max() + 100));
            graphedData.Plot.Render();
            graphedData.Refresh();
            //showProcessedData();
        }
        private void onSpanDrag(object sender, EventArgs e)
        {
            var hSpan = sender as ScottPlot.Plottable.HSpan;
            double startTime = hSpan.X1;
            double endTime = hSpan.X2;
            double newTime = endTime - startTime;
            if (startTime > 100)
            {
                startTime = Math.Round(startTime, 0);
            }
            else
            {
                startTime = Math.Round(startTime, 1);
            }
            if (endTime > 100)
            {
                endTime = Math.Round(endTime, 0);
            }
            else
            {
                endTime = Math.Round(endTime, 1);
            }
            if (newTime > 100)
            {
                newTime = Math.Round(newTime, 0);
            }
            else
            {
                newTime = Math.Round(newTime, 1);
            }
            startLabel.Text = startTime.ToString();
            endLabel.Text = endTime.ToString();
            rtLabel.Text = newTime.ToString() + " ms";
            
            Console.WriteLine(hSpan.X1 + "," + hSpan.X2);
        }
        private void onVSpanDrag(object sender, EventArgs e)
        {
            var vSpan = sender as ScottPlot.Plottable.VSpan;
            double startTime = vSpan.Y1;
            double endTime = vSpan.Y2;
            double newTime = endTime - startTime;
            newTime /= startTime;
            newTime *= 100;
            if (startTime > 100)
            {
                startTime = Math.Round(startTime, 0);
            }
            else
            {
                startTime = Math.Round(startTime, 1);
            }
            if (endTime > 100)
            {
                endTime = Math.Round(endTime, 0);
            }
            else
            {
                endTime = Math.Round(endTime, 1);
            }
            if (newTime > 100)
            {
                newTime = Math.Round(newTime, 0);
            }
            else
            {
                newTime = Math.Round(newTime, 1);
            }
            osStartLbl.Text = startTime.ToString();
            osEndLbl.Text = endTime.ToString();
            osMeasureLbl.Text = newTime.ToString() + "%";

            Console.WriteLine(vSpan.Y1 + "," + vSpan.Y2);
        }

        private void saveAsPNGBtn_Click(object sender, EventArgs e)
        {
            /*if (runSelectBox.SelectedIndex != null)
            {
                run = runSelectBox.Items[runSelectBox.SelectedIndex].ToString();
            }
            if (transSelect1.SelectedIndex != null)
            {
                result = transSelect1.Items[transSelect1.SelectedIndex].ToString();
            }*/
            Color bnColor = BackColor;
            DateTime d = DateTime.Now;
            graphedData.Plot.Style(figureBackground: Color.Transparent, dataBackground: Color.Transparent);
            graphedData.Plot.SaveFig(path + "\\" + d.ToString("yyMMdd-HHmmss") + "-Live-OSRTT.png", 1920, 1080, false);
            graphedData.Plot.Style(figureBackground: bnColor, dataBackground: bnColor);
            Process.Start("explorer.exe", resultsFolderPath);
        }

        private void saveGraphNoHSpanBtn_Click(object sender, EventArgs e)
        {
            //if (runSelectBox.SelectedIndex != null)
            //{
            //    run = runSelectBox.Items[runSelectBox.SelectedIndex].ToString();
            //}
            //if (transSelect1.SelectedIndex != null)
            //{
            //    result = transSelect1.Items[transSelect1.SelectedIndex].ToString();
            //}
            var plots = graphedData.Plot.GetPlottables();
            foreach (var i in plots)
            {
                if (i.ToString().Contains("span"))
                {
                    i.IsVisible = false;
                }
            }
            Color bnColor = BackColor;
            DateTime d = DateTime.Now;
            graphedData.Plot.Style(figureBackground: Color.Transparent, dataBackground: Color.Transparent);
            graphedData.Plot.SaveFig(path + "\\" + d.ToString("yyMMdd-HHmmss") + "-Live-OSRTT.png", 1920, 1080, false);
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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            if (n.Focused)
            {
                var v = numericUpDown1.Value;
                int vInt = Convert.ToInt32(v); // doesn't like converting to hex from decimal. Ironically.
                string potVal = vInt.ToString("X");
                m.sendText(potVal);
            }
        }
       
        public void startStopBtn_Click(object sender, EventArgs e)
        { 
            running = !running;
            string t = "RECORD";
            string lblT = "Press RECORD or F10 to capture 3s of data";
            bool showLabel = false;
            Color c = Color.LimeGreen;
            // change state
            changeState(!running);
            if (running)
            {
                t = "RUNNING";
                lblT = "Capturing data...";
                showLabel = true;
                c = Color.IndianRed;
                clearData();
                m.sendText("P"); // start the testing
            }
            else
            {
                t = "RECORD";
                showLabel = false;
                lblT = "Press RECORD or F10 to capture 3s of data";
                c = Color.LimeGreen;
                // m.sendText("P"); // stop the testing
            }
            if (startStopBtn.InvokeRequired || mainLabel.InvokeRequired)
            {
                startStopBtn.Invoke((MethodInvoker)(() => startStopBtn.Text = t));
                startStopBtn.Invoke((MethodInvoker)(() => startStopBtn.BackColor = c));
                mainLabel.Invoke((MethodInvoker)(() => mainLabel.Text = lblT));
                mainLabel.Invoke((MethodInvoker)(() => mainLabel.Visible = showLabel));
            }
            else
            {
                startStopBtn.Text = t;
                startStopBtn.BackColor = c;
                mainLabel.Text = lblT;
                mainLabel.Visible = showLabel;
            }

        }
        private void clearBtn_Click(object sender, EventArgs e)
        {
            clearData();
        }

        private void LiveView_Closing(object sender, FormClosingEventArgs e)
        {
            m.sendText("X");
            foreach (HotKey h in hotKeyList)
            { hotKeys.Unregister(h); }
            hotKeys.Dispose();
            m.exitLiveView();
        }

        private void HotKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.HotKey.Key == Key.F10)
            {
                if (!running)
                {
                    startStopBtn_Click(null, null);
                }
            }
        }

        private void verticalSpanBtn_Click(object sender, EventArgs e)
        {
            double max = yData.Max();
            double min = yData.Min();

            double start = max - min;
            double end = max - min;
            start /= 2.2;
            start = Math.Round(start, 2);
            end /= 1.8;
            end = Math.Round(end, 2);
            var vSpan = graphedData.Plot.AddVerticalSpan(start, end);
            // graphedData.Plot.AddHorizontalSpan(start, end);
            vSpan.DragEnabled = true;
            vSpan.Dragged += new EventHandler(onVSpanDrag);
            graphedData.Plot.Render();
            graphedData.Refresh();
        }

        private void addRemoveSpanBtn_Click(object sender, EventArgs e)
        {
            double start = (xData.Max() - xData.Min()) / 2.2;
            double end = (xData.Max() - xData.Min()) / 1.8;
            start = Math.Round(start, 2);
            end = Math.Round(end, 2);
            var hSpan = graphedData.Plot.AddHorizontalSpan(start, end);
            // graphedData.Plot.AddHorizontalSpan(start, end);
            hSpan.DragEnabled = true;
            hSpan.Dragged += new EventHandler(onSpanDrag);
            graphedData.Plot.Render();
            graphedData.Refresh();
        }

        private void clearHSpanBtn_Click(object sender, EventArgs e)
        {
            graphedData.Plot.Clear(typeof(ScottPlot.Plottable.HSpan));
            graphedData.Render();
        }

        private void cleanVSpanBtn_Click(object sender, EventArgs e)
        {
            graphedData.Plot.Clear(typeof(ScottPlot.Plottable.VSpan));
            graphedData.Render();
        }

        private void zoomToFitBtn_Click(object sender, EventArgs e)
        {
            graphedData.Plot.AxisAuto(0, 0.1);
            graphedData.Refresh();
        }

        private void recordToolStripBtn_Click(object sender, EventArgs e)
        {
            startStopBtn_Click(sender, e);
        }

        private void clearDataToolStripBtn_Click(object sender, EventArgs e)
        {
            clearBtn_Click(sender, e);
        }

        private void zoomToFitToolStripBtn_Click(object sender, EventArgs e)
        {
            zoomToFitBtn_Click(sender, e);
        }

        private void saveAsPNGToolStripBtn_Click(object sender, EventArgs e)
        {
            saveAsPNGBtn_Click(sender, e);
        }
    }
}
