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
        public List<float> TextureLayer { get; } = new List<float>();
        public List<float> Color { get; } = new List<float>();

        int _vao;
        VertexBuffer _vbo;

        bool _disposed = false;

        
        public BlockMesh()
        {
            _vao = GL.GenVertexArray();

            _vbo = new VertexBuffer();
        }

        public void BufferData()
        {
            GL.BindVertexArray(_vao);

            _vbo.Bind();

            int bufferSize = sizeof(float) * (Positions.Count + Normals.Count + TextureLayer.Count + Color.Count);
            GL.BufferData(BufferTarget.ArrayBuffer, bufferSize, new float[bufferSize], BufferUsageHint.StreamDraw);

            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(0), sizeof(float) * Positions.Count, Positions.ToArray());
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * Positions.Count), sizeof(float) * Normals.Count, Normals.ToArray());
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * (Positions.Count + Normals.Count)), sizeof(float) * TextureLayer.Count, TextureLayer.ToArray());
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * (Positions.Count + Normals.Count + TextureLayer.Count)), sizeof(float) * Color.Count, Color.ToArray());

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, sizeof(float) * Positions.Count);
            GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Float, false, sizeof(float) * 1, sizeof(float) * (Positions.Count + Normals.Count));
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, sizeof(float) * (Positions.Count + Normals.Count + TextureLayer.Count));

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.EnableVertexAttribArray(3);


        }

        public void EraseData()
        {
            Positions.Clear();
            Normals.Clear();
            TextureLayer.Clear();
            Color.Clear();
        }

        public void Render()
        {
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, Positions.Count / 3);
        }

        /// <summary>
        /// Prepares the data for a quad to be sent to the GPU.
        /// </summary>
        /// <param name="topLeft">Top-left corner of the quad</param>
        /// <param name="topRight">Top-right corner of the quad</param>
        /// <param name="bottomLeft">Bottom-left corner of the quad</param>
        /// <param name="bottomRight">Bottom-right corner of the quad</param>
        /// <param name="face">Which part of the block the quad belongs to</param>
        /// <param name="block">Block data for the quad</param>
        public void AppendQuad(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight, BlockFace face, Block block)
        {
            var data = BlockRegistry.GetData(block.Kind);
            float textureLayer = data.Faces[(int)face];

            float blockCurrentHP = (int)block.CurrentHP;
            float blockMaxHP = (int)data.Health;
            Color color = data.Color * MathHelper.Clamp(blockCurrentHP / blockMaxHP + 0.3f, 0.3f, 1.0f);
  
            // Texture layer
            TextureLayer.Add(textureLayer);
            TextureLayer.Add(textureLayer);
            TextureLayer.Add(textureLayer);
            TextureLayer.Add(textureLayer);
            TextureLayer.Add(textureLayer);
            TextureLayer.Add(textureLayer);

            // Color
            Color.Add(color.R);
            Color.Add(color.G);
            Color.Add(color.B);

            Color.Add(color.R);
            Color.Add(color.G);
            Color.Add(color.B);

            Color.Add(color.R);
            Color.Add(color.G);
            Color.Add(color.B);

            Color.Add(color.R);
            Color.Add(color.G);
            Color.Add(color.B);

            Color.Add(color.R);
            Color.Add(color.G);
            Color.Add(color.B);

            Color.Add(color.R);
            Color.Add(color.G);
            Color.Add(color.B);

            // Positions
            Positions.Add(topLeft.X);
            Positions.Add(topLeft.Y);
            Positions.Add(topLeft.Z);
            Positions.Add(bottomLeft.X);
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

            // Normals
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
                case BlockFace.Back:

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
                case BlockFace.Front:
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
