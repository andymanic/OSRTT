using AutoUpdaterDotNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSRTT_Launcher
{
    public class UpdateHandler
    {
        public static void UpdateMe(string softwareVersion)
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

        private static void AutoUpdaterOnParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
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
        private static void AutoUpdater_ApplicationExitEvent()
        {
            Properties.Settings.Default.updateInProgress = true;
            Properties.Settings.Default.Save();
            Application.Exit();
        }

        public class AnnouncementText
        {
            public string titleText { get; set; } = "No updates found";
            public string descriptionText { get; set; } = "";
            public bool Read { get; set; }
        }

        public static AnnouncementText GetAnnouncements(string localPath)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("user-agent", "OSLTT");

                try
                {
                    List<AnnouncementText> existingList;
                    if (File.Exists(localPath + @"\\announcements.json"))
                    {
                        existingList = JsonConvert.DeserializeObject<List<AnnouncementText>>(File.ReadAllText(localPath + @"\\announcements.json"));
                    }
                    else
                    {
                        existingList = new List<AnnouncementText>();
                    }

                    string url = "https://raw.githubusercontent.com/OSRTT/OSLTT/main/OSLTT/announcements.json";
                    //wc.DownloadFile(url, localPath + @"announcements.json");
                    string content = wc.DownloadString(url);
                    List<AnnouncementText> jsonified = JsonConvert.DeserializeObject<List<AnnouncementText>>(content);

                    if (jsonified.Count() > existingList.Count())
                    {
                        // there's new shit probably.
                        // save new list to file
                        jsonified.Last().Read = true;
                        File.Delete(localPath + @"\\announcements.json");
                        File.WriteAllText(localPath + @"\\announcements.json", JsonConvert.SerializeObject(jsonified));
                        // return newest results
                        return jsonified.Last();
                    }
                    else if (!existingList.Last().Read)
                    {
                        // last result hasn't been shown yet. (somehow)
                        existingList.Last().Read = true;
                        File.Delete(localPath + @"\\announcements.json");
                        File.WriteAllText(localPath + @"\\announcements.json", JsonConvert.SerializeObject(existingList));
                        return existingList.Last();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }

            }

            return null;
        }

        public static void getDownloadedFirmwareVersions(string localPath, double ProDLFW, double V1DLFW)
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

        public static void getODModesJson()
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

    }
}
