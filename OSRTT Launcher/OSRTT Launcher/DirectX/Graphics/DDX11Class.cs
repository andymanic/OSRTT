using OSRTT_Launcher.DirectX.System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;

namespace OSRTT_Launcher.DirectX.Graphics
{
    public class DDX11                  // 384 lines
    {
        // Properties.
        private bool VerticalSyncEnabled { get; set; }
        public long VideoCardMemory { get; private set; }
        public string VideoCardDescription { get; private set; }
        private SwapChain SwapChain { get; set; }
        public SharpDX.Direct3D11.Device Device { get; private set; }
        public DeviceContext DeviceContext { get; private set; }
        private RenderTargetView RenderTargetView { get; set; }
        private Texture2D DepthStencilBuffer { get; set; }
        public DepthStencilState DepthStencilState { get; private set; }
        private DepthStencilView DepthStencilView { get; set; }
        private RasterizerState RasterState { get; set; }
        public Matrix ProjectionMatrix { get; private set; }
        public Matrix WorldMatrix { get; private set; }
        public Matrix OrthoMatrix { get; private set; }
        public DepthStencilState DepthDisabledStencilState { get; private set; }
        public BlendState AlphaEnableBlendingState { get; private set; }
        public BlendState AlphaDisableBlendingState { get; private set; }

        // Constructor
        public DDX11() { }

        public bool Initialize(DSystemConfiguration configuration, IntPtr windowHandle)
        {
            try
            {
                #region Environment Configuration
                // Store the vsync setting.
                VerticalSyncEnabled = DSystemConfiguration.VerticalSyncEnabled;

                // Create a DirectX graphics interface factory.
                var factory = new Factory1();

                // Use the factory to create an adapter for the primary graphics interface (video card).
                var adapter = factory.GetAdapter1(0);

                // Get the primary adapter output (monitor).
                var monitor = adapter.GetOutput(0);

                // Get modes that fit the DXGI_FORMAT_R8G8B8A8_UNORM display format for the adapter output (monitor).
                var modes = monitor.GetDisplayModeList(Format.R8G8B8A8_UNorm, DisplayModeEnumerationFlags.Interlaced);

                // Now go through all the display modes and find the one that matches the screen width and height.
                // When a match is found store the the refresh rate for that monitor, if vertical sync is enabled. 
                // Otherwise we use maximum refresh rate.
                var rational = new Rational(0, 1);
                if (VerticalSyncEnabled)
                {
                    foreach (var mode in modes)
                    {
                        if (mode.Width == configuration.Width && mode.Height == configuration.Height)
                        {
                            rational = new Rational(mode.RefreshRate.Numerator, mode.RefreshRate.Denominator);
                            break;
                        }
                    }
                }

                // Get the adapter (video card) description.
                var adapterDescription = adapter.Description;

                // Store the dedicated video card memory in megabytes.
                long VRAM = adapterDescription.DedicatedVideoMemory;
                VideoCardMemory = VRAM >> 10 >> 10;

                // Convert the name of the video card to a character array and store it.
                VideoCardDescription = adapterDescription.Description.Trim('\0');

                // Release the adapter output.
                monitor.Dispose();
                // Release the adapter.
                adapter.Dispose();
                // Release the factory.
                factory.Dispose();
                #endregion

                #region Initialize swap chain and d3d device
                // Initialize the swap chain description.
                var swapChainDesc = new SwapChainDescription()
                {
                    // Set to a single back buffer.
                    BufferCount = 1,
                    // Set the width and height of the back buffer.
                    ModeDescription = new ModeDescription(configuration.Width, configuration.Height, rational, Format.R8G8B8A8_UNorm),
                    // Set the usage of the back buffer.
                    Usage = Usage.RenderTargetOutput,
                    // Set the handle for the window to render to.
                    OutputHandle = windowHandle,
                    // Turn multisampling off.
                    SampleDescription = new SampleDescription(1, 0),
                    // Set to full screen or windowed mode.
                    IsWindowed = !DSystemConfiguration.FullScreen,
                    // Don't set the advanced flags.
                    Flags = SwapChainFlags.None,
                    // Discard the back buffer content after presenting.
                    SwapEffect = SwapEffect.Discard
                };

                // Create the swap chain, Direct3D device, and Direct3D device context.
                SharpDX.Direct3D11.Device device;
                SwapChain swapChain;
                SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapChainDesc, out device, out swapChain);

                Device = device;
                SwapChain = swapChain;
                DeviceContext = device.ImmediateContext;
                #endregion

                #region Initialize buffers
                // Get the pointer to the back buffer.
                var backBuffer = Texture2D.FromSwapChain<Texture2D>(SwapChain, 0);

                // Create the render target view with the back buffer pointer.
                RenderTargetView = new RenderTargetView(device, backBuffer);

                // Release pointer to the back buffer as we no longer need it.
                backBuffer.Dispose();

                // Initialize and set up the description of the depth buffer.
                var depthBufferDesc = new Texture2DDescription()
                {
                    Width = configuration.Width,
                    Height = configuration.Height,
                    MipLevels = 1,
                    ArraySize = 1,
                    Format = Format.D24_UNorm_S8_UInt,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Default,
                    BindFlags = BindFlags.DepthStencil,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                };

                // Create the texture for the depth buffer using the filled out description.
                DepthStencilBuffer = new Texture2D(device, depthBufferDesc);
                #endregion

                #region Initialize Depth Enabled Stencil
                // Initialize and set up the description of the stencil state.
                var depthStencilDesc = new DepthStencilStateDescription()
                {
                    IsDepthEnabled = true,
                    DepthWriteMask = DepthWriteMask.All,
                    DepthComparison = Comparison.Less,
                    IsStencilEnabled = true,
                    StencilReadMask = 0xFF,
                    StencilWriteMask = 0xFF,
                    // Stencil operation if pixel front-facing.
                    FrontFace = new DepthStencilOperationDescription()
                    {
                        FailOperation = StencilOperation.Keep,
                        DepthFailOperation = StencilOperation.Increment,
                        PassOperation = StencilOperation.Keep,
                        Comparison = Comparison.Always
                    },
                    // Stencil operation if pixel is back-facing.
                    BackFace = new DepthStencilOperationDescription()
                    {
                        FailOperation = StencilOperation.Keep,
                        DepthFailOperation = StencilOperation.Decrement,
                        PassOperation = StencilOperation.Keep,
                        Comparison = Comparison.Always
                    }
                };

                // Create the depth stencil state.
                DepthStencilState = new DepthStencilState(Device, depthStencilDesc);
                #endregion

                #region Initialize Output Merger
                // Set the depth stencil state.
                DeviceContext.OutputMerger.SetDepthStencilState(DepthStencilState, 1);

                // Initialize and set up the depth stencil view.
                var depthStencilViewDesc = new DepthStencilViewDescription()
                {
                    Format = Format.D24_UNorm_S8_UInt,
                    Dimension = DepthStencilViewDimension.Texture2D,
                    Texture2D = new DepthStencilViewDescription.Texture2DResource()
                    {
                        MipSlice = 0
                    }
                };

                // Create the depth stencil view.
                DepthStencilView = new DepthStencilView(Device, DepthStencilBuffer, depthStencilViewDesc);

                // Bind the render target view and depth stencil buffer to the output render pipeline.
                DeviceContext.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);
                #endregion

                #region Initialize Raster State
                // Setup the raster description which will determine how and what polygon will be drawn.
                var rasterDesc = new RasterizerStateDescription()
                {
                    IsAntialiasedLineEnabled = false,
                    CullMode = CullMode.Back,
                    DepthBias = 0,
                    DepthBiasClamp = .0f,
                    IsDepthClipEnabled = true,
                    FillMode = FillMode.Solid,
                    IsFrontCounterClockwise = false,
                    IsMultisampleEnabled = false,
                    IsScissorEnabled = false,
                    SlopeScaledDepthBias = .0f
                };

                // Create the rasterizer state from the description we just filled out.
                RasterState = new RasterizerState(Device, rasterDesc);
                #endregion

                #region Initialize Rasterizer
                // Now set the rasterizer state.
                DeviceContext.Rasterizer.State = RasterState;

                // Setup and create the viewport for rendering.
                DeviceContext.Rasterizer.SetViewport(0, 0, configuration.Width, configuration.Height, 0, 1);
                #endregion

                #region Initialize matrices
                // Setup and create the projection matrix.
                ProjectionMatrix = Matrix.PerspectiveFovLH((float)(Math.PI / 4), ((float)configuration.Width / (float)configuration.Height), DSystemConfiguration.ScreenNear, DSystemConfiguration.ScreenDepth);

                // Initialize the world matrix to the identity matrix.
                WorldMatrix = Matrix.Identity;

                // Create an orthographic projection matrix for 2D rendering.
                OrthoMatrix = Matrix.OrthoLH(configuration.Width, configuration.Height, DSystemConfiguration.ScreenNear, DSystemConfiguration.ScreenDepth);
                #endregion

                #region Initialize Depth Disabled Stencil
                // Now create a second depth stencil state which turns off the Z buffer for 2D rendering.
                // The difference is that DepthEnable is set to false.
                // All other parameters are the same as the other depth stencil state.
                var depthDisabledStencilDesc = new DepthStencilStateDescription()
                {
                    IsDepthEnabled = false,
                    DepthWriteMask = DepthWriteMask.All,
                    DepthComparison = Comparison.Less,
                    IsStencilEnabled = true,
                    StencilReadMask = 0xFF,
                    StencilWriteMask = 0xFF,
                    // Stencil operation if pixel front-facing.
                    FrontFace = new DepthStencilOperationDescription()
                    {
                        FailOperation = StencilOperation.Keep,
                        DepthFailOperation = StencilOperation.Increment,
                        PassOperation = StencilOperation.Keep,
                        Comparison = Comparison.Always
                    },
                    // Stencil operation if pixel is back-facing.
                    BackFace = new DepthStencilOperationDescription()
                    {
                        FailOperation = StencilOperation.Keep,
                        DepthFailOperation = StencilOperation.Decrement,
                        PassOperation = StencilOperation.Keep,
                        Comparison = Comparison.Always
                    }
                };

                // Create the depth stencil state.
                DepthDisabledStencilState = new DepthStencilState(Device, depthDisabledStencilDesc);
                #endregion

                #region Initialize Blend States
                // Create an alpha enabled blend state description.
                var blendStateDesc = new BlendStateDescription();
                blendStateDesc.RenderTarget[0].IsBlendEnabled = true;
                blendStateDesc.RenderTarget[0].SourceBlend = BlendOption.One;
                blendStateDesc.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
                blendStateDesc.RenderTarget[0].BlendOperation = BlendOperation.Add;
                blendStateDesc.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
                blendStateDesc.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
                blendStateDesc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
                blendStateDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

                // Create the blend state using the description.
                AlphaEnableBlendingState = new BlendState(device, blendStateDesc);

                // Modify the description to create an disabled blend state description.
                blendStateDesc.RenderTarget[0].IsBlendEnabled = false;
                
                // Create the blend state using the description.
                AlphaDisableBlendingState = new BlendState(device, blendStateDesc);
                #endregion

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void ShutDown()
        {
            // Before shutting down set to windowed mode or when you release the swap chain it will throw an exception.
            SwapChain?.SetFullscreenState(false, null);

            // Dispose of all objects.
            AlphaEnableBlendingState?.Dispose();
            AlphaEnableBlendingState = null;
            AlphaDisableBlendingState?.Dispose();
            AlphaDisableBlendingState = null;
            DepthDisabledStencilState?.Dispose();
            DepthDisabledStencilState = null;
            RasterState?.Dispose();
            RasterState = null;
            DepthStencilView?.Dispose();
            DepthStencilView = null;
            DepthStencilState?.Dispose();
            DepthStencilState = null;
            DepthStencilBuffer?.Dispose();
            DepthStencilBuffer = null;
            RenderTargetView?.Dispose();
            RenderTargetView = null;
            DeviceContext?.Dispose();
            DeviceContext = null;
            Device?.Dispose();
            Device = null;
            SwapChain?.Dispose();
            SwapChain = null;
        }
        public void TurnOnAlphaBlending()
        {
            // Setup the blend factor.
            var blendFactor = new Color4(0, 0, 0, 0);

            // Turn on the alpha blending.
            DeviceContext.OutputMerger.SetBlendState(AlphaEnableBlendingState, blendFactor, -1);
        }
        public void TurnOffAlphaBlending()
        {
            // Setup the blend factor.
            var blendFactor = new Color4(0, 0, 0, 0);

            // Turn on the alpha blending.
            DeviceContext.OutputMerger.SetBlendState(AlphaDisableBlendingState, blendFactor, -1);
        }
        public void BeginScene(float red, float green, float blue, float alpha)
        {
            BeginScene(new Color4(red, green, blue, alpha));
        }
        public void BeginScene(Color4 color)
        {
            // Clear the depth buffer.
            DeviceContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1, 0);

            // Clear the back buffer.
            DeviceContext.ClearRenderTargetView(RenderTargetView, color);
        }
        public void EndScene()
        {
            // Present the back buffer to the screen since rendering is complete.
            if (VerticalSyncEnabled)
            {
                // Lock to screen refresh rate.
                SwapChain.Present(1, PresentFlags.None);
            }
            else
            {
                // Present as fast as possible.
                SwapChain.Present(0, PresentFlags.None);
            }
        }
        public void TurnZBufferOn()
        {
            DeviceContext.OutputMerger.SetDepthStencilState(DepthStencilState, 1);
        }
        public void TurnZBufferOff()
        {
            DeviceContext.OutputMerger.SetDepthStencilState(DepthDisabledStencilState, 1);
        }
    }
}