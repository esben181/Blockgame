using System;


using OpenTK;
using OpenTK.Graphics.OpenGL4;

using Blockgame.Resources;
using Blockgame.Extensions;

namespace Blockgame.World
{
    public class Chunk : IDisposable
    {
        public readonly static int ChunkSize = 16;

        Block[,,] _blocks = new Block[ChunkSize, ChunkSize, ChunkSize];

        // Global chunk position
        public readonly int ChunkPosX, ChunkPosY, ChunkPosZ;

        BlockMesh _mesh;

        Map _map;

        bool _disposed = false;

        public Chunk(int i, int j, int k, Map map)
        {
            ChunkPosX = i * ChunkSize;
            ChunkPosY = j * ChunkSize;
            ChunkPosZ = k * ChunkSize;

            _map = map;

            _mesh = new BlockMesh();
            Setup();
        }

        public void Setup()
        {
            // Generate map
            for (int x = 0; x < ChunkSize; ++x)
            {
                for (int z = 0; z < ChunkSize; ++z)
                {
                    for (int y = 0; y < ChunkSize; ++y)
                    {
                        _blocks[x, y, z] = new Block(BlockMaterial.Empty);
                        //if (Math.Sqrt((float)(x - ChunkSize / 2) * (x - ChunkSize / 2) + (y - ChunkSize / 2) * (y - ChunkSize / 2) + (z - ChunkSize / 2) * (z - ChunkSize / 2)) <= ChunkSize / 2)
                        {
                            if (y < ChunkSize/2)
                                _blocks[x, y, z].Material = BlockMaterial.Grass;
                            if (y < ChunkSize/2-1)
                                _blocks[x, y, z].Material = BlockMaterial.Dirt;


                        }
                    }
                }
            }
        }

        public void GenerateMesh()
        {
            _mesh.Clear();
            GreedyMesh();
            _mesh.Buffer();
        }

        private void GreedyMesh()
        {
            int i, j, k, l, w, h, u, v, r, s, t;
            Block[] mask = new Block[ChunkSize * ChunkSize];
            BlockMesh.Face face = BlockMesh.Face.None;

            for (bool backFace = true, b = false; b != backFace; backFace = backFace && b, b = !b)
            {

                // Sweep over each axis
                for (var d = 0; d < 3; ++d)
                {

                    u = (d + 1) % 3;
                    v = (d + 2) % 3;
                    var x = new int[3];
                    var q = new int[3];
                    q[d] = 1;


                    if (d == 0)
                        face = backFace ? BlockMesh.Face.Right : BlockMesh.Face.Left;
                    else if (d == 1)
                        face = backFace ? BlockMesh.Face.Top : BlockMesh.Face.Bottom;
                    else if (d == 2)
                        face = backFace ? BlockMesh.Face.Front : BlockMesh.Face.Back;

                    // Run thru each slice (d) of the chunk
                    for (x[d] = -1; x[d] < ChunkSize;)
                    {

                        // Compute mask
                        var n = 0;
                        for (x[v] = 0; x[v] < ChunkSize; ++x[v])
                        {
                            for (x[u] = 0; x[u] < ChunkSize; ++x[u])
                            {

                                // q determines the direction that we are searching (X, Y, Z)
                                Block blockCurrent = (x[d] >= 0) ? _map.GetblockAt(x[0] + ChunkPosX, x[1] + ChunkPosY, x[2] + ChunkPosZ) : new Block();
                                Block blockCompare = (x[d] < ChunkSize - 1) ? _map.GetblockAt(x[0] + q[0] + ChunkPosX, x[1] + q[1] + ChunkPosY, x[2] + q[2] + ChunkPosZ) : new Block();

                                //Mask is set to true if there is a visible face between two blocks
                                mask[n++] = (!blockCurrent.IsEmpty() && !blockCompare.IsEmpty() && blockCurrent.Equals(blockCompare))
                                    ? new Block() : backFace ? blockCompare : blockCurrent;
                            }
                        }
                        ++x[d];

                        n = 0;

                        // Generate mesh from mask using lexicographic ordering, by looping over each block
                        // in this slice of the chunk
                        for (j = 0; j < ChunkSize; ++j)
                        {
                            for (i = 0; i < ChunkSize;)
                            {
                                if (!mask[n].IsEmpty())
                                {
                                    // Compute width of quad and store it in w
                                    // This is done by seraching along the current axis until mask[n + w] is false
                                    for (w = 1; i + w < ChunkSize && !mask[n + w].IsEmpty() && mask[n + w].Equals(mask[n]); ++w) { }


                                    var done = false;
                                    for (h = 1; j + h < ChunkSize; ++h)
                                    {
                                        // Check every block next to this quad
                                        for (k = 0; k < w; ++k)
                                        {
                                            // Exit if there's a hole in the mask
                                            if (mask[n + k + h * ChunkSize].IsEmpty() || !mask[n + k + h * ChunkSize].Equals(mask[n]))
                                            {
                                                done = true;
                                                break;
                                            }
                                        }

                                        if (done)
                                            break;
                                    }
                                    x[u] = i;
                                    x[v] = j;

                                    // Let du and dv determine the size and orientation of this face
                                    var du = new int[3];
                                    du[u] = w;

                                    var dv = new int[3];
                                    dv[v] = h;


                                    r = x[0];
                                    s = x[1];
                                    t = x[2];

                                    // Create a quad for this face
                                    // AppendQuad(top-left, top-right, bottom-left, bottom-right);
                                    _mesh.AppendQuad(new Vector3(r, s, t),
                                                     new Vector3(r + du[0], s + du[1], t + du[2]),
                                                     new Vector3(r + dv[0], s + dv[1], t + dv[2]),
                                                     new Vector3(r + du[0] + dv[0], s + du[1] + dv[1], t + du[2] + dv[2]),
                                                     face,
                                                     mask[n].Material
                                                     );
                                    // Zero-out mask
                                    for (l = 0; l < h; ++l)
                                        for (k = 0; k < w; ++k)
                                            mask[n + k + l * ChunkSize] = new Block();

                                    i += w;
                                    n += w;
                                }
                                else
                                {
                                    i++;
                                    n++;
                                }
                            }
                        }
                    }
                }
            }

        }

        public void Render()
        {
            _mesh.Draw();
        }

        public Block GetBlock(int x, int y, int z)
        {
            if (_blocks.TryGetValue(x, y, z, out var block))
            {
                return block;
            }
            else
            {
                return new Block(BlockMaterial.Empty);
            }
        }

        public void PlaceBlock(BlockMaterial blockMaterial, int i, int j, int k)
        {
            // Check if the block-space is available
            if (_blocks.TryGetValue(i, j, k, out var block))
            {
                if (block.Material == BlockMaterial.Empty)
                {
                    block.Material = blockMaterial;
                    GenerateMesh();

                }
            }

        }

        public void DestroyBlock(int i, int j, int k)
        {

            if (_blocks.TryGetValue(i, j, k, out var block))
            {
                block.Material = BlockMaterial.Empty;
                GenerateMesh();
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

            _mesh.Dispose();
            _disposed = true;
        }

    }
}
