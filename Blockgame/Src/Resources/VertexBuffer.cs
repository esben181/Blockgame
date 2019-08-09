using System;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Blockgame.Resources
{

    public class VertexBuffer : IDisposable
    {
        private int _vboId;
        private bool _disposed = false;
        public VertexBuffer()
        {
            _vboId = GL.GenBuffer();
            Bind();

        }

        public void Bind() 
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboId);
        }

        public void Clear()
        {
        }

        ~VertexBuffer()
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

            GL.DeleteBuffer(_vboId);
        }
    }
}
