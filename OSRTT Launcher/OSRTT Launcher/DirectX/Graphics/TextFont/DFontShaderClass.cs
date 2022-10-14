using OSRTT_Launcher.DirectX.System;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OSRTT_Launcher.DirectX.Graphics.TextFont
{
    public class DFontShader                    // 275 lines
    {
        // Structs
        [StructLayout(LayoutKind.Sequential)]
        internal struct DMatrixBuffer
        {
            public Matrix world;
            public Matrix view;
            public Matrix projection;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct DPixelBuffer
        {
            public Vector4 pixelColor;
        }

        // Properties
        public VertexShader VertexShader { get; set; }
        public PixelShader PixelShader { get; set; }
        public InputLayout Layout { get; set; }
        public SharpDX.Direct3D11.Buffer ConstantMatrixBuffer { get; set; }
        public SamplerState SamplerState { get; set; }
        public SharpDX.Direct3D11.Buffer ConstantPixelBuffer { get; set; }
  
        // Constructor
        public DFontShader() { }

        // Methods
        public bool Initialize(SharpDX.Direct3D11.Device device, IntPtr windowHandler)
        {
            // Initialize the vertex and pixel shaders.
            return InitializeShader(device, windowHandler, "font.vs", "font.ps");
        }
        public void Shuddown()
        {
            // Shutdown the vertex and pixel shaders as well as the related objects.
            ShuddownShader();
        }
        private void ShuddownShader()
        {
            // Release the pixel constant buffer.
            ConstantPixelBuffer?.Dispose();
            ConstantPixelBuffer = null;
            // Release the sampler state.
            SamplerState?.Dispose();
            SamplerState = null;
            // Release the matrix constant buffer.
            ConstantMatrixBuffer?.Dispose();
            ConstantMatrixBuffer = null;
            // Release the layout.
            Layout?.Dispose();
            Layout = null;
            // Release the pixel shader.
            PixelShader?.Dispose();
            PixelShader = null;
            // Release the vertex shader.
            VertexShader?.Dispose();
            VertexShader = null;
        }
        public bool Render(DeviceContext deviceContext, int indexCount, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, ShaderResourceView texture, Vector4 pixelColor)
        {
            // Set the shader parameters that it will use for rendering.
            if (!SetShaderParameters(deviceContext, worldMatrix, viewMatrix, projectionMatrix, texture, pixelColor))
                return false;

            // Now render the prepared buffers with the shader.
            RenderShader(deviceContext, indexCount);

            return true;
        }
        private bool InitializeShader(SharpDX.Direct3D11.Device device, IntPtr windowHandler, string vsFileName, string psFileName)
        {
            try
            {
                #region Input Layout Configuration
                // Setup full paths
                vsFileName = DSystemConfiguration.FontFilePath + vsFileName;
                psFileName = DSystemConfiguration.FontFilePath + psFileName;

                // Compile the vertex shader code.
                var vertexShaderByteCode = ShaderBytecode.CompileFromFile(vsFileName, "FontVertexShader", "vs_4_0", ShaderFlags.EnableStrictness, EffectFlags.None);
                // Compile the pixel shader code.
                var pixelShaderByteCode = ShaderBytecode.CompileFromFile(psFileName, "FontPixelShader", "ps_4_0", ShaderFlags.EnableStrictness, EffectFlags.None);

                // Create the vertex shader from the buffer.
                VertexShader = new VertexShader(device, vertexShaderByteCode);
                // Create the pixel shader from the buffer.
                PixelShader = new PixelShader(device, pixelShaderByteCode);

                // Now setup the layout of the data that goes into the shader.
                // This setup needs to match the VertexType structure in the Model and in the shader.
                var inputElements = new InputElement[]
				{
					new InputElement()
					{
						SemanticName = "POSITION",
						SemanticIndex = 0,
						Format = Format.R32G32B32_Float,
						Slot = 0,
						AlignedByteOffset = 0,
						Classification = InputClassification.PerVertexData,
						InstanceDataStepRate = 0
					},
					new InputElement()
					{
						SemanticName = "TEXCOORD",
						SemanticIndex = 0,
						Format = Format.R32G32_Float,
						Slot = 0,
						AlignedByteOffset = InputElement.AppendAligned,
						Classification = InputClassification.PerVertexData,
						InstanceDataStepRate = 0
					}
				};

                // Create the vertex input the layout.
                Layout = new InputLayout(device, ShaderSignature.GetInputSignature(vertexShaderByteCode), inputElements);

                // Release the vertex and pixel shader buffers, since they are no longer needed.
                vertexShaderByteCode.Dispose();
                pixelShaderByteCode.Dispose();
                #endregion

                #region Matrix Constant Buffer
                // Setup the description of the dynamic matrix constant buffer that is in the vertex shader.
                var matrixBufferDesc = new BufferDescription()
                {
                    Usage = ResourceUsage.Dynamic,
                    SizeInBytes = Utilities.SizeOf<DMatrixBuffer>(),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };

                // Create the constant buffer pointer so we can access the vertex shader constant buffer from within this class.
                ConstantMatrixBuffer = new SharpDX.Direct3D11.Buffer(device, matrixBufferDesc);
                #endregion

                #region Sampler State
                // Create a texture sampler state description.
                var samplerDesc = new SamplerStateDescription()
                {
                    Filter = Filter.MinMagMipLinear,
                    AddressU = TextureAddressMode.Wrap,
                    AddressV = TextureAddressMode.Wrap,
                    AddressW = TextureAddressMode.Wrap,
                    MipLodBias = 0,
                    MaximumAnisotropy = 1,
                    ComparisonFunction = Comparison.Always,
                    BorderColor = new Color4(0, 0, 0, 0),
                    MinimumLod = 0,
                    MaximumLod =  float.MaxValue
                };

                // Create the texture sampler state.
                SamplerState = new SamplerState(device, samplerDesc);
                #endregion

                #region Pixel Constant Buffer
                // Setup the description of the dynamic pixel constant buffer that is in the pixel shader.
                var pixelBufferDesc = new BufferDescription()
                {
                    Usage = ResourceUsage.Dynamic,
                    SizeInBytes = Utilities.SizeOf<DPixelBuffer>(),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };

                // Create the constant buffer pointer so we can access the vertex shader constant buffer from within this class.
                ConstantPixelBuffer = new SharpDX.Direct3D11.Buffer(device, pixelBufferDesc);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing shader. Error is " + ex.Message);
                return false;
            };
        }
        private bool SetShaderParameters(DeviceContext deviceContext, Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix, ShaderResourceView texture, Vector4 pixelColor)
        {
            try
            {
                DataStream mappedResource;

                #region Matrix Constant Buffer
                // Transpose the matrices to prepare them for shader.
                worldMatrix.Transpose();
                viewMatrix.Transpose();
                projectionMatrix.Transpose();

                // Lock the constant buffer so it can be written to.
                deviceContext.MapSubresource(ConstantMatrixBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out mappedResource);

                // Copy the matrices into the constant buffer.
                var matrixBuffer = new DMatrixBuffer()
                {
                    world = worldMatrix,
                    view = viewMatrix,
                    projection = projectionMatrix
                };
                mappedResource.Write(matrixBuffer);

                // Unlock the constant buffer.
                deviceContext.UnmapSubresource(ConstantMatrixBuffer, 0);

                // Set the position of the constant buffer in the vertex shader.
                var bufferNumber = 0;

                //  Now set the constant buffer in the vertex shader with the updated values.
                deviceContext.VertexShader.SetConstantBuffer(bufferNumber, ConstantMatrixBuffer);

                // Set shader resource in the pixel shader.
                deviceContext.PixelShader.SetShaderResource(0, texture);
                #endregion

                #region Pixel Constant Shader
                // Lock the pixel constant buffer so it can be written to.
                deviceContext.MapSubresource(ConstantPixelBuffer, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out mappedResource);

                // Copy the matrices into the constant buffer.
                var pixelBuffer = new DPixelBuffer()
                {
                    pixelColor = pixelColor
                };
                mappedResource.Write(pixelBuffer);

                // Unlock the constant buffer.
                deviceContext.UnmapSubresource(ConstantPixelBuffer, 0);

                // Set the position of the constant buffer in the vertex shader.
                bufferNumber = 0;

                // Finally set the constant buffer in the vertex shader with the updated values.
                deviceContext.PixelShader.SetConstantBuffer(bufferNumber, ConstantPixelBuffer);
                #endregion

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void RenderShader(DeviceContext deviceContext, int indexCount)
        {
            // Set the vertex input layout.
            deviceContext.InputAssembler.InputLayout = Layout;

            // Set the vertex and pixel shaders that will be used to render this triangle.
            deviceContext.VertexShader.Set(VertexShader);
            deviceContext.PixelShader.Set(PixelShader);

            // Set the sampler state in the pixel shader.
            deviceContext.PixelShader.SetSampler(0, SamplerState);
            // Render the triangle.
            deviceContext.DrawIndexed(indexCount, 0, 0);
        }
    }
}