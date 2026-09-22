using System;
using MaterialSkin;
using MaterialSkin.Controls;
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
    public partial class MainWindow : MaterialForm
    {
        private double boardVersion = 2.6;
        private double V1DLFW = 2.8;
        private double ProDLFW = 1.7;
        private double ExpertDLFW = 1.0;
        public int boardType = -1;
        private string softwareVersion = "4.90";

        public static System.IO.Ports.SerialPort port;
        delegate void SetTextCallback(string text);
        private bool boardUpdate = false;
        private bool forceUpdate = false;
        private bool portConnected = false;
        private bool brightnessCheck = false;
        public bool brightnessWindowOpen = false;
        public bool brightnessCanceled = false;
        private TimeSpan uptime;

        private bool debugMode = false;
        private bool progressBarActive = false;
        private bool testRunning = false;
        private bool processingFailed = false;
        private bool ready = false;
        private bool testMode = false;
        private bool gammaTest = false;
        private bool paused = false;
        private bool testStarted = false;
        private bool triggerNextResult = false;
        private bool vsyncTrigger = false;
        private bool liveView = false;
        private bool boardCalibration = false;
        private bool latencyTest = false;
        private bool inputLagRun = false;
        public bool cancelTest = false;

        private List<int> RGBArr = new List<int> { 0, 51, 102, 153, 204, 255 };
        private List<float> RGBKeys = new List<float> { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
        private List<int> GammaArr = new List<int> { 0, 17, 34, 51, 68, 85, 102, 119, 136, 153, 170, 187, 204, 221, 238, 255 };
        private int currentStart = 0;
        private int currentEnd = 0;
        private int currentRun = 0;
        private List<float> inputLagEvents = new List<float>();

        private int potVal = 0;
        private int basePotVal = 0;
        private double timeBetween = 0.3;
        private int numberOfClicks = 20;

        string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        string localPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        string resultsFolderPath = "";
        public ProcessData.runSettings runSettings;
        private List<List<ProcessData.rawResultData>> results = new List<List<ProcessData.rawResultData>>();
        private List<ProcessData.rawResultData> singleResults = new List<ProcessData.rawResultData>();
        private List<ProcessData.rawResultData> smoothedData = new List<ProcessData.rawResultData>();
        private List<int[]> gamma = new List<int[]>();
        private List<int[]> noiseLevel = new List<int[]>();
        private List<ProcessData.rawInputLagResult> inputLagRawData = new List<ProcessData.rawInputLagResult>();
        private List<ProcessData.inputLagResult> inputLagProcessed = new List<ProcessData.inputLagResult>();
        private List<int[]> importedFile = new List<int[]>();
        private List<int> testLatency = new List<int>();

        public ProcessData.rtMethods rtMethod;
        public ProcessData.osMethods osMethod;

        private List<List<ProcessData.processedResult>> multipleRunData = new List<List<ProcessData.processedResult>>();
        private List<ProcessData.processedResult> averageData = new List<ProcessData.processedResult>();
        private List<ProcessData.gammaResult> processedGamma = new List<ProcessData.gammaResult>();
        private bool excelInstalled = false;
        private DirectX.System.DSystem testProgram;

        public MainWindow()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);




            CleanupDevTools();
        }

        private void CleanupDevTools()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                materialButton1.Visible = true;
                materialButton1.Location = new Point(418, 2);
                materialButton2.Visible = true;
                materialButton2.Location = new Point(260, 2);
            }
            else
            {
                materialButton1.Visible = false;
                materialButton1.Location = new Point(418, -50);
                materialButton2.Visible = false;
                materialButton2.Location = new Point(260, -50);
            }
        }


        private void materialButton2_Click_1(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
