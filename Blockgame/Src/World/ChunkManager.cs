using System;

using System.Collections.Generic;
using Blockgame.Resources;
using OpenTK;

namespace Blockgame.World
{
    public class ChunkManager : IDisposable
    {
        public static readonly int Width = 2;
        public static readonly int Height = 1;
        public static readonly int Depth = 2;

        Shader _shader;
        TextureArray _textureArray;

        Dictionary<Vector3, Chunk> _chunkMap;

        bool _disposed = false;

         
        public ChunkManager()
        {
            GenerateChunks();

            _shader = new Shader("Shaders/block.vert", "Shaders/block.frag");
            _shader.Bind();

            _textureArray = new TextureArray("Textures/map.png", (int)BlockType.NumberOfBlocks);
            _textureArray.Bind();

        }

        private void GenerateChunks()
        {
            _chunkMap = new Dictionary<Vector3, Chunk>();

            // Example: generate 4 chunks next to each other
            for (var x = 0; x < Width; ++x)
            {
                for (var z = 0; z < Depth;  ++z)
                {
                    _chunkMap.Add(new Vector3(x, 0, z), new Chunk());

                }

            }
            
        }

        public void Render(Camera camera)
        {
            _shader.Bind();
            _textureArray.Bind();
            _shader.SetInt("u_texture", 0);
            _shader.SetMatrix4("u_view", camera.GetWorldToViewMatrix());
            _shader.SetMatrix4("u_projection", camera.GetViewToProjectionMatrix());
            _shader.SetVector3("u_lightPos", camera.Position);

            foreach (var entry in _chunkMap)
            {
                Vector3 position = entry.Key * Chunk.Size;

                _shader.SetMatrix4("u_model", Matrix4.CreateTranslation(position));
                entry.Value.Render();
            }
        }

        public class BlockLocation
        {
            public Vector3 ChunkIndex;
            public Vector3 BlockIndex;
        }

        public BlockLocation GetBlock(Vector3 worldPosition)
        {
            int chunkX = (int)Math.Floor(worldPosition.X / Chunk.Size);
            int chunkY = (int)Math.Floor(worldPosition.Y / Chunk.Size);
            int chunkZ = (int)Math.Floor(worldPosition.Z / Chunk.Size);

            int blockX = (int)Math.Floor(worldPosition.X) - (chunkX * Chunk.Size);
            int blockY = (int)Math.Floor(worldPosition.Y) - (chunkY * Chunk.Size);
            int blockZ = (int)Math.Floor(worldPosition.Z) - (chunkZ * Chunk.Size);

            //Console.WriteLine($"Accessing block ({blockX}, {blockY}, {blockZ}) in chunk ({chunkX}, {chunkY}, {chunkZ}). World position is {worldPosition}");

            return new BlockLocation() { ChunkIndex = new Vector3(chunkX, chunkY, chunkZ), BlockIndex = new Vector3(blockX, blockY, blockZ) };
        }

        public void DestroyBlock(Vector3 worldPosition)
        {
            BlockLocation location = GetBlock(worldPosition);

            _chunkMap[location.ChunkIndex].DestroyBlock((int)location.BlockIndex.X, (int)location.BlockIndex.Y, (int)location.BlockIndex.Z);
        }

        public void PlaceBlock(BlockType blockType, Vector3 worldPosition)
        {
            BlockLocation location = GetBlock(worldPosition);

            _chunkMap[location.ChunkIndex].PlaceBlock(blockType, (int)location.BlockIndex.X, (int)location.BlockIndex.Y, (int)location.BlockIndex.Z);
        }

        // Disposing

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            foreach (var entry in _chunkMap)
            {
                entry.Value.Dispose();
            }
            _shader.Dispose();
            _textureArray.Dispose();
        }

        ~ChunkManager()
        {
            Dispose(false);
        }

        

    }
}
