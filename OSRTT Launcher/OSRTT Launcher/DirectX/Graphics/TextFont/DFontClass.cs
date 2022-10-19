using OSRTT_Launcher.DirectX.System;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace OSRTT_Launcher.DirectX.Graphics.TextFont
{
    public class DFont                  // 179 lines
    {
        // Structs
        [StructLayout(LayoutKind.Sequential)]
        public struct DCharacter
        {
            // Variables.
            public float left, right;
            public int size;

            // Constructor
            public DCharacter(string fontData)
            {
                var data = fontData.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                left = float.Parse(data[data.Length - 3]);
                right = float.Parse(data[data.Length - 2]);
                size = int.Parse(data[data.Length - 1]);
            }
        }

        // Properties.
        public List<DCharacter> FontCharacters { get; private set; }
        public DTexture Texture { get; private set; }

        // Methods
        public bool Initialize(Device device, string fontFileName, string textureFileName)
        {
            // Load in the text file containing the font data.
            if (!LoadFontData(fontFileName))
                return false;
         
            // Load the texture that has font characters on it.
            if (!LoadTexture(device, textureFileName))
                return false;
         
            return true;
        }
        public void Shutdown()
        {
            // Release the font texture.
            ReleaseTexture();

            // Release the font data.
            ReleaseFontData();
        }
        private bool LoadFontData(string fontFileName)
        {
            try
            {
                fontFileName = DSystemConfiguration.FontFilePath + fontFileName;

                // Get all the lines containing the font data.
                var fontDataLines = File.ReadAllLines(fontFileName);

                // Create Font and fill with characters.
                FontCharacters = new List<DCharacter>();
                foreach (var line in fontDataLines)
                    FontCharacters.Add(new DCharacter(line));

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return false;
            }
        }
        private void ReleaseFontData()
        {
            // Release the font data array.
            FontCharacters?.Clear();
            FontCharacters = null;
        }
        private bool LoadTexture(Device device, string textureFileName)
        {
            textureFileName = DSystemConfiguration.FontFilePath + textureFileName;

            // Create new texture object.
            Texture = new DTexture();

            // Initialize the texture object.
            if (!Texture.Initialize(device, textureFileName))
                return false;

            return true;
        }
        private void ReleaseTexture()
        {
            // Release the texture object.
            Texture?.ShutDown();
            Texture = null;
        }
        public void BuildVertexArray(out List<DTextClass.DVertex> vertices, string sentence, float drawX, float drawY)
        {
            // Create list of the vertices
            vertices = new List<DTextClass.DVertex>();

            // Draw each letter onto a quad.
            foreach (char ch in sentence)
            {
                var letter = ch - 32;

                // If the letter is a space then just move over three pixel.
                if (letter == 0)
                    drawX += 3;
                else
                {
                    // Add quad vertices for the character.
                    BuildVertexArray(vertices, letter, ref drawX, ref drawY);

                    // Update the x location for drawing be the size of the letter and one pixel.
                    drawX += FontCharacters[letter].size + 1;
                }
            }
        }
        private void BuildVertexArray(List<DTextClass.DVertex> vertices, int letter, ref float drawX, ref float drawY)
        {
            // First triangle in the quad
            vertices.Add // Top left.
            (
                new DTextClass.DVertex()
                {
                    position = new Vector3(drawX, drawY, 0),
                    texture = new Vector2(FontCharacters[letter].left, 0)
                }
            );
            vertices.Add // Bottom right.
            (
                new DTextClass.DVertex()
                {
                    position = new Vector3(drawX + FontCharacters[letter].size, drawY - 16, 0),
                    texture = new Vector2(FontCharacters[letter].right, 1)
                }
            );
            vertices.Add // Bottom left.
            (
                new DTextClass.DVertex()
                {
                    position = new Vector3(drawX, drawY - 16, 0),
                    texture = new Vector2(FontCharacters[letter].left, 1)
                }
            );
            // Second triangle in quad.
            vertices.Add // Top left.
            (
                new DTextClass.DVertex()
                {
                    position = new Vector3(drawX, drawY, 0),
                    texture = new Vector2(FontCharacters[letter].left, 0)
                }
            );
            vertices.Add // Top right.
            (
                new DTextClass.DVertex()
                {
                    position = new Vector3(drawX + FontCharacters[letter].size, drawY, 0),
                    texture = new Vector2(FontCharacters[letter].right, 0)
                }
            );
            vertices.Add // Bottom right.
            (
                new DTextClass.DVertex()
                {
                    position = new Vector3(drawX + FontCharacters[letter].size, drawY - 16, 0),
                    texture = new Vector2(FontCharacters[letter].right, 1)
                }
            );
        }
    }
}