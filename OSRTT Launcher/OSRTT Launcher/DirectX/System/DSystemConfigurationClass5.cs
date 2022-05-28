using System.Windows.Forms;

namespace OSRTT_Launcher.DirectX.System
{
    public class DSystemConfiguration                   // 52 lines
    {
        // Properties
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // Static Variables.
        public static bool FullScreen { get; private set; }
        public static bool VerticalSyncEnabled { get; private set; }
        public static float ScreenDepth { get; private set; }
        public static float ScreenNear { get; private set; }
        public static FormBorderStyle BorderStyle { get; private set; }
        public static string FontFilePath { get; private set; }

        // Constructors
        public DSystemConfiguration(bool fullScreen, bool vSync) : this("SharpDX Demo", fullScreen, vSync) { }
        public DSystemConfiguration(string title, bool fullScreen, bool vSync) : this(title, 800, 600, fullScreen, vSync) { }
        public DSystemConfiguration(string title, int width, int height, bool fullScreen, bool vSync, int display = 0)
        {
            FullScreen = fullScreen;
            Title = title;
            VerticalSyncEnabled = vSync;

            if (!FullScreen)
            {
                Width = width;
                Height = height;
            }
            else
            {
                Screen[] screens = Screen.AllScreens;
                Width = screens[display].Bounds.Width;
                Height = screens[display].Bounds.Height;
            }
        }

        // Static Constructor
        static DSystemConfiguration()
        {
            FullScreen = false;
            VerticalSyncEnabled = false;
            ScreenDepth = 1000.0f;
            ScreenNear = 0.1f;
            BorderStyle = FormBorderStyle.None;

            FontFilePath = @"DirectX\Font\";
        }
    }
}