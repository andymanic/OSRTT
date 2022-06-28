using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Management;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSRTT_Launcher
{
    class DataUpload
    {
        public class ShareData
        {
            public string GUID { get; set; }
            public List<ProcessData.rawResultData> rawData { get; set; }
            public List<int[]> gammaData { get; set; }
            public List<int> testLatency { get; set; }
            public ProcessData.runSettings runSettings { get; set; }
            public SystemInfo sysInfo { get; set; }
        }
        public class SystemInfo
        {
            public string boardSerial { get; set; }
            public CPU cpu { get; set; }
            public GPU gpu { get; set; }
            public RAM ram { get; set; }
            public string MACAddress { get; set; }
        }
        public class CPU
        {
            public string CPUName { get; set; }
            public int Cores { get; set; }
            public int LogicalProcessors { get; set; }
        }
        public class GPU
        {
            public string GPUName { get; set; }
            public Int64 VRAM { get; set; }
            public string GPUDriver { get; set; }
        }
        public class RAM
        {
            public Int64 totalCapcity { get; set; }
            public int sticks { get; set; }
            public int FormFactor { get; set; }
            public string PartNumber { get; set; }
            public int RamSpeed { get; set; }
            public int RamVolts { get; set; }
        }
        Thread uploadThread;
        public async void UploadData(object data, string url)
        {
            string json = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            try
            {
                var httpResponse = await httpClient.PostAsync(url, httpContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
        public async void UploadBlobData(byte[] data, string url)
        {
            var httpContent = new ByteArrayContent(data);
            var httpClient = new HttpClient();
            try
            {
                var httpResponse = await httpClient.PostAsync(url, httpContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }

        public SystemInfo GetSystemInfo()
        {
            SystemInfo si = new SystemInfo
            {
                boardSerial = Properties.Settings.Default.serialNumber,
                cpu = new CPU(),
                gpu = new GPU(),
                ram = new RAM()
            };
            ManagementObjectSearcher cpu = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject cpuObj in cpu.Get())
            {
                si.cpu.CPUName = cpuObj["Name"].ToString();
                si.cpu.Cores = Convert.ToInt32(cpuObj["NumberOfCores"]);
                si.cpu.LogicalProcessors = Convert.ToInt32(cpuObj["NumberOfLogicalProcessors"]);
            }
            ManagementObjectSearcher gpu = new ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (ManagementObject gpuObj in gpu.Get())
            {
                si.gpu.GPUName = gpuObj["Name"].ToString();
                si.gpu.VRAM = Convert.ToInt64(gpuObj["AdapterRAM"]);
                si.gpu.GPUDriver = gpuObj["DriverVersion"].ToString();
            }
            ManagementObjectSearcher ram = new ManagementObjectSearcher("select * from Win32_PhysicalMemory ");
            Int64 capacity = 0;
            int sticks = 0;
            foreach (ManagementObject ramObj in ram.Get())
            {
                capacity += Convert.ToInt64(ramObj["Capacity"]);
                sticks += 1;
                si.ram.FormFactor = Convert.ToInt32(ramObj["FormFactor"]);
                si.ram.PartNumber = ramObj["PartNumber"].ToString();
                si.ram.RamSpeed = Convert.ToInt32(ramObj["ConfiguredClockSpeed"]);
                si.ram.RamVolts = Convert.ToInt32(ramObj["ConfiguredVoltage"]);
            }
            si.ram.totalCapcity = capacity;
            si.ram.sticks = sticks;
            ManagementObjectSearcher nac = new ManagementObjectSearcher("select * from Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject nacObj in nac.Get())
            {
                bool enabled = (bool)nacObj["IPEnabled"];
                if (enabled)
                {
                    si.MACAddress = nacObj["MACAddress"].ToString();
                }
            }
            return si;
            //UploadData(si, "https://api.osrtt.com/systemInfo");
        }
        public void UploadRawData(List<List<ProcessData.rawResultData>> rawData)
        {
            // thread this
            string url = "https://api.osrtt.com/rawData";
            UploadData(rawData, url);
        }
        public void UploadGammaData(List<ProcessData.gammaResult> gamma)
        {
            string url = "https://api.osrtt.com/gammaData";
            UploadData(gamma, url);
        }
        public void UploadTestLatency(List<int> testLatency)
        {
            string url = "https://api.osrtt.com/testLatency";
            UploadData(testLatency, url);
        }
        public void UploadRunSettings(ProcessData.runSettings testLatency)
        {
            string url = "https://api.osrtt.com/runSetting";
            UploadData(testLatency, url);
        }

        // PC config...

        public void ShareResults(
            List<List<ProcessData.rawResultData>> rawData, 
            List<int[]> gamma, 
            List<int> testLatency,
            ProcessData.runSettings runSetting
            // PC config
            )
        {
            Guid g = Guid.NewGuid();
            SystemInfo systemInfo = GetSystemInfo();
            List<ShareData> allRuns = new List<ShareData>();
            foreach (var r in rawData)
            {
                ShareData share = new ShareData
                {
                    GUID = g.ToString(),
                    rawData =  r ,
                    gammaData = gamma,
                    testLatency = testLatency,
                    runSettings = runSetting,
                    sysInfo = systemInfo
                };
                allRuns.Add(share);
                Thread shareThread = new Thread(()=> UploadData(share, "https://api.locally.link/osrtt"));
                shareThread.Start();
                //UploadData(share, "https://api.locally.link/osrtt");
            }
            //var binFormatter = new BinaryFormatter();
            //var mStream = new MemoryStream();
            //binFormatter.Serialize(mStream, allRuns);

            //byte[] blob = Compress(mStream.ToArray());
            //string base64Blob = Convert.ToBase64String(blob);
            //Thread shareThreadV2 = new Thread(() => UploadData(base64Blob, "https://api.locally.link/osrtt"));
            //shareThreadV2.Start();

            //UploadRawData(rawData);
            //UploadGammaData(gamma);
            //UploadTestLatency(testLatency);
            //UploadRunSettings(runSetting);
        }
        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory,
                    CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }
    }
}
