using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace OSRTT_Launcher
{
    class DisplayInitCheck
    {
        public void getMonitorInitTime()
        {
            ManagementObjectSearcher win32Monitor = new ManagementObjectSearcher("select * from Win32_DesktopMonitor");

            foreach (ManagementObject obj in win32Monitor.Get())
            {
                
                Console.WriteLine(obj["ScreenWidth"].ToString());
                Console.WriteLine(obj["ScreenHeight"].ToString());
                Console.WriteLine(obj["Manufacturer"].ToString());
                Console.WriteLine(obj["DeviceID"].ToString());
                Console.WriteLine(obj["InstallDate"].ToString());
            }
        }

    }
}
