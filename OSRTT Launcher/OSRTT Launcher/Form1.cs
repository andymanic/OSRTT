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

namespace OSRTT_Launcher
{
    public partial class Form1 : Form
    {
        // TODO
        // Get system refresh rate?
        // 
        // Actually save data into Results dictionary
        // Save contents of dictionary to CSV "000-OSRTT-RAW-RESULTS.csv"
        // Low ish priority - fix get firmware version
        // LOW PRIORITY - Possibly set RGB values in C# 
        // Ultra low priority - Find monitor name if possible to save results file under than name




        public static System.IO.Ports.SerialPort port;
        delegate void SetTextCallback(string text);
        private bool boardConfirmed = false;
        private bool portConnected = false;
        private int[] RGBArr;
        private int resultsCounter = 0;
        private int resultsLimit = 110;
        class Results
        {
            public int FromRGB { get; set; }
            public int ToRGB { get; set; }
            public int SampleTime { get; set; }
            public int SampleNum { get; set; }
            public int[] Vals { get; set; }
        }
        private Dictionary<int, Results> res;

        // This BackgroundWorker is used to demonstrate the 
        // preferred way of performing asynchronous operations.
        private BackgroundWorker hardWorker;

        private Thread readThread = null;
        private Thread connectThread = null;

        public Form1()
        {
            InitializeComponent();
            this.launchBtn.Enabled = false;
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            hardWorker = new BackgroundWorker();
            connectThread = new Thread(new ThreadStart(this.findAndConnectToBoard));
            connectThread.Start();
            Size = new Size(630, 206);
            

            
           

        }
        private void findAndConnectToBoard()
        {
            while (true)
            {
                if (!portConnected)
                {
                    foreach (string s in SerialPort.GetPortNames().Reverse())
                    {
                        Console.WriteLine(s);
                        if (!portConnected)
                        {
                            connectToBoard(s);
                        }
                        else
                        {
                            break;
                        }

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

                
                

                // Check if board is correct, reading verification string

                // Have board send RGB Array to load in C#

            }
            else
            {
                SetDeviceStatus("Board Disconnected");
            } 
        }

        private void sendText(String textToSend)
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

        public void Read()
        {

            while (port.IsOpen)
            {
                try
                {
                    string message = port.ReadLine();
                    Console.WriteLine(message);
                    if (!boardConfirmed)
                    {
                        if (message.Contains("OSRTT"))
                        {
                            boardConfirmed = true;
                            SetDeviceStatus("Connected to Device!");
                            this.launchBtn.Enabled = true;  // Invoke this too
                            port.Write("1");
                        }
                        else if (message.Contains("Established"))
                        {
                            Console.WriteLine("C# Check");
                            port.Write("RST");
                        }
                    }
                    else if (message.Contains("RGB Array"))
                    {
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
                                Console.WriteLine(RGBArr[counter]); // stop it from recording the last empty value
                                counter++;
                            }

                            
                        }
                        Console.Write("RGBArr Length: ");
                        Console.WriteLine(RGBArr.Length);

                        resultsLimit = RGBArr.Length * (RGBArr.Length - 1);
                        Console.Write("Results Limit: ");
                        Console.WriteLine(resultsLimit);
                    }
                    else if (message.Contains("Result"))
                    {
                        // Split results 
                        String newMessage = message.Remove(0, 7);
                        string[] values = newMessage.Split(',');
                        int[] intValues = Array.ConvertAll<string, int>(values, int.Parse);
                        int fromRGBVal = intValues[0];
                        int toRGBVal = intValues[1];
                        int sampleTimeVal = intValues[2];
                        int sampleNumVal = intValues[3];
                        intValues = intValues.Where((source, index) => index > 3).ToArray();
                        // Check if this shit works.
                        // 
                        res = new Dictionary<int, Results>();
                        res.Add(resultsCounter, new Results { FromRGB = fromRGBVal, ToRGB = toRGBVal, SampleTime = sampleTimeVal, SampleNum = sampleTimeVal, Vals = intValues });
                        resultsCounter++;
                        Console.WriteLine(res[resultsCounter].FromRGB + " " + res[0].Vals.Length);
                        if (resultsCounter == resultsLimit)
                        {
                            resultsCounter = 0;
                            // Flush dictionary
                        }
                    }
                    else if (message.Contains("FW:"))
                    {
                        MessageBox.Show(message, "Firmware Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        this.SetText(message);
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    Console.WriteLine("Trying to reconnect");
                    port.Close();
                    portConnected = false;
                    boardConfirmed = false;
                    this.launchBtn.Enabled = false; // invoke via thread...
                    SetDeviceStatus("Board Disconnected");
                    readThread.Abort();
                    // findAndConnectToBoard();
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // When form is closed halt read thread & close Serial Port
            readThread.Abort();
            connectThread.Abort();
            port.Close();
            
        } 

        private void launchBtn_Click(object sender, EventArgs e)
        {
            Size = new Size(1145, 868);
            // richTextBox1.Text = sb.ToString();
        }

        private void resultsBtn_Click(object sender, EventArgs e)
        {
            // Save results table to csv
        }

        private void reconnectDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            findAndConnectToBoard();
        }

        private void checkDeviceFirmwareVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (port.IsOpen)
            {
                Console.WriteLine("Checking Firmware version...");
                port.Write("FW");
            }
            else
            {
                MessageBox.Show("Please connect to the device first!", "Firmware Version", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void analyseResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size = new Size(630, 406);
        }

        private void debugModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Size = new Size(1120, 850);
        }
    }
}
