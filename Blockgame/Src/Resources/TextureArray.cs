using System;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics.OpenGL4;


namespace Blockgame.Resources
{
    public class TextureArray : IDisposable
    {
        private int _textureId;

        private bool _disposed = false;

        // Create texture from path.
        public TextureArray(string path, int width, int height, int layerCount)
        {

            _textureId = GL.GenTexture();

            Bind();

            // Load the image.
            using (var image = new Bitmap(path))
            {
                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgb8, width, height, layerCount, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);


            }

            // Now set texture parameters.
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2DArray);
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2DArray, _textureId);
        }

        ~TextureArray()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            GL.DeleteTexture(_textureId);

            _disposed = true;
        }
    }
}