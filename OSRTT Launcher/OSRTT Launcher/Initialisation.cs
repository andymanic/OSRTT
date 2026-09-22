using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSRTT_Launcher
{
    class Initialisation
    {
        public class userSettings
        {
            public class usersetting
            {
                public string name { get; set; }
                public string value { get; set; }
            }
            public List<usersetting> usersettings { get; set; } = new List<usersetting>();
        }
        public static void readAndSaveUserSettings(bool closing)
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
                        us.usersettings.Add(new userSettings.usersetting { name = s.Name, value = Properties.Settings.Default[s.Name].ToString() });
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

        public static void initialSetup(bool excelInstalled)
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
                    CFuncs.showMessageBox("Warning: Excel doesn't seem to be installed. Saving to the XLSX or XLSM templates (results or graph view templates) won't work and have been disabled.", "Excel Not Installed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Properties.Settings.Default.saveXLSX = false;
                    Properties.Settings.Default.saveGraphs = false;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public static void appRunning()
        {
            Process[] p = Process.GetProcessesByName("OSRTT Launcher");
            if (p.Length > 1)
            {
                MessageBox.Show("ERROR: Program already open! Please close it before running again, or check the task bar and system tray for the running app.", "Program Open Already", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(p.Length);
                //this.Close();
            }
        }
    }
}
