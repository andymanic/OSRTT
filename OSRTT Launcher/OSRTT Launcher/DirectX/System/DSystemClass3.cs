using OSRTT_Launcher.DirectX.Graphics;
using OSRTT_Launcher.DirectX.Input;
using SharpDX.Windows;
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

        // Statuc Properties
        public static bool IsMouseOffScreen { get; set; }

        // Constructor
        public DSystem() { }

        public static void StartRenderForm(string title, int width, int height, bool vSync, bool fullScreen = true, int testTimeSeconds = 0, int display = 0, double fpsLimit = 1)
        {
            DSystem system = new DSystem();
            system.Initialize(title, width, height, vSync, fullScreen, testTimeSeconds, display);
            system.RunRenderForm(fpsLimit);
        }

        // Methods
        public virtual bool Initialize(string title, int width, int height, bool vSync, bool fullScreen, int testTimeSeconds, int display = 0)
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

            DPerfLogger.Initialize("RenderForm C# SharpDX: " + Configuration.Width + "x" + Configuration.Height + " VSync:" + DSystemConfiguration.VerticalSyncEnabled + " FullScreen:" + DSystemConfiguration.FullScreen + "   " + RenderForm.Text, testTimeSeconds, Configuration.Width, Configuration.Height);;

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
            // float frameTickLimit = (float)fpsLimit * ticksPerMs;
            RenderLoop.Run(RenderForm, () =>
            {
                sw.Restart();
                if (!Frame())
                    ShutDown();
                while (sw.ElapsedTicks < 10002) // ticks - 2
                {
                    wasteTime = 0;
                }
            });
        }
        public bool Frame(float RGB = 0f)
        {
            // Check if the user pressed escape and wants to exit the application.
            if (!Input.Frame() || Input.IsEscapePressed())
                return false;

            // Update the system stats.
            CPU.Frame();
            FPS.Frame();

            // Performance Logging.
            Timer.Frame2();
            if (DPerfLogger.IsTimedTest)
            {
                DPerfLogger.Frame(Graphics.Timer.FrameTime);
                if (Graphics.Timer.CumulativeFrameTime >= DPerfLogger.TestTimeInSeconds * 1000)
                    return false;
            }

            // Do the frame processing for the graphics object.
            if (!Graphics.Frame(FPS.Value, CPU.Value, Timer.FrameTime))
                return false;

            if (Input.PressedKeys != null)
            {
                if (Input.PressedKeys.Contains("G:"))
                {
                    RGB = 0.5f;
                }
                else if (Input.PressedKeys.Contains("LeftMouseButton"))
                {
                    RGB = 0.2f;
                }
            }
            // Finally render the graphics to the screen.
            if (!Graphics.Render(RGB))
                return false;

            return true;
        }
        public void ShutDown()
        {
            ShutdownWindows();
            DPerfLogger.ShutDown();

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