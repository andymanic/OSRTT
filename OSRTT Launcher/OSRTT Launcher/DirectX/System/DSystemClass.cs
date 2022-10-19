using OSRTT_Launcher.DirectX.Graphics;
using OSRTT_Launcher.DirectX.Input;
using SharpDX.Windows;
using SharpDX.RawInput;
using SharpDX.DirectInput;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TestConsole;
using System;
using System.Windows.Input;
using Key = System.Windows.Input.Key;
using System.Reflection;

namespace OSRTT_Launcher.DirectX.System
{
    // I have to give a massive thank you to both xoofx for making SharpDX and all the work on it over the years
    // https://github.com/sharpdx/SharpDX
    // And to Dan6040 for the Rastertek tutorial series convertion to C#
    // https://github.com/Dan6040/SharpDX-Rastertek-Tutorials

    public class DSystem
    {
        // Properties
        private RenderForm RenderForm { get; set; }
        public DSystemConfiguration Configuration { get; private set; }
        public DInput Input { get; private set; }
        public static KeyboardInputEventArgs KeyboardEvent { get; set; }
        public static MouseInputEventArgs MouseEvent { get; set; }
        public DGraphics Graphics { get; private set; }
        public DFPS FPS { get; private set; }
        public DCPU CPU { get; private set; }
        public static DTimer Timer { get; private set; }
        public List<float> FrameTimeList { get; private set; }
        public static List<float> EventList { get; private set; }
        private int ILFrameCounter { get; set; }
        public static bool inputLagMode { get; set; }
        public static float RGB { get; set; }
        //private Stopwatch eventTimer { get; set; }
        public static Main mainWindow { get; set; }

        public static bool exit { get; set; }

        private static int failedFrames { get; set; }
        private static bool directInput = false;

        // Statuc Properties
        public static bool IsMouseOffScreen { get; set; }

        // Constructor
        public DSystem() { }

        public static void StartRenderForm(string title, int width, int height, bool vSync, bool fullScreen = true, int display = 0, double fpsLimit = 1)
        {
            DSystem system = new DSystem();
            system.Initialize(title, width, height, vSync, fullScreen, display);
            if (!directInput) // !directInput
            {
                Console.WriteLine("Registering Devices");
                SharpDX.RawInput.Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericMouse, SharpDX.RawInput.DeviceFlags.None);
                SharpDX.RawInput.Device.MouseInput += MouseEvents;
                SharpDX.RawInput.Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericKeyboard, SharpDX.RawInput.DeviceFlags.None);
                SharpDX.RawInput.Device.KeyboardInput += KeyboardEvents;
                KeyboardEvent = new KeyboardInputEventArgs();
                MouseEvent = new MouseInputEventArgs();
            }
            failedFrames = 0;
            
            system.RunRenderForm(fpsLimit);
            system = null;
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // Methods
        public virtual bool Initialize(string title, int width, int height, bool vSync, bool fullScreen, int display = 0)
        {
            bool result = false;

            if (Configuration == null)
                Configuration = new DSystemConfiguration(title, width, height, fullScreen, vSync, display);

            // Initialize Window.
            InitializeWindows(title, display);
            if (directInput)
            {
                if (Input == null)
                {
                    Input = new DInput();
                    if (!Input.Initialize(Configuration, RenderForm.Handle))
                        return false;
                }
            }
            if (Graphics == null)
            {
                Graphics = new DGraphics();
                result = Graphics.Initialize(Configuration, RenderForm.Handle);
            }

            DPerfLogger.Initialize("RenderForm C# SharpDX: " + Configuration.Width + "x" + Configuration.Height + " VSync:" + DSystemConfiguration.VerticalSyncEnabled + " FullScreen:" + DSystemConfiguration.FullScreen + "   " + RenderForm.Text, 0, Configuration.Width, Configuration.Height); ;

            // Create and initialize the FpsClass. 
            FPS = new DFPS();
            FPS.Initialize();

            // Create and initialize the CPU.
            CPU = new DCPU();
            CPU.Initialize();

            // Create and initialize Timer.
            Timer = new DTimer();
            if (!Timer.Initialize())
            {
                MessageBox.Show("Could not initialize Timer object", "Error", MessageBoxButtons.OK);
                return false;
            }

            FrameTimeList = new List<float>();
            EventList = new List<float>();
            ILFrameCounter = 0;
            //eventTimer = new Stopwatch();

            /*Hotkey hk1 = new Hotkey(Keys.D1);
            Hotkey hk2 = new Hotkey(Keys.D6);
            hotkeyListener.Add(hk1);
            hotkeyListener.Add(hk2);
            */
            

            //hotKeys = new HotKeyManager(RenderForm.Handle);
            //hotKeys = new HotKeyManager();
            return result;
        }

        private void InitializeWindows(string title, int display = 0)
        {
            Screen[] screens = Screen.AllScreens;
            int width = screens[display].Bounds.Width;
            int height = screens[display].Bounds.Height;

            // Initialize Window.
            RenderForm = new RenderForm(title)
            {
                ClientSize = new Size(Configuration.Width, Configuration.Height),
                FormBorderStyle = DSystemConfiguration.BorderStyle
            };

            // The form must be showing in order for the handle to be used in Input and Graphics objects.
            RenderForm.Show();
            RenderForm.Location = new Point(screens[display].Bounds.Location.X, screens[display].Bounds.Location.Y);
            //RenderForm.Location = new Point((width / 2) - (Configuration.Width / 2), (height / 2) - (Configuration.Height / 2));
        }
        private void RunRenderForm(double fpsLimit = 1)
        {
            Stopwatch sw = Stopwatch.StartNew();
            float ticksPerMs = (float)(Stopwatch.Frequency / 1000.0f);
            int wasteTime = 0;
            // Calc fps limit based on ticks per ms 
            float frameTickLimit = (float)fpsLimit * ticksPerMs;
            frameTickLimit -= 2; // += ?
            Console.WriteLine("Starting render form");
            RenderLoop.Run(RenderForm, () =>
            {
                sw.Restart();
                if (!Frame())
                    ShutDown();
                while (sw.ElapsedTicks < frameTickLimit) // ticks - 2
                {
                    //wasteTime = 0;
                }
            });
        }
        private static void KeyboardEvents(object sender, KeyboardInputEventArgs e)
        {
            KeyboardEvent = e;
        }
        private static void MouseEvents(object sender, MouseInputEventArgs e)
        {
            MouseEvent = e;
        }
        public bool Frame()
        {
            // Check if the user pressed escape and wants to exit the application.
            
            //Console.WriteLine("1");
            if (exit)
                return false;
            //Console.WriteLine("2");
            // Update the system stats.
            CPU.Frame();
            //Console.WriteLine("3");
            FPS.Frame();
            //Console.WriteLine("4");
            // Performance Logging.
            Timer.Frame2();
            //Console.WriteLine("5");
            /*if (true)
            {
                //DPerfLogger.Frame(Timer.FrameTime);
                //FrameTimeList.Add(Timer.FrameTime);

                //if (Timer.CumulativeFrameTime >= DPerfLogger.TestTimeInSeconds * 1000)
                  //  return false;
            }*/

            // Do the frame processing for the graphics object.
            if (!Graphics.Frame(FPS.Value, CPU.Value))
            {
                //failedFrames++;
                //if (failedFrames > 10)
                //{
                    return false;
                //}
            }
            //Console.WriteLine("6");
            if (!directInput) //!directInput
            {
                if (KeyboardEvent != null)
                {
                    if (KeyboardEvent.Key == Keys.Escape)
                        return false;
                    switch (KeyboardEvent.Key)
                    {
                        case Keys.D1:  // RGB 0
                            RGB = 0f;
                            break;
                        case Keys.D2:  // RGB 51
                            RGB = 0.2f;
                            break;
                        case Keys.D3:  // RGB 102
                            RGB = 0.4f;
                            break;
                        case Keys.D4:  // RGB 153
                            RGB = 0.6f;
                            break;
                        case Keys.D5:  // RGB 204
                            RGB = 0.8f;
                            break;
                        case Keys.D6:  // RGB 255
                            RGB = 1f;
                            if (inputLagMode)
                                EventList.Add(Timer.FrameTime); // I think this needs to be done on the next frame for perfect accuracy but I'll see.
                            break;
                        case Keys.D7:  // RGB 17
                            RGB = 17f / 255f;
                            break;
                        case Keys.D8:  // RGB 34
                            RGB = 34f / 255f;
                            break;
                        case Keys.D9:  // RGB 68
                            RGB = 68f / 255f;
                            break;
                        case Keys.D0:  // RGB 85 
                            RGB = 85f / 255f;
                            break;
                        case Keys.Q:  // RGB 119
                            RGB = 119f / 255f;
                            break;
                        case Keys.W:  // RGB 136
                            RGB = 136f / 255f;
                            break;
                        case Keys.E:  // RGB 170
                            RGB = 170f / 255f;
                            break;
                        case Keys.A:  // RGB 187
                            RGB = 187f / 255f;
                            break;
                        case Keys.S:  // RGB 221
                            RGB = 221f / 255f;
                            break;
                        case Keys.D:  // RGB 238
                            RGB = 238f / 255f;
                            break;
                    }
                }
                if (MouseEvent != null)
                {
                    if (MouseEvent.ButtonFlags == MouseButtonFlags.LeftButtonDown)
                    {
                        RGB = 1f;
                        if (inputLagMode)
                        {
                            EventList.Add(Timer.FrameTime); // I think this needs to be done on the next frame for perfect accuracy but I'll see.
                            ILFrameCounter += 1;
                        }
                    }
                    if (MouseEvent.ButtonFlags == MouseButtonFlags.RightButtonDown)
                    {
                        RGB = 0f;
                    }
                }
            }
            //Console.WriteLine("7");


            // Finally render the graphics to the screen.
            if (!Graphics.Render(RGB))
                return false;

            return true;
        }
        public void ShutDown()
        {
            Console.WriteLine("Shutting Down");
            SharpDX.RawInput.Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericKeyboard, SharpDX.RawInput.DeviceFlags.Remove);
            SharpDX.RawInput.Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericMouse, SharpDX.RawInput.DeviceFlags.Remove);
            ShutdownWindows();
            DPerfLogger.ShutDown();
            //mainWindow.getTestFPS(FrameTimeList);
            mainWindow.getInputLagEvents(EventList);
            // Release graphics and related objects.
            Graphics?.Shutdown();
            Graphics = null;
            // Release DriectInput related object.
            Input?.Shutdown();
            Input = null;

            Configuration = null;



            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        private void ShutdownWindows()
        {
            RenderForm?.Dispose();
            RenderForm = null;
        }




    }
}