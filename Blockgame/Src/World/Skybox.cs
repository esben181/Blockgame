using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;


using OpenTK;
using OpenTK.Graphics.OpenGL4;

using Blockgame.Resources;

namespace Blockgame.World
{
    public class Skybox : IDisposable
    {
        int _textureId;
        int _vao;
        VertexBuffer _vbo;
        Shader _shader;

        float[] _skyboxCubeVertices = {
            -1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            -1.0f,  1.0f, -1.0f,
             1.0f,  1.0f, -1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
             1.0f, -1.0f,  1.0f
        };

        bool _disposed = false;

        public Skybox(string[] texturePaths)
        {
            _textureId = LoadCubemap(texturePaths);
            Bind();
            _vbo = new VertexBuffer();
            _vbo.Bind();
            _vao = GL.GenVertexArray();

            GL.BindVertexArray(_vao);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * _skyboxCubeVertices.Length, _skyboxCubeVertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            GL.EnableVertexAttribArray(0);
            _shader = new Shader("Shaders/skybox.vert", "Shaders/skybox.frag");
            _shader.Bind();
            
        }

        private int LoadCubemap(string[] texturePaths)
        {
            int textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, textureId);
            for (int i = 0; i < 6; ++i)
            {
                using (var image = new Bitmap(texturePaths[i]))
                {
                    var data = image.LockBits(
                        new Rectangle(0, 0, image.Width, image.Height),
                        ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D((TextureTarget)(int)(TextureTarget.TextureCubeMapPositiveX + i),
                        0,
                        PixelInternalFormat.Rgb8,
                        image.Width, image.Height, 0,
                        OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                        PixelType.UnsignedByte,
                        data.Scan0);
                }
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            return textureId;
        }

        public void Render(Camera camera)
        {
            GL.DepthFunc(DepthFunction.Lequal);
            _shader.SetMatrix4("u_projection", camera.GetViewToProjectionMatrix());
            _shader.SetMatrix4("u_view", new Matrix4(new Matrix3(camera.GetWorldToViewMatrix())));
            GL.DepthMask(false);
            _shader.Bind();
            GL.BindVertexArray(_vao);
            Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _skyboxCubeVertices.Length / 3);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.TextureCubeMap, _textureId);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _shader.Dispose();
            _vbo.Dispose();
        }

        

    }
}
