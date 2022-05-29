using System;
using System.Diagnostics;

namespace OSRTT_Launcher.DirectX.System
{
    public class DCPU                   // 60 lines
    {
        // Variables
        private bool _CanReadCPU;
        private PerformanceCounter counter;
        private TimeSpan _LastSampleTime;
        private long _CpuUsage;

        // Properties
        public int Value { get { return _CanReadCPU ? (int)_CpuUsage : 0; } }

        // Public Methods.
        public void Initialize()
        {
            // Initialize the flag indicating whether this object can read the system cpu usage or not.
            _CanReadCPU = true;

            try
            {
                // Create performance counter.
                counter = new PerformanceCounter();
                counter.CategoryName = "Processor";
                counter.CounterName = "% Processor Time";
                counter.InstanceName = "_Total";

                _LastSampleTime = DateTime.Now.TimeOfDay;

                _CpuUsage = 0;
            }
            catch
            {
                _CanReadCPU = false;
            }
        }
 
        public void Shutdown()
        {
            if (_CanReadCPU)
                counter.Close();
        }
        public void Frame()
        {
            if (_CanReadCPU)
            {
                int secondsElapsed = (DateTime.Now.TimeOfDay - _LastSampleTime).Seconds;

                if (secondsElapsed >= 1)
                {
                    _LastSampleTime = DateTime.Now.TimeOfDay;
                    _CpuUsage = (int)counter.NextValue();
                }
            }
        }
    }
}