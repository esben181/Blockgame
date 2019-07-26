using System;


using OpenTK;
using OpenTK.Graphics.OpenGL4;

using Blockgame.Resources;
using Blockgame.Extensions;

namespace Blockgame.World
{
    public class Chunk : IDisposable
    {
        public readonly static int Size = 16;

        BlockMesh _mesh = new BlockMesh();

        Block[,,] _blocks = new Block[Size, Size, Size];

        int _vao;
        VertexBuffer _vbo;

        bool _disposed = false;
        
        bool _wasModified = false;

        public Chunk()
        {
            Setup(BlockType.Debug);

            _vao = GL.GenVertexArray();

            _vbo = new VertexBuffer();

            GenerateMesh();
        }

        public void Setup(BlockType blockType)
        {
            // Generate map
            for (int x = 0; x < Size; ++x)
            {
                for (int z = 0; z < Size; ++z)
                {
                    for (int y = 0; y < Size; ++y)
                    {
                        _blocks[x, y, z] = new Block();
                        //if (Math.Sqrt((float)(x - Size / 2) * (x - Size / 2) + (y - Size / 2) * (y - Size / 2) + (z - Size / 2) * (z - Size / 2)) <= Size / 2)
                        {
                            _blocks[x, y, z].Disabled = false;
                            _blocks[x, y, z].Type = blockType;
                             
                        }
                    } 
                }
            }
        }

        public void GenerateMesh()
        {

            _mesh.ClearData();
            for (var x = 0; x < Size; ++x)
            {
                for (var y = 0; y < Size; ++y)
                {
                    for (var z = 0; z < Size; ++z)
                    {
                        if (!_blocks[x, y, z].Disabled)
                        {
                            //if (!(x > 0 && x < Size - 1 && y > 0 && y < Size - 1 && z > 0 && z < Size - 1 && !_blocks[x - 1, y, z].Disabled && !_blocks[x + 1, y, z].Disabled && !_blocks[x, y - 1, z].Disabled && !_blocks[x, y + 1, z].Disabled && !_blocks[x, y, z - 1].Disabled && !_blocks[x, y, z + 1].Disabled))
                            {
                                _mesh.Update(BlockMesh.GetFace(BlockFace.Front, _blocks[x, y, z].Type), x, y, z);
                                _mesh.Update(BlockMesh.GetFace(BlockFace.Back, _blocks[x, y, z].Type), x, y, z);
                                _mesh.Update(BlockMesh.GetFace(BlockFace.Top, _blocks[x, y, z].Type), x, y, z);
                                _mesh.Update(BlockMesh.GetFace(BlockFace.Bottom, _blocks[x, y, z].Type), x, y, z);
                                _mesh.Update(BlockMesh.GetFace(BlockFace.Left, _blocks[x, y, z].Type), x, y, z);
                                _mesh.Update(BlockMesh.GetFace(BlockFace.Right, _blocks[x, y, z].Type), x, y, z);
                            }
                        }
                        
                    }
                }
            }
            GL.BindVertexArray(_vao);

            _vbo.Bind();
            int bufferSize = sizeof(float) * (_mesh.Positions.Count + _mesh.Normals.Count + _mesh.TexCoords.Count + _mesh.TextureId.Count)+1;
            GL.BufferData(BufferTarget.ArrayBuffer, bufferSize, new float[bufferSize], BufferUsageHint.StaticDraw);

            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(0), sizeof(float) * _mesh.Positions.Count, _mesh.Positions.ToArray());
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * _mesh.Positions.Count), sizeof(float) * _mesh.Normals.Count, _mesh.Normals.ToArray());
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * (_mesh.Positions.Count + _mesh.Normals.Count)), sizeof(float) * _mesh.TexCoords.Count, _mesh.TexCoords.ToArray());
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * (_mesh.Positions.Count + _mesh.Normals.Count + _mesh.TexCoords.Count)), sizeof(float) * _mesh.TextureId.Count, _mesh.TextureId.ToArray());

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, sizeof(float) * _mesh.Positions.Count);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, sizeof(float) * (_mesh.Positions.Count + _mesh.Normals.Count));
            GL.VertexAttribPointer(3, 1, VertexAttribPointerType.Float, false, sizeof(float) * 1, sizeof(float) * (_mesh.Positions.Count + _mesh.Normals.Count + _mesh.TexCoords.Count));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.EnableVertexAttribArray(3);
        }

        public void Render()
        {

            if (_wasModified)
            {
                GenerateMesh();
            }

            GL.BindVertexArray(_vao);
            _vbo.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _mesh.Positions.Count/3);
        }

        // This method is not array-bound safe
        public Block GetBlock(int x, int y, int z)
        {
            return _blocks[x, y, z];
        }

        public void PlaceBlock(BlockType blockType, int x, int y, int z)
        {
            // Check if the block-space is available
            if (_blocks.TryGetValue(x, y, z, out var block))
            {
                if (block.Disabled)
                {
                    block.Type = blockType;
                    _wasModified = true;

                }
            }

        }

        public void DestroyBlock(int x, int y, int z)
        {

            if (_blocks.TryGetValue(x, y, z, out var block))
            {
                block.Disabled = true;
                _wasModified = true;

            }

        }

        // Disposing

        ~Chunk()
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
            _disposed = true;
        }

    }
}
