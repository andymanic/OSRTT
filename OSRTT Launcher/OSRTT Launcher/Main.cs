using AutoUpdaterDotNET;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using SharpDX.RawInput;
using System.Reflection;

namespace OSRTT_Launcher
{
    public partial class Main : Form
    {
        // CHANGE THESE VALUES WHEN ISSUING A NEW RELEASE
        private double boardVersion = 2.6;
        private double V1DLFW = 2.8;
        private double ProDLFW = 1.7;
        public int boardType = -1;
        private string softwareVersion = "4.7";

        // TODO //
        //
        //
        // Current known issues //
        //

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

        private List<int> RGBArr = new List<int>{0, 51, 102, 153, 204, 255};
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

        public class Displays
        {
            public string Name { get; set; }
            public int Freq { get; set; }
            public string Resolution { get; set; }
            public string Connection { get; set; }
            public string ManufacturerCode { get; set; }
            public string EDIDModel { get; set; }
        }
        public List<Displays> displayList = new List<Displays>();
        public class FPS
        {
            public string FPSValue { get; set; }
            public string Key { get; set; }
            public string CKey { get; set; }
        }
        public List<FPS> fpsList = new List<FPS>();
        private BackgroundWorker hardWorker;
        private Thread readThread = null;
        private Thread connectThread = null;
        private Thread processThread = null;
        private Thread launchGameThread = null;
        private Thread checkWindowThread = null;
        private Thread runTestThread = null;

        private ResourceManager rm = OSRTT_Launcher.Properties.Resources.ResourceManager;

        private ContextMenu contextMenu1 = new ContextMenu();
        private MenuItem statusTrayBtn = new MenuItem();
        private MenuItem openTrayBtn = new MenuItem();
        private MenuItem closeTrayBtn = new MenuItem();

        LiveView LiveViewObject;

        public void UpdateMe()
        {
            AutoUpdater.InstalledVersion = new Version(softwareVersion);
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Days;
            AutoUpdater.RemindLaterAt = 2;
            AutoUpdater.RunUpdateAsAdmin = false;
            AutoUpdater.HttpUserAgent = "Autoupdater";
            AutoUpdater.UpdateFormSize = new System.Drawing.Size(1200, 800);
            AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
            AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;
            AutoUpdater.Start("https://api.github.com/repos/andymanic/OSRTT/releases/latest");
        }

        private void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            dynamic json = JsonConvert.DeserializeObject(args.RemoteData);
            string dlUrl = "";
            foreach (var a in json.assets)
            {
                string tmp = a.browser_download_url;
                if (tmp.Contains(".zip"))
                {
                    dlUrl = tmp;
                }
            }
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                CurrentVersion = json.tag_name,
                ChangelogURL = json.html_url,
                DownloadURL = dlUrl,
            };
        }
        private void AutoUpdater_ApplicationExitEvent()
        {
            Properties.Settings.Default.updateInProgress = true;
            Properties.Settings.Default.Save();
            notifyIcon.Visible = false;
            if (port != null)
            {
                try
                {
                    port.Write("X");
                }
                catch { Console.WriteLine("Port not open"); }
            }
            Process[] p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
            if (p.Length != 0)
            {
                p[0].Kill();
            }
            if (checkWindowThread != null)
            {
                checkWindowThread.Abort();
            }
            if (connectThread != null)
            {
                connectThread.Abort();
            }
            if (readThread != null)
            {
                readThread.Abort();
            }
            if (port != null)
            {
                port.Close();
            }
            Application.Exit();
        }

        private void getODModesJson()
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "OSRTT");
                string jsonPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                jsonPath = new Uri(System.IO.Path.GetDirectoryName(jsonPath)).LocalPath + @"\lib\odmodes.json";
                try
                {
                    client.DownloadFile("https://raw.githubusercontent.com/andymanic/OSRTT/main/OSRTT%20Launcher/OSRTT%20Launcher/odmodes.json", jsonPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
            }
        }

        private void appRunning()
        {
            Process[] p = Process.GetProcessesByName("OSRTT Launcher");
            if (p.Length > 1)
            {
                MessageBox.Show("ERROR: Program already open! Please close it before running again, or check the task bar and system tray for the running app.", "Program Open Already", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(p.Length);
                //this.Close();
            }
        }

        private void CleanupDevTools()
        {
            if (System.Diagnostics.Debugger.IsAttached) { 
                testButtonMenuItem.Visible = true;
                toolStripMenuItem1.Visible = true;
            }
            else { 
                testButtonMenuItem.Visible = false;
                toolStripMenuItem1.Visible = false;
            }
        }

        private void setupFormElements()
        {
            this.Icon = (Icon)rm.GetObject("osrttIcon");
            notifyIcon.Icon = (Icon)rm.GetObject("osrttIcon");
            this.contextMenu1.MenuItems.AddRange(new MenuItem[] { this.statusTrayBtn });
            this.contextMenu1.MenuItems.AddRange(new MenuItem[] { this.openTrayBtn });
            this.contextMenu1.MenuItems.AddRange(new MenuItem[] { this.closeTrayBtn });
            this.statusTrayBtn.Index = 0;
            this.openTrayBtn.Index = 1;
            this.closeTrayBtn.Index = 2;
            this.statusTrayBtn.Enabled = false;
            this.statusTrayBtn.Text = "Waiting for connection";
            this.openTrayBtn.Text = "Open Program";
            this.closeTrayBtn.Text = "Close Program";
            this.openTrayBtn.Click += new System.EventHandler(this.openTrayBtn_Click);
            this.closeTrayBtn.Click += new System.EventHandler(this.quitTrayBtn_Click);
            notifyIcon.ContextMenu = contextMenu1;
            notifyIcon.Visible = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.softVerLbl.Text = "V" + softwareVersion;
            this.firmVerLbl.Text = "N/A";
            this.richTextBox1.TextChanged += richTextBox1_TextChanged;
        }

        private void initialiseSettings()
        {
            testCount.Value = Properties.Settings.Default.Runs;
            saveUSBOutputToolStripMenuItem.Checked = Properties.Settings.Default.USBOutput;
            minimiseToTrayToolStripMenuItem.Checked = Properties.Settings.Default.MinToTray;
            timeBetween = Properties.Settings.Default.timeBetween;
            timeBetweenLabel.Text = timeBetween.ToString();
            timeBetweenSlider.Value = Convert.ToInt32(timeBetween * 2);
            numberOfClicks = Properties.Settings.Default.numberOfClicks;
            numberOfClicksLabel.Text = numberOfClicks.ToString();
            numberOfClicksSlider.Value = numberOfClicks;

        }

        class userSettings
        {
            public class usersetting
            {
                public string name { get; set; }
                public string value { get; set; }
            }
            public List<usersetting> usersettings { get; set; } = new List<usersetting>();
        }
        private void readAndSaveUserSettings(bool closing)
        {
            string UserSettingsFile = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            UserSettingsFile = new Uri(System.IO.Path.GetDirectoryName(UserSettingsFile)).LocalPath;
            UserSettingsFile += @"\\userSettings.json";
            try
            {
                if (closing)
                {
                    // write settings to file
                    userSettings us = new userSettings();
                    foreach (System.Configuration.SettingsProperty s in Properties.Settings.Default.Properties)
                    {
                        us.usersettings.Add(new userSettings.usersetting {name = s.Name, value = Properties.Settings.Default[s.Name].ToString() });
                    }
                    string jsonData = JsonConvert.SerializeObject(us);
                    File.WriteAllText(UserSettingsFile, jsonData);
                }
                else if (File.Exists(UserSettingsFile))
                {
                    // opening program, read settings from file
                    string contents = File.ReadAllText(UserSettingsFile);
                    userSettings settings = JsonConvert.DeserializeObject<userSettings>(contents);
                    foreach (userSettings.usersetting s in settings.usersettings)
                    {
                        try
                        {
                            Type t = Properties.Settings.Default[s.name].GetType();
                            Properties.Settings.Default[s.name] = Convert.ChangeType(s.value, t);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + e.StackTrace);
                        }
                    }
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
        private void getDownloadedFirmwareVersions()
        {
            string ProFirmwareVersion = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "OSRTT");
                
                try
                {
                    string latest = wc.DownloadString("https://api.github.com/repos/andymanic/OSRTT/releases/latest");
                    //object jsonData = JsonConvert.DeserializeObject(latest);
                    //Console.WriteLine(jsonData);
                    string[] splitMessage = latest.Split('}');
                    foreach (var s in splitMessage)
                    {
                        if (s.Contains("browser_download_url"))
                        {
                            string[] splitForUrl = s.Split('"');
                            foreach (var st in splitForUrl)
                            {
                                if (st.StartsWith("https://github.com/") && st.Contains(".ino.bin")) // change to .ino.bin for prod
                                {
                                    ProFirmwareVersion = st;
                                    break;
                                }
                            }
                            if (ProFirmwareVersion != "")
                            {
                                break;
                            }
                        }
                    }
                    if (ProFirmwareVersion != "")
                    {
                        string[] fileSplit = ProFirmwareVersion.Split('/');
                        wc.DownloadFile(ProFirmwareVersion, localPath + @"\\arduinoCLI\\" + fileSplit.Last());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }

            }
            Console.WriteLine("");
            string V1FWPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            V1FWPath = new Uri(System.IO.Path.GetDirectoryName(V1FWPath)).LocalPath;
            string ProFWPath = V1FWPath;
            string ProNewFWPath = V1FWPath;
            V1FWPath += @"\\arduinoCLI\\OSRTT_Full_Code\\OSRTT_Full_Code.ino";
            ProFWPath += @"\\arduinoCLI\\OSRTT_Pro_Code\\file2.ino";
            ProNewFWPath += @"\\arduinoCLI";
            try
            {
                if (File.Exists(V1FWPath))
                {
                    foreach (var l in File.ReadAllLines(V1FWPath))
                    {
                        if (l.Contains("firmware"))
                        {
                            string[] splitLine = l.Split('"');
                            V1DLFW = double.Parse(splitLine[1]);
                            break;
                        }
                    }
                }
                if (File.Exists(ProNewFWPath))
                {
                    foreach (var f in Directory.GetFiles(ProNewFWPath))
                    {
                        if (f.Contains(".ino.bin") && f.Contains("Pro"))
                        {
                            var splitName = f.Split('_');
                            var splitVersion = splitName.Last().Remove(3);
                            ProDLFW = double.Parse(splitVersion);
                        }
                    }
                }
                else if (File.Exists(ProFWPath))
                {
                    foreach (var l in File.ReadAllLines(ProFWPath))
                    {
                        if (l.Contains("firmware"))
                        {
                            string[] splitLine = l.Split('"');
                            ProDLFW = double.Parse(splitLine[1]);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

        }

        public Main()
        {
            InitializeComponent();
            UpdateMe();
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            Thread.CurrentThread.CurrentCulture = customCulture;
            setupFormElements();
            SetDeviceStatus(0);
            ControlDeviceButtons(false);
            path = new Uri(System.IO.Path.GetDirectoryName(path)).LocalPath;
            path += @"\Results";
            localPath = new Uri(System.IO.Path.GetDirectoryName(path)).LocalPath;
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            this.FormClosing += new FormClosingEventHandler(Main_FormClosing);
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(IFailedYou);
            this.Resize += new EventHandler(Main_Resize);
            readAndSaveUserSettings(false);
            initialiseSettings();
            hardWorker = new BackgroundWorker();
            connectThread = new Thread(new ThreadStart(this.findAndConnectToBoard));
            connectThread.Start();
            changeSizeAndState("standard");
            listMonitors(0);
            listFramerates();
            listCaptureTimes();
            listVsyncState();
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
            initialSetup();
            checkFolderPermissions();
            getDownloadedFirmwareVersions();
            uptime = GetUpTime();
            if (uptime.TotalMinutes < 30)
            {
                showMessageBox("It is HIGHLY RECOMMENDED you allow the display to warm up BEFORE testing - it looks like your system hasn't been running for more than 30 minutes." +
                    "\n You are welcome to continue, but it's recommended you allow the display to run for around 30 minutes before testing.","Monitor Warm Up",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            Thread jsonThread = new Thread(new ThreadStart(getODModesJson));
            jsonThread.Start();
            CleanupDevTools();
            addKeys();
        }

        private void initialSetup()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var samdCore = appData + "\\Arduino15\\packages\\adafruit\\hardware\\samd";
            Console.WriteLine(samdCore);
            if (!Directory.Exists(samdCore))
            {
                DialogResult d = MessageBox.Show("Further setup is required to connect and update device, do that now?", "Setup Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d == DialogResult.Yes)
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/C .\\arduinoCLI\\arduino-cli.exe config init";
                    //process.StartInfo.UseShellExecute = false;
                    //process.StartInfo.RedirectStandardOutput = true;
                    //process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    //string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    //Console.WriteLine(output);
                    process.StartInfo.Arguments = "/C .\\arduinoCLI\\arduino-cli.exe config set directories.user \"C:\\OSRTT Launcher\\arduinoCLI\"";
                    process.Start();
                    process.WaitForExit();
                    process.StartInfo.Arguments = "/C .\\arduinoCLI\\arduino-cli.exe config add board_manager.additional_urls https://adafruit.github.io/arduino-board-index/package_adafruit_index.json";
                    process.Start();
                    process.WaitForExit();
                    process.StartInfo.Arguments = "/C .\\arduinoCLI\\arduino-cli.exe core update-index && .\\arduinoCLI\\arduino-cli.exe core install arduino:samd && .\\arduinoCLI\\arduino-cli.exe core install adafruit:samd";
                    process.Start();
                    process.WaitForExit();
                    process.StartInfo.Arguments = "/C .\\arduinoCLI\\arduino-cli.exe lib install Keyboard && .\\arduinoCLI\\arduino-cli.exe lib install Mouse && .\\arduinoCLI\\arduino-cli.exe lib install ArduinoUniqueID";
                    process.Start();
                    process.WaitForExit();
                }
            }
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\excel.exe");
            if (key != null)
            {
                key.Close();
                excelInstalled = true;
            }
            else
            {
                if (Properties.Settings.Default.saveXLSX || Properties.Settings.Default.saveGraphs)
                {
                    showMessageBox("Warning: Excel doesn't seem to be installed. Saving to the XLSX or XLSM templates (results or graph view templates) won't work and have been disabled.","Excel Not Installed",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    Properties.Settings.Default.saveXLSX = false;
                    Properties.Settings.Default.saveGraphs = false;
                    Properties.Settings.Default.Save();
                }
            }
        }
        private void Main_Resize(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.MinToTray)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    Hide();
                    notifyIcon.Visible = true;
                }
            }
        }
        private static IEnumerable<EventRecord> ReadEventsReverse(string logName)
        {
            using (
                var reader = new EventLogReader(
                    new EventLogQuery(logName, PathType.LogName) { ReverseDirection = true }
                )
            )
            {
                EventRecord eventRecord;
                while ((eventRecord = reader.ReadEvent()) != null)
                {
                    yield return eventRecord;
                }
            }
        }
       
        public static TimeSpan GetUpTime()
        {
            /*var reverseEvents = Main.ReadEventsReverse("Microsoft-Windows-Kernel-PnP*");
            var reverseEventsToday = reverseEvents.TakeWhile(e => e.TimeCreated >= DateTime.Now.Date);
            foreach (var eventRecord in reverseEventsToday)
            {
                Console.WriteLine("{0:s} {1}", eventRecord.TimeCreated, eventRecord.FormatDescription());
            }*/
            return TimeSpan.FromMilliseconds(GetTickCount64());
        }
        [DllImport("kernel32")]
        extern static UInt64 GetTickCount64();

        private void listMonitors(int selected)
        {
            monitorCB.Items.Clear(); // Clear existing array and list before filling them
            displayList.Clear();
            var i = WindowsDisplayAPI.Display.GetDisplays();
            
            foreach (var target in WindowsDisplayAPI.DisplayConfig.PathInfo.GetActivePaths())
            {
                foreach (var item in target.TargetsInfo)
                {
                    try
                    {
                        string con = "Other";
                        if (item.OutputTechnology.ToString() == "DisplayPortExternal")
                        {
                            con = "DP";
                        }
                        else if (item.OutputTechnology.ToString() == "HDMI")
                        {
                            con = "HDMI";
                        }
                        double refreshRate = item.FrequencyInMillihertz;
                        refreshRate /= 1000;
                        refreshRate = Math.Round(refreshRate, 0);
                        int refresh = (int)refreshRate;
                        string name = item.DisplayTarget.ToString();
                        string manCode = "";
                        if (name == "")
                        {
                            name = target.DisplaySource.ToString().Remove(0, 4);
                        }
                        else { manCode = item.DisplayTarget.EDIDManufactureCode; }
                        string res = "";
                        try
                        {
                            res = item.DisplayTarget.PreferredResolution.Width.ToString() + "x" + item.DisplayTarget.PreferredResolution.Height.ToString();
                        }
                        catch { }
                        if (res == "")
                        {
                            res = "Failed to Aquire";
                        }
                        string edidCode = item.DisplayTarget.EDIDProductCode.ToString();
                        var data = new Displays { Name = name, Freq = refresh, Resolution = res, Connection = con, ManufacturerCode = manCode, EDIDModel = edidCode };
                        displayList.Add(data);
                        monitorCB.Items.Add(name);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ex.StackTrace);
                    }
                }
            }
            monitorCB.SelectedIndex = selected; // Pre-select the primary display
        }

        private void addKeys()
        {
            FieldInfo info = typeof(SendKeys).GetField("keywords",
            BindingFlags.Static | BindingFlags.NonPublic);
            Array oldKeys = (Array)info.GetValue(null);
            Type elementType = oldKeys.GetType().GetElementType();
            Array newKeys = Array.CreateInstance(elementType, oldKeys.Length + 10);
            Array.Copy(oldKeys, newKeys, oldKeys.Length);
            for (int i = 0; i < 10; i++)
            {
                var newItem = Activator.CreateInstance(elementType, "NUM" + i, (int)Keys.NumPad0 + i);
                newKeys.SetValue(newItem, oldKeys.Length + i);
            }
            info.SetValue(null, newKeys);
        }
        private void listFramerates()
        {
            fpsLimitList.Items.Clear();
            fpsList.Clear();
            fpsList.Add(new FPS { FPSValue = "1000", Key = "49", CKey = "1" });
            fpsList.Add(new FPS { FPSValue = "540", Key = "234" , CKey="{NUM0}" });
            fpsList.Add(new FPS { FPSValue = "500", Key = "225", CKey="{NUM1}" });
            fpsList.Add(new FPS { FPSValue = "480", Key = "226", CKey="{NUM2}" });
            fpsList.Add(new FPS { FPSValue = "360", Key = "50", CKey="2" });
            fpsList.Add(new FPS { FPSValue = "240", Key = "51", CKey = "3" });
            fpsList.Add(new FPS { FPSValue = "170", Key = "52", CKey = "4" });
            fpsList.Add(new FPS { FPSValue = "165", Key = "53", CKey = "5" });
            fpsList.Add(new FPS { FPSValue = "144", Key = "54", CKey = "6" });
            fpsList.Add(new FPS { FPSValue = "120", Key = "55", CKey = "7" });
            fpsList.Add(new FPS { FPSValue = "100", Key = "56", CKey = "8" });
            fpsList.Add(new FPS { FPSValue = "75", Key = "57", CKey = "9" });
            fpsList.Add(new FPS { FPSValue = "60", Key = "48", CKey = "0" });
            foreach (var f in fpsList)
            {
                fpsLimitList.Items.Add(f.FPSValue);
            }
            fpsLimitList.SelectedIndex = Properties.Settings.Default.FPS;
        }

        private void listCaptureTimes()
        {
            captureTimeBox.Items.Clear();
            captureTimeBox.Items.Add("50ms");
            captureTimeBox.Items.Add("100ms");
            captureTimeBox.Items.Add("150ms");
            captureTimeBox.Items.Add("200ms");
            captureTimeBox.Items.Add("250ms");
            captureTimeBox.SelectedIndex = Properties.Settings.Default.captureTime;
        }

        private void listVsyncState()
        {
            vsyncStateList.Items.Clear();
            vsyncStateList.Items.Add("Disabled");
            vsyncStateList.Items.Add("Enabled");
            if (Properties.Settings.Default.VSyncState)
            {
                vsyncStateList.SelectedIndex = 1;
            }
            else
            {
                vsyncStateList.SelectedIndex = 0;
            }
        }

        private void checkFolderPermissions()
        {
            string filePath = path + "\\permissionsTest";
            bool test = false;
            try
            {
                Directory.CreateDirectory(filePath);
                test = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Access to the path"))
                {
                    MessageBox.Show("Permissions Error - program unable to create new results folders, please relaunch the program as admin.", "Permissions Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            finally
            {
                if (test)
                {
                    Directory.Delete(filePath);
                }
            }
        }

        private void findAndConnectToBoard()
        {
            Thread.Sleep(1000);
            bool checkedRunning = false;
            while (true)
            {
                if (!portConnected)
                {
                    ControlDeviceButtons(false);
                    SetDeviceStatus(0);
                    testRunning = false;
                    testStarted = false;
                    testMode = false;
                    boardUpdate = false;
                    portConnected = false;
                    brightnessCheck = false;
                    brightnessWindowOpen = false;
                    brightnessCanceled = false;
                    if (this.firmVerLbl.IsHandleCreated)
                    {
                        this.firmVerLbl.Invoke((MethodInvoker)(() => this.firmVerLbl.Text = "N/A"));
                    }
                    if (this.boardSerialLbl.IsHandleCreated)
                    {
                        this.boardSerialLbl.Invoke((MethodInvoker)(() => this.boardSerialLbl.Text = "Not Connected"));
                    }
                    testRunning = false;
                    if (!Properties.Settings.Default.updateInProgress)
                    {
                        if (!checkedRunning)
                        {
                            appRunning();
                            checkedRunning = true;
                        }
                    }
                    else
                    {
                        Properties.Settings.Default.updateInProgress = false;
                        Properties.Settings.Default.Save();
                    }
                    Process[] game = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                    if (game.Length != 0)
                    {
                        game[0].Kill();
                    }
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/C .\\arduinoCLI\\arduino-cli.exe board list";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    string[] lines = output.Split(
                        new[] { "\r\n", "\r", "\n" },
                        StringSplitOptions.None
                    );
                    string p = "";
                    foreach (var s in lines)
                    {
                        if (s.Contains("adafruit:samd:adafruit_itsybitsy_m4") || s.Contains("adafruit:samd:adafruit_feather_m4"))
                        {
                            char[] whitespace = new char[] { ' ', '\t' };
                            string[] split = s.Split(whitespace);
                            p = split[0];
                        }
                        if (s.Contains("adafruit:samd:adafruit_feather_m4"))
                        {
                            //boardType = 2; // probably not needed 
                        }
                    }
                    if (p != "")
                    {
                        try
                        {
                            connectToBoard(p);
                            Thread.Sleep(1000);
                            SetDeviceStatus(1);
                            ControlDeviceButtons(true);
                            setBoardSerial();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            SetText(e.Message + e.StackTrace);
                        }
                    }
                }
                else if (boardUpdate)
                {
                    if ((boardVersion < V1DLFW && boardType == 0) || forceUpdate || (boardVersion < ProDLFW && boardType == 1))
                    {
                        string p = ""; 
                        p = port.PortName;
                        if (port.IsOpen)
                        {
                            ControlDeviceButtons(false);
                            // readThread.Abort();
                            port.Close();
                        }                        
                        if (p == "")
                        {
                            MessageBox.Show("Please connect to the device first!", "Update Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            SetDeviceStatus(2);
                            setProgressBar(true);
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            process.StartInfo.FileName = "cmd.exe";
                            string installCommand = "";
                            string updateCommand = "";
                            if (boardType == 1)
                            {
                                string binFileAvailable = "";
                                foreach (var f in Directory.GetFiles(localPath + @"\\arduinoCLI")) 
                                {
                                    if (f.Contains("ino.bin") && f.Contains("Pro")) { binFileAvailable = f; }
                                }
                                if (binFileAvailable != "")
                                {
                                    Console.WriteLine(binFileAvailable);
                                    installCommand = "";
                                    updateCommand = "/C .\\arduinoCLI\\arduino-cli.exe upload --port " + p + " --fqbn adafruit:samd:adafruit_itsybitsy_m4 -i \""+ binFileAvailable + "\"";
                                    Console.WriteLine(updateCommand);
                                }
                                else
                                {
                                    installCommand = "/C .\\arduinoCLI\\arduino-cli.exe lib install Keyboard && .\\arduinoCLI\\arduino-cli.exe lib install Mouse && .\\arduinoCLI\\arduino-cli.exe lib install ArduinoUniqueID" +
                                    "&& .\\arduinoCLI\\arduino-cli.exe lib install Adafruit_SSD1306 && .\\arduinoCLI\\arduino-cli.exe lib install Adafruit_GFX_Library";
                                    updateCommand = "/C .\\arduinoCLI\\arduino-cli.exe compile --fqbn adafruit:samd:adafruit_itsybitsy_m4 .\\arduinoCLI\\OSRTT_Pro_Code && .\\arduinoCLI\\arduino-cli.exe upload --port " + p + " --fqbn adafruit:samd:adafruit_itsybitsy_m4 .\\arduinoCLI\\OSRTT_Pro_Code";
                                }
                            }
                            else if (boardType == 0)
                            {
                                string binFileAvailable = "";
                                foreach (var f in Directory.GetFiles(localPath + @"\\arduinoCLI"))
                                {
                                    if (f.Contains("ino.bin") && f.Contains("Full")) { binFileAvailable = f; }
                                }
                                if (binFileAvailable != "")
                                {
                                    Console.WriteLine(binFileAvailable);
                                    installCommand = "";
                                    updateCommand = "/C .\\arduinoCLI\\arduino-cli.exe upload --port " + p + " --fqbn adafruit:samd:adafruit_itsybitsy_m4 -i \"" + binFileAvailable + "\"";
                                    Console.WriteLine(updateCommand);
                                }
                                else
                                {
                                    installCommand = "/C .\\arduinoCLI\\arduino-cli.exe lib install Keyboard && .\\arduinoCLI\\arduino-cli.exe lib install Mouse && .\\arduinoCLI\\arduino-cli.exe lib install ArduinoUniqueID";
                                    updateCommand = "/C .\\arduinoCLI\\arduino-cli.exe compile --fqbn adafruit:samd:adafruit_itsybitsy_m4 .\\arduinoCLI\\OSRTT_Full_Code && .\\arduinoCLI\\arduino-cli.exe upload --port " + p + " --fqbn adafruit:samd:adafruit_itsybitsy_m4 .\\arduinoCLI\\OSRTT_Full_Code";
                                }
                            }
                            Console.WriteLine("ready to start");
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.RedirectStandardOutput = true;
                            process.StartInfo.CreateNoWindow = true;
                            if (installCommand == "")
                            {
                                process.StartInfo.Arguments = updateCommand;
                            }
                            else
                            {
                                process.StartInfo.Arguments = installCommand;
                                process.Start();
                                process.WaitForExit();
                            }
                            process.StartInfo.Arguments = updateCommand;
                            try
                            {
                                Console.WriteLine("starting");
                                process.Start();
                                string output = process.StandardOutput.ReadToEnd();
                                Console.WriteLine(output);
                                SetText(output);
                                process.WaitForExit();
                                //MessageBox.Show(output);
                                if (output.Contains("Error"))
                                {
                                    MessageBox.Show("Firmware update failed. Error message: " + output, "Update Device Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    SetDeviceStatus(4);
                                }
                                else
                                {
                                    MessageBox.Show("Device has been updated successfully!", "Updated Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    SetDeviceStatus(3);
                                }
                                boardUpdate = false;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Unable to write to device, check it's connected via USB.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Console.WriteLine(ex);
                                SetText(ex.Message + ex.StackTrace);
                            }
                            setProgressBar(false);
                        }
                        this.firmVerLbl.Invoke((MethodInvoker)(() => this.firmVerLbl.Text = "V" + boardVersion));
                        forceUpdate = false;
                    }
                    else
                    {
                        MessageBox.Show("Device already up to date!", "Update Device", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void connectToBoard(string comPort)
        {  
            System.ComponentModel.IContainer components =
                new System.ComponentModel.Container();
            port = new System.IO.Ports.SerialPort(components);
            port.PortName = comPort;
            port.BaudRate = 115200;
            port.DtrEnable = true;
            port.ReadTimeout = 5000;
            port.WriteTimeout = 5000;
            port.ReadBufferSize = 1048576;
            Console.WriteLine("Port details set");
            try
            { port.Open(); }
            catch (Exception ex)
            { Console.WriteLine(ex); }

            if (port.IsOpen)
            {
                portConnected = true;
                readThread = new Thread(new ThreadStart(this.Read));
                readThread.Start();
                this.hardWorker.RunWorkerAsync();
                //port.Write("X");
                //Thread.Sleep(250);
                port.Write("I" + (this.testCount.Value - 1).ToString());
                //setFPSLimit();
                if (displayList[0].Freq < 140)
                {
                    if (Properties.Settings.Default.captureTime == 0)
                    {
                        this.captureTimeBox.Invoke((MethodInvoker)(() => this.captureTimeBox.SelectedIndex = 1));
                        Properties.Settings.Default.captureTime = 1;
                        Properties.Settings.Default.Save();
                        setCaptureTime();
                    }
                }
                port.Write("V" + Properties.Settings.Default.VSyncState.ToString());
            }
            else
            {
                SetDeviceStatus(0);
                ControlDeviceButtons(false);
            }
        }

        private void compareFirmware(bool force = false)
        {
            if ((boardVersion < V1DLFW && boardType == 0) && !Properties.Settings.Default.SuppressDiagBox)
            {
                DialogResult dialogResult = MessageBox.Show("OSRTT V1 Firmware Update Available! Update now? \n Current version: " + boardVersion + "\n New version: " + V1DLFW, "Board Firmware Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    //updateFirmware();
                    boardUpdate = true;
                }
            }
            else if (!Properties.Settings.Default.SuppressDiagBox && (boardVersion < ProDLFW && boardType == 1))
            {
                DialogResult dialogResult = MessageBox.Show("OSRTT PRO Firmware Update Available! Update now? \n Current version: " + boardVersion + "\n New version: " + ProDLFW, "Board Firmware Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    //updateFirmware();
                    boardUpdate = true;
                }
            }
            else if (force)
            {
                DialogResult dialogResult = MessageBox.Show("There isn't a newer version of the firmware available right now. Would you like to force it to re-flash anyway? (Not recommended)", "No New Updates Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    //updateFirmware();
                    forceUpdate = true;
                    boardUpdate = true;
                }
            }
        }

        private void setBoardSerial()
        {
            if (boardSerialLbl.InvokeRequired)
            {
                this.boardSerialLbl.Invoke((MethodInvoker)(() => this.boardSerialLbl.Text = Properties.Settings.Default.serialNumber));
            }
            else
            {
                boardSerialLbl.Text = Properties.Settings.Default.serialNumber;
            }
        }
        
        private void ControlDeviceButtons(bool state)
        {
            bool brightnessCal = state;
            if (boardType == 1)
            {
                brightnessCal = false;
            }
            if (this.launchBtn.InvokeRequired)
            {
                this.launchBtn.Invoke((MethodInvoker)(() => launchBtn.Enabled = state));
                this.menuStrip1.Invoke((MethodInvoker)(() => BrightnessCalBtn.Visible = brightnessCal));
                this.inputLagButton.Invoke((MethodInvoker)(() => inputLagButton.Enabled = state));
                this.LiveViewBtn.Invoke((MethodInvoker)(() => LiveViewBtn.Enabled = state));
            }
            else
            {
                this.launchBtn.Enabled = state;
                this.BrightnessCalBtn.Visible = brightnessCal;
                this.inputLagButton.Enabled = state;
                this.LiveViewBtn.Enabled = state;
            }
        }

        private void setRepeatCounter(int runs)
        {
            if (this.testCount.InvokeRequired)
            {
                this.testCount.Invoke((MethodInvoker)(() => testCount.Value = runs));
            }
            else { this.testCount.Value = runs;}
        }

        private void setSelectedFps(string limit)
        {
            if (this.fpsLimitList.InvokeRequired)
            {
                this.fpsLimitList.Invoke((MethodInvoker)(() => fpsLimitList.SelectedItem = limit));
            }
            else { this.fpsLimitList.SelectedItem = limit; }
        }

        public void sendText(string textToSend)
        {
            if (port.IsOpen)
            {
                port.Write(textToSend);
            }
        }

        private void setVsyncState(int state)
        {
            if (this.vsyncStateList.InvokeRequired)
            {
                this.vsyncStateList.Invoke((MethodInvoker)(() => vsyncStateList.SelectedIndex = state));
            }
            else { this.vsyncStateList.SelectedIndex = state; }
        }

        private void SetDeviceStatus(int state)
        {
            string text = " Device Not Connected";
            Color bg = Color.FromArgb(255, 255, 131, 21);
            Color btnBg = Color.Gray;
            bool active = false;
            bool check = false;
            if (state == 1)
            {
                text = "Device Connected";
                bg = Color.White;
                check = true;
                active = true;
                btnBg = Color.FromArgb(255, 105, 180, 76);
            }
            else if (state == 2)
            {
                text = "Updating Firmware Now";
                bg = Color.Violet;
            }
            else if (state == 3)
            {
                text = "Update Successful";
                bg = Color.FromArgb(255, 105, 180, 76);
                check = true;
            }
            else if (state == 4)
            {
                text = "Firmware Update Failed";
                bg = Color.FromArgb(255, 255, 80, 80);
            }
            if (this.devStat.InvokeRequired)
            {
                this.devStat.Invoke((MethodInvoker)(() => this.devStat.Text = text));
                this.checkImg.Invoke((MethodInvoker)(() => this.checkImg.Visible = check));
                this.deviceStatusPanel.Invoke((MethodInvoker)(() => this.deviceStatusPanel.BackColor = bg));
                //this.controlsPanel.Invoke((MethodInvoker)(() => this.controlsPanel.Enabled = active));
                this.launchBtn.Invoke((MethodInvoker)(() => this.launchBtn.Enabled = active));
                this.fpsLimitList.Invoke((MethodInvoker)(() => this.fpsLimitList.Enabled = active));
                this.testCount.Invoke((MethodInvoker)(() => this.testCount.Enabled = active));
                this.captureTimeBox.Invoke((MethodInvoker)(() => this.captureTimeBox.Enabled = active));
                this.vsyncStateList.Invoke((MethodInvoker)(() => this.vsyncStateList.Enabled = active));
                this.inputLagPanel.Invoke((MethodInvoker)(() => this.inputLagPanel.Enabled = active));
                this.launchBtn.Invoke((MethodInvoker)(() => this.launchBtn.BackColor = btnBg));
                this.inputLagButton.Invoke((MethodInvoker)(() => this.inputLagButton.BackColor = btnBg));
                this.LiveViewBtn.Invoke((MethodInvoker)(() => LiveViewBtn.Enabled = active));
                this.LiveViewBtn.Invoke((MethodInvoker)(() => LiveViewBtn.BackColor = btnBg));
            }
            else 
            {
                this.devStat.Text = text;
                this.checkImg.Visible = check;
                this.deviceStatusPanel.BackColor = bg;
                //this.controlsPanel.Enabled = active;
                this.launchBtn.Enabled = active;
                this.fpsLimitList.Enabled = active;
                this.testCount.Enabled = active;
                this.captureTimeBox.Enabled = active;
                this.vsyncStateList.Enabled = active;
                this.inputLagPanel.Enabled = active;
                this.launchBtn.BackColor = btnBg;
                this.inputLagButton.BackColor = btnBg;
                this.LiveViewBtn.Enabled = active;
                this.LiveViewBtn.BackColor = btnBg;
            }
            this.statusTrayBtn.Text = text;
            this.notifyIcon.Text = text;

        }

        private void SetText(string text)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBox1.Text += text;
                this.richTextBox1.Text += "\n";
            }
        }

        private int getSelectedMonitor()
        {
            if (monitorCB.InvokeRequired)
            {
                return (int)monitorCB.Invoke(
                  new Func<int>(() => monitorCB.SelectedIndex)
                );
            }
            else { return monitorCB.SelectedIndex; }
        }

        private string getSelectedFps()
        {
            if (fpsLimitList.InvokeRequired)
            {
                return (string)fpsLimitList.Invoke(
                  new Func<string>(() => fpsLimitList.SelectedItem.ToString())
                );
            }
            else { return fpsLimitList.SelectedItem.ToString(); }
        }

        private bool getVsyncState()
        {
            if (vsyncStateList.InvokeRequired)
            {
                if ( (string)vsyncStateList.Invoke(
                  new Func<string>(() => vsyncStateList.SelectedItem.ToString())
                ) == "Enabled")
                {
                    return true;
                }
                else
                {
                    return false;
                }    
            }
            else { 
                if (vsyncStateList.SelectedItem.ToString() == "Enabled")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private int getRunCount()
        {
            if (testCount.InvokeRequired)
            {
                return (int)testCount.Invoke(
                  new Func<int>(() => Decimal.ToInt32(testCount.Value))
                );
            }
            else { return Decimal.ToInt32(testCount.Value); }
        }

        private string getSelectedCaptureWindow()
        {
            if (captureTimeBox.InvokeRequired)
            {
                return (string)captureTimeBox.Invoke(
                  new Func<string>(() => captureTimeBox.SelectedItem.ToString())
                );
            }
            else { return captureTimeBox.SelectedItem.ToString(); }
        }

        public void Read()
        {
            while (port.IsOpen)
            {
                try
                {
                    string message = port.ReadLine();
                    if (liveView)
                    {
                        if (message.Contains("pot"))
                        {
                            // update sensitivity
                        }  
                        else if (message.Contains("LIVE VIEW"))
                        {
                            // catch this so it doesn't wreck the rest
                        }
                        else if (message.Contains("End"))
                        {
                            
                        }
                        else if (message.Contains("LiveData:"))
                        {
                            string newMessage = message.Remove(0, 9);
                            // send to live view window
                            string[] splitMessage = newMessage.Split(',');
                            List<LiveView.LiveData> dataList = new List<LiveView.LiveData>();
                            foreach (string s in splitMessage)
                            {
                                if (s.Contains(":"))
                                {
                                    string[] stringArr = s.Split(':');
                                    double t = double.Parse(stringArr[0]);
                                    LiveView.LiveData d = new LiveView.LiveData
                                    {
                                        time = t / 1000,
                                        result = double.Parse(stringArr[1])
                                    };
                                    LiveViewObject.addData(d);
                                    dataList.Add(d);
                                }
                            }
                            Thread.Sleep(5);
                            LiveViewObject.Invoke((MethodInvoker)(() => LiveViewObject.copyListToArray()));
                            LiveViewObject.Invoke((MethodInvoker)(() => LiveViewObject.renderGraph()));
                            LiveViewObject.Invoke((MethodInvoker)(() => LiveViewObject.startStopBtn_Click(null, null)));
                            // Save data to file
                            decimal fileNumber = 001;
                            // search /Results folder for existing file names, pick new name
                            string[] existingFiles = Directory.GetFiles(path + "\\Live Data", "*-LIVE-OSRTT.csv");
                            //search files for number
                            foreach (var s in existingFiles)
                            {
                                decimal num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                                if (num >= fileNumber)
                                {
                                    fileNumber = num + 1;
                                }
                            }
                            string filePath = path + "\\Live Data\\" + fileNumber.ToString("000") + "-LIVE-OSRTT.csv";
                            
                            StringBuilder csvString = new StringBuilder();
                            csvString.AppendLine("Timestamp,Light Reading");
                            foreach (var res in dataList)
                            {
                                csvString.AppendLine(res.time.ToString() + "," + res.result.ToString());
                            }
                            File.WriteAllText(filePath, csvString.ToString());
                        }
                    }
                    Console.WriteLine(message);
                    if (debugMode)
                    {
                        SetText(message);
                    }
                    if (message.Contains("RGB Array"))
                    {
                        RGBArr.Clear();
                        string newMessage = message.Remove(0, 10);
                        string[] values = newMessage.Split(',');
                        int[] intValues = new int[values.Length - 1];
                        for (int i = 0; i < values.Length - 1; i++)
                        {
                            if (values[i] == "0")
                            {
                                intValues[i] = 0;
                            }
                            else if (values[i] != "")
                            {
                                try
                                {
                                    intValues[i] = int.Parse(values[i]);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(values[i]);
                                }
                            }
                            else { continue; }
                        }
                        RGBArr.AddRange(intValues);
                    }
                    else if (message.Contains("Board Calibrated"))
                    {
                        boardCalibration = true;
                    }
                    else if (message.Contains("Results"))
                    {
                        // Split result string into individual results
                        String newMessage = message.Remove(0, 9);
                        string[] values = newMessage.Split(',');
                        int[] intValues = new int[values.Length - 1];
                        for (int i = 0; i < values.Length - 1; i++)
                        {
                            if (values[i] == "0")
                            {
                                intValues[i] = 0;
                            }
                            else if (values[i] != "")
                            {
                                try
                                {
                                    intValues[i] = int.Parse(values[i]);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(values[i]);
                                }
                            }
                            else { continue; }
                        }
                        // Added extra check to make sure test is cancelled if there isn't a light level difference between start and end. Using 5 samples to account for noise.
                        int start = intValues[50] + intValues[51] + intValues[52] + intValues[53] + intValues[54];
                        int end = intValues[intValues.Length - 50] + intValues[intValues.Length - 51] + intValues[intValues.Length - 52] + intValues[intValues.Length - 53] + intValues[intValues.Length - 54];
                        if (intValues[0] < intValues[1])
                        {
                            if (start < end)
                            {
                                ProcessData.rawResultData rawResult = new ProcessData.rawResultData
                                {
                                    StartingRGB = intValues[0],
                                    EndRGB = intValues[1],
                                    TimeTaken = intValues[2],
                                    SampleCount = intValues[3],
                                    SampleTime = ((double)intValues[2] / (double)intValues[3]),
                                    Samples = intValues.Skip(4).ToList()
                                };
                                results[currentRun].Add(rawResult);
                            }
                            else
                            {
                                if (!Properties.Settings.Default.ignoreErrors)
                                {
                                    port.Write("X");
                                    port.Write("X");
                                    port.Write("X");
                                    testRunning = false;
                                    if (runTestThread != null)
                                    {
                                        runTestThread.Abort();
                                    }
                                    MessageBox.Show("The last test result showed no difference in light level. The brightness may be too high. The test has been cancelled.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    ProcessData.rawResultData rawResult = new ProcessData.rawResultData
                                    {
                                        StartingRGB = intValues[0],
                                        EndRGB = intValues[1],
                                        TimeTaken = intValues[2],
                                        SampleCount = intValues[3],
                                        SampleTime = ((double)intValues[2] / (double)intValues[3]),
                                        Samples = intValues.Skip(4).ToList()
                                    };
                                    results[currentRun].Add(rawResult);
                                }
                            }
                        }
                        else
                        {
                            if (start > end)
                            {
                                ProcessData.rawResultData rawResult = new ProcessData.rawResultData
                                {
                                    StartingRGB = intValues[0],
                                    EndRGB = intValues[1],
                                    TimeTaken = intValues[2],
                                    SampleCount = intValues[3],
                                    SampleTime = ((double)intValues[2] / (double)intValues[3]),
                                    Samples = intValues.Skip(4).ToList()
                                };
                                results[currentRun].Add(rawResult);
                            }
                            else
                            {
                                if (!Properties.Settings.Default.ignoreErrors)
                                {
                                    port.Write("X");
                                    port.Write("X");
                                    port.Write("X");
                                    testRunning = false;
                                    if (runTestThread != null)
                                    {
                                        runTestThread.Abort();
                                    }
                                    MessageBox.Show("The last test result showed no difference in light level. The brightness may be too high. The test has been cancelled.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    ProcessData.rawResultData rawResult = new ProcessData.rawResultData
                                    {
                                        StartingRGB = intValues[0],
                                        EndRGB = intValues[1],
                                        TimeTaken = intValues[2],
                                        SampleCount = intValues[3],
                                        SampleTime = ((double)intValues[2] / (double)intValues[3]),
                                        Samples = intValues.Skip(4).ToList()
                                    };
                                    results[currentRun].Add(rawResult);
                                }
                            }
                        }
                        
                            currentStart = intValues[0];
                            currentEnd = intValues[1];
                        
                    }
                    else if (message.Contains("Gamma"))
                    {
                        // Split result string into individual results
                        String newMessage = message.Remove(0, 7);
                        string[] values = newMessage.Split(',');
                        int[] intValues = new int[values.Length - 1];
                        for (int i = 0; i < values.Length - 1; i++)
                        {
                            if (values[i] == "0")
                            {
                                intValues[i] = 0;
                            }
                            else if (values[i] != "")
                            {
                                try
                                {
                                    intValues[i] = int.Parse(values[i]);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(values[i]);
                                }
                            }
                            else { continue; }
                        }
                        gamma.Add(intValues);
                    }
                    else if (message.Contains("Stability"))
                    {
                        // Split result string into individual results
                        String newMessage = message.Remove(0, 10);
                        string[] values = newMessage.Split(',');
                        List<int> intValues = new List<int>();
                        for (int i = 0; i < values.Length - 1; i++)
                        {
                            if (values[i] == "0")
                            {
                                intValues.Add(0);
                            }
                            else if (values[i] != "")
                            {
                                try
                                {
                                    intValues.Add(int.Parse(values[i]));
                                }
                                catch
                                {
                                    Console.WriteLine(values[i]);
                                }
                            }
                            else { continue; }
                        }
                        intValues.RemoveRange(0, 200);
                        int diff = intValues.Max() - intValues.Min();
                        if (diff > 1000)
                        {
                            showMessageBox("ERROR: The monitor's backlight appears to be strobing significantly. This is likely to make any data collected innacurate, so it's best to turn any strobing off if possible." +
                                "\n You are free to continue the test, but you may need to verify the results manually and the data may be unusable.", "Backlight Strobing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (diff > 750)
                        {
                            showMessageBox("ERROR: The monitor's backlight appears to be strobing. This may make any data collected innacurate, so it's best to turn any strobing off if possible." +
                                "\n You are free to continue the test, but you may need to verify the results manually.", "Backlight Strobing Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if (message.Contains("Test Started"))
                    {
                        testStarted = true;
                    }
                    else if (message.Contains("NEXT"))
                    {
                        triggerNextResult = true;
                        //Console.WriteLine("trigger next result true");
                    }
                    else if (message.Contains("G Test"))
                    {
                        if (message.Contains("Starting"))
                        {
                            gamma.Clear();
                            gammaTest = true;
                        }
                        else if (message.Contains("Complete"))
                        {
                            //processGammaTable();
                            gammaTest = false;
                        }
                    }
                    else if (message.Contains("FW:"))
                    {
                        string[] sp = message.Split(':');
                        boardVersion = double.Parse(sp[1]);
                        compareFirmware();
                        this.firmVerLbl.Invoke((MethodInvoker)(() => this.firmVerLbl.Text = "V" + boardVersion));
                        if ((boardVersion < 3.2 && boardType == 0) || (boardVersion < 1.7 && boardType == 1))
                        {
                            MessageBox.Show("Warning - the FPS limit feature won't work with your current firmware version.", "Incompatible Firmware Version", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    }
                    else if (message.Contains("Runs:"))
                    {
                        /*string[] sp = message.Split(':');
                        int runs = Int32.Parse(sp[1]);
                        if (runs != (this.testCount.Value - 1))
                        {
                            setRepeatCounter(runs);
                        }*/
                    }
                    else if (message.Contains("FPS Key:"))
                    {
                        /*string[] sp = message.Split(':');
                        string k = sp[1];
                        k = k.Replace("\r", string.Empty);
                        var lim = fpsList.Find(x => x.Key == k);
                        string selectedFps = getSelectedFps();
                        if (lim.FPSValue != selectedFps)
                        {
                            setSelectedFps(lim.FPSValue);
                        }*/
                    }
                    else if (message.Contains("Brightness:"))
                    {
                        if (brightnessWindowOpen)
                        {
                            string[] sp = message.Split(':');
                            updateBrightness(Int32.Parse(sp[1]), Int32.Parse(sp[2]));
                        }
                    }
                    else if (message.Contains("BRIGHTNESS CHECK"))
                    {
                        port.Write(basePotVal.ToString("X"));
                    }
                    else if (message.Contains("USB V:"))
                    {
                        String newMessage = message.Remove(0, 6);
                        Console.WriteLine("USB Output: " + Properties.Settings.Default.USBOutput);
                        if (Properties.Settings.Default.USBOutput && newMessage.Length > 49999)
                        {
                            // search /Results folder for existing file names, pick new name
                            string[] existingUSBFile = Directory.GetFiles(path, "USB-Voltage-Output.csv");
                            // Search \Results folder for existing results to not overwrite existing or have save conflict errors
                            foreach (var s in existingUSBFile)
                            {
                                // Delete existing file if present
                                File.Delete(s);
                                Console.WriteLine(s);
                            }
                            string USBOutputPath = path + "\\USB-Voltage-Output.csv";
                            StringBuilder USBOutputString = new StringBuilder();
                            USBOutputString.AppendLine(newMessage);
                            File.WriteAllText(USBOutputPath, USBOutputString.ToString());
                        }
                        else if (Properties.Settings.Default.USBOutput)
                        {
                            port.Write("U");
                        }
                        string[] values = newMessage.Split(',');
                        List<int> intValues = new List<int>();
                        for (int i = 0; i < values.Length - 1; i++)
                        {
                            if (values[i] == "0")
                            {
                                intValues.Add(0);
                            }
                            else if (values[i] != "")
                            {
                                try
                                {
                                    intValues.Add(int.Parse(values[i]));
                                }
                                catch
                                {
                                    Console.WriteLine(values[i]);
                                }
                            }
                            else { continue; }
                        }
                        intValues.RemoveRange(0, 200);
                        int diff = intValues.Max() - intValues.Min();
                        if (diff > 1000)
                        {
                            showMessageBox("ERROR: The USB supply voltage is very noisy. This may mean the results are unusable. It's recommended to try a different USB port/controller before continuing.", "USB Voltage Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        /*else if (diff > 500)
                        {
                            MessageBox.Show("ERROR: The USB supply voltage is noisy. This may mean the results are unusable, or inaccurate.. It's recommended to try a different USB port/controller before continuing.", "USB Voltage Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }*/
                        double voltage = intValues.Average();
                        Console.WriteLine(voltage);
                        if (voltage < 46000 || voltage > 52500)
                        {
                            double v = voltage / 65520;
                            v *= 3.3;
                            v *= 2;
                            DialogResult d = MessageBox.Show("USB voltage appears to be too low. Current reading is: " + Math.Round(v,2) + "V. Target is 5V. \n Try moving the device to a powered hub or a direct port. Do you want to check again or quit?", "USB Voltage Out Of Spec", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            if (d == DialogResult.Retry)
                            {
                                port.Write("I");
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                        else
                        {
                            if (voltage > 50000) { basePotVal = 0; }
                            else if (voltage > 49500) { basePotVal = 1; }
                            else if (voltage > 49000) { basePotVal = 2; }
                            else if (voltage > 48500) { basePotVal = 3; }
                            else if (voltage > 48000) { basePotVal = 4; }
                            else if (voltage > 47500) { basePotVal = 5; }
                            else { basePotVal = 6; }
                        }
                    }
                    else if (message.Contains("TEST CANCELLED"))
                    {
                        testRunning = false;
                        testMode = false;
                        if (message.Contains("LIGHT LEVEL"))
                        {
                            MessageBox.Show("ERROR - TEST CANCELLED. Monitor's brightness may not be in the acceptable range.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            brightnessCheck = false;
                        }
                        else if (message.Contains("USB VOLTAGE"))
                        {
                            MessageBox.Show("ERROR - TEST CANCELLED. USB supply voltage may be too low - please plug the device either directly into your system or a powered USB hub.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (message.Contains("Ready to test"))
                    {
                        testMode = true;
                    }
                    else if (message.Contains("UniqueID"))
                    {
                        string s = message.Remove(0,9);
                        Regex.Replace(s, @"\s+", "");
                        Properties.Settings.Default.serialNumber = s;
                        Properties.Settings.Default.Save();
                    }
                    else if (message.Contains("Handshake"))
                    {
                        setCaptureTime();
                        setFPSLimit();
                        //port.Write("V" + Properties.Settings.Default.VSyncState.ToString());
                    }
                    else if (message.Contains("IL"))
                    {
                        if (message.Contains("IL:"))
                        {
                            // Results Data
                            String newMessage = message.Remove(0, 3);
                            string[] values = newMessage.Split(',');
                            List<int> intValues = new List<int>();
                            for (int i = 0; i < values.Length - 1; i++)
                            {
                                if (values[i] == "0")
                                {
                                    intValues.Add(0);
                                }
                                else if (values[i] != "")
                                {
                                    try
                                    {
                                        intValues.Add(int.Parse(values[i]));
                                    }
                                    catch
                                    {
                                        Console.WriteLine(values[i]);
                                    }
                                }
                                else { continue; }
                            }
                            if (DirectX.System.DSystem.EventList.Count == 0)
                            { Thread.Sleep(50); } // add continuous check
                            float frameTime = 0;
                            try
                            {
                                frameTime = DirectX.System.DSystem.EventList.Last();
                            }
                            catch
                            { }
                                                        
                            ProcessData.rawInputLagResult rawLag = new ProcessData.rawInputLagResult
                            {
                                ClickTime = intValues[0],
                                TimeTaken = intValues[1],
                                SampleCount = intValues[2],
                                SampleTime = (double)intValues[1] / (double)intValues[2],
                                Samples = intValues.Skip(4).ToList(),
                                FrameTime = frameTime
                            };
                            inputLagRawData.Add(rawLag);
                        }
                        else if (message.Contains("Clicks"))
                        {
                            // Send number of clicks to run
                            int clicks = numberOfClicks / 10;
                            Console.WriteLine("Clicks: " + clicks);
                            port.Write(clicks.ToString("00"));
                        }
                        else if (message.Contains("Time"))
                        {
                            // Send time between setting
                            double t = timeBetween * 10;
                            port.Write(t.ToString());
                            Console.WriteLine("Time Between: " + t);
                        }
                        else if (message.Contains("Finished"))
                        {
                            if (Properties.Settings.Default.saveInputLagRaw)
                            {
                                string[] folders = resultsFolderPath.Split('\\');
                                string monitorInfo = folders.Last();
                                string filePath = resultsFolderPath + "\\" + monitorInfo + "-INPUT-LATENCY-RAW.csv";
                                /*
                                decimal fileNumber = 001;
                                // search /Results folder for existing file names, pick new name
                                string[] existingFiles = Directory.GetFiles(resultsFolderPath, "*-INPUT-LAG-RAW-OSRTT.csv");
                                //search files for number
                                foreach (var s in existingFiles)
                                {
                                    decimal num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                                    if (num >= fileNumber)
                                    {
                                        fileNumber = num + 1;
                                    }
                                }

                                string filePath = resultsFolderPath + "\\" + fileNumber.ToString("000") + "-INPUT-LAG-RAW-OSRTT.csv";
                                */
                                string strSeparator = ",";
                                StringBuilder csvString = new StringBuilder();
                                foreach (var res in inputLagRawData)
                                {
                                    csvString.AppendLine(res.ClickTime.ToString() + "," + res.FrameTime.ToString() + ","  + res.TimeTaken.ToString() + "," + res.SampleCount.ToString() + "," + string.Join(strSeparator, res.Samples));
                                }
                                File.WriteAllText(filePath, csvString.ToString());
                            }
                            Thread inputLagThread = new Thread(new ThreadStart(processInputLagData));
                            inputLagThread.Start();
                            //processInputLagData();
                        }
                    }
                    else if (message.Contains("TL"))
                    {
                        // Split result string into individual results
                        String newMessage = message.Remove(0, 3);
                        string[] values = newMessage.Split(',');
                        int[] intValues = new int[values.Length - 1];
                        for (int i = 0; i < values.Length - 1; i++)
                        {
                            if (values[i] == "0")
                            {
                                intValues[i] = 0;
                            }
                            else if (values[i] != "")
                            {
                                try
                                {
                                    intValues[i] = int.Parse(values[i]);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(values[i]);
                                }
                            }
                            else { continue; }
                        }
                        testLatency.AddRange(intValues);
                        int start = 0;
                        for (int m = 0; m < intValues.Length; m++)
                        {
                            if (intValues[m] > 8000)
                            {
                                start = m;
                                break;
                            }
                        }
                        if (Properties.Settings.Default.captureTime == 0)
                        {
                            if (start == 0 || start > 1100)
                            {
                               Thread.Sleep(100);
                               port.Write("S1");
                            }
                            else
                            {
                                Thread.Sleep(100);
                                port.Write("S0");
                            }
                        }
                        else
                        {
                            Thread.Sleep(100);
                            port.Write("S" + Properties.Settings.Default.captureTime.ToString());
                        }
                        latencyTest = true;
                    }
                    else if (message.Contains("BoardType"))
                    {
                        string newMessage = message.Remove(0, 10);
                        try
                        {
                            boardType = Int32.Parse(newMessage);
                            if (boardType == 1)
                            {
                                port.Write("R" + Properties.Settings.Default.proDisplayOrient);
                            }
                        }
                        catch (Exception parseEx)
                        {
                            Console.WriteLine(parseEx.Message + parseEx.StackTrace);
                        }
                    }
                    else if (message.Contains("PROADC"))
                    {
                        string newMessage = message.Remove(0, 7);
                        string[] stringLine = newMessage.Split(',');
                        int[] intValues = new int[stringLine.Length - 1];
                        for (int i = 0; i < stringLine.Length - 1; i++)
                        {
                            string[] values = stringLine[i].Split(':');
                            if (values[1] == "0")
                            {
                                intValues[i] = 0;
                            }
                            else if (values[1] != "")
                            {
                                try
                                {
                                    intValues[i] = int.Parse(values[1]);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(values[1]);
                                }
                            }
                            else { continue; }
                        }
                        /*for (int k = 0; k < intValues.Length; k++)
                        {
                            if ((k != 0 && k < 129) && (intValues[k] > intValues[k - 1] && intValues[k] != 65520))
                            {
                                Console.WriteLine("FAILED");
                            }
                            else if ((k > 129 && k < 160) && (intValues[k] > intValues[k - 1] && intValues[k] != 65520))
                            {
                                Console.WriteLine("FAILED");
                            }
                            else if ((k > 161 && k < 224) && (intValues[k] > intValues[k - 1] && intValues[k] != 65520))
                            {
                                Console.WriteLine("FAILED");
                            }
                            else if ((k > 225 && k < 256) && (intValues[k] > intValues[k - 1] && intValues[k] != 65520))
                            {
                                Console.WriteLine("FAILED");
                            }
                            
                        }*/
                        
                        this.Invoke((MethodInvoker)delegate ()
                        {
                            protest rv = new protest();
                            rv.adcres = intValues.ToList();
                            rv.drawScatter();
                            rv.Show();
                        });
                        // search /Results folder for existing file names, pick new name
                        string[] existingUSBFile = Directory.GetFiles(path, "PRO-DATA.csv");
                        // Search \Results folder for existing results to not overwrite existing or have save conflict errors
                        foreach (var s in existingUSBFile)
                        {
                            // Delete existing file if present
                            File.Delete(s);
                            Console.WriteLine(s);
                        }
                        string USBOutputPath = path + "\\PRO-DATA.csv";
                        StringBuilder USBOutputString = new StringBuilder();
                        foreach (var i in intValues)
                        {
                            USBOutputString.AppendLine(i.ToString());
                        }
                        File.WriteAllText(USBOutputPath, USBOutputString.ToString());
                        Console.WriteLine("Test finished");
                    }
                    else
                    {
                        this.SetText(message);
                    }
                }
                catch (TimeoutException ex)
                {
                    //Console.WriteLine(ex);
                    //SetText(ex.Message + ex.StackTrace);
                }
                catch (ArgumentOutOfRangeException aex)
                {
                    Console.WriteLine(aex);
                    SetText(aex.Message + aex.StackTrace);
                }
                catch (Exception e)
                {
                    try
                    {
                        port.Write("X");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc);
                        SetText(exc.Message + exc.StackTrace);
                    }
                    Console.WriteLine(e);
                    SetText(e.Message + e.StackTrace);
                    port.Close();
                    portConnected = false;
                    testRunning = false;
                    testMode = false;
                    testStarted = false;
                    brightnessCheck = false;
                    if (runTestThread != null)
                    {   runTestThread.Abort(); }
                    readThread.Abort();
                }
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // When form is closed halt read thread & close Serial Port
            notifyIcon.Visible = false;
            if (port != null)
            { 
                try
                {
                    port.Write("X");
                }
                catch { Console.WriteLine("Port not open"); } 
            }
            Process[] p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
            if (p.Length != 0)
            {
                p[0].Kill();
            }
            if (checkWindowThread != null)
            {
                checkWindowThread.Abort();
            }
            if (connectThread != null)
            {
                connectThread.Abort();
            }
            if (readThread != null)
            {
                readThread.Abort();
            }
            if (port != null)
            {
                port.Close();
            }
            readAndSaveUserSettings(true);
            Environment.Exit(Environment.ExitCode);
        }

        private void launchBtn_Click(object sender, EventArgs e)
        {
            int monitor = getSelectedMonitor();
            listMonitors(monitor);
            if (boardType == 0)
            {
                if (!brightnessCheck)
                {
                    launchBrightnessCal();
                }
            }
            Properties.Settings.Default.FPS = fpsLimitList.SelectedIndex;
            Properties.Settings.Default.Runs = Decimal.ToInt32(testCount.Value);
            Properties.Settings.Default.Save();
            string manCode = displayList[monitor].ManufacturerCode;
            overdriveModes1.initialiseList(manCode);

            if (launchGameThread == null || !launchGameThread.IsAlive)
            {
                launchGameThread = new Thread(new ThreadStart(this.launchGameAndWaitForExit));
                launchGameThread.Start();
            }
            else
            {
                CFuncs cf = new CFuncs();
                DialogResult d = cf.showMessageBox("Error: Can't run test with previous test results open. Close and continue?","Close Results",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if (d == DialogResult.Yes)
                {
                    launchGameThread.Abort();
                    launchGameThread = new Thread(new ThreadStart(this.launchGameAndWaitForExit));
                    launchGameThread.Start();
                }
            }
        }

        private void launchGameAndWaitForExit()
        {
            while (brightnessWindowOpen)
            {
                Thread.Sleep(100);
            }
            if (!brightnessCanceled)
            {
                initRtOsMethods();
                makeResultsFolder("RT");
                overdriveModes1.runSetting = runSettings;
                changeSizeAndState("overdrive");
                while (runSettings.OverdriveMode == "")
                {
                    if (cancelTest)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                string odmode = runSettings.OverdriveMode.Replace(" ", "");
                odmode = odmode.Replace("/", "");
                odmode = odmode.Replace("\\", "");
                resultsFolderPath += "-" + odmode;
                Directory.CreateDirectory(resultsFolderPath);
                if (cancelTest)
                {
                    port.Write("X");
                    changeSizeAndState("close brightness");
                    cancelTest = false;
                }
                else
                {
                    changeSizeAndState("close brightness");
                    ControlDeviceButtons(false);
                    setProgressBar(true);
                    // Save current & FPS to hardware on run
                    Thread.Sleep(200);
                    setFPSLimit();
                    Thread.Sleep(200);
                    setRepeats();
                    if (port != null)
                    {
                        port.Write("V" + Properties.Settings.Default.VSyncState.ToString());
                    }
                    Thread.Sleep(200);
                    if (runSettings.OverdriveMode == null)
                    {
                        // OD Mode not entered - cause validation?
                    }
                    testRunning = true;
                    vsyncTrigger = false;
                    gammaTest = true;
                    // Launch UE4 game
                    // thinking about it you can probably just bundle this into one process instead of launching, then finding it again...
                    string ue4Path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                    ue4Path = new Uri(System.IO.Path.GetDirectoryName(ue4Path)).LocalPath;
                    ue4Path += @"\OSRTT UE4\ResponseTimeTest.exe";
                    // Move UE4 window to selected monitor if that isn't the primary (will open by default there).
                    int selectedDisplay = getSelectedMonitor();
                    var display = Screen.AllScreens[selectedDisplay];
                    int WinX = 0;
                    int WinY = 0;
                    string vsync = " VSync";
                    if (!Properties.Settings.Default.VSyncState)
                    {
                        vsync = " NoVSync";
                    }
                
                    Process ue4 = new Process();
                    try
                    {
                        ue4.StartInfo.FileName = ue4Path;
                        if (display.Primary == false)
                        {
                            // Force UE4 window to selected display if selected is not primary
                            WinX = display.Bounds.Location.X;
                            WinY = display.Bounds.Location.Y;
                            ue4.StartInfo.Arguments = ue4Path + " WinX=" + WinX + " WinY=" + WinY;
                        }
                        else
                        {
                            ue4.StartInfo.Arguments = ue4Path;
                        }
                        ue4.Start();
                        // Process.Start(ue4Path);
                    }
                    catch (Exception strE)
                    {
                        Console.WriteLine(strE.Message + strE.StackTrace);
                        SetText(strE.Message + strE.StackTrace);
                    }
                    try
                    {
                        Process[] p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                        while (p.Length == 0)
                        {
                            // Added in case game hasn't finished launching yet
                            p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                            Thread.Sleep(100);
                        }
                    
                        try
                        {
                            gamma.Clear();
                            processedGamma.Clear();
                            results.Clear();
                            multipleRunData.Clear();
                            singleResults.Clear();
                            testLatency.Clear();
                            int runCount = getRunCount();
                            for (int r = 0; r < runCount; r++)
                            {
                                results.Add(new List<ProcessData.rawResultData>());
                            }
                            testStarted = false;
                            port.Write("T");
                            while (!testMode)
                            {
                                Thread.Sleep(200);
                                port.Write("T");
                            }
                        }
                        catch (Exception exc)
                        {
                            SetText(exc.Message + exc.StackTrace);
                            Console.WriteLine(exc.Message + exc.StackTrace);
                        }
                        Thread.Sleep(200);
                    
                        checkWindowThread = new Thread(new ThreadStart(this.checkFocusedWindow));
                        checkWindowThread.Start();
                    
                            testRunning = true;
                        
                            runTestThread = new Thread(new ThreadStart(this.runTest));
                            runTestThread.Start();
                    
                        // Wait for game to close then send cancel command to board
                        p[0].WaitForExit();
                        /*if (runTestThread != null)
                        {
                            runTestThread.Abort();
                        }*/
                        if (!testRunning)
                        {
                            if (runTestThread != null)
                            {
                                runTestThread.Abort();
                            }
                        }
                        checkWindowThread.Abort();
                        Console.WriteLine("Game closed");
                        SetText("Game closed");
                        port.Write("X");
                    
                        ControlDeviceButtons(true);
                        setProgressBar(false);
                        testRunning = false;
                        testStarted = false;
                        testMode = false;
                        /*if (Properties.Settings.Default.shareResults)
                        {
                            DataUpload du = new DataUpload();
                            Thread uploadThread = new Thread(() => du.ShareResults(results,processedGamma,testLatency,runSettings));
                            uploadThread.Start();
                        }*/
                        if (results.Count != 0 && results[0].Count != 0)
                        {
                            processThread = new Thread(new ThreadStart(runProcessing));
                            processThread.Start();
                    
                            while (processThread.IsAlive)
                            {
                                Thread.Sleep(100);
                            }
                            if (multipleRunData.Count != 0 && averageData.Count != 0)
                            {
                                this.Invoke((MethodInvoker)delegate ()
                                {
                                    ResultsView rv = new ResultsView();
                                    rv.setRawData(results);
                                    rv.setGammaData(processedGamma);
                                    rv.setTestLatency(testLatency);
                                    rv.setMultiRunData(multipleRunData);
                                    rv.setAverageData(averageData);
                                    rv.setResultsFolder(resultsFolderPath);
                                    rv.setRtMethod(rtMethod);
                                    rv.setOsMethod(osMethod);
                                    rv.setRunSettings(runSettings);
                                    rv.setStandardView();
                                    rv.Show();
                                });
                            }
                            else
                            {
                                MessageBox.Show("One or more results failed to process. Take a look at the raw data in the graph view and adjust your test settings accordingly.", "Failed to Process Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                this.Invoke((MethodInvoker)delegate ()
                                {
                                    ResultsView rv = new ResultsView();
                                    rv.setRawData(results);
                                    rv.setResultsFolder(resultsFolderPath);
                                    rv.setRtMethod(rtMethod);
                                    rv.setOsMethod(osMethod);
                                    rv.setRunSettings(runSettings);
                                    rv.setGraphView();
                                    rv.setGammaData(processedGamma);
                                    rv.setTestLatency(testLatency);
                                    rv.Show();
                                });
                            }
                        }
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine(e);
                        SetText(e.Message + e.StackTrace);
                    }
                    catch (IOException ioex)
                    {
                        Console.WriteLine(ioex);
                        SetText(ioex.Message + ioex.StackTrace + ioex.Source);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " " + ex.StackTrace, "Error Launching Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                brightnessCanceled = false;
            }
        }

        private void checkFocusedWindow()
        {
            FocusedWindow fw = new FocusedWindow();
            string pName = fw.GetForegroundProcessName();
            while (true)
            {
                Thread.Sleep(300);
                pName = fw.GetForegroundProcessName();
                if (pName != "ResponseTimeTest-Win64-Shipping")
                {
                    Console.WriteLine("Process not selected");
                    if (!paused)
                    {
                        //port.Write("P");
                        paused = true;
                    }
                }
                else 
                { 
                    if (!vsyncTrigger)
                    {
                        paused = true;
                        Thread.Sleep(300);
                        var item = fpsList.Find(x => x.FPSValue == getSelectedFps());
                        SendKeys.SendWait(item.CKey);
                        Thread.Sleep(100);
                        if (Properties.Settings.Default.VSyncState)
                        {
                            SendKeys.SendWait("{PGUP}");
                        }
                        else
                        {
                            SendKeys.SendWait("{PGDN}");
                        }
                        Thread.Sleep(100);
                        vsyncTrigger = true;
                    }
                    if (paused)
                    {
                        //port.Write("S");
                        paused = false;
                    }
                }
                if (testRunning == false)
                {
                    Process[] p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                    if (p.Length != 0)
                    {
                        p[0].Kill();
                        port.Write("X");
                        testStarted = false;
                        if (runTestThread != null)
                        {
                            runTestThread.Abort();
                        }
                        break;
                    }
                }
            }
        }

        private void runTest()
        {
            while (!testStarted)
            { // Wait for user to press the button
                Thread.Sleep(100);
            }
            testStarted = false;
            Thread.Sleep(100);
            while (gammaTest)
            {
                Thread.Sleep(100);
            }
            Console.WriteLine("Gamma test complete");
            while(!testRunning)
            {
                Thread.Sleep(10);
            }
            Console.WriteLine("test running");
            while(testRunning)
            {
                currentRun = 0;
                currentStart = 0;
                currentEnd = 0;
                int testPatternSize = RGBArr.Count * (RGBArr.Count - 1);
                for (int r = 0; r < testCount.Value; r++)
                { // how many runs to do
                    singleResults.Clear();
                    for (int i = 0; i < RGBArr.Count; i++)
                    { // loop through each "base" value
                        for (int k = i + 1; k < RGBArr.Count; k++)
                        { // loop through each combination of "base" value and the rest of the array
                            if (port == null)
                            {
                                testRunning = false;
                                break;
                            }
                            while (paused)
                            { // check if test should be paused first, if so sleep for 100ms.
                                Thread.Sleep(100);
                            }
                            Thread.Sleep(10);
                            try
                            { port.Write(i.ToString() + k.ToString()); }
                            catch (Exception ex) { Console.WriteLine(ex.Message + ex.StackTrace); SetText(ex.Message + ex.StackTrace); }
                            Stopwatch sw = new Stopwatch();
                            sw.Reset();
                            sw.Start();
                            while (sw.ElapsedMilliseconds < 5000)
                            { // wait for CORRECT result to come back
                                if (currentStart == RGBArr[i] && currentEnd == RGBArr[k] && triggerNextResult)
                                {
                                    break;
                                }
                                Thread.Sleep(10);
                            }
                            if (sw.ElapsedMilliseconds > 5000)
                            {
                                DialogResult d = showMessageBox("Error: The test was unable to run the last transition, try again?", "Test Timed Out", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                                if (d == DialogResult.Retry)
                                {
                                    try
                                    { port.Write(i.ToString() + k.ToString()); }
                                    catch (Exception ex) { Console.WriteLine(ex.Message + ex.StackTrace); SetText(ex.Message + ex.StackTrace); }
                                }
                                else
                                {
                                    testRunning = false;
                                    break;
                                }
                            }
                            triggerNextResult = false;
                            Thread.Sleep(100);
                            if (port == null)
                            {
                                testRunning = false;
                                break;
                            }
                            try
                            { port.Write(k.ToString() + i.ToString()); }
                            catch (Exception ex) { Console.WriteLine(ex.Message + ex.StackTrace); SetText(ex.Message + ex.StackTrace); }
                            sw.Reset();
                            sw.Start();
                            while (sw.ElapsedMilliseconds < 5000)
                            { // wait for CORRECT result to come back
                                if (currentStart == RGBArr[k] && currentEnd == RGBArr[i] && triggerNextResult)
                                {
                                    break;
                                }
                                Thread.Sleep(10);
                            }
                            if (sw.ElapsedMilliseconds > 5000)
                            {
                                DialogResult d = showMessageBox("Error: The test was unable to run the last transition, try again?", "Test Timed Out", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                                if (d == DialogResult.Retry)
                                {
                                    try
                                    { port.Write(k.ToString() + i.ToString()); }
                                    catch (Exception ex) { Console.WriteLine(ex.Message + ex.StackTrace); SetText(ex.Message + ex.StackTrace); }
                                }
                                else
                                {
                                    testRunning = false;
                                    break;
                                }
                            }
                            triggerNextResult = false;
                            if (!testRunning) { break; }
                            Thread.Sleep(100);
                        }
                        if (!testRunning) { break; }
                        Thread.Sleep(100);
                    }
                    if (!testRunning) { break; }
                    Thread.Sleep(200);
                    //results.Add(singleResults);
                    runComplete();
                    Thread.Sleep(500);
                    currentRun++;
                    singleResults.Clear();
                }
                if (!testRunning) { break; }
                port.Write("X");
                Process.Start("explorer.exe", resultsFolderPath);
                testRunning = false;
            }
        }

        private void runComplete()
        {
            decimal fileNumber = 001;
            // search /Results folder for existing file names, pick new name
            string[] existingFiles = Directory.GetFiles(resultsFolderPath, "*-RAW-OSRTT.csv");
            //search files for number
            foreach (var s in existingFiles)
            {
                decimal num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                if (num >= fileNumber)
                {
                    fileNumber = num + 1;
                }
            }

            string filePath = resultsFolderPath + "\\" + fileNumber.ToString("000") + "-RAW-OSRTT.csv";

            string strSeparator = ",";
            StringBuilder csvString = new StringBuilder();
            if (testLatency.Count != 0)
            {
                csvString.AppendLine(string.Join(strSeparator, testLatency));
            }
            foreach (var res in gamma)
            {
                csvString.AppendLine(string.Join(strSeparator, res));
            }
            foreach (var res in results[currentRun])
            {
                csvString.AppendLine(
                    res.StartingRGB + ","+
                    res.EndRGB + "," + 
                    res.TimeTaken + "," +
                    res.SampleCount + "," +
                    string.Join(strSeparator, res.Samples)
                    );
            }
            if (runSettings != null)
            {
                csvString.AppendLine(JsonConvert.SerializeObject(runSettings));
            }
            File.WriteAllText(filePath, csvString.ToString());

            // Process that raw data
            //processThread = new Thread(new ThreadStart(this.processResponseTimeData));
            //processThread.Start();
            //processResponseTimeData();
        }

        private void refreshMonitorListBtn_Click(object sender, EventArgs e)
        {
            listMonitors(0);
        }
        private void updateDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compareFirmware();
        }

        private void heatmapsMenuItem_Click(object sender, EventArgs e)
        {
            ResultsView rs = new ResultsView();
            rs.Show();
        }

        private void debugModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeSizeAndState("debug");
            debugMode = debugModeToolStripMenuItem.Checked;
        }

        private void setRepeats()
        {
            decimal runs = getRunCount() - 1;
            port.Write("M" + runs.ToString());
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

        private void makeResultsFolder(string testType = "RT")
        {
            int monitor = getSelectedMonitor();
            string monitorName = displayList[monitor].Name;
            monitorName = Regex.Replace(monitorName, "[^\\w\\d\\s -]", "");
            string monitorInfo = testType + "-" + monitorName.Replace(" ", "-") + "-" + displayList[monitor].Freq.ToString() + "-" + displayList[monitor].Connection;
            if (runSettings != null)
            {
                if (runSettings.OverdriveMode != null && runSettings.OverdriveMode != "")
                {
                    //monitorInfo += "-" + runSettings.OverdriveMode.Replace(" ", "");
                }
            }

            decimal fileNumber = 001;
            // search /Results folder for existing file names, pick new name
            string[] existingFiles = Directory.GetDirectories(path, "*-" + monitorInfo + "*");
            //search files for number
            if (existingFiles.Length != 0)
            {
                foreach (var s in existingFiles)
                {
                    var name = new DirectoryInfo(s).Name;
                    decimal num = decimal.Parse(name.Remove(3));
                    if (num >= fileNumber)
                    {
                        fileNumber = num + 1;
                    }
                }
            }
            string filePath = path + "\\" + fileNumber.ToString("000") + "-" + monitorInfo;
            if (testType != "RT")
            {
                Directory.CreateDirectory(filePath);
            }
            resultsFolderPath = filePath;
            initRunSettingsFile(filePath, monitor);
        }
        private void initRunSettingsFile(string filePath, int monitor)
        {
            string fileName = filePath.Substring(filePath.LastIndexOf('\\'));
            runSettings = new ProcessData.runSettings
            {
                RunName = fileName,
                DateAndTime = DateTime.Now.ToString(),
                MonitorName = displayList[monitor].ManufacturerCode + " " + displayList[monitor].Name,
                RefreshRate = displayList[monitor].Freq,
                Resolution = displayList[monitor].Resolution,
                EDIDProductcode = displayList[monitor].EDIDModel,
                FPSLimit = Convert.ToInt32(getSelectedFps()),
                Vsync = getVsyncState(),
                rtMethod = new ProcessData.rtMethods
                {
                    Name = Properties.Settings.Default.rtName,
                    Tolerance = Properties.Settings.Default.rtTolerance,
                    gammaCorrected = Properties.Settings.Default.rtGammaCorrected,
                    percentage = Properties.Settings.Default.rtPercentage
                },
                osMethod = new ProcessData.osMethods
                {
                    Name = Properties.Settings.Default.osName,
                    gammaCorrected = Properties.Settings.Default.osGammaCorrected,
                    endPercent = Properties.Settings.Default.osEndPercent,
                    rangePercent = Properties.Settings.Default.osRangePercent
                }
            };
        }

        private void Main_Load(object sender, EventArgs e)
        {
            
        }
        
        private void setFPSLimit()
        {
            var item = fpsList.Find(x => x.FPSValue == getSelectedFps()); 
            //var item = fpsList.Find(x => x.FPSValue == Properties.Settings.Default.FPS.ToString()); //Doesn't work, cba to work out why atm.
            port.Write("L" + int.Parse(item.Key).ToString("000"));
            Console.WriteLine("L" + int.Parse(item.Key).ToString("000"));
        }

        private void setCaptureTime()
        {
            port.Write("N" + Properties.Settings.Default.captureTime);
        }

        private void BrightnessCalBtn_Click(object sender, EventArgs e)
        {
            if (port != null)
            {
                //Thread brightCalThread = new Thread(new ThreadStart(this.launchBrightnessCal));
                //brightCalThread.Start();
                closeWindowBtn.Text = "Stop Calibration";

                launchBrightnessCal();
            }
            else
            {
                MessageBox.Show("Error: device not connected. Please connect the hardware via USB and try again!", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void launchBrightnessCal()
        {
            changeSizeAndState("brightness");
            ready = false;
            

            if (!brightnessCheck)
            {
                closeWindowBtn.Text = "Continue";
            }
            else
            {
                closeWindowBtn.Text = "Stop Calibration";
            }
            try
            {
                port.Write("B" + potVal.ToString());
                brightnessWindowOpen = true;
                brightnessCanceled = false;
                potVal = basePotVal;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void updateBrightness(int lvl, int val)
        {
            string txt = "";
            if (lvl < 61500)
            {
                txt = "Too Low!";
            }
            else if (lvl >= 61500 && lvl < 64000)
            {
                ready = true;
                txt = "Perfect!";
            }
            else if (lvl >= 64000)
            {
                txt = "Too High!";
            }
            else
            {
                txt = "No Results";
            }

            this.rawValText.Invoke((MethodInvoker)(() => rawValText.Text = lvl.ToString()));
            this.brightnessText.Invoke((MethodInvoker)(() => brightnessText.Text = txt));
            this.potValLabel.Invoke((MethodInvoker)(() => potValLabel.Text = val.ToString()));
            if (ready)
            {
                this.closeWindowBtn.Invoke((MethodInvoker)(() => closeWindowBtn.Enabled = true));
            }
        }

        private void closeWindowBtn_Click(object sender, EventArgs e)
        {
            try
            {
                brightnessCheck = true;
                port.Write("X");
                menuStrip1.Visible = true;
                changeSizeAndState("close brightness");
                brightnessWindowOpen = false;
                ready = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void incPotValBtn_Click(object sender, EventArgs e)
        {
            potVal += 1;
            if (potVal > 15)
            {
                MessageBox.Show("Your monitor's brightness may be too low. Make sure your monitor is set to its maximum brightness, otherwise any results the tool generates may be inaccurate.", "Brightness Too Low", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                incPotValBtn.Enabled = false;
            }
            else
            {
                try
                {
                    port.Write(potVal.ToString("X"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            potVal = basePotVal;
            incPotValBtn.Enabled = true;
            try
            {
                port.Write(basePotVal.ToString("X"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        delegate void changeSizeAndStateCallback(string state);
        private void changeSizeAndState(string state)
        {
            if (this.InvokeRequired)
            {
                var del = new changeSizeAndStateCallback(changeSizeAndState);
                this.Invoke(del, state);
            }
            else
            {
                switch (state)
                {
                    case "standard":
                        if (progressBarActive)
                        {
                            progressBar1.Location = new Point(0, 389);
                            Size = new Size(679, 451);
                        }
                        else { Size = new Size(679, 429); }
                        break;
                    case "brightness":
                        if (progressBarActive)
                        {
                            setProgressBar(false);
                        }
                        mainPanel.Location = new Point(1500, 26);
                        brightnessPanel.Location = new Point(0, 0);
                        aboutPanel.Location = new Point(1500, 402);
                        Size = new Size(1000, 800);
                        debugPanel.Location = new Point(1500, 30);
                        menuStrip1.Visible = false;
                        overdriveModes1.Location = new Point(1500, 559);
                        break;
                    case "close brightness":
                        mainPanel.Location = new Point(2, 29);
                        brightnessPanel.Location = new Point(1100, 36);
                        aboutPanel.Location = new Point(10, 412);
                        Size = new Size(679, 429);
                        debugPanel.Location = new Point(619, 30);
                        menuStrip1.Visible = true;
                        overdriveModes1.Location = new Point(10, 559);
                        break;
                    case "about":
                        aboutPanel.Location = new Point(10, 395);
                        if (progressBarActive)
                        {
                            progressBar1.Location = new Point(0, 508);
                            Size = new Size(679, 569);
                        }
                        else
                        { Size = new Size(679, 547); }
                        break;
                    case "debug":
                        Size = new Size(1089, 436);
                        debugPanel.Location = new Point(673, 32);
                        break;
                    case "overdrive":
                        if (progressBarActive)
                        {
                            setProgressBar(false);
                        }
                        overdriveModes1.mainWindow = this;
                        mainPanel.Location = new Point(1500, 26);
                        brightnessPanel.Location = new Point(1500, 0);
                        aboutPanel.Location = new Point(1500, 402);
                        Size = new Size(525, 330);
                        debugPanel.Location = new Point(1500, 30);
                        menuStrip1.Visible = false;
                        overdriveModes1.Location = new Point(0, 0);
                        break;
                    case "show progress bar":
                        Size s = Size;
                        progressBar1.Location = new Point(0, (s.Height - 45));
                        progressBar1.Visible = true;
                        progressBar1.BringToFront();
                        s.Height += 18;
                        Size = s;
                        break;
                    case "hide progress bar":
                        Size s2 = Size;
                        progressBar1.Location = new Point(0, 710);
                        progressBar1.Visible = false;
                        s2.Height -= 18;
                        Size = s2;
                        break;
                    default:
                        Size = new Size(679, 429);
                        break;
                }
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void openTrayBtn_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void quitTrayBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        static void IFailedYou(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            string type = e.Message.ToString();
            
            if (type != "Safe handle has been closed")
            {
                MessageBox.Show(e.Message, "Unexpected Error - Program Closing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (type.Contains("TimeoutException") || type.Contains("operation has timed out"))
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
            else
            {
                Console.WriteLine(e.Message + " " + e.StackTrace);
                Process[] p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                if (p.Length != 0)
                {
                    p[0].Kill();
                }
            }    
        }

        private void fpsLimitList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FPS = fpsLimitList.SelectedIndex;
            Properties.Settings.Default.Save();
            if (port != null)
            {
                if (port.IsOpen)
                {
                    setFPSLimit();
                }
            }
        }

        private void testCount_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Runs = Decimal.ToInt32(testCount.Value);
            Properties.Settings.Default.Save();
            if (port != null)
            {
                if (port.IsOpen)
                {
                    setRepeats();
                }
            }
        }
        private void saveUSBOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.USBOutput)
            {
                Properties.Settings.Default.USBOutput = false; 
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.USBOutput = true;
                Properties.Settings.Default.Save();
                if (port != null)
                {
                    port.Write("U");
                }
            }
        }

        private void minimiseToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MinToTray = minimiseToTrayToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void debugModeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (debugModeToolStripMenuItem.Checked)
            {
                changeSizeAndState("debug");
                debugMode = debugModeToolStripMenuItem.Checked;
            }
            else
            {
                changeSizeAndState("standard");
                debugMode = debugModeToolStripMenuItem.Checked;
            }
        }

        private void opnResultsBtn_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", path);
        }


        private DialogResult showMessageBox(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (!Properties.Settings.Default.SuppressDiagBox)
            {
                DialogResult d = MessageBox.Show(title, message, buttons, icon);
                return d;
            }
            else
            {
                return DialogResult.None;
            }
        }

        private void closeBrightnessBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //brightnessCheck = true;
                port.Write("X");
                menuStrip1.Visible = true;
                changeSizeAndState("close brightness");
                brightnessWindowOpen = false;
                ready = false;
                testRunning = false;
                brightnessCanceled = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("port"))
                {
                    menuStrip1.Visible = true;
                    changeSizeAndState("close brightness");
                    brightnessWindowOpen = false;
                    ready = false;
                    testRunning = false;
                    brightnessCanceled = true;
                }
                else
                { 
                    showMessageBox(ex.Message + ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    menuStrip1.Visible = true;
                    changeSizeAndState("close brightness");
                    brightnessWindowOpen = false;
                    ready = false;
                    testRunning = false;
                    brightnessCanceled = true;
                }

            }
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aboutProgramToolStripMenuItem.Checked)
            {
                changeSizeAndState("about");
            }
            else
            {
                changeSizeAndState("standard");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/andymanic/OSRTT/releases");
        }

        private void serialSendBtn_Click(object sender, EventArgs e)
        {
            if (port != null)
            {
                if (port.IsOpen)
                {
                    port.Write(serialSendBox.Text);
                    serialSendBox.Clear();
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // scroll it automatically
            richTextBox1.ScrollToCaret();
        }

        // very unfinished
        private void processInputLagData()
        {
            inputLagProcessed.Clear();

            try //Wrapped whole thing in try just in case
            {
                // Then process the lines
                //ProcessData pd = new ProcessData();
                ProcessData.averagedInputLag inputLagProcessed = ProcessData.AverageInputLagResults(inputLagRawData);

                // Write results to csv using new name
                decimal fileNumber = 001;
                // search /Results folder for existing file names, pick new name
                string[] existingFiles = Directory.GetFiles(resultsFolderPath, "*-INPUT-LATENCY-OSRTT.csv");
                // Search \Results folder for existing results to not overwrite existing or have save conflict errors
                foreach (var s in existingFiles)
                {
                    decimal num = 0;
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
                
                foreach (var res in inputLagProcessed.inputLagResults)
                {
                    csvString.AppendLine(
                        res.shotNumber.ToString() + "," +
                        res.clickTimeMs.ToString() + "," +
                        res.frameTimeMs.ToString() + "," +
                        res.onDisplayLatency.ToString() + "," +
                        res.totalInputLag.ToString()
                        );
                }
                csvString.AppendLine("AVERAGE," + inputLagProcessed.ClickTime.AVG.ToString() + "," + inputLagProcessed.FrameTime.AVG.ToString() + "," + inputLagProcessed.onDisplayLatency.AVG.ToString() + "," + inputLagProcessed.totalInputLag.AVG.ToString());
                csvString.AppendLine("MINIMUM," + inputLagProcessed.ClickTime.MIN.ToString() + "," + inputLagProcessed.FrameTime.MIN.ToString() + "," + inputLagProcessed.onDisplayLatency.MIN.ToString() + "," + inputLagProcessed.totalInputLag.MIN.ToString());
                csvString.AppendLine("MAXIMUM," + inputLagProcessed.ClickTime.MAX.ToString() + "," + inputLagProcessed.FrameTime.MAX.ToString() + "," + inputLagProcessed.onDisplayLatency.MAX.ToString() + "," + inputLagProcessed.totalInputLag.MAX.ToString());
                Console.WriteLine(filePath);
                File.WriteAllText(filePath, csvString.ToString());
                Process[] p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                if (p.Length != 0)
                {
                    p[0].Kill();
                }

                this.Invoke((MethodInvoker)delegate ()
                {
                    ResultsView rv = new ResultsView();
                    rv.setResultsFolder(resultsFolderPath);
                    rv.inputLagMode(inputLagProcessed);
                    rv.Show();
                });
                //Process.Start("explorer.exe", resultsFolderPath);
            }
            catch (Exception procEx)
            {
                Console.WriteLine(procEx);
                processingFailed = true;
                if (port != null)
                {
                    if (port.IsOpen)
                    {
                        port.Write("X");
                        showMessageBox("One or more set of results failed to process and won't be included in the multi-run averaging. \n " +
                            "Brightness may be too high or monitor may be strobing it's backlight. \n" +
                            "Try calibrating the brightness again, or use the Graph View Template to view the raw data.", "Processing Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        processingFailed = false;
                    }
                }
            }
        }

        private void timeBetweenSlider_Scroll(object sender, EventArgs e)
        {
            timeBetween = timeBetweenSlider.Value;
            timeBetween *= 0.5;
            Console.WriteLine(timeBetween);
            timeBetweenLabel.Text = timeBetween.ToString() + "s";
            // send to device?

            Properties.Settings.Default.timeBetween = timeBetween;
            Properties.Settings.Default.Save();
        }

        private void numberOfClicksSlider_Scroll(object sender, EventArgs e)
        {
            int value = (sender as TrackBar).Value;
            double indexDbl = (value * 1.0) / numberOfClicksSlider.TickFrequency;
            int index = Convert.ToInt32(Math.Round(indexDbl));

            numberOfClicksSlider.Value = numberOfClicksSlider.TickFrequency * index;

            numberOfClicks = numberOfClicksSlider.Value;
            numberOfClicksLabel.Text = numberOfClicks.ToString();
            Properties.Settings.Default.numberOfClicks = numberOfClicks;
            Properties.Settings.Default.Save();
        }

        private void inputLagButton_Click(object sender, EventArgs e)
        {
            if ( (boardType == 0 && boardVersion > 2.6) || (boardType == 1 && boardVersion > 1.0))
            {
                if (port != null)
                {
                    try
                    {
                        makeResultsFolder("IL");
                        port.Write("P");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ex.StackTrace);
                    }
                    //launchGameThread = new Thread(new ThreadStart(this.launchInputLagTest));
                    //launchGameThread.Start();
                    launchInputLagTest();
                }
            }
            else
            {
                DialogResult d = MessageBox.Show("ERROR: Can't run input lag test with your current board firmware version. Would you like to update?", "Firmware Version too Low", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (d == DialogResult.Yes)
                {
                    compareFirmware();
                }
            }
        }

        private void launchInputLagTest()
        {
            int selectedDisplay = getSelectedMonitor();
            try
            {
                ControlDeviceButtons(false);
                inputLagEvents.Clear();
                inputLagProcessed.Clear();
                inputLagRawData.Clear();
                if (OSRTT_Launcher.DirectX.System.DSystem.mainWindow == null)
                    OSRTT_Launcher.DirectX.System.DSystem.mainWindow = this;

                
                OSRTT_Launcher.DirectX.System.DSystem.inputLagMode = true;
                OSRTT_Launcher.DirectX.System.DSystem.StartRenderForm("OSRTT Test Window (DirectX 11)", 800, 600, false, true, selectedDisplay, 1);
                inputLagRun = !inputLagRun;
                port.Write("X");
                OSRTT_Launcher.DirectX.System.DSystem.inputLagMode = false;
                //OSRTT_Launcher.DirectX.System.DSystem.exit = true;
                Thread.Sleep(10);
                //OSRTT_Launcher.DirectX.System.DSystem.exit = false;
                //OSRTT_Launcher.DirectX.System.DSystem.mainWindow = null;
                //OSRTT_Launcher.DirectX.System.DSystem.
                ControlDeviceButtons(true);
            }
            catch (Exception strE)
            {
                Console.WriteLine(strE);
                SetText(strE.Message + strE.StackTrace);
            }
            //launchGameThread.Abort();
        }
        private void runDirectXWindow()
        {
            OSRTT_Launcher.DirectX.System.DSystem.mainWindow = this;
            OSRTT_Launcher.DirectX.System.DSystem.inputLagMode = true;
            OSRTT_Launcher.DirectX.System.DSystem.StartRenderForm("OSRTT Test Window (DirectX 11)", 800, 600, false, true, 1, 1);
            OSRTT_Launcher.DirectX.System.DSystem.inputLagMode = false;
        }
        public void getTestFPS(List<float> fpsList)
        {
            Console.WriteLine(fpsList.Average().ToString());
        }
        public void getInputLagEvents(List<float> fpsList)
        {
            inputLagEvents = fpsList;
            //Console.WriteLine(fpsList.Average().ToString());
        }

        private void captureTimeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.captureTime = captureTimeBox.SelectedIndex;
            Properties.Settings.Default.Save();
            if (port != null)
            {
                if (port.IsOpen)
                {
                    setCaptureTime();
                }
            }
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            HelpView hv = new HelpView();
            hv.Show();
        }

        private void bugReportMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/andymanic/OSRTT/issues/new/choose");
        }

        private void monitorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (displayList[monitorCB.SelectedIndex].Freq < 140)
            {
                if (captureTimeBox.SelectedIndex == 0)
                {
                    captureTimeBox.SelectedIndex = 1;
                    captureTimeBox_SelectedIndexChanged(null, null);
                }
            }
        }

        private void vsyncStateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (vsyncStateList.SelectedIndex == 0)
            {
                Properties.Settings.Default.VSyncState = false;
            }
            else
            {
                Properties.Settings.Default.VSyncState = true;
            }
            Properties.Settings.Default.Save();
            if (port != null)
            {
                port.Write("V" + vsyncStateList.SelectedIndex.ToString());
            }
        }

        private void setProgressBar(bool on)
        {
            if (on)
            {
                progressBarActive = true;
                changeSizeAndState("show progress bar");
                if (progressBar1.InvokeRequired)
                {
                    this.progressBar1.Invoke((MethodInvoker)(() => { this.progressBar1.Style = ProgressBarStyle.Marquee; this.progressBar1.MarqueeAnimationSpeed = 30; }));
                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    progressBar1.MarqueeAnimationSpeed = 30;
                }
            }
            else
            {
                progressBarActive = false;
                changeSizeAndState("hide progress bar");
                if (progressBar1.InvokeRequired)
                {
                    this.progressBar1.Invoke((MethodInvoker)(() => { this.progressBar1.Style = ProgressBarStyle.Continuous; this.progressBar1.MarqueeAnimationSpeed = 0; }));
                }
                else
                {
                    progressBar1.Style = ProgressBarStyle.Continuous;
                    progressBar1.MarqueeAnimationSpeed = 0;
                }
            }
        }

        private void testButtonToolStripMenuItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //testRawInput();
            port.Write("J");
            //port.Write("W");
            //Thread.Sleep(500);
            //SendKeys.SendWait("{NUM0}");
            //runDirectXWindow();
        }
        static void testRawInput()
        {
            Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericMouse, DeviceFlags.None);
            Device.MouseInput += (sender, args) => UpdateMouseText(args);

            Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericKeyboard, DeviceFlags.None);
            Device.KeyboardInput += (sender, args) => UpdateKeyboardText(args);
        }
        public delegate void UpdateTextCallback(EventHandler<KeyboardInputEventArgs> args);
        static void UpdateMouseText(RawInputEventArgs rawArgs)
        {
            var args = (MouseInputEventArgs)rawArgs;

            Console.WriteLine(string.Format("(x,y):({0},{1}) Buttons: {2} State: {3} Wheel: {4}\r\n", args.X, args.Y, args.ButtonFlags, args.Mode, args.WheelDelta));
        }

        static void UpdateKeyboardText(RawInputEventArgs rawArgs)
        {
            var args = (KeyboardInputEventArgs)rawArgs;
            
            Console.WriteLine(string.Format("Key: {0} State: {1} ScanCodeFlags: {2}\r\n", args.Key, args.State, args.ScanCodeFlags));
        }

        private void resultsViewBtn_Click(object sender, EventArgs e)
        {
            ResultsView resultsView = new ResultsView();
            resultsView.Show();
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
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

        private void testSettingsBtn_Click(object sender, EventArgs e)
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

        private void ResultsFolderBtn_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", path);
        }

        private void processAllRuns(ProcessData.rtMethods rtMethod, ProcessData.osMethods osMethod)
        {
            averageData.Clear();
            multipleRunData.Clear();
            CFuncs cf = new CFuncs();
            ProcessData pd = new ProcessData();
            int startDelay = pd.processTestLatency(testLatency);
            foreach (var i in results)
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
            List<List<ProcessData.processedResult>> processedData = new List<List<ProcessData.processedResult>>();
            processedData.AddRange(pd.ProcessAllResults(results, new ProcessData.resultSelection
            {
                rtStyle = rtMethod,
                osStyle = osMethod
            }, startDelay, processedGamma, runSettings));
            
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
                    cf.showMessageBox("Failed to Process", "One or more of the results failed to process and has been left blank.",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                if (runSettings != null)
                {
                    csvString.AppendLine(JsonConvert.SerializeObject(runSettings));
                }
                string fullFilePath = resultsFolderPath + "\\" + fullFileName;
                File.WriteAllText(fullFilePath, csvString.ToString());
                multipleRunData.Add(res);
            }
            if (multipleRunData.Count != 0)
            {
                averageData.AddRange(pd.AverageMultipleRuns(processedData, osMethod));
            }
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
        private void runProcessing()
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

            int monitor = getSelectedMonitor();
            int fps = Convert.ToInt32(getSelectedFps());
            bool vsync = getVsyncState();
            string[] folderSplit = resultsFolderPath.Split('\\');
            string runName = folderSplit.Last();
            if (runSettings == null)
            {
                runSettings = new ProcessData.runSettings
                {
                    RunName = runName,
                    RefreshRate = displayList[monitor].Freq,
                    Resolution = displayList[monitor].Resolution,
                    EDIDProductcode = displayList[monitor].EDIDModel,
                    FPSLimit = fps,
                    DateAndTime = DateTime.Now.ToString(),
                    MonitorName = displayList[monitor].ManufacturerCode + " " + displayList[monitor].Name,
                    Vsync = vsync,
                    osMethod = os,
                    rtMethod = rt,

                };
            }
            try
            {
                processAllRuns(rt, os);
            }
            catch (Exception ex)
            {
                SetText(ex.Message + ex.StackTrace);
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            DataUpload ds = new DataUpload();
            ds.ShareResults(results, gamma, testLatency, runSettings);
        }
        private void initRtOsMethods()
        {
            rtMethod = new ProcessData.rtMethods
            {
                Name = Properties.Settings.Default.rtName,
                Tolerance = Properties.Settings.Default.rtTolerance,
                gammaCorrected = Properties.Settings.Default.rtGammaCorrected,
                percentage = Properties.Settings.Default.rtPercentage
            };
            osMethod = new ProcessData.osMethods
            {
                Name = Properties.Settings.Default.osName,
                gammaCorrected = Properties.Settings.Default.osGammaCorrected,
                endPercent = Properties.Settings.Default.osEndPercent,
                rangePercent = Properties.Settings.Default.osRangePercent
            };
        }

        private void LiveViewBtn_Click(object sender, EventArgs e)
        {
            liveView = true;
            ControlDeviceButtons(false);
            LiveViewObject = new LiveView();
            port.Write("O");
            LiveViewObject.m = this;
            LiveViewObject.Show();

        }

        public void exitLiveView()
        {
            liveView = false;
            ControlDeviceButtons(true);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            port.Write("W");
        }
    }
}

