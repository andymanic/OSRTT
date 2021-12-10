using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;
using Microsoft.Management.Infrastructure;
using System.Management;
using System.Threading;
using System.IO;
using WindowsDisplayAPI.DisplayConfig;
using AutoUpdaterDotNET;
using Newtonsoft.Json;
using System.Resources;
using System.Runtime.InteropServices;

namespace OSRTT_Launcher
{
    public partial class Main : Form
    {
        // CHANGE THESE VALUES WHEN ISSUING A NEW RELEASE
        private double boardVersion = 1.5;
        private double downloadedFirmwareVersion = 1.5;
        private string softwareVersion = "1.7";

        // TODO //
        // Create dedicated gamma test, possibly move to 8x8 instead of 11x11
        //
        // CANCEL TEST IF GAME CLOSED!!! (serial buffer still full of multiple results?? use checkfocusedwindow to also check if program is open? Although launchgame func should handle that and close..)
        // 
        // Add check if keyboard folder exists/when update happens catch error say thats Keyboard isn't installed, offer user an option to retry and install keyboard then (separately) run update again...
        // or download the zip file I'll upload to the github and extract to documents folder.
        //
        // Add better error messages to say results weren't processed
        //
        //
        // Current known issues //
        //

        public static System.IO.Ports.SerialPort port;
        delegate void SetTextCallback(string text);
        private bool boardUpdate = false;
        private bool portConnected = false;
        private bool brightnessCheck = false;
        public bool brightnessWindowOpen = false;
        public bool brightnessCanceled = false;
        private TimeSpan uptime;

        private bool debugMode = false;
        private bool testRunning = false;
        private bool processingFailed = false;
        private bool ready = false;
        private bool testMode = false;
        private bool gammaTest = false;

        private int[] RGBArr;
        private int resultsLimit = 110;

        private int potVal = 0;

        string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        string resultsFolderPath = "";
        private List<int[]> results = new List<int[]>();
        private List<int[]> gamma = new List<int[]>();

        private List<List<double[]>> multipleRunData = new List<List<double[]>>();
        public class Displays
        {
            public string Name { get; set; }
            public int Freq { get; set; }
            public string Connection { get; set; }
        }
        public List<Displays> displayList = new List<Displays>();
        public class FPS
        {
            public string FPSValue { get; set; }
            public string Key { get; set; }
        }
        public List<FPS> fpsList = new List<FPS>();
        private BackgroundWorker hardWorker;
        private Thread readThread = null;
        private Thread connectThread = null;
        private Thread processThread = null;
        private Thread launchGameThread = null;
        private Thread checkWindowThread = null;

        private ResourceManager rm = OSRTT_Launcher.Properties.Resources.ResourceManager;

        private ContextMenu contextMenu1 = new ContextMenu();
        private MenuItem statusTrayBtn = new MenuItem();
        private MenuItem openTrayBtn = new MenuItem();
        private MenuItem closeTrayBtn = new MenuItem();

        public void UpdateMe()
        {
            AutoUpdater.InstalledVersion = new Version(softwareVersion);
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Days;
            AutoUpdater.RemindLaterAt = 2;
            AutoUpdater.RunUpdateAsAdmin = false;
            AutoUpdater.HttpUserAgent = "Autoupdater";
            AutoUpdater.UpdateFormSize = new System.Drawing.Size(800, 600);
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

        private void appRunning()
        {
            Process[] p = Process.GetProcessesByName("OSRTT Launcher");
            Console.WriteLine(p.Length);
            if (p.Length > 1)
            {
                MessageBox.Show("ERROR: Program already open! Please close it before running again, or check the task bar and system tray for the running app.", "Program Open Already", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(p.Length);
                this.Close();
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
            verboseOutputToolStripMenuItem.Checked = Properties.Settings.Default.Verbose;
            saveGammaTableToolStripMenuItem.Checked = Properties.Settings.Default.saveGammaTable;
            saveSmoothedDataToolStripMenuItem.Checked = Properties.Settings.Default.saveSmoothData;
            threePercentMenuItem.Checked = Properties.Settings.Default.threePercentSetting;
            tenPercentMenuItem.Checked = Properties.Settings.Default.tenPercentSetting;
            fixedRGB5OffsetToolStripMenuItem.Checked = Properties.Settings.Default.RGB5Offset;
            fixedRGB10OffsetToolStripMenuItem.Checked = Properties.Settings.Default.RGB10Offset;
            gammaCorrectedToolStripMenuItem.Checked = Properties.Settings.Default.gammaCorrectedSetting;
            percentageToolStripMenuItem.Checked = Properties.Settings.Default.gammaPercentSetting;
            gamCorMenuItem.Checked = Properties.Settings.Default.gammaCorrRT;
            saveUSBOutputToolStripMenuItem.Checked = Properties.Settings.Default.USBOutput;
            minimiseToTrayToolStripMenuItem.Checked = Properties.Settings.Default.MinToTray;
            suppressDialogBoxesToolStripMenuItem.Checked = Properties.Settings.Default.SuppressDiagBox;
            perceivedRGB5MenuItem.Checked = Properties.Settings.Default.PerceivedRGB5;
            perceivedRGB10MenuItem.Checked = Properties.Settings.Default.PerceivedRGB10;
        }

        public Main()
        {
            InitializeComponent();
            UpdateMe();
            setupFormElements();
            ControlDeviceButtons(false);
            path = new Uri(System.IO.Path.GetDirectoryName(path)).LocalPath;
            path += @"\Results";
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            this.FormClosing += new FormClosingEventHandler(Main_FormClosing);
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(IFailedYou);
            this.Resize += new EventHandler(Main_Resize);
            initialiseSettings();
            hardWorker = new BackgroundWorker();
            connectThread = new Thread(new ThreadStart(this.findAndConnectToBoard));
            connectThread.Start();
            changeSizeAndState("standard");
            listMonitors();
            listFramerates();
            initialSetup();
            checkFolderPermissions();
            uptime = GetUpTime();
            if (uptime.TotalMinutes < 30)
            {
                showMessageBox("It is HIGHLY RECOMMENDED you allow the display to warm up BEFORE testing - it looks like your system hasn't been running for more than 30 minutes." +
                    "\n You are welcome to continue, but it's recommended you allow the display to run for around 30 minutes before testing.","Monitor Warm Up",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
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
                    process.StartInfo.Arguments = "/C .\\arduinoCLI\\arduino-cli.exe config init && .\\arduinoCLI\\arduino-cli.exe config add board_manager.additional_urls https://adafruit.github.io/arduino-board-index/package_adafruit_index.json && .\\arduinoCLI\\arduino-cli.exe core update-index && .\\arduinoCLI\\arduino-cli.exe core install arduino:samd && .\\arduinoCLI\\arduino-cli.exe core install adafruit:samd && .\\arduinoCLI\\arduino-cli.exe lib install Keyboard";
                    //process.StartInfo.UseShellExecute = false;
                    //process.StartInfo.RedirectStandardOutput = true;
                    //process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    //string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    //Console.WriteLine(output);
                }
            }
        }
        private void AutoUpdater_ApplicationExitEvent()
        {
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
        public static TimeSpan GetUpTime()
        {
            return TimeSpan.FromMilliseconds(GetTickCount64());
        }
        [DllImport("kernel32")]
        extern static UInt64 GetTickCount64();

        private void listMonitors()
        {
            monitorCB.Items.Clear(); // Clear existing array and list before filling them
            displayList.Clear();
            
            foreach (var target in WindowsDisplayAPI.DisplayConfig.PathInfo.GetActivePaths())
            {
                foreach (var item in target.TargetsInfo)
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
                    int refresh = ((int)item.FrequencyInMillihertz) / 1000;
                    string name = item.DisplayTarget.ToString();
                    if (name == "")
                    {
                        name = target.DisplaySource.ToString().Remove(0, 4);
                    }
                    var data = new Displays { Name = name, Freq = refresh, Connection = con };
                    displayList.Add(data);
                    monitorCB.Items.Add(name);
                }
            }
            monitorCB.SelectedIndex = 0; // Pre-select the primary display
        }

        private void listFramerates()
        {
            fpsLimitList.Items.Clear();
            fpsList.Clear();
            fpsList.Add(new FPS { FPSValue = "1000", Key = "1" });
            fpsList.Add(new FPS { FPSValue = "360", Key = "2" });
            fpsList.Add(new FPS { FPSValue = "240", Key = "3" });
            fpsList.Add(new FPS { FPSValue = "170", Key = "4" });
            fpsList.Add(new FPS { FPSValue = "165", Key = "5" });
            fpsList.Add(new FPS { FPSValue = "144", Key = "6" });
            fpsList.Add(new FPS { FPSValue = "120", Key = "7" });
            fpsList.Add(new FPS { FPSValue = "100", Key = "8" });
            fpsList.Add(new FPS { FPSValue = "75", Key = "9" });
            fpsList.Add(new FPS { FPSValue = "60", Key = "0" });
            foreach (var f in fpsList)
            {
                fpsLimitList.Items.Add(f.FPSValue);
            }
            fpsLimitList.SelectedIndex = Properties.Settings.Default.FPS;
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
            while (true)
            {
                if (!portConnected)
                {
                    ControlDeviceButtons(false);
                    SetDeviceStatus("Board Disconnected");
                    Thread.Sleep(1000);
                    if (this.firmVerLbl.IsHandleCreated)
                    {
                        this.firmVerLbl.Invoke((MethodInvoker)(() => this.firmVerLbl.Text = "N/A"));
                    }
                    testRunning = false;
                    appRunning();
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
                        if (s.Contains("adafruit:samd:adafruit_itsybitsy_m4"))
                        {
                            char[] whitespace = new char[] { ' ', '\t' };
                            string[] split = s.Split(whitespace);
                            p = split[0];
                        }
                    }
                    if (p != "")
                    {
                        try
                        {
                            connectToBoard(p);
                            Thread.Sleep(2000);
                            SetDeviceStatus("Connected to Device!");
                            ControlDeviceButtons(true);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
                else if (boardUpdate)
                {
                    if (boardVersion < downloadedFirmwareVersion)
                    {
                        string p = ""; 
                        p = port.PortName;
                        if (port.IsOpen)
                        {
                            ControlDeviceButtons(false);
                            // readThread.Abort();
                            port.Close();
                        }
                        Console.WriteLine("Outside the port close");
                        
                        if (p == "")
                        {
                            MessageBox.Show("Please connect to the device first!", "Update Device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            SetDeviceStatus("Updating Firmware");
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            process.StartInfo.FileName = "cmd.exe";
                            
                            Console.WriteLine("ready to start");
                            process.StartInfo.Arguments = "/C .\\arduinoCLI\\arduino-cli.exe compile --fqbn adafruit:samd:adafruit_itsybitsy_m4 .\\arduinoCLI\\OSRTT_Full_Code && .\\arduinoCLI\\arduino-cli.exe upload --port " + p + " --fqbn adafruit:samd:adafruit_itsybitsy_m4 .\\arduinoCLI\\OSRTT_Full_Code";
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.RedirectStandardOutput = true;
                            process.StartInfo.CreateNoWindow = true;
                            try
                            {
                                Console.WriteLine("starting");
                                process.Start();
                                string output = process.StandardOutput.ReadToEnd();
                                Console.WriteLine(output);
                                process.WaitForExit();
                                //MessageBox.Show(output);
                                if (output.Contains("Error"))
                                {
                                    MessageBox.Show("Firmware update failed. Error message: " + output, "Update Device Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    SetDeviceStatus("Update Failed");
                                }
                                else
                                {
                                    MessageBox.Show("Device has been updated successfully!", "Updated Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    SetDeviceStatus("Update Complete");
                                }
                                
                                boardUpdate = false;
                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Unable to write to device, check it's connected via USB.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Console.WriteLine(ex);
                            }
                        }
                        this.firmVerLbl.Invoke((MethodInvoker)(() => this.firmVerLbl.Text = "V" + boardVersion));
                    }
                    else
                    {
                        MessageBox.Show("Device already up to date!", "Update Device", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Thread.Sleep(2000);
                }
            }
        }

        private void connectToBoard(String comPort)
        {
            System.ComponentModel.IContainer components =
                new System.ComponentModel.Container();
            port = new System.IO.Ports.SerialPort(components);
            port.PortName = comPort;
            port.BaudRate = 115200;
            port.DtrEnable = true;
            port.ReadTimeout = 5000;
            port.WriteTimeout = 1000;
            port.ReadBufferSize = 128000;
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
                port.Write("I" + (this.testCount.Value - 1).ToString());
                setFPSLimit();
            }
            else
            {
                SetDeviceStatus("Board Disconnected");
                ControlDeviceButtons(false);
            }
        }

        private void compareFirmware()
        {
            if (boardVersion < downloadedFirmwareVersion && !Properties.Settings.Default.SuppressDiagBox)
            {
                DialogResult dialogResult = MessageBox.Show("A newer version of the board's firmware is available, do you want to update now? \n Current version: " + boardVersion + "\n New version: " + downloadedFirmwareVersion, "Board Firmware Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    //updateFirmware();
                    boardUpdate = true;
                }
            }
        }
        
        private void ControlDeviceButtons(bool state)
        {
            if (this.launchBtn.InvokeRequired)
            {
                this.launchBtn.Invoke((MethodInvoker)(() => launchBtn.Enabled = state));
                this.menuStrip1.Invoke((MethodInvoker)(() => BrightnessCalBtn.Visible = state));
            }
            else
            {
                this.launchBtn.Enabled = state;
                this.BrightnessCalBtn.Visible = state;
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

        private void SetDeviceStatus(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.devStat.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetDeviceStatus);
                this.Invoke(d, new object[] { text }); //check if this needs to be an array
                this.statusTrayBtn.Text = text;
                this.notifyIcon.Text = text;
            }
            else { this.devStat.Text = text; }
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

        public void Read()
        {
            while (port.IsOpen)
            {
                try
                {
                    string message = port.ReadLine();
                    Console.WriteLine(message);
                    if (debugMode)
                    {
                        SetText(message);
                    }
                    if (message.Contains("RGB Array"))
                    {
                        // Don't really need this bit.. in theory it's good for expandability but... meh
                        string newMessage = message.Remove(0, 12);
                        string[] values = newMessage.Split(',');
                        RGBArr = new int[values.Length - 1];
                        int counter = 0;
                        foreach (string s in values)
                        {
                            Console.WriteLine("String = " + s);
                            if (s.Length == 1 && !s.Contains("0"))
                            {
                                Console.WriteLine("Filtered the bugger");
                            }
                            else
                            {
                                RGBArr[counter] = int.Parse(s);
                                counter++;
                            }
                        }
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
                                results.Add(intValues);
                            }
                            else
                            {
                                port.Write("X");
                                port.Write("X");
                                port.Write("X");
                                testRunning = false;
                                MessageBox.Show("The last test result showed no difference in light level. The brightness may be too high. The test has been cancelled.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            if (start > end)
                            {
                                results.Add(intValues);
                            }
                            else
                            {
                                port.Write("X");
                                port.Write("X");
                                port.Write("X");
                                testRunning = false;
                                MessageBox.Show("The last test result showed no difference in light level. The brightness may be too high. The test has been cancelled.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
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
                    else if (message.Contains("STARTING RUN"))
                    {
                        results.Clear();
                        testRunning = true;
                    }
                    else if (message.Contains("STARTING TEST"))
                    {
                        testRunning = true;
                        makeResultsFolder();
                        multipleRunData.Clear();
                        results.Clear();
                    }
                    else if (message.Contains("Run Complete"))
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
                        foreach (var res in results)
                        {
                            csvString.AppendLine(string.Join(strSeparator, res));
                        }
                        File.WriteAllText(filePath, csvString.ToString());

                        decimal gammaFileNumber = 001;
                        // search /Results folder for existing file names, pick new name
                        string[] existingGammaFiles = Directory.GetFiles(resultsFolderPath, "*-GAMMA-RAW-OSRTT.csv");
                        //search files for number
                        foreach (var s in existingGammaFiles)
                        {
                            decimal num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                            if (num >= gammaFileNumber)
                            {
                                gammaFileNumber = num + 1;
                            }
                        }

                        string gammaFilePath = resultsFolderPath + "\\" + gammaFileNumber.ToString("000") + "-GAMMA-RAW-OSRTT.csv";

                        StringBuilder gammaCsvString = new StringBuilder();
                        foreach (var res in gamma)
                        {
                            gammaCsvString.AppendLine(string.Join(strSeparator, res));
                        }
                        File.WriteAllText(gammaFilePath, gammaCsvString.ToString());

                        // Process that raw data
                        processThread = new Thread(new ThreadStart(this.processResponseTimeData));
                        processThread.Start();
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
                            gammaTest = false;
                        }
                    }
                    else if (message.Contains("Test Complete"))
                    {
                        if (processingFailed)
                        {
                            MessageBox.Show("One or more set of results failed to process and won't be included in the multi-run averaging. Brightness may be too high - try calibrating the brightness and running the test again.", "Processing Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            processingFailed = false;
                        }
                        Thread.Sleep(500); //Had an issue with data processing not being finished by the time the command comes it to start averaging the data.
                        processMultipleRuns();
                        port.Write("T");
                        DialogResult d = MessageBox.Show("Test complete, open results folder?","Test Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (d == DialogResult.Yes)
                        {
                            //open folder
                            Process.Start("explorer.exe", resultsFolderPath);
                            testRunning = false;
                        }
                        else
                        {
                            testRunning = false;
                        }
                    }
                    else if (message.Contains("FW:"))
                    {
                        string[] sp = message.Split(':');
                        boardVersion = double.Parse(sp[1]);
                        compareFirmware();
                        this.firmVerLbl.Invoke((MethodInvoker)(() => this.firmVerLbl.Text = "V" + boardVersion));
                    }
                    else if (message.Contains("Runs:"))
                    {
                        string[] sp = message.Split(':');
                        int runs = Int32.Parse(sp[1]);
                        if (runs != (this.testCount.Value - 1))
                        {
                            setRepeatCounter(runs);
                        }
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
                        port.Write(potVal.ToString("X"));
                    }
                    else if (message.Contains("USB V:"))
                    {
                        String newMessage = message.Remove(0, 6);
                        if (Properties.Settings.Default.USBOutput)
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
                            if (voltage > 50000) { potVal = 0; }
                            else if (voltage > 49500) { potVal = 1; }
                            else if (voltage > 49000) { potVal = 2; }
                            else if (voltage > 48500) { potVal = 3; }
                            else if (voltage > 48000) { potVal = 4; }
                            else if (voltage > 47500) { potVal = 5; }
                            else { potVal = 6; }
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
                    else
                    {
                        this.SetText(message);
                    }
                }
                catch (TimeoutException ex)
                {
                    Console.WriteLine(ex);
                    SetText(ex.Message + ex.StackTrace);
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
                    }
                    SetText(e.Message + e.StackTrace);
                    Console.WriteLine(e);
                    Console.WriteLine("Trying to reconnect");
                    port.Close();
                    portConnected = false;
                    readThread.Abort();
                    testRunning = false;
                    testMode = false;
                    brightnessCheck = false;
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
            Environment.Exit(Environment.ExitCode);
        }

        private void launchBtn_Click(object sender, EventArgs e)
        {
            if (!brightnessCheck)
            {
                launchBrightnessCal();
            }
            Properties.Settings.Default.FPS = fpsLimitList.SelectedIndex;
            Properties.Settings.Default.Runs = Decimal.ToInt32(testCount.Value);
            Properties.Settings.Default.Save();
            
            // block game until brightness window closes - that done already thanks to dialog result?
            launchGameThread = new Thread(new ThreadStart(this.launchGameAndWaitForExit));
            launchGameThread.Start();
        }

        private void launchGameAndWaitForExit()
        {
            while (brightnessWindowOpen)
            {
                Thread.Sleep(100);
            }
            if (!brightnessCanceled)
            {
                ControlDeviceButtons(false);
                // Save current & FPS to hardware on run
                Thread.Sleep(200);
                setFPSLimit();
                Thread.Sleep(200);
                setRepeats();
                testRunning = true;

                // Launch UE4 game
                // thinking about it you can probably just bundle this into one process instead of launching, then finding it again...
                string ue4Path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                ue4Path = new Uri(System.IO.Path.GetDirectoryName(ue4Path)).LocalPath;
                ue4Path += @"\OSRTT UE4\ResponseTimeTest.exe";
                try
                {
                    Process.Start(ue4Path);
                }
                catch (Exception strE)
                {
                    Console.WriteLine(strE);
                    SetText(strE.Message + strE.StackTrace);
                }
                try
                {
                    Process[] p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                    while (p.Length == 0)
                    {
                        // Added in case game hasn't finished launching yet
                        p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                    }
                    while (!testMode) // hacky and I don't like it but for some reason it's not detecting this
                    {
                        try
                        {
                            port.Write("T");
                        }
                        catch (Exception exc)
                        {
                            SetText(exc.Message + exc.StackTrace);
                            Console.WriteLine(exc);
                        }
                        Thread.Sleep(200);
                    }
                    checkWindowThread = new Thread(new ThreadStart(this.checkFocusedWindow));
                    checkWindowThread.Start();
                    // Wait for game to close then send cancel command to board
                    p[0].WaitForExit();
                    checkWindowThread.Abort();
                    Console.WriteLine("Game closed");
                    SetText("Game closed");
                    port.Write("X");
                    ControlDeviceButtons(true);
                    testRunning = false;
                    testMode = false;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e);
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
            else
            {
                brightnessCanceled = false;
            }
        }

        private void checkFocusedWindow()
        {
            FocusedWindow fw = new FocusedWindow();
            string pName = fw.GetForegroundProcessName();
            bool paused = false;
            while (true)
            {
                Thread.Sleep(1000);
                pName = fw.GetForegroundProcessName();
                if (pName != "ResponseTimeTest-Win64-Shipping")
                {
                    Console.WriteLine("Process not selected");
                    if (!paused)
                    {
                        port.Write("P");
                        paused = true;
                    }
                }
                else 
                { 
                    if (paused)
                    {
                        port.Write("S");
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
                        break;
                    }
                }
            }
        }

        private void refreshMonitorListBtn_Click(object sender, EventArgs e)
        {
            listMonitors();
        }

        private void resultsBtn_Click(object sender, EventArgs e)
        {
            // Open file picker dialogue
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    results.Clear();
                    gamma.Clear();
                    if (filePath.Contains("GAMMA-RAW"))
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
                                    gamma.Add(intLine);
                                }
                            }
                            resultsFolderPath = filePath.Substring(0, filePath.LastIndexOf('\\'));
                            // Save Gamma curve to a file too
                            decimal gammaFileNumber = 001;
                            // search /Results folder for existing file names, pick new name
                            string[] existingGammaFiles = Directory.GetFiles(resultsFolderPath, "*-GAMMA-OSRTT.csv");
                            // Search \Results folder for existing results to not overwrite existing or have save conflict errors
                            foreach (var s in existingGammaFiles)
                            {
                                decimal num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                                if (num >= gammaFileNumber)
                                {
                                    gammaFileNumber = num + 1;
                                }
                            }

                            string gammaFilePath = resultsFolderPath + "\\" + gammaFileNumber.ToString("000") + "-GAMMA-OSRTT.csv";
                            StringBuilder gammaCsvString = new StringBuilder();
                            gammaCsvString.AppendLine("RGB, Light Reading");
                            string strSeparator = ",";
                            List<int[]> fullGammaTable = processGammaTable();
                            foreach (var res in fullGammaTable)
                            {
                                gammaCsvString.AppendLine(string.Join(strSeparator, res));
                            }
                            File.WriteAllText(gammaFilePath, gammaCsvString.ToString());
                            Process.Start("explorer.exe", resultsFolderPath);

                        }
                        catch
                        {
                            DialogResult d = MessageBox.Show("File may be in use by another program, please make sure it's not open elsewhere and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (filePath.Contains("RAW-OSRTT"))
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
                                    results.Add(intLine);
                                }
                            }
                            resultsFolderPath = filePath.Substring(0, filePath.LastIndexOf('\\'));
                            //processThread = new Thread(new ThreadStart(this.processResponseTimeData));
                            //processThread.Start();
                            processResponseTimeData();
                            Process.Start("explorer.exe", resultsFolderPath);

                        }
                        catch
                        {
                            DialogResult d = MessageBox.Show("File may be in use by another program, please make sure it's not open elsewhere and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, only 'RAW' files can be imported. Please select either a 'RAW-OSRTT.csv' file, or 'GAMMA-RAW-OSRTT.csv' file instead.", "Importer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }             
                }
            }
        }

        private void updateDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compareFirmware();
        }

        private void analyseResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (analyseResultsToolStripMenuItem.Checked)
            {
                changeSizeAndState("analyse");
            }
            else
            {
                changeSizeAndState("standard");
            }
        }

        private void debugModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            changeSizeAndState("debug");
            debugMode = debugModeToolStripMenuItem.Checked;
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
                    lineAverage /= (dataLine.Length - 100);

                    rgbVals[i] = gamma[i][0];
                    lightLevelVals[i] = lineAverage;
                }
                /*int gSize = tempGamma.Count;
                PointF[] points = new PointF[gSize];
                for (int i = 0; i < gSize; i++)
                {
                    points[i] = new PointF { X = tempGamma[i][0], Y = tempGamma[i][1] };
                };
                int numberOfPoints = 256;
                PointF[] partGamma = InterpolatePoints(points, numberOfPoints);*/

                var interpPoints = new ScottPlot.Statistics.Interpolation.NaturalSpline(rgbVals, lightLevelVals, 51);
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

        private void processResponseTimeData()
        {
            //This is a long one. This is the code that builds the gamma curve, finds the start/end points and calculates response times and overshoot % (gamma corrected)
            List<double[]> processedData = new List<double[]>();

            // First, create gamma array from the data
            List<int[]> localGamma = new List<int[]>();
            List<int[]> fullGammaTable = new List<int[]>();
            List<int[]> smoothedDataTable = new List<int[]>();

            try //Wrapped whole thing in try just in case
            {
                if (gamma.Count == 6)
                { // if using the new test pattern (Constant step of 51)
                    fullGammaTable.AddRange(processGammaTable());
                }
                else
                { // if using old test pattern (Steps of 25 or 26)
                    int steps = 0;
                    if (results.Count == 30)
                    {
                        steps = 9;
                    }
                    else
                    {
                        steps = 20;
                    }
                    for (int i = 0; i < steps; i += 2)
                    {
                        int[] resLine = this.results[i];
                        int avg = 0;
                        if (resLine[0] == 0 && localGamma.Count == 0)
                        {
                            for (int j = 5; j < 250; j++)
                            {
                                avg += resLine[j];
                            }
                            avg = avg / 245;
                            localGamma.Add(new int[] { resLine[0], avg });
                            for (int j = resLine.Length - 455; j < resLine.Length - 5; j++)
                            {
                                avg += resLine[j];
                            }
                            avg = avg / 450;
                            localGamma.Add(new int[] { resLine[1], avg });
                        }
                        else
                        {
                            for (int j = resLine.Length - 455; j < resLine.Length - 5; j++)
                            {
                                avg += resLine[j];
                            }
                            avg = avg / 450;
                            localGamma.Add(new int[] { resLine[1], avg });
                        }
                    }
                    // Extrapolate rough values for every RGB value
                    for (int i = 0; i < localGamma.Count - 1; i++)
                    {
                        PointF[] points = new PointF[]
                            {
                                new PointF { X = localGamma[i][0], Y = localGamma[i][1]},
                                new PointF { X = localGamma[i+1][0],  Y = localGamma[i+1][1]}
                            };
                        int numberOfPoints = localGamma[i + 1][0] - localGamma[i][0];

                        PointF[] partGamma = InterpolatePoints(points, numberOfPoints);
                        foreach (var p in partGamma)
                        {
                            int[] tempGamma = {
                                Convert.ToInt32(p.X), Convert.ToInt32(p.Y)
                            };
                            fullGammaTable.Add(tempGamma);
                        }
                    }
                    if (results.Count == 30)
                    {
                        fullGammaTable.Add(localGamma[5]);
                    }
                    else
                    {
                        fullGammaTable.Add(localGamma[10]);
                    }
                }

                // Then process the lines
                foreach (int[] item in this.results)
                {
                    // Save start, end, time and sample count then clear the values from the array
                    int StartingRGB = item[0];
                    int EndRGB = item[1];
                    int TimeTaken = item[2];
                    int SampleCount = item[3];
                    int[] samples = item.Skip(4).ToArray();

                    double SampleTime = ((double)TimeTaken / (double)SampleCount); // Get the time taken between samples

                    // Clean up noisy data using moving average function
                    int period = 10;
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
                    for (int l = 0; l < 250; l++) //CHANGE TO 180 FOR RUN 2 DATA
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
                    for (int m = samples.Length - 5; m > samples.Length - 450; m--)
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
                        int t = transEnd / 5;
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
                    // Getting min/max from INSIDE the transition window
                    if ((transEnd - transStart) != 0)
                    {
                        int[] transitionSamples = new int[(transEnd - transStart)];
                        Array.Copy(samples, transStart, transitionSamples, 0, (transEnd - transStart));
                        maxValue = transitionSamples.Max();
                        minValue = transitionSamples.Min();
                    }
                    //Overshoot calculations
                    double overshootPercent = 0;
                    double overshootRGBDiff = 0;
                    double peakValue = 0;
                    if (StartingRGB < EndRGB)
                    {
                        peakValue = maxValue;
                        // Dark to light transition
                        if (maxValue > (endAverage + 100) && maxValue > (fullGammaTable[EndRGB][1] + 100))
                        {
                            // undershoot may have occurred
                            Console.WriteLine("Overshoot found");
                            // convert maxValue to RGB using gamma table
                            for (int i = 0; i < fullGammaTable.Count; i++)
                            {
                                // Find what RGB value matches or exceeds the peak light reading for this run
                                if (maxValue <= fullGammaTable[i][1])
                                {
                                    // Check if peak light reading is closer to upper or lower bound value
                                    int diff1 = fullGammaTable[i][1] - maxValue;
                                    int diff2 = maxValue - fullGammaTable[i - 1][1];
                                    if (diff1 < diff2)
                                    {
                                        overUnderRGB = fullGammaTable[i][0];
                                    }
                                    else
                                    {
                                        overUnderRGB = fullGammaTable[i - 1][0];
                                    }
                                    break;
                                }
                                else if (maxValue > fullGammaTable.Last()[1])
                                {
                                    if (maxValue > 65500)
                                    {
                                        overUnderRGB = -1;
                                        break;
                                    }
                                    else
                                    {
                                        overUnderRGB = 255;
                                        break;
                                    }
                                }
                            }
                            if (overUnderRGB == -1)
                            {
                                overshootPercent = 100;
                            }
                            else
                            {
                                double os = (overUnderRGB - EndRGB) / EndRGB;
                                os *= 100;
                                overshootPercent = Math.Round(os, 2);
                                overshootRGBDiff = overUnderRGB - EndRGB;
                            }
                        }
                    }
                    else
                    {
                        peakValue = minValue;
                        // Light to dark transistion
                        if (minValue < (endAverage - 100) && minValue < (fullGammaTable[EndRGB][1] - 100))
                        {
                            // overshoot may have occurred
                            // convert minValue to RGB using gamma table
                            Console.WriteLine("Undershoot found");
                            for (int i = 0; i < fullGammaTable.Count; i++)
                            {
                                // Find what RGB value matches or exceeds the peak light reading for this run
                                if (minValue <= fullGammaTable[i][1])
                                {
                                    if (i == 0)
                                    {
                                        overUnderRGB = 0;
                                        break;
                                    }
                                    else
                                    {
                                        // Check if peak light reading is closer to upper or lower bound value
                                        int diff1 = fullGammaTable[i][1] - minValue;
                                        int diff2 = minValue - fullGammaTable[i - 1][1];
                                        if (diff1 < diff2)
                                        {
                                            overUnderRGB = fullGammaTable[i][0];
                                        }
                                        else
                                        {
                                            overUnderRGB = fullGammaTable[i - 1][0];
                                        }
                                        break;
                                    }
                                }
                            }
                            double os = (overUnderRGB - EndRGB) / EndRGB;
                            os *= -1;
                            os *= 100;
                            overshootPercent = Math.Round(os, 2);
                            overshootRGBDiff = EndRGB - overUnderRGB;
                        }
                    }

                    if (Properties.Settings.Default.RGB10Offset)
                    {
                        if (StartingRGB < EndRGB)
                        {
                            double start3 = fullGammaTable[Convert.ToInt32(StartingRGB + 10)][1];
                            double end3 = fullGammaTable[Convert.ToInt32(EndRGB - 10)][1];

                            for (int j = 0; j < samples.Length; j++) // search samples for start & end trigger points
                            {
                                if (samples[j] >= start3 && initialTransStart == 0) // save the FIRST time value exceeds start trigger
                                {
                                    initialTransStart = j;
                                }
                                else if (samples[j] >= end3) // Save when value exceeds end trigger then break.
                                {
                                    initialTransEnd = j;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            double start3 = fullGammaTable[Convert.ToInt32(StartingRGB - 10)][1];
                            double end3 = fullGammaTable[Convert.ToInt32(EndRGB + 10)][1];

                            for (int j = 0; j < samples.Length; j++)
                            {
                                if (samples[j] <= start3 && initialTransStart == 0)
                                {
                                    initialTransStart = j;
                                }
                                else if (samples[j] <= end3)
                                {
                                    initialTransEnd = j;
                                    break;
                                }
                            }
                        }
                    }
                    else if (Properties.Settings.Default.RGB5Offset)
                    {
                        if (StartingRGB < EndRGB)
                        {
                            double start3 = fullGammaTable[Convert.ToInt32(StartingRGB + 5)][1];
                            double end3 = fullGammaTable[Convert.ToInt32(EndRGB - 5)][1];

                            for (int j = 0; j < samples.Length; j++) // search samples for start & end trigger points
                            {
                                if (samples[j] >= start3 && initialTransStart == 0) // save the FIRST time value exceeds start trigger
                                {
                                    initialTransStart = j;
                                }
                                else if (samples[j] >= end3) // Save when value exceeds end trigger then break.
                                {
                                    initialTransEnd = j;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            double start3 = fullGammaTable[Convert.ToInt32(StartingRGB - 5)][1];
                            double end3 = fullGammaTable[Convert.ToInt32(EndRGB + 5)][1];

                            for (int j = 0; j < samples.Length; j++)
                            {
                                if (samples[j] <= start3 && initialTransStart == 0)
                                {
                                    initialTransStart = j;
                                }
                                else if (samples[j] <= end3)
                                {
                                    initialTransEnd = j;
                                    break;
                                }
                            }
                        }
                    }
                    // 10% / 90% Measurements 
                    else if (Properties.Settings.Default.tenPercentSetting)
                    {
                        if (StartingRGB < EndRGB)
                        {
                            double range3 = (endAverage - startAverage) * 0.1; // Subtract low value from high value to get light level range
                            double start3 = startAverage + range3; // Start trigger value
                            double end3 = endAverage - range3; // End trigger value
                            if (Properties.Settings.Default.gammaCorrRT) // RGB corrected overwrites light level trigger points
                            {
                                double rgbRange = (EndRGB - StartingRGB) * 0.1;
                                rgbRange = Math.Round(rgbRange, 0);
                                start3 = fullGammaTable[Convert.ToInt32(StartingRGB + rgbRange)][1];
                                end3 = fullGammaTable[Convert.ToInt32(EndRGB - rgbRange)][1];
                            }
                            for (int j = 0; j < samples.Length; j++) // search samples for start & end trigger points
                            {
                                if (samples[j] >= start3 && initialTransStart == 0) // save the FIRST time value exceeds start trigger
                                {
                                    initialTransStart = j;
                                }
                                else if (samples[j] >= end3) // Save when value exceeds end trigger then break.
                                {
                                    initialTransEnd = j;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            double range3 = (startAverage - endAverage) * 0.1;
                            double start3 = startAverage - range3;
                            double end3 = endAverage + range3;
                            if (Properties.Settings.Default.gammaCorrRT)
                            {
                                double rgbRange = (StartingRGB - EndRGB) * 0.1;
                                rgbRange = Math.Round(rgbRange, 0);
                                start3 = fullGammaTable[Convert.ToInt32(StartingRGB - rgbRange)][1];
                                end3 = fullGammaTable[Convert.ToInt32(EndRGB + rgbRange)][1];
                            }
                            for (int j = 0; j < samples.Length; j++)
                            {
                                if (samples[j] <= start3 && initialTransStart == 0)
                                {
                                    initialTransStart = j;
                                }
                                else if (samples[j] <= end3)
                                {
                                    initialTransEnd = j;
                                    break;
                                }
                            }
                        }
                    }
                    // 3% / 97% measurements
                    else
                    {
                        if (StartingRGB < EndRGB)
                        {
                            double range3 = (endAverage - startAverage) * 0.03;
                            double start3 = startAverage + range3;
                            double end3 = endAverage - range3;
                            for (int j = 0; j < samples.Length; j++)
                            {
                                if (samples[j] >= start3 && initialTransStart == 0)
                                {
                                    initialTransStart = j;
                                }
                                else if (samples[j] >= end3)
                                {
                                    initialTransEnd = j;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            double range3 = (startAverage - endAverage) * 0.03;
                            double start3 = startAverage - range3;
                            double end3 = endAverage + range3;
                            for (int j = 0; j < samples.Length; j++)
                            {
                                if (samples[j] <= start3 && initialTransStart == 0)
                                {
                                    initialTransStart = j;
                                }
                                else if (samples[j] <= end3)
                                {
                                    initialTransEnd = j;
                                    break;
                                }
                            }
                        }
                    }

                    // Perceived Response Time
                    double RGBTolerance = 5;
                    if (Properties.Settings.Default.PerceivedRGB10)
                    {
                        RGBTolerance = 10;
                    }
                    // RGB 5/10 Offset measuring response time including overshoot/undershoot
                    if (StartingRGB < EndRGB)
                    {
                        double start3 = fullGammaTable[Convert.ToInt32(StartingRGB + RGBTolerance)][1];
                        double endOffsetRGB = EndRGB - RGBTolerance;
                        if (overUnderRGB > (EndRGB + RGBTolerance) && overUnderRGB != 0)
                        {
                            endOffsetRGB = EndRGB + RGBTolerance;
                        }
                        else if (overUnderRGB == -1)
                        {
                            endOffsetRGB = EndRGB;
                        }
                        double end3 = fullGammaTable[Convert.ToInt32(endOffsetRGB)][1];
                        if (overUnderRGB == -1)
                        {
                            end3 *= 1.02;
                            if (end3 > 65520)
                            {
                                end3 = 65500;
                            }
                        }
                        for (int j = 0; j < samples.Length; j++) // search samples for start point
                        {
                            if (samples[j] >= start3)
                            {
                                perceivedTransStart = j;
                                break;
                            }
                        }
                        for (int j = samples.Length - 1; j > 0; j--) // search samples for end point
                        {
                            if (endOffsetRGB > EndRGB || overUnderRGB == -1) // Including overshoot in the curve
                            {
                                if (samples[j] >= end3)
                                {
                                    perceivedTransEnd = j;
                                    break;
                                }
                            }
                            else // No overshoot found within RGB tolerance
                            {
                                if (samples[j] <= end3)
                                {
                                    perceivedTransEnd = j;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        double start3 = fullGammaTable[Convert.ToInt32(StartingRGB - RGBTolerance)][1];
                        double endOffsetRGB = EndRGB + RGBTolerance;
                        if (overUnderRGB < (EndRGB - RGBTolerance) && overUnderRGB != 0)
                        {
                            endOffsetRGB = EndRGB - RGBTolerance;
                        }
                        double end3 = fullGammaTable[Convert.ToInt32(endOffsetRGB)][1];
                        for (int j = 0; j < samples.Length; j++) // search samples for start point
                        {
                            if (samples[j] <= start3)
                            {
                                perceivedTransStart = j;
                                break;
                            }
                        }
                        for (int j = samples.Length - 1; j > 0; j--) // search samples for end point
                        {
                            if (endOffsetRGB < EndRGB) // Including undershoot in the curve
                            {
                                if (samples[j] <= end3)
                                {
                                    perceivedTransEnd = j;
                                    break;
                                }
                            }
                            else // No overshoot found within RGB tolerance
                            {
                                if (samples[j] >= end3)
                                {
                                    perceivedTransEnd = j;
                                    break;
                                }
                            }
                        }
                    }

                    double transCount = transEnd - transStart;
                    double transTime = (transCount * SampleTime) / 1000;

                    double initialTransCount = initialTransEnd - initialTransStart;
                    double initialTransTime = (initialTransCount * SampleTime) / 1000;

                    double perceivedTransCount = perceivedTransEnd - perceivedTransStart;
                    double perceivedTransTime = (perceivedTransCount * SampleTime) / 1000;

                    double responseTime = Math.Round(transTime, 1);
                    double initialResponseTime = Math.Round(initialTransTime, 1);
                    double perceivedResponseTime = Math.Round(perceivedTransTime, 1);
                    if (verboseOutputToolStripMenuItem.Checked)
                    {
                        // Verbose output with ALLLL the data
                        double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, initialResponseTime, perceivedResponseTime, overshootPercent, transStart, transEnd, SampleTime, endAverage, peakValue, overUnderRGB };
                        processedData.Add(completeResult);
                    }
                    else if (!percentageToolStripMenuItem.Checked && gammaCorrectedToolStripMenuItem.Checked)
                    {
                        // Standard output with total transition time & gamma corrected overshoot value
                        if (overUnderRGB == -1)
                        {
                            overshootRGBDiff = 100;
                        }
                        double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, initialResponseTime, perceivedResponseTime, overshootRGBDiff };
                        processedData.Add(completeResult);
                    }
                    else if (!gammaCorrectedToolStripMenuItem.Checked && percentageToolStripMenuItem.Checked)
                    {
                        // Standard output with total transition time & overshoot light level percentage
                        double os = 0;
                        if (StartingRGB < EndRGB)
                        {
                            os = (maxValue - endAverage) / endAverage;
                            os *= 100;
                            os = Math.Round(os, 2);
                        }
                        else
                        {
                            os = (endAverage - minValue) / endAverage;
                            os *= 100;
                            os = Math.Round(os, 2);
                        }
                        double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, initialResponseTime, perceivedResponseTime, os };
                        processedData.Add(completeResult);
                    }
                    else
                    {
                        // Standard output with total transition time & gamma corrected overshoot percentage
                        double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, initialResponseTime, perceivedResponseTime, overshootPercent };
                        processedData.Add(completeResult);
                    }

                }
                List<double[]> temp = new List<double[]>(); //probably not needed now processedData is a local variable
                temp.AddRange(processedData);
                multipleRunData.Add(temp);

                // Write results to csv using new name
                decimal fileNumber = 001;
                // search /Results folder for existing file names, pick new name
                string[] existingFiles = Directory.GetFiles(resultsFolderPath, "*-FULL-OSRTT.csv");
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

                string filePath = resultsFolderPath + "\\" + fileNumber.ToString("000") + "-FULL-OSRTT.csv";

                string strSeparator = ",";
                StringBuilder csvString = new StringBuilder();
                string rtType = "Initial Response Time - 3% (ms)";
                string osType = "Overshoot";
                string osSign = "(%)";
                string perType = "Perceived Response Time - RGB5 (ms)";
                if (tenPercentMenuItem.Checked)
                {
                    rtType = "Initial Response Time - 10% (ms)";
                }
                else if (fixedRGB10OffsetToolStripMenuItem.Checked)
                {
                    rtType = "Initial Response Time - RGB10 (ms)";
                }
                else if (fixedRGB5OffsetToolStripMenuItem.Checked)
                {
                    rtType = "Initial Response Time - RGB5 (ms)";
                }
                if (perceivedRGB10MenuItem.Checked)
                {
                    perType = "Perceived Response Time - RGB10 (ms)";
                }
                if (gammaCorrectedToolStripMenuItem.Checked)
                {
                    osSign = "(RGB)";
                }
                if (gammaCorrectedToolStripMenuItem.Checked && percentageToolStripMenuItem.Checked)
                {
                    osSign = "(RGB %)";
                }
                if (verboseOutputToolStripMenuItem.Checked)
                {
                    csvString.AppendLine("Starting RGB,End RGB,Complete Response Time (ms)," + rtType + "," + perType + "," + osType + " " + osSign + ",Transition Start Position,Transition End Position,Sampling Time (ms),End Light Level,Min/Max Light Level,Overshoot/Undershoot RGB Value");
                }
                else
                {
                    csvString.AppendLine("Starting RGB, End RGB, Complete Response Time (ms), " + rtType + "," + perType + "," + osType + " " + osSign);
                }
                foreach (var res in processedData)
                {
                    csvString.AppendLine(string.Join(strSeparator, res));
                }
                Console.WriteLine(filePath);
                File.WriteAllText(filePath, csvString.ToString());

                if (saveGammaTableToolStripMenuItem.Checked)
                {
                    // Save Gamma curve to a file too
                    decimal gammaFileNumber = 001;
                    // search /Results folder for existing file names, pick new name
                    string[] existingGammaFiles = Directory.GetFiles(resultsFolderPath, "*-GAMMA-OSRTT.csv");
                    // Search \Results folder for existing results to not overwrite existing or have save conflict errors
                    foreach (var s in existingGammaFiles)
                    {
                        decimal num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                        if (num >= gammaFileNumber)
                        {
                            gammaFileNumber = num + 1;
                        }
                    }

                    string gammaFilePath = resultsFolderPath + "\\" + gammaFileNumber.ToString("000") + "-GAMMA-OSRTT.csv";
                    StringBuilder gammaCsvString = new StringBuilder();
                    gammaCsvString.AppendLine("RGB, Light Reading");
                    foreach (var res in fullGammaTable)
                    {
                        gammaCsvString.AppendLine(string.Join(strSeparator, res));
                    }
                    File.WriteAllText(gammaFilePath, gammaCsvString.ToString());
                }
                if (saveSmoothedDataToolStripMenuItem.Checked)
                {
                    //Save Smoothed Data To File
                    decimal smoothedFileNumber = 001;
                    // search /Results folder for existing file names, pick new name
                    string[] existingSmoothedFiles = Directory.GetFiles(resultsFolderPath, "*-CLEAN-OSRTT.csv");
                    // Search \Results folder for existing results to not overwrite existing or have save conflict errors
                    foreach (var s in existingSmoothedFiles)
                    {
                        decimal num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                        if (num >= smoothedFileNumber)
                        {
                            smoothedFileNumber = num + 1;
                        }
                    }

                    string smoothedFilePath = resultsFolderPath + "\\" + smoothedFileNumber.ToString("000") + "-CLEAN-OSRTT.csv";
                    StringBuilder smoothedCsvString = new StringBuilder();
                    foreach (var res in smoothedDataTable)
                    {
                        smoothedCsvString.AppendLine(string.Join(strSeparator, res));
                    }
                    File.WriteAllText(smoothedFilePath, smoothedCsvString.ToString());
                }
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
                        showMessageBox("One or more set of results failed to process and won't be included in the multi-run averaging. Brightness may be too high - try calibrating the brightness and running the test again.", "Processing Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        processingFailed = false;
                    }
                }
            }
        }

        private void setRepeats()
        {
            decimal runs = getRunCount() - 1;
            port.Write("M" + runs.ToString());
        }

        private void processMultipleRuns()
        {
            if (multipleRunData.Count == 0)
            {
                MessageBox.Show("No results are imported. Use the Import Raw Results Folder button to import an existing set of results.", "No Results Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    // Fill processed data with the first set of results then average from there                
                    int resultCount = multipleRunData[0].Count();

                    List<double[]> averageData = new List<double[]>();
                    for (int p = 0; p < resultCount; p++)
                    {
                        double[] row = { multipleRunData[0][p][0], multipleRunData[0][p][1], 0, 0, 0, 0 };
                        averageData.Add(row);
                    }

                    // Average the data, excluding outliers
                    for (int k = 0 ; k < resultCount; k++)
                    {
                        List<double> rTLine = new List<double>();
                        List<double> initRTLine = new List<double>();
                        List<double> perRTLine = new List<double>();
                        List<double> oSLine = new List<double>();
                        foreach (var list in multipleRunData)
                        {
                            rTLine.Add(list[k][2]);
                            initRTLine.Add(list[k][3]);
                            perRTLine.Add(list[k][4]);
                            oSLine.Add(list[k][5]);
                        }
                        double rtMedian = GetMedian(rTLine.ToArray());
                        double initRtMedian = GetMedian(initRTLine.ToArray());
                        double perRtMedian = GetMedian(perRTLine.ToArray());
                        double osMedian = GetMedian(oSLine.ToArray());
                        int validTimeResults = 0;
                        int validInitialTimeResults = 0;
                        int validPerceivedTimeResults = 0;
                        int validOvershootResults = 0;
                        foreach (var o in multipleRunData)
                        {
                            if (o[k][2] < (rtMedian * 1.2) && o[k][2] > (rtMedian * 0.8))
                            {
                                averageData[k][2] += o[k][2];
                                validTimeResults++;
                            }
                            if (o[k][3] < (initRtMedian * 1.2) && o[k][3] > (initRtMedian * 0.8))
                            {
                                averageData[k][3] += o[k][3];
                                validInitialTimeResults++;
                            }
                            if (o[k][4] < (perRtMedian * 1.2) && o[k][4] > (perRtMedian * 0.8))
                            {
                                averageData[k][4] += o[k][4];
                                validPerceivedTimeResults++;
                            }
                            if (o[k][5] < (osMedian * 1.2) && o[k][5] > (osMedian * 0.8))
                            {
                                averageData[k][5] += o[k][5];
                                validOvershootResults++;
                            }
                        }
                        averageData[k][2] = averageData[k][2] / validTimeResults;
                        averageData[k][2] = Math.Round(averageData[k][2], 1);
                        averageData[k][3] = averageData[k][3] / validInitialTimeResults;
                        averageData[k][3] = Math.Round(averageData[k][3], 1);
                        averageData[k][4] = averageData[k][4] / validPerceivedTimeResults;
                        averageData[k][4] = Math.Round(averageData[k][4], 1);
                        if (averageData[k][5] != 0)
                        {
                            averageData[k][5] = averageData[k][5] / validOvershootResults;
                            if (gammaCorrectedToolStripMenuItem.Checked && !percentageToolStripMenuItem.Checked)
                            {
                                averageData[k][5] = Math.Round(averageData[k][5], 0);
                            }
                            else
                            {
                                averageData[k][5] = Math.Round(averageData[k][5], 1);
                            }
                        }
                    }

                    // Output averaged results to file using folder name/monitor info
                    string[] folders = resultsFolderPath.Split('\\');
                    string monitorInfo = folders.Last();
                    //monitorInfo = monitorInfo.Remove(0, 4);
                    string filePath = resultsFolderPath + "\\" + monitorInfo + "-FINAL-DATA-OSRTT.csv";
                    string strSeparator = ",";
                    StringBuilder csvString = new StringBuilder();
                    string rtType = "Initial Response Time - 3% (ms)";
                    string osType = "Overshoot";
                    string osSign = "(%)";
                    string perType = "Perceived Response Time - RGB5 (ms)";
                    if (tenPercentMenuItem.Checked)
                    {
                        rtType = "Initial Response Time - 10% (ms)";
                    }
                    else if (fixedRGB10OffsetToolStripMenuItem.Checked)
                    {
                        rtType = "Initial Response Time - RGB10 (ms)";
                    }
                    else if (fixedRGB5OffsetToolStripMenuItem.Checked)
                    {
                        rtType = "Initial Response Time - RGB5 (ms)";
                    }
                    if (perceivedRGB10MenuItem.Checked)
                    {
                        perType = "Perceived Response Time - RGB10 (ms)";
                    }
                    if (gammaCorrectedToolStripMenuItem.Checked)
                    {
                        osSign = "(RGB)";
                    }
                    if (gammaCorrectedToolStripMenuItem.Checked && percentageToolStripMenuItem.Checked)
                    {
                        osSign = "(RGB %)";
                    }
                    csvString.AppendLine("Starting RGB,End RGB,Complete Response Time (ms), " + rtType + "," + perType + "," + osType + " " + osSign);
                    foreach (var res in averageData)
                    {
                        csvString.AppendLine(string.Join(strSeparator, res));
                    }
                    try
                    {
                        File.WriteAllText(filePath, csvString.ToString());
                    }
                    catch (IOException e)
                    {
                        DialogResult d = MessageBox.Show("Unable to write final results file as the file is open in another program. Please close it then hit retry.", "Unable to write file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                        if (d == DialogResult.Retry)
                        {
                            try
                            {
                                File.WriteAllText(filePath, csvString.ToString());
                            }
                            catch
                            {
                                MessageBox.Show("Still can't write to the file. Please importing the folder or running the test again", "Write Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    showMessageBox(ex.Message + " " + ex.StackTrace, "Error Processing Multiple Runs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

        private void makeResultsFolder()
        {
            int monitor = getSelectedMonitor();
            string monitorName = displayList[monitor].Name;
            string monitorInfo = monitorName.Replace(" ", "-") + "-" + displayList[monitor].Freq.ToString() + "-" + displayList[monitor].Connection;

            decimal fileNumber = 001;
            // search /Results folder for existing file names, pick new name
            string[] existingFiles = Directory.GetDirectories(path, "*-" + monitorInfo);
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
            Directory.CreateDirectory(filePath);
            resultsFolderPath = filePath;
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
                    resultsFolderPath = folderDiag.SelectedPath;

                    if (filePath != path)
                    {
                        multipleRunData.Clear();
                        gamma.Clear();
                        //results.Clear();
                        string[] files = Directory.GetFiles(filePath);
                        bool valid = false;
                        foreach (var f in files)
                        {
                            if (f.Contains("-GAMMA-RAW-OSRTT"))
                            {
                                valid = true;
                                try
                                {
                                    using (OpenFileDialog OFD = new OpenFileDialog())
                                    {
                                        OFD.FileName = f;
                                        //Read the contents of the file into a stream

                                        var fileStream = OFD.OpenFile();
                                        using (StreamReader reader = new StreamReader(fileStream))
                                        {
                                            gamma.Clear();
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
                                                gamma.Add(intLine);
                                            }
                                        }
                                    }
                                    processGammaTable();
                                }
                                catch (IOException iex)
                                {
                                    MessageBox.Show("Unable to open file - it may be in use in another program. Please close it out and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else if (f.Contains("-RAW-OSRTT"))
                            {
                                valid = true;
                                try
                                {
                                    using (OpenFileDialog OFD = new OpenFileDialog())
                                    {
                                        OFD.FileName = f;
                                        //Read the contents of the file into a stream

                                        var fileStream = OFD.OpenFile();
                                        using (StreamReader reader = new StreamReader(fileStream))
                                        {
                                            results.Clear();
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
                                                results.Add(intLine);
                                            }
                                        }
                                    }
                                    processResponseTimeData();
                                }
                                catch (IOException iex)
                                {
                                    MessageBox.Show("Unable to open file - it may be in use in another program. Please close it out and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        if (valid)
                        {
                            processMultipleRuns();
                            Process.Start("explorer.exe", resultsFolderPath);
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

        private void Main_Load(object sender, EventArgs e)
        {
            
        }
        
        private void setFPSLimit()
        {
            var item = fpsList.Find(x => x.FPSValue == getSelectedFps()); 
            //var item = fpsList.Find(x => x.FPSValue == Properties.Settings.Default.FPS.ToString()); //Doesn't work, cba to work out why atm.
            port.Write("L" + item.Key);
        }

        private void BrightnessCalBtn_Click(object sender, EventArgs e)
        {
            if (port != null)
            {
                //Thread brightCalThread = new Thread(new ThreadStart(this.launchBrightnessCal));
                //brightCalThread.Start();
                closeBrightnessBtn.Text = "Stop Calibration";

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
                port.Write("B");
                brightnessWindowOpen = true;
                brightnessCanceled = false;
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
            potVal = 0;
            incPotValBtn.Enabled = true;
            try
            {
                port.Write(potVal.ToString("X"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void changeSizeAndState(string state)
        {
            switch (state)
            {
                case "standard":
                    Size = new Size(628, 275);
                    break;
                case "analyse":
                    Size = new Size(628, 424);
                    break;
                case "brightness":
                    analysePanel.Location = new Point(1500, 238);
                    controlsPanel.Location = new Point(1500, 36);
                    brightnessPanel.Location = new Point(0, 0);
                    aboutPanel.Location = new Point(1500, 386);
                    Size = new Size(1000, 800);
                    debugPanel.Location = new Point(1500, 36);
                    menuStrip1.Visible = false;
                    break;
                case "close brightness":
                    analysePanel.Location = new Point(12, 238);
                    controlsPanel.Location = new Point(12, 36);
                    brightnessPanel.Location = new Point(1100, 36);
                    aboutPanel.Location = new Point(12, 386);
                    Size = new Size(628, 275);
                    debugPanel.Location = new Point(619, 36);
                    break;
                case "about":
                    Size = new Size(628,542);
                    break;
                case "debug":
                    Size = new Size(1120, 850);
                    debugPanel.Location = new Point(619, 36);
                    break;
                default:
                    Size = new Size(628, 275);
                    break;
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

        private void verboseOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Verbose = verboseOutputToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void threePercentMenuItem_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.threePercentSetting)
            {
                Properties.Settings.Default.threePercentSetting = threePercentMenuItem.Checked;
                Properties.Settings.Default.tenPercentSetting = false;
                tenPercentMenuItem.Checked = false;
                Properties.Settings.Default.RGB10Offset = false;
                fixedRGB10OffsetToolStripMenuItem.Checked = false;
                Properties.Settings.Default.RGB5Offset = false;
                fixedRGB5OffsetToolStripMenuItem.Checked = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                threePercentMenuItem.Checked = true;
            }
        }

        private void tenPercentMenuItem_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.tenPercentSetting)
            {
                Properties.Settings.Default.tenPercentSetting = tenPercentMenuItem.Checked;
                Properties.Settings.Default.threePercentSetting = false;
                threePercentMenuItem.Checked = false;
                Properties.Settings.Default.RGB10Offset = false;
                fixedRGB10OffsetToolStripMenuItem.Checked = false;
                Properties.Settings.Default.RGB5Offset = false;
                fixedRGB5OffsetToolStripMenuItem.Checked = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                tenPercentMenuItem.Checked = true;
            }
        }

        private void fixedRGB10OffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.RGB10Offset)
            {
                Properties.Settings.Default.RGB10Offset = fixedRGB10OffsetToolStripMenuItem.Checked;
                Properties.Settings.Default.threePercentSetting = false;
                threePercentMenuItem.Checked = false;
                Properties.Settings.Default.tenPercentSetting = false;
                tenPercentMenuItem.Checked = false;
                Properties.Settings.Default.RGB5Offset = false;
                fixedRGB5OffsetToolStripMenuItem.Checked = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                fixedRGB10OffsetToolStripMenuItem.Checked = true;
            }
        }

        private void fixedRGB5OffsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.RGB5Offset)
            {
                Properties.Settings.Default.RGB5Offset = fixedRGB5OffsetToolStripMenuItem.Checked;
                Properties.Settings.Default.threePercentSetting = false;
                threePercentMenuItem.Checked = false;
                Properties.Settings.Default.tenPercentSetting = false;
                tenPercentMenuItem.Checked = false;
                Properties.Settings.Default.RGB10Offset = false;
                fixedRGB10OffsetToolStripMenuItem.Checked = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                fixedRGB10OffsetToolStripMenuItem.Checked = true;
            }
        }

        private void gammaCorrectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.gammaPercentSetting && Properties.Settings.Default.gammaCorrectedSetting)
            {
                Properties.Settings.Default.gammaCorrectedSetting = gammaCorrectedToolStripMenuItem.Checked;
                Properties.Settings.Default.Save();
            }
            else if (Properties.Settings.Default.gammaPercentSetting)
            {
                Properties.Settings.Default.gammaCorrectedSetting = gammaCorrectedToolStripMenuItem.Checked;
                Properties.Settings.Default.Save();
            }
            else if (Properties.Settings.Default.gammaCorrectedSetting)
            {
                gammaCorrectedToolStripMenuItem.Checked = true;
            }
            else
            {
                Properties.Settings.Default.gammaCorrectedSetting = true;
                gammaCorrectedToolStripMenuItem.Checked = true;
                Properties.Settings.Default.Save();
            }
        }

        private void percentageToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (Properties.Settings.Default.gammaPercentSetting && Properties.Settings.Default.gammaCorrectedSetting)
            {
                Properties.Settings.Default.gammaPercentSetting = percentageToolStripMenuItem.Checked;
                Properties.Settings.Default.Save();
            }
            else if (Properties.Settings.Default.gammaCorrectedSetting)
            {
                Properties.Settings.Default.gammaPercentSetting = percentageToolStripMenuItem.Checked;
                Properties.Settings.Default.Save();
            }
            else if (Properties.Settings.Default.gammaPercentSetting)
            {
                percentageToolStripMenuItem.Checked = true;
            }
            else
            {
                Properties.Settings.Default.gammaPercentSetting = true;
                percentageToolStripMenuItem.Checked = true;
                Properties.Settings.Default.Save();
            }
        }

        private void gamCorMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.gammaCorrRT = gamCorMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void testButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                makeResultsFolder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
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

        private void saveGammaTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.saveGammaTable = saveGammaTableToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void saveSmoothedDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.saveSmoothData = saveSmoothedDataToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void saveUSBOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.USBOutput = saveUSBOutputToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
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

        private void suppressDialogBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SuppressDiagBox = suppressDialogBoxesToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void showMessageBox(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (!Properties.Settings.Default.SuppressDiagBox)
            {
                MessageBox.Show(title, message, buttons, icon);
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
                MessageBox.Show(ex.Message);
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

        private void perceivedRGB5MenuItem_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.PerceivedRGB5)
            {
                Properties.Settings.Default.PerceivedRGB5 = perceivedRGB5MenuItem.Checked;
                Properties.Settings.Default.PerceivedRGB10 = false;
                perceivedRGB10MenuItem.Checked = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                perceivedRGB5MenuItem.Checked = true;
            }
        }

        private void perceivedRGB10MenuItem_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.PerceivedRGB10)
            {
                Properties.Settings.Default.PerceivedRGB10 = perceivedRGB10MenuItem.Checked;
                Properties.Settings.Default.PerceivedRGB5 = false;
                perceivedRGB5MenuItem.Checked = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                perceivedRGB10MenuItem.Checked = true;
            }
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
    }
}
