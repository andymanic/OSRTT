using OSRTT_Launcher.DirectX.Graphics.TextFont;
using OSRTT_Launcher.DirectX.System;
using System;
using System.Windows.Forms;

namespace OSRTT_Launcher.DirectX.Graphics
{
    public class DGraphics                  // 120 lines
    {
        // Properties
        private DDX11 D3D { get; set; }
        private DCamera Camera { get; set; }
        public DTextClass Text { get; set; }
        public DTimer Timer { get; set; }

        // Construtor
        public DGraphics() { }

        // Methods.
        public bool Initialize(DSystemConfiguration configuration, IntPtr windowHandle)
        {
            try
            {
                // Create the Direct3D object.
                D3D = new DDX11();
                
                // Initialize the Direct3D object.
                if (!D3D.Initialize(configuration, windowHandle))
                    return false;

                // Create the Timer
                Timer = new DTimer();

                // Initialize the Timer
                if (!Timer.Initialize())
                    return false;

                // Create the camera object
                Camera = new DCamera();

                // Initialize a base view matrix the camera for 2D user interface rendering.
                Camera.SetPosition(0, 0, -1);
                Camera.Render();
                var baseViewMatrix = Camera.ViewMatrix;

                // Create the text object.
                Text = new DTextClass();
                if (!Text.Initialize(D3D.Device, D3D.DeviceContext, windowHandle, configuration.Width, configuration.Height, baseViewMatrix))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not initialize Direct3D\nError is '" + ex.Message + "'");
                return false;
            }
        }
        public void Shutdown()
        {
            Timer = null;
            Camera = null;

            // Release the text object.
            Text?.Shutdown();
            Text = null;
            // Release the Direct3D object.
            D3D?.ShutDown();
            D3D = null;
        }
        public bool Frame(int fps = 0, int cpu = 0)
        {
            
            // Set the frames per second.
            if (!Text.SetFps(D3D.DeviceContext, fps))
                return false;
            
            // Set the cpu usage.
            if (!Text.SetCpu(D3D.DeviceContext, cpu))
                return false;
            
            // Set the position of the camera.
            Camera.SetPosition(0, 0, -10f);

            return true;
        }
        public bool Render(float RGB = 0.0f)
        {
            // Clear the buffer to begin the scene.
            D3D.BeginScene(RGB, RGB, RGB, 1f);

            // Generate the view matrix based on the camera position.
            Camera.Render();

            // Get the world, view, and projection matrices from camera and d3d objects.
            var viewMatrix = Camera.ViewMatrix;
            var worldMatrix = D3D.WorldMatrix;
            var projectionMatrix = D3D.ProjectionMatrix;
            var orthoMatrix = D3D.OrthoMatrix;

            // Turn off the Z buffer to begin all 2D rendering.
            D3D.TurnZBufferOff();

            // Turn on the alpha blending before rendering the text.
            D3D.TurnOnAlphaBlending();

            // Render the text string.
            if (!Text.Render(D3D.DeviceContext, worldMatrix, orthoMatrix))
                return false;

            // Turn off the alpha blending before rendering the text.
            D3D.TurnOffAlphaBlending();

            // Turn on the Z buffer to begin all 2D rendering.
            D3D.TurnZBufferOn();

            // Present the rendered scene to the screen.
            D3D.EndScene();

            return true;
        }
    }
}