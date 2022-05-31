using OSRTT_Launcher.DirectX.Graphics;
using OSRTT_Launcher.DirectX.Input;
using SharpDX.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TestConsole;

namespace OSRTT_Launcher.DirectX.System
{
    public class DSystem                    // 135 lines
    {
        // Properties
        private RenderForm RenderForm { get; set; }
        public DSystemConfiguration Configuration { get; private set; }
        public DInput Input { get; private set; }
        public DGraphics Graphics { get; private set; }
        public DFPS FPS { get; private set; }
        public DCPU CPU { get; private set; }
        public DTimer Timer { get; private set; }
        public List<float> FrameTimeList { get; private set; }
        public static float RGB { get; set; }
        //private Stopwatch eventTimer { get; set; }
        public static Main mainWindow { get; set; }

        // Statuc Properties
        public static bool IsMouseOffScreen { get; set; }

        // Constructor
        public DSystem() { }

        public static void StartRenderForm(string title, int width, int height, bool vSync, bool fullScreen = true, int display = 0, double fpsLimit = 1)
        {
            DSystem system = new DSystem();
            system.Initialize(title, width, height, vSync, fullScreen, display);
            system.RunRenderForm(fpsLimit);
        }

        // Methods
        public virtual bool Initialize(string title, int width, int height, bool vSync, bool fullScreen, int display = 0)
        {
            bool result = false;

            if (Configuration == null)
                Configuration = new DSystemConfiguration(title, width, height, fullScreen, vSync, display);

            // Initialize Window.
            InitializeWindows(title, display);

            if (Input == null)
            {
                Input = new DInput();
                if (!Input.Initialize(Configuration, RenderForm.Handle))
                    return false;
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
            //eventTimer = new Stopwatch();

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
            RenderLoop.Run(RenderForm, () =>
            {
                sw.Restart();
                if (!Frame())
                    ShutDown();
                while (sw.ElapsedTicks < frameTickLimit) // ticks - 2
                {
                    wasteTime = 0;
                }
            });
        }
        public bool Frame()
        {
            // Check if the user pressed escape and wants to exit the application.
            if (!Input.Frame() || Input.IsEscapePressed())
                return false;

            // Update the system stats.
            CPU.Frame();
            FPS.Frame();

            // Performance Logging.
            Timer.Frame2();
            if (true)
            {
                //DPerfLogger.Frame(Timer.FrameTime);
                FrameTimeList.Add(Timer.FrameTime);

                //if (Timer.CumulativeFrameTime >= DPerfLogger.TestTimeInSeconds * 1000)
                  //  return false;
            }

            // Do the frame processing for the graphics object.
            if (!Graphics.Frame(FPS.Value, CPU.Value))
                return false;

            if (Input.PressedKeys != null)
            {
                switch (Input.PressedKeys)
                {
                    case "LeftMouseButton":
                        RGB = 1f;
                        //eventTimer.Start();
                        break;
                    case "D1":  // RGB 0
                        RGB = 0f;
                        break;
                    case "D2":  // RGB 51
                        RGB = 0.2f;
                        break;
                    case "D3":  // RGB 102
                        RGB = 0.4f;
                        break;
                    case "D4":  // RGB 153
                        RGB = 0.6f;
                        break;
                    case "D5":  // RGB 204
                        RGB = 0.8f;
                        break;
                    case "D6":  // RGB 255
                        RGB = 1f;
                        break;
                    case "D7":  // RGB 17
                        RGB = 17f / 255f;
                        break;
                    case "D8":  // RGB 34
                        RGB = 34f / 255f;
                        break;
                    case "D9":  // RGB 68
                        RGB = 68f / 255f;
                        break;
                    case "D0":  // RGB 85 
                        RGB = 85f / 255f;
                        break; 
                    case "Q":  // RGB 119
                        RGB = 119f / 255f;
                        break;
                    case "W":  // RGB 136
                        RGB = 136f / 255f;
                        break;
                    case "E":  // RGB 170
                        RGB = 170f / 255f;
                        break;
                    case "A":  // RGB 187
                        RGB = 187f / 255f;
                        break;
                    case "S":  // RGB 221
                        RGB = 221f / 255f;
                        break;
                    case "D":  // RGB 238
                        RGB = 238f / 255f;
                        break;
                    case "RightMouseButton": // Set to black
                        RGB = 0f;
                        break;
                }
            }
            /*if (eventTimer.IsRunning && eventTimer.ElapsedMilliseconds >= 100)
            {
                eventTimer.Restart();
                RGB = 0f;
            }*/
            // Finally render the graphics to the screen.
            if (!Graphics.Render(RGB))
                return false;

            return true;
        }
        public void ShutDown()
        {
            ShutdownWindows();
            DPerfLogger.ShutDown();
            mainWindow.getTestFPS(FrameTimeList);
            // Release graphics and related objects.
            Graphics?.Shutdown();
            Graphics = null;
            // Release DriectInput related object.
            Input?.Shutdown();
            Input = null;
            Configuration = null;
        }
        private void ShutdownWindows()
        {
            RenderForm?.Dispose();
            RenderForm = null;
        }
    }
}