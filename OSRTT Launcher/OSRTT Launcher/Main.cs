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

namespace OSRTT_Launcher
{
    public partial class Main : Form
    {
        // CHANGE THESE VALUES WHEN ISSUING A NEW RELEASE
        private double boardVersion = 1.0;
        private double downloadedFirmwareVersion = 1.0;
        private string softwareVersion = "1.0";

        // TODO //
        // handle failed results inputs just in case 
        // get current focused window, if ue4 game isn't selected port.write("C"); until it is then port.write("T"); ----------------- TEST THIS
        // messagebox don't always play sound, may not draw over ue4 game? - MAKE THEM PLAY SOUNDS!
        // program can crash - change process data functions to separate files & call them in try/catch with message boxes
        // test wouldn't start(button activate?) after initial launch - perhaps brightness was too high but no messagebox to show it?
        //
        //
        // Brightness calibration window that loops brightness test when entered, fixed pot value of 170(?), readout to one side saying "too bright", "too dark"
        // Lanuch button will have messagebox show on first press saying do you want to calibrate monitor brightness
        // 
        // TESTING
        // Test overshoot properly
        // Test brightness setup (calibration window? extra step in arduino code?)
        // 
        //
        // Current known issues //
        // Device will continue to run test even if game closed/not selected/program error
        // Device will still activate button even if program/game closed - check if serial connected on the board, if not connected break loop
        //
        // LOW PRIORITY
        // "Set framerate cap" option (just presses the correspoinding key based on which dropdown option is selected and if checkbox is ticked, by default no?)
        // - Possibly set RGB values in C# 

        public static System.IO.Ports.SerialPort port;
        delegate void SetTextCallback(string text);
        private bool boardUpdate = false;
        private bool portConnected = false;
        private bool brightnessCheck = false;
        public bool brightnessWindowOpen = false;

        private int[] RGBArr;
        private int resultsLimit = 110;

        private int potVal = 0;

        string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        string resultsFolderPath = "";
        private List<int[]> results = new List<int[]>();
        
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
            AutoUpdater.Start("https://api.github.com/repos/andymanic/OSRTT/releases/latest");
        }

        private void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            dynamic json = JsonConvert.DeserializeObject(args.RemoteData);
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                CurrentVersion = json.tag_name,
                ChangelogURL = json.html_url,
                DownloadURL = json.assets[0].browser_download_url,
            };
        }

        public Main()
        {
            InitializeComponent();
            this.Icon = (Icon)rm.GetObject("osrttIcon");
            ControlDeviceButtons(false);
            path = new Uri(System.IO.Path.GetDirectoryName(path)).LocalPath;
            path += @"\Results";
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            this.FormClosed += new FormClosedEventHandler(Main_FormClosed);
            hardWorker = new BackgroundWorker();
            connectThread = new Thread(new ThreadStart(this.findAndConnectToBoard));
            connectThread.Start();
            Size = new Size(628, 275);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            listMonitors();
            listFramerates();
            initialSetup();
            testCount.Value = Properties.Settings.Default.Runs;
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
                    process.StartInfo.Arguments = "/C .\\arduinoCLI\\arduino-cli.exe config init && .\\arduinoCLI\\arduino-cli.exe config add board_manager.additional_urls https://adafruit.github.io/arduino-board-index/package_adafruit_index.json && .\\arduinoCLI\\arduino-cli.exe core update-index && .\\arduinoCLI\\arduino-cli.exe core install arduino:samd && .\\arduinoCLI\\arduino-cli.exe core install adafruit:samd";
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
        private void listMonitors()
        {
            monitorCB.Items.Clear(); // Clear existing array and list before filling them
            displayList.Clear();
            // Find monitor names, refresh rate and connection
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
            fpsList.Add(new FPS { FPSValue = "165", Key = "4" });
            fpsList.Add(new FPS { FPSValue = "144", Key = "5" });
            fpsList.Add(new FPS { FPSValue = "120", Key = "6" });
            fpsList.Add(new FPS { FPSValue = "100", Key = "7" });
            fpsList.Add(new FPS { FPSValue = "60", Key = "8" });
            foreach (var f in fpsList)
            {
                fpsLimitList.Items.Add(f.FPSValue);
            }
            fpsLimitList.SelectedIndex = Properties.Settings.Default.FPS;
        }

        private void findAndConnectToBoard()
        {
            while (true)
            {
                if (!portConnected)
                {
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
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            process.StartInfo.FileName = "cmd.exe";
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.RedirectStandardOutput = true;
                            process.StartInfo.CreateNoWindow = true;
                            Console.WriteLine("ready to start");
                            process.StartInfo.Arguments = "/C .\\arduinoCLI\\arduino-cli.exe compile --fqbn adafruit:samd:adafruit_itsybitsy_m4 .\\arduinoCLI\\OSRTT_Full_Code && .\\arduinoCLI\\arduino-cli.exe upload --port " + p + " --fqbn adafruit:samd:adafruit_itsybitsy_m4 .\\arduinoCLI\\OSRTT_Full_Code";
                            try
                            {
                                Console.WriteLine("starting");
                                process.Start();
                                string output = process.StandardOutput.ReadToEnd();
                                Console.WriteLine(output);
                                process.WaitForExit();
                                if (output.Contains("Error"))
                                {
                                    MessageBox.Show("Firmware update failed. Error message: " + output, "Update Device Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                                else
                                {
                                    MessageBox.Show("Device has been updated successfully!", "Updated Device", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                
                                boardUpdate = false;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Unable to write to device, check it's connected via USB.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Console.WriteLine(ex);
                            }
                        }
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
            port.WriteTimeout = 500;
            port.ReadBufferSize = 64000;
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
            if (boardVersion < downloadedFirmwareVersion)
            {
                DialogResult dialogResult = MessageBox.Show("A newer version of the board's firmware is available, do you want to update now?", "Board Firmware Update Available!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            else
            {
                this.testCount.Value = runs;
            }
        }

        private void setSelectedFps(string limit)
        {
            if (this.fpsLimitList.InvokeRequired)
            {
                this.fpsLimitList.Invoke((MethodInvoker)(() => fpsLimitList.SelectedItem = limit));
            }
            else
            {
                this.fpsLimitList.SelectedItem = limit;
            }
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
            }
            else
            {
                this.devStat.Text = text;
            }
        }

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
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
            else
            {
                return monitorCB.SelectedIndex;
            }
        }

        private string getSelectedFps()
        {
            if (fpsLimitList.InvokeRequired)
            {
                return (string)fpsLimitList.Invoke(
                  new Func<string>(() => fpsLimitList.SelectedItem.ToString())
                );
            }
            else
            {
                return fpsLimitList.SelectedItem.ToString();
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
            else
            {
                return Decimal.ToInt32(testCount.Value);
            }
        }

        public void Read()
        {
            while (port.IsOpen)
            {
                try
                {
                    string message = port.ReadLine();
                    Console.WriteLine(message);
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
                            else
                            {
                                continue;
                            }
                        }
                        results.Add(intValues);
                    }
                    else if (message.Contains("STARTING RUN"))
                    {
                        results.Clear();
                    }
                    else if (message.Contains("STARTING TEST"))
                    {
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

                        // Process that raw data
                        processThread = new Thread(new ThreadStart(this.processResponseTimeData));
                        processThread.Start();
                    }
                    else if (message.Contains("Test Complete"))
                    {
                        Thread.Sleep(500); //Had an issue with data processing not being finished by the time the command comes it to start averaging the data.
                        processMultipleRuns();
                        port.Write("T");
                        DialogResult d = MessageBox.Show("Test complete, open results folder?","Test Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (d == DialogResult.Yes)
                        {
                            //open folder
                            Process.Start("explorer.exe", resultsFolderPath);
                        }
                    }
                    else if (message.Contains("FW:"))
                    {
                        string[] sp = message.Split(':');
                        boardVersion = double.Parse(sp[1]);
                        compareFirmware();
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
                        string[] sp = message.Split(':');
                        string k = sp[1];
                        k = k.Replace("\r", string.Empty);
                        var lim = fpsList.Find(x => x.Key == k);
                        string selectedFps = getSelectedFps();
                        if (lim.FPSValue != selectedFps)
                        {
                            setSelectedFps(lim.FPSValue);
                        }
                    }
                    else if (message.Contains("Brightness:"))
                    {
                        if (brightnessWindowOpen)
                        {
                            string[] sp = message.Split(':');
                            updateBrightness(Int32.Parse(sp[1]));
                        }
                    }
                    else if (message.Contains("TEST CANCELLED"))
                    {
                        if (message.Contains("LIGHT LEVEL"))
                        {
                            MessageBox.Show("ERROR - TEST CANCELLED. Monitor's brightness may not be in the acceptable range.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (message.Contains("USB VOLTAGE"))
                        {
                            MessageBox.Show("ERROR - TEST CANCELLED. USB supply voltage may be too low - please plug the device either directly into your system or a powered USB hub.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        this.SetText(message);
                    }
                }
                catch (TimeoutException ex)
                {
                    Console.WriteLine(ex);
                }
                catch (Exception e)
                {
                    try
                    {
                        port.Write("C");
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc);
                    }
                    Console.WriteLine(e);
                    Console.WriteLine("Trying to reconnect");
                    port.Close();
                    portConnected = false;
                    ControlDeviceButtons(false);
                    SetDeviceStatus("Board Disconnected");
                    readThread.Abort();
                }
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            // When form is closed halt read thread & close Serial Port
            ControlDeviceButtons(false);
            if (checkWindowThread != null)
            {
                checkWindowThread.Abort();
            }
            if (readThread != null)
            {
                readThread.Abort();
            }
            if (connectThread != null)
            {
                connectThread.Abort();
            }
            if (port != null)
            {
                try
                {
                    port.Write("C");
                }
                catch {}
                port.Close();
            }

        }

        private void launchBtn_Click(object sender, EventArgs e)
        {
            if (!brightnessCheck)
            {
                //DialogResult d = MessageBox.Show("It looks like you haven't calibrated your monitor brightness since launch, do you want to do that first? " +
                //"\n\n" + "Not calibrating the brightness level may lead to errors, failed tests or invalid data.", "Calibrate Monitor Brightness?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (d == DialogResult.Yes)
                //{
                    launchBrightnessCal();
                //}
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
                Thread.Sleep(500);
            }
            // Save current & FPS to hardware on run
            setFPSLimit();
            setRepeats();


            // Launch UE4 game
            // thinking about it you can probably just bundle this into one process instead of launching, then finding it again...
            string ue4Path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            ue4Path = new Uri(System.IO.Path.GetDirectoryName(ue4Path)).LocalPath;
            ue4Path += @"\OSRTT UE4\ResponseTimeTest.exe";

            try
            {
                Process.Start(ue4Path);
                port.Write("T");
                Process[] p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                ControlDeviceButtons(false);
                while (p.Length == 0)
                {
                    // Added in case game hasn't finished launching yet
                    p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                }
                //checkWindowThread = new Thread(new ThreadStart(this.checkFocusedWindow));
                //checkWindowThread.Start();
                // Wait for game to close then send cancel command to board
                p[0].WaitForExit();
                //checkWindowThread.Abort();
                Console.WriteLine("Game closed");
                port.Write("C");
                ControlDeviceButtons(true);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UE4 Project Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkFocusedWindow()
        {
            FocusedWindow fw = new FocusedWindow();
            string pName = fw.GetForegroundProcessName();
            while (true)
            { 
                pName = fw.GetForegroundProcessName();
                Thread.Sleep(1000);
                if (pName != "ResponseTimeTest-Win64-Shipping")
                {
                    // cancel test? idk
                    port.Write("C");
                    Console.WriteLine("Process not selected");
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
                        processThread = new Thread(new ThreadStart(this.processResponseTimeData));
                        processThread.Start();
                    }
                    catch
                    {
                        DialogResult d = MessageBox.Show("File may be in use by another program, please make sure it's not open elsewhere and try again.", "Unable to open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Size = new Size(628, 424);
            }
            else
            {
                Size = new Size(628, 275);
            }

        }

        private void debugModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // REMOVE MEEEEEEEEEEEEEEEEEEEE
            Size = new Size(1120, 850);
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

        private void processResponseTimeData()
        {
            //This is a long one. This is the code that builds the gamma curve, finds the start/end points and calculates response times and overshoot % (gamma corrected)
            List<double[]> processedData = new List<double[]>();
            processedData.Clear();

            // First, create gamma array from the data
            List<int[]> gamma = new List<int[]>();
            List<int[]> fullGammaTable = new List<int[]>();
            for (int i = 0; i < 20; i += 2)
            {
                int[] resLine = this.results[i];
                int avg = 0;
                if (resLine[0] == 0 && gamma.Count == 0)
                {
                    for (int j = 5; j < 250; j++)
                    {
                        avg += resLine[j];
                    }
                    avg = avg / 245;
                    gamma.Add(new int[] { resLine[0], avg });
                    for (int j = resLine.Length - 455; j < resLine.Length - 5; j++)
                    {
                        avg += resLine[j];
                    }
                    avg = avg / 450;
                    gamma.Add(new int[] { resLine[1], avg });
                }
                else
                {
                    for (int j = resLine.Length - 455; j < resLine.Length - 5; j++)
                    {
                        avg += resLine[j];
                    }
                    avg = avg / 450;
                    gamma.Add(new int[] { resLine[1], avg });
                }
            }
            // Extrapolate rough values for every RGB value
            for (int i = 0; i < gamma.Count - 1; i++)
            {
                PointF[] points = new PointF[]
                    {
                        new PointF { X = gamma[i][0], Y = gamma[i][1]},
                        new PointF { X = gamma[i+1][0],  Y = gamma[i+1][1]}
                    };
                int numberOfPoints = gamma[i + 1][0] - gamma[i][0];

                PointF[] partGamma = InterpolatePoints(points, numberOfPoints);
                foreach (var p in partGamma)
                {
                    int[] tempGamma = {
                        Convert.ToInt32(p.X), Convert.ToInt32(p.Y)
                    };
                    fullGammaTable.Add(tempGamma);
                }
            }
            fullGammaTable.Add(gamma[10]);

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
                for (int a = 0; a<samples.Length; a++)
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

                int maxValue = samples.Max(); // Find the maximum value for overshoot
                int minValue = samples.Min(); // Find the minimum value for undershoot
                // Initialise in-use variables
                int transStart = 0;
                int transEnd = 0;

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

                //Overshoot calculations
                double overshootPercent = 0;
                if (StartingRGB < EndRGB)
                {
                    // Dark to light transition
                    if (maxValue > (endMax + 100) && maxValue > (fullGammaTable[EndRGB][1] + 100))
                    {
                        // undershoot may have occurred
                        // convert maxValue to RGB using gamma table
                        Console.WriteLine("Overshoot found");
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
                            overshootPercent = Math.Round((os * 100), 2);
                        }
                    }
                }
                else
                {
                    // Light to dark transistion
                    if (minValue < (endMin - 100) && minValue < (fullGammaTable[EndRGB][1] - 100))
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
                        os = os * -1;
                        overshootPercent = Math.Round((os * 100), 2);
                    }
                }
                double transCount = transEnd - transStart;
                double transTime = (transCount * SampleTime) / 1000;

                double responseTime = Math.Round(transTime, 1);

                double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, overshootPercent };
                processedData.Add(completeResult);
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
                decimal num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                if (num >= fileNumber)
                {
                    fileNumber = num + 1;
                }
            }

            string filePath = resultsFolderPath + "\\" + fileNumber.ToString("000") + "-FULL-OSRTT.csv";

            string strSeparator = ",";
            StringBuilder csvString = new StringBuilder();
            csvString.AppendLine("Starting RGB, End RGB, Response Time (ms), Overshoot RGB (%)");
            foreach (var res in processedData)
            {
                csvString.AppendLine(string.Join(strSeparator, res));
            }
            Console.WriteLine(filePath);
            File.WriteAllText(filePath, csvString.ToString());

            // Save Gamme curve to a file too
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
                // Fill processed data with the first set of results then average from there                
                int resultCount = multipleRunData[0].Count();

                List<double[]> averageData = new List<double[]>();
                for (int p = 0; p < resultCount; p++)
                {
                    double[] row = { multipleRunData[0][p][0], multipleRunData[0][p][1], 0, 0 };
                    averageData.Add(row);
                }

                // Average the data, excluding outliers
                for (int k = 0 ; k < resultCount; k++)
                {
                    List<double> rTLine = new List<double>();
                    List<double> oSLine = new List<double>();
                    foreach (var list in multipleRunData)
                    {
                        rTLine.Add(list[k][2]);
                        oSLine.Add(list[k][3]);
                    }
                    double rtMedian = GetMedian(rTLine.ToArray());
                    double osMedian = GetMedian(oSLine.ToArray());
                    int validTimeResults = 0;
                    int validOvershootResults = 0;
                    foreach (var o in multipleRunData)
                    {
                        if (o[k][2] < (rtMedian * 1.2) && o[k][2] > (rtMedian * 0.8))
                        {
                            averageData[k][2] += o[k][2];
                            validTimeResults++;
                        }
                        if (o[k][3] < (osMedian * 1.2) && o[k][3] > (osMedian * 0.8))
                        {
                            averageData[k][3] += o[k][3];
                            validOvershootResults++;
                        }
                    }
                    averageData[k][2] = averageData[k][2] / validTimeResults;
                    averageData[k][2] = Math.Round(averageData[k][2], 1);
                    if (averageData[k][3] != 0)
                    {
                        averageData[k][3] = averageData[k][3] / validOvershootResults;
                        averageData[k][3] = Math.Round(averageData[k][3], 1);
                    }
                }

                // Output averaged results to file using folder name/monitor info
                string[] folders = resultsFolderPath.Split('\\');
                string monitorInfo = folders.Last();
                //monitorInfo = monitorInfo.Remove(0, 4);
                string filePath = resultsFolderPath + "\\" + monitorInfo + "-FINAL-DATA-OSRTT.csv";
                string strSeparator = ",";
                StringBuilder csvString = new StringBuilder();
                csvString.AppendLine("Starting RGB, End RGB, Response Time (ms), Overshoot RGB (%)");
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
                        string[] files = Directory.GetFiles(filePath);
                        bool valid = false;
                        foreach (var f in files)
                        {
                            if (f.Contains("-RAW-OSRTT"))
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

        private void testButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread checkWindowThread = new Thread(new ThreadStart(this.checkFocusedWindow));
            checkWindowThread.Start();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            UpdateMe();
        }

        
        private void setFPSLimit()
        {
            var item = fpsList.Find(x => x.FPSValue == getSelectedFps());
            port.Write("L" + item.Key);
        }

        private void BrightnessCalBtn_Click(object sender, EventArgs e)
        {
            if (port != null)
            {
                //Thread brightCalThread = new Thread(new ThreadStart(this.launchBrightnessCal));
                //brightCalThread.Start();

                launchBrightnessCal();
            }
            else
            {
                MessageBox.Show("Error: device not connected. Please connect the hardware via USB and try again!", "Device Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }    
        }

        private void launchBrightnessCal()
        {
            analysePanel.Location = new Point(1100, 238);
            controlsPanel.Location = new Point(1100, 36);
            brightnessPanel.Location = new Point(0, 0);
            Size = new Size(1000, 800);
            menuStrip1.Visible = false;
            
            richTextBox1.Location = new Point(1500, 36);

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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void updateBrightness(int lvl)
        {
            string txt = "";
            
            if (lvl < 63500)
            {
                txt = "Too Low!";
            }
            else if (lvl >= 63500 && lvl < 64500)
            {
                txt = "Perfect!";
            }
            else if (lvl >= 64500)
            {
                txt = "Too High!";
            }
            else
            {
                txt = "No Results";
            }

            this.rawValText.Invoke((MethodInvoker)(() => rawValText.Text = lvl.ToString()));
            this.brightnessText.Invoke((MethodInvoker)(() => brightnessText.Text = txt));
        }

        private void closeWindowBtn_Click(object sender, EventArgs e)
        {
            try
            {
                brightnessCheck = true;
                port.Write("C");
                menuStrip1.Visible = true;
                analysePanel.Location = new Point(12, 238);
                controlsPanel.Location = new Point(12, 36);
                brightnessPanel.Location = new Point(1100, 36);
                Size = new Size(628, 275);
                brightnessWindowOpen = false;

                richTextBox1.Location = new Point(723, 36);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void incPotValBtn_Click(object sender, EventArgs e)
        {
            potVal += 1;
            if (potVal > 5)
            {
                MessageBox.Show("Your monitor's brightness may be too low. Make sure your monitor is set to its maximum brightness, otherwise any results the tool generates may be inaccurate.", "Brightness Too Low", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    port.Write(potVal.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            potVal = 1;
            try
            {
                port.Write(potVal.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
