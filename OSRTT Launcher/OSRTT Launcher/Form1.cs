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



namespace OSRTT_Launcher
{
    public partial class Form1 : Form
    {
        // CHANGE THESE VALUES WHEN ISSUING A NEW RELEASE
        private double boardVersion = 1.0;
        private double downloadedFirmwareVersion = 1.0;
        private double softwareVersion = 1.0;

        // TODO //
        // Fix averaging multiple runs
        // Initial Setup for ArduinoCLI on boot/startup (check board listall?) - done? TEST IT
        //
        // Current known issues //
        // Device will continue to run test even if game closed/not selected/program error
        // Device will still activate button even if program/game closed - check if serial connected on the board, if not connected break loop
        //
        // LOW PRIORITY - Possibly set RGB values in C# 

        public static System.IO.Ports.SerialPort port;
        delegate void SetTextCallback(string text);
        private bool boardUpdate = false;
        private bool portConnected = false;

        private int[] RGBArr;
        private int resultsLimit = 110;

        string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        string resultsFolderPath = "";
        public List<int[]> results = new List<int[]>();
        public List<double[]> processedData = new List<double[]>();
        public List<List<double[]>> multipleRunData = new List<List<double[]>>();
        public class multiRunData
        {
            public List<double[]> runData { get; set; }
        }
        public List<multiRunData> multipleRuns = new List<multiRunData>();
        public class Displays
        {
            public string Name { get; set; }
            public int Freq { get; set; }
            public string Connection { get; set; }
        }
        public List<Displays> displayList = new List<Displays>();
        private BackgroundWorker hardWorker;
        private Thread readThread = null;
        private Thread connectThread = null;
        private Thread processThread = null;
        private Thread launchGameThread = null;

        public void UpdateMe()
        {
            AutoUpdater.LetUserSelectRemindLater = false;
            AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Days;
            AutoUpdater.RemindLaterAt = 2;
            AutoUpdater.RunUpdateAsAdmin = false;
            AutoUpdater.ParseUpdateInfoEvent += AutoUpdaterOnParseUpdateInfoEvent;
            AutoUpdater.InstalledVersion = new Version(softwareVersion.ToString());
            AutoUpdater.Start("https://api.github.com/repos/andymanic/OSRTT/releases/latest");
        }

        private void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
        {
            dynamic json = JsonConvert.DeserializeObject(args.RemoteData);
            args.UpdateInfo = new UpdateInfoEventArgs
            {
                CurrentVersion = json.tag_name,
                ChangelogURL = "https://github.com/andymanic/OSRTT/releases/latest",
                DownloadURL = json.assets[0].browser_download_url,
            };
        }

        public Form1()
        {
            InitializeComponent();
            
            //UpdateMe();
            this.launchBtn.Enabled = false;
            this.setRepeatBtn.Enabled = false;
            path = new Uri(System.IO.Path.GetDirectoryName(path)).LocalPath;
            path = path + @"\Results";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            hardWorker = new BackgroundWorker();
            connectThread = new Thread(new ThreadStart(this.findAndConnectToBoard));
            connectThread.Start();
            Size = new Size(627, 283);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            listMonitors();
            initialSetup();
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
                Console.WriteLine(target);
                Console.WriteLine(target.DisplaySource);
                foreach (var item in target.TargetsInfo)
                { 
                    Console.WriteLine(item);

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
                            SetDeviceStatus("Connected to Device!");
                            ControlLaunchButton(true);
                            ControlRepeatButton(true);
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
                            ControlLaunchButton(false);
                            ControlRepeatButton(false);
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
                port.Write("F");
            }
            else
            {
                SetDeviceStatus("Board Disconnected");
                ControlLaunchButton(false);
                ControlRepeatButton(false);
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

        private void ControlLaunchButton(bool state)
        {
            if (this.launchBtn.InvokeRequired)
            {
                this.launchBtn.Invoke((MethodInvoker)(() => launchBtn.Enabled = state));
            }
            else
            {
                this.launchBtn.Enabled = state;
            }
        }

        private void ControlRepeatButton(bool state)
        {
            if (this.setRepeatBtn.InvokeRequired)
            {
                this.launchBtn.Invoke((MethodInvoker)(() => setRepeatBtn.Enabled = state));
            }
            else
            {
                this.setRepeatBtn.Enabled = state;
            }
        }

        private void sendText(string textToSend)
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

        public void Read()
        {
            while (port.IsOpen)
            {
                try
                {
                    string message = port.ReadLine();
                    Console.WriteLine(message);
                    SetText(message);
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
                        processedData.Clear();
                        results.Clear();
                    }
                    else if (message.Contains("STARTING TEST"))
                    {
                        // CREATE FOLDER WITH CURRENT FILE NAME SHIT
                        // SAVE FOLDER PATH AS currentResultsPath
                        makeResultsFolder();
                        multipleRuns.Clear();
                        processedData.Clear();
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
                        // READ IN 
                        processMultipleRuns();
                        DialogResult d = MessageBox.Show("Test complete, open results folder?","Test Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (d == DialogResult.Yes)
                        {
                            //open folder
                            Process.Start("explorer.exe", resultsFolderPath);
                        }
                    }
                    else if (message.Contains("low"))
                    {
                        // Brightness too low - take brightnes reading and convert to a percentage to show how big a change is needed. 65,000 is the max supported reading, but 60,000 is the lower bound so picking 62,000 gives a middleground percentage
                        string[] currentLevel = message.Split(':');
                        int currLvl = int.Parse(currentLevel[1]);
                        double percentOff = (100 - (currLvl / 64000) * 100);
                        MessageBox.Show("Monitor Brightness Too Low! It's roughly " + percentOff + "% too dim. Please increase the brightness and press the button again.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (message.Contains("high"))
                    {
                        // Brightness too high - can't show % off figure as the values clip at 65,539
                        MessageBox.Show("Monitor Brightness Too High! Please decrease the brightness and press the button again.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (message.Contains("Unable to set brightness"))
                    {
                        string[] currentLevel = message.Split(':');
                        int currLvl = int.Parse(currentLevel[1]);
                        MessageBox.Show("Unable to set sensor calibration, monitor brightness may be too low.", "Test Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    else if (message.Contains("FW:"))
                    {
                        string[] sp = message.Split(':');
                        boardVersion = double.Parse(sp[1]);
                        compareFirmware();
                        //MessageBox.Show(message, "Firmware Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    ControlLaunchButton(false);
                    ControlRepeatButton(false);
                    SetDeviceStatus("Board Disconnected");
                    readThread.Abort();
                    // findAndConnectToBoard();
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // When form is closed halt read thread & close Serial Port
            ControlLaunchButton(false);
            ControlRepeatButton(false);
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
                port.Close();
            }

        }

        private void launchBtn_Click(object sender, EventArgs e)
        {
            launchGameThread = new Thread(new ThreadStart(this.launchGameAndWaitForExit));
            launchGameThread.Start();
        }

        private void launchGameAndWaitForExit()
        {
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
                while (p.Length == 0)
                {
                    p = Process.GetProcessesByName("ResponseTimeTest-Win64-Shipping");
                }
                p[0].WaitForExit();
                Console.WriteLine("Game closed");
                port.Write("C");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UE4 Project Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Size = new Size(627, 432);
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


            foreach (var item in fullGammaTable)
            {
                Console.WriteLine(item[0] + "," + item[1]);
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

                // Clean up noisy data
                // CONSIDER REDOING HOW THIS WORKS to average data better
                for (int k = 0; k < samples.Length - 5; k += 5)
                {
                    int partAvg = samples[k] + samples[k + 1] + samples[k + 2] + samples[k + 3] + samples[k + 4];
                    int avg = partAvg / 5;
                    samples[k] = avg;
                    samples[k + 1] = avg;
                    samples[k + 2] = avg;
                    samples[k + 3] = avg;
                    samples[k + 4] = avg;
                }
                for (int k = 2; k < samples.Length - 5; k += 5)
                {
                    int partAvg = samples[k] + samples[k + 1] + samples[k + 2] + samples[k + 3] + samples[k + 4];
                    int avg = partAvg / 5;
                    samples[k] = avg;
                    samples[k + 1] = avg;
                    samples[k + 2] = avg;
                    samples[k + 3] = avg;
                    samples[k + 4] = avg;
                }
                for (int k = 0; k < samples.Length - 10; k += 10)
                {
                    int partAvg = samples[k] + samples[k + 1] + samples[k + 2] + samples[k + 3] + samples[k + 4] + samples[k + 5] + samples[k + 6] + samples[k + 7] + samples[k + 8] + samples[k + 9];
                    int avg = partAvg / 10;
                    samples[k] = avg;
                    samples[k + 1] = avg;
                    samples[k + 2] = avg;
                    samples[k + 3] = avg;
                    samples[k + 4] = avg;
                    samples[k + 5] = avg;
                    samples[k + 6] = avg;
                    samples[k + 7] = avg;
                    samples[k + 8] = avg;
                    samples[k + 9] = avg;
                }

                samples = samples.Take(samples.Length - 5).ToArray();

                int maxValue = samples.Max(); // Find the maximum value for overshoot
                int minValue = samples.Min(); // Find the minimum value for undershoot
                // Initialise in-use variables
                int transStart = 0;
                int transEnd = 0;

                double overUnderRGB = 0.0;

                int startMax = samples[5]; // Initialise these variables with a real value 
                int startMin = samples[5]; // Initialise these variables with a real value 
                int endMax = samples[item.Length - 10]; // Initialise these variables with a real value 
                int endMin = samples[item.Length - 10]; // Initialise these variables with a real value 

                // Build start min/max to compare against
                for (int j = 0; j < 250; j++) //CHANGE TO 180 FOR RUN 2 DATA
                {
                    if (samples[j] < startMin)
                    {
                        startMin = samples[j];
                    }
                    else if (samples[j] > startMax)
                    {
                        startMax = samples[j];
                    }
                }

                // Build end min/max to compare against
                for (int j = samples.Length - 5; j > samples.Length - 450; j--)
                {
                    if (samples[j] < endMin)
                    {
                        endMin = samples[j];
                    }
                    else if (samples[j] > endMax)
                    {
                        endMax = samples[j];
                    }
                }

                // Search for where the result starts transitioning - start is almost always less sensitive
                for (int j = 0; j < samples.Length; j++)
                {
                    if (StartingRGB < EndRGB)
                    {
                        if (samples[j] > (startMax))
                        {
                            if ((samples[j + 50] > (samples[j] + 100) || samples[j + 56] > (samples[j] + 50))
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
                    else
                    {
                        if (samples[j] < (startMin))
                        {
                            if ((samples[j + 50] < (samples[j] - 100) || samples[j + 56] < (samples[j] - 100))
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

                // Search for where the result stops transitioning (from the end) - end position is almost always more sensitive hence lower values - also must account for over/undershoot
                for (int j = samples.Length - 1; j > 0; j--)
                {
                    if (StartingRGB < EndRGB)
                    {
                        if (maxValue > (endMax + 200)) //Check for overshoot
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
                        if (minValue < (endMin - 200)) //Check for undershoot
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
                        }
                        overshootPercent = Math.Round((((overUnderRGB - EndRGB) / EndRGB) * 100), 2);
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
                        }
                        overshootPercent = Math.Round((((overUnderRGB - EndRGB) / EndRGB) * 100), 2);
                    }
                }
                double transCount = transEnd - transStart;
                double transTime = (transCount * SampleTime) / 1000;

                double responseTime = Math.Round(transTime, 1);

                double[] completeResult = new double[] { StartingRGB, EndRGB, responseTime, overshootPercent };
                processedData.Add(completeResult);
            }
            //multiRunData tempData = new multiRunData { runData = processedData };
            //multipleRuns.Add(tempData);
            List<double[]> temp = new List<double[]>();
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
            File.WriteAllText(filePath, csvString.ToString());

            // Save Gamme curve to a file too
            decimal gammaFileNumber = 001;
            // search /Results folder for existing file names, pick new name
            string[] existingGammaFiles = Directory.GetFiles(resultsFolderPath, "*-GAMMA-OSRTT.csv");
            // Search \Results folder for existing results to not overwrite existing or have save conflict errors
            foreach (var s in existingGammaFiles)
            {
                decimal num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3));
                Console.WriteLine("Num: " + num);
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

        private void setRepeatBtn_Click(object sender, EventArgs e)
        {
            port.Write("M" + this.testCount.Value.ToString());
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
                    double[] row = { processedData[p][0], processedData[p][1], 0, 0 };
                    averageData.Add(row);
                }

                // Average response time and overshoot results
                foreach (var L in multipleRunData)
                {
                    if (L.Count() != resultCount)
                    {
                        MessageBox.Show("Unable to process results, a file may be missing result entries.", "Results Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        for (int i = 0; i < L.Count; i++)
                        {
                            // Add response time values
                            averageData[i][2] += L[i][2];

                            // Add overshoot values
                            averageData[i][3] = L[i][3];
                        }
                    }
                }
                int runCount = multipleRunData.Count();
                foreach (var res in averageData)
                {
                    res[2] = res[2] / runCount;
                    res[2] = Math.Round(res[2], 1);
                    if (res[3] != 0)
                    {
                        res[3] = res[3] / runCount;
                        res[3] = Math.Round(res[3], 1);
                    }
                }
                

                // Output averaged results to file
                int monitor = getSelectedMonitor();
                string monitorName = displayList[monitor].Name;
                string monitorInfo = monitorName.Replace(" ", "-") + "-" + displayList[monitor].Freq.ToString() + "-" + displayList[monitor].Connection;
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
                        multipleRuns.Clear();
                        string[] files = Directory.GetFiles(filePath);
                        bool valid = false;
                        foreach (var f in files)
                        {
                            if (f.Contains("-RAW-OSRTT"))
                            {
                                valid = true;
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
            Process.Start("explorer.exe", path + "\\008-AORUS-FI27Q-P-165-DP");
        }
    }
}
