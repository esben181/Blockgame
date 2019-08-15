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
        public void AppendQuad(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight, BlockFace face, Block block)
        {
            var textureLayer = BlockRegistry.GetData(block.Kind).Faces[(int)face]+1;
            TextureId.Add(textureLayer);
            TextureId.Add(textureLayer);
            TextureId.Add(textureLayer);
            TextureId.Add(textureLayer);
            TextureId.Add(textureLayer);
            TextureId.Add(textureLayer);


            Positions.Add(topLeft.X * Chunk.BlockSize);
            Positions.Add(topLeft.Y * Chunk.BlockSize);
            Positions.Add(topLeft.Z * Chunk.BlockSize);
            Positions.Add(bottomLeft.X * Chunk.BlockSize);
            Positions.Add(bottomLeft.Y * Chunk.BlockSize);
            Positions.Add(bottomLeft.Z * Chunk.BlockSize);
            Positions.Add(bottomRight.X * Chunk.BlockSize);
            Positions.Add(bottomRight.Y * Chunk.BlockSize);
            Positions.Add(bottomRight.Z * Chunk.BlockSize);
            Positions.Add(topLeft.X * Chunk.BlockSize);
            Positions.Add(topLeft.Y * Chunk.BlockSize);
            Positions.Add(topLeft.Z * Chunk.BlockSize);
            Positions.Add(bottomRight.X * Chunk.BlockSize);
            Positions.Add(bottomRight.Y * Chunk.BlockSize);
            Positions.Add(bottomRight.Z * Chunk.BlockSize);
            Positions.Add(topRight.X * Chunk.BlockSize);
            Positions.Add(topRight.Y * Chunk.BlockSize);
            Positions.Add(topRight.Z * Chunk.BlockSize);

            switch (face)
            {
                case BlockFace.Top:
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
                case BlockFace.Bottom:
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
                case BlockFace.Front:

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
                case BlockFace.Back:
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

                case BlockFace.Right:
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

                case BlockFace.Left:

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
