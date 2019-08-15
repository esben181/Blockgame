using System;

using System.Collections.Generic;
using Blockgame.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Blockgame.World
{
    public class Map : IDisposable
    {
        public static readonly int MapWidth = 3;
        public static readonly int MapHeight = 1;
        public static readonly int MapDepth = 3;

        Shader _shader;

        Dictionary<Vector3, Chunk> _chunks;
        TextureArray _textureArray;

        
        bool _disposed = false;

         
        public Map()
        {
            // Todo: load required block types from file.
            BlockRegistry.RegisterBlockType(BlockKind.Wood, new BlockKindData(4, 4, 3, 3, 3, 3));
            BlockRegistry.RegisterBlockType(BlockKind.Grass, new BlockKindData(1, 0, 2, 2, 2, 2));
            BlockRegistry.RegisterBlockType(BlockKind.Stone, new BlockKindData(5, 5, 5, 5, 5, 5));
            BlockRegistry.RegisterBlockType(BlockKind.Dirt, new BlockKindData(0,0,0,0,0,0));
            BlockRegistry.RegisterBlockType(BlockKind.Mushroom, new BlockKindData(6, 6, 6, 6, 6, 6));
            BlockRegistry.RegisterBlockType(BlockKind.MushroomStem, new BlockKindData(7, 7, 7, 7, 7, 7));

            GenerateChunks();

            _shader = new Shader("Shaders/block.vert", "Shaders/block.frag");
            _shader.Bind();

            _textureArray = new TextureArray("Textures/map.png", 128, 128);

        }

        private void GenerateChunks()
        {
            _chunks = new Dictionary<Vector3, Chunk>();

            for (var x = -2; x < MapWidth-2; ++x)
                for (var y = 0; y < MapHeight; ++y)
                    for (var z = -2; z < MapDepth-2;  ++z)
                    {
                        _chunks.Add(new Vector3(x, y, z), new Chunk(x, y, z, this));
                        _chunks[new Vector3(x, y, z)].GenerateMesh();
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

            foreach (var entry in _chunks)
            {
                Vector3 position = entry.Key * Chunk.ChunkSize * Chunk.BlockSize;

                _shader.SetMatrix4("u_model", Matrix4.CreateTranslation(position));
                entry.Value.Render();
            }
        }

        private (Vector3 chunkPos, Vector3 blockPos) GetBlockLocation(Vector3 worldPosition)
        {
            int chunkX = (int)Math.Floor(worldPosition.X / Chunk.ChunkSize);
            int chunkY = (int)Math.Floor(worldPosition.Y / Chunk.ChunkSize);
            int chunkZ = (int)Math.Floor(worldPosition.Z / Chunk.ChunkSize);

            int blockX = (int)Math.Floor(worldPosition.X) - (chunkX * Chunk.ChunkSize);
            int blockY = (int)Math.Floor(worldPosition.Y) - (chunkY * Chunk.ChunkSize);
            int blockZ = (int)Math.Floor(worldPosition.Z) - (chunkZ * Chunk.ChunkSize);

            //Console.WriteLine($"Accessing block ({blockX}, {blockY}, {blockZ}) in chunk ({chunkX}, {chunkY}, {chunkZ}). World position is {worldPosition}");

            return (new Vector3(chunkX, chunkY, chunkZ), new Vector3(blockX, blockY, blockZ));
        }

        public Block GetblockAt(int x, int y, int z)
        {
            var (chunkPos, blockPos) = GetBlockLocation(new Vector3(x, y, z));
            if (_chunks.TryGetValue(chunkPos, out var chunk))
            {
                return chunk.GetBlock((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z);

            }
            return new Block();

        }

        public void DestroyBlock(Vector3 worldPosition)
        {
            var (chunkPos, blockPos) = GetBlockLocation(worldPosition);

            if(_chunks.TryGetValue(chunkPos, out var chunk))
            {
                chunk.DestroyBlock((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z);
            }
        }

        public void PlaceBlock(BlockKind kind, Vector3 worldPosition)
        {
            var (chunkPos, blockPos) = GetBlockLocation(worldPosition);

            if (_chunks.TryGetValue(chunkPos, out var chunk))
            {
                chunk.PlaceBlock(kind, (int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z);
            }
        }

        public bool IsBlockAt(int x, int y, int z)
        {
            var (chunkPos, blockPos) = GetBlockLocation(new Vector3(x, y, z));
            if (_chunks.TryGetValue(chunkPos, out var chunk))
            {
                if (chunk.GetBlock((int)blockPos.X, (int)blockPos.Y, (int)blockPos.Z).Kind != BlockKind.Air)
                {
                    return true;
                }
            }
            return false;

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

            foreach (var entry in _chunks)
            {
                entry.Value.Dispose();
            }
            _shader.Dispose();
            _textureArray.Dispose();
        }

        ~Map()
        {
            Dispose(false);
        }

        

    }
}
