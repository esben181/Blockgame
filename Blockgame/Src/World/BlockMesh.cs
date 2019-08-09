using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

using Blockgame.Resources;


namespace Blockgame.World
{

    public class BlockMesh : IDisposable
    {
        public List<float> Positions { get; } = new List<float>();
        public List<float> Normals { get; } = new List<float>();
        public List<float> TextureId { get; } = new List<float>();

        int _vao;
        VertexBuffer _vbo;

        bool _disposed = false;

        public enum Face
        {
            None = 0,
            Top,
            Bottom,
            Right,
            Left,
            Back,
            Front
        }
        public BlockMesh()
        {
            _vao = GL.GenVertexArray();

            _vbo = new VertexBuffer();
        }

        public void Buffer()
        {
            GL.BindVertexArray(_vao);

            _vbo.Bind();

            int bufferSize = sizeof(float) * (Positions.Count + Normals.Count + TextureId.Count);
            GL.BufferData(BufferTarget.ArrayBuffer, bufferSize, new float[bufferSize], BufferUsageHint.StaticDraw);

            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(0), sizeof(float) * Positions.Count, Positions.ToArray());
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * Positions.Count), sizeof(float) * Normals.Count, Normals.ToArray());
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * (Positions.Count + Normals.Count)), sizeof(float) * TextureId.Count, TextureId.ToArray());

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, sizeof(float) * Positions.Count);
            GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Float, false, sizeof(float) * 1, sizeof(float) * (Positions.Count + Normals.Count));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);


        }

        public void Clear()
        {
            Positions.Clear();
            Normals.Clear();
            TextureId.Clear();
        }

        public void Draw()
        {
            GL.BindVertexArray(_vao);
            _vbo.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, Positions.Count / 3);
        }

        // AppendQuad(top-left, top-right, bottom-left, bottom-right);
        public void AppendQuad(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight, Face face, BlockMaterial material)
        {
            TextureId.Add((int)material);
            TextureId.Add((int)material);
            TextureId.Add((int)material);
            TextureId.Add((int)material);
            TextureId.Add((int)material);
            TextureId.Add((int)material);

            Positions.Add(topLeft.X);
            Positions.Add(topLeft.Y);
            Positions.Add(topLeft.Z);
            Positions.Add(bottomLeft.X );
            Positions.Add(bottomLeft.Y );
            Positions.Add(bottomLeft.Z );
            Positions.Add(bottomRight.X );
            Positions.Add(bottomRight.Y );
            Positions.Add(bottomRight.Z );
            Positions.Add(topLeft.X );
            Positions.Add(topLeft.Y );
            Positions.Add(topLeft.Z );
            Positions.Add(bottomRight.X );
            Positions.Add(bottomRight.Y );
            Positions.Add(bottomRight.Z );
            Positions.Add(topRight.X );
            Positions.Add(topRight.Y );
            Positions.Add(topRight.Z );

            switch (face)
            {
                case Face.Top:
                    Normals.Add(0.0f);
                    Normals.Add(1.0f);
                    Normals.Add(0.0f);

                    Normals.Add(0.0f);
                    Normals.Add(1.0f);
                    Normals.Add(0.0f);

                    Normals.Add(0.0f);
                    Normals.Add(1.0f);
                    Normals.Add(0.0f);

                    Normals.Add(0.0f);
                    Normals.Add(1.0f);
                    Normals.Add(0.0f);

                    Normals.Add(0.0f);
                    Normals.Add(1.0f);
                    Normals.Add(0.0f);

                    Normals.Add(0.0f);
                    Normals.Add(1.0f);
                    Normals.Add(0.0f);

                    break;
                case Face.Bottom:
                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);

                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);

                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);

                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);

                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);

                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);

                    break;
                case Face.Front:

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(-1.0f);

                    break;
                case Face.Back:
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(1.0f);

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(1.0f);

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(1.0f);

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(1.0f);

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(1.0f);

                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    Normals.Add(1.0f);
                    break;

                case Face.Right:
                    Normals.Add(1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    Normals.Add(1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    Normals.Add(1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    Normals.Add(1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    Normals.Add(1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    Normals.Add(1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);
                    break;

                case Face.Left:

                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    Normals.Add(-1.0f);
                    Normals.Add(0.0f);
                    Normals.Add(0.0f);

                    break;
                 
            }

        }

        ~BlockMesh()
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

            _vbo.Dispose();
            GL.DeleteVertexArray(_vao);
            _disposed = true;
        }

    }
}
