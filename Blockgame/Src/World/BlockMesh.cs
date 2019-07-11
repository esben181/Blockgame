
using System;
using System.Collections.Generic;

namespace Blockgame.World
{
    public enum BlockFace
    {
        Top,
        Bottom,
        Front,
        Back,
        Left,
        Right
    }

    public class BlockMesh
    {

        public List<float> Positions { get;  } = new List<float>();
        public List<float> Normals { get; } = new List<float>();
        public List<float> TexCoords { get; } = new List<float>();
        public List<float> TextureId { get; } = new List<float>();


        public void Update(BlockMesh other, int x, int y, int z)
        {
            for (var i = 0; i < other.Positions.Count/3; ++i)
            {
                Positions.Add(other.Positions[0 + i * 3] + x);
                Positions.Add(other.Positions[1 + i * 3] + y);
                Positions.Add(other.Positions[2 + i * 3] + z);
            }
            Normals.AddRange(other.Normals);
            TexCoords.AddRange(other.TexCoords);
            TextureId.AddRange(other.TextureId);
            
        }

        public void ClearData()
        {
            Positions.Clear();
            Normals.Clear();
            TexCoords.Clear();
            TextureId.Clear();
        }

        public static BlockMesh GetFace(BlockFace face, BlockType blockType)
        {
            int blockTypeIndex = (int)blockType;
            if (face == BlockFace.Top)
            {
                // Top face
                return new BlockMesh
                {
                    Positions = {
                        -0.5f, 0.5f, -0.5f,
                         0.5f, 0.5f, -0.5f,
                         0.5f, 0.5f,  0.5f,
                         0.5f, 0.5f,  0.5f,
                        -0.5f, 0.5f,  0.5f,
                        -0.5f, 0.5f, -0.5f
                    },
                    Normals = {
                        0.0f, 1.0f,  0.0f,
                        0.0f, 1.0f,  0.0f,
                        0.0f, 1.0f,  0.0f,
                        0.0f, 1.0f,  0.0f,
                        0.0f, 1.0f,  0.0f,
                        0.0f, 1.0f,  0.0f
                    },
                    TexCoords = {
                    0.0f, 1.0f,
                    1.0f, 1.0f,
                    1.0f, 0.0f,
                    1.0f, 0.0f,
                    0.0f, 0.0f,
                    0.0f, 1.0f
                    },
                    TextureId =
                    {
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex
                    }

                };
            }
            else if (face == BlockFace.Bottom)
            {
                // Bottom face
                return new BlockMesh
                {
                    Positions = {
                        -0.5f, -0.5f, -0.5f,
                         0.5f, -0.5f, -0.5f,
                         0.5f, -0.5f,  0.5f,
                         0.5f, -0.5f,  0.5f,
                        -0.5f, -0.5f,  0.5f,
                        -0.5f, -0.5f, -0.5f
                    },
                    Normals = {
                        0.0f, -1.0f,  0.0f,
                        0.0f, -1.0f,  0.0f,
                        0.0f, -1.0f,  0.0f,
                        0.0f, -1.0f,  0.0f,
                        0.0f, -1.0f,  0.0f,
                        0.0f, -1.0f,  0.0f
                    },
                    TexCoords = {
                        0.0f, 1.0f,
                        1.0f, 1.0f,
                        1.0f, 0.0f,
                        1.0f, 0.0f,
                        0.0f, 0.0f,
                        0.0f, 1.0f
                    },
                    TextureId =
                    {
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex
                    }
                };
            }
            else if (face == BlockFace.Front)
            {
                // Front face
                return new BlockMesh
                {
                    Positions = {
                        -0.5f, -0.5f, -0.5f,
                         0.5f, -0.5f, -0.5f,
                         0.5f,  0.5f, -0.5f,
                         0.5f,  0.5f, -0.5f,
                        -0.5f,  0.5f, -0.5f,
                        -0.5f, -0.5f, -0.5f,
                    },
                    Normals = {
                        0.0f,  0.0f, -1.0f,
                        0.0f,  0.0f, -1.0f,
                        0.0f,  0.0f, -1.0f,
                        0.0f,  0.0f, -1.0f,
                        0.0f,  0.0f, -1.0f,
                        0.0f,  0.0f, -1.0f
                    },
                    TexCoords = {
                        1.0f, 1.0f,
                        0.0f, 1.0f,
                        0.0f, 0.0f,
                        0.0f, 0.0f,
                        1.0f, 0.0f,
                        1.0f, 1.0f
                    },
                    TextureId =
                    {
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex
                    }
                };
            }
            else if (face == BlockFace.Back)
            {

                // Back face
                return new BlockMesh
                {
                    Positions = {
                        -0.5f, -0.5f, 0.5f,
                         0.5f, -0.5f, 0.5f,
                         0.5f,  0.5f, 0.5f,
                         0.5f,  0.5f, 0.5f,
                        -0.5f,  0.5f, 0.5f,
                        -0.5f, -0.5f, 0.5f,
                    },
                    Normals = {
                        0.0f,  0.0f, 1.0f,
                        0.0f,  0.0f, 1.0f,
                        0.0f,  0.0f, 1.0f,
                        0.0f,  0.0f, 1.0f,
                        0.0f,  0.0f, 1.0f,
                        0.0f,  0.0f, 1.0f
                    },
                    TexCoords = {
                        1.0f, 1.0f,
                        0.0f, 1.0f,
                        0.0f, 0.0f,
                        0.0f, 0.0f,
                        1.0f, 0.0f,
                        1.0f, 1.0f
                    },
                    TextureId =
                    {
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex
                    }
                };
            }
            else if (face == BlockFace.Right)
            {

                // Right face
                return new BlockMesh
                {
                    Positions = {
                        0.5f,  0.5f,  0.5f,
                        0.5f,  0.5f, -0.5f,
                        0.5f, -0.5f, -0.5f,
                        0.5f, -0.5f, -0.5f,
                        0.5f, -0.5f,  0.5f,
                        0.5f,  0.5f,  0.5f
                    },
                    Normals = {
                       1.0f,  0.0f,  0.0f,
                       1.0f,  0.0f,  0.0f,
                       1.0f,  0.0f,  0.0f,
                       1.0f,  0.0f,  0.0f,
                       1.0f,  0.0f,  0.0f,
                       1.0f,  0.0f,  0.0f
                    },
                    TexCoords = {
                        0.0f, 0.0f,
                        1.0f, 0.0f,
                        1.0f, 1.0f,
                        1.0f, 1.0f,
                        0.0f, 1.0f,
                        0.0f, 0.0f
                    },
                    TextureId =
                    {
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex
                    }
                };
            }
            else  // face == BlockFace.Left
            {
                // Left Face
                return new BlockMesh
                {
                    Positions = {
                        -0.5f,  0.5f,  0.5f,
                        -0.5f,  0.5f, -0.5f,
                        -0.5f, -0.5f, -0.5f,
                        -0.5f, -0.5f, -0.5f,
                        -0.5f, -0.5f,  0.5f,
                        -0.5f,  0.5f,  0.5f
                    },
                    Normals = {
                       -1.0f,  0.0f,  0.0f,
                       -1.0f,  0.0f,  0.0f,
                       -1.0f,  0.0f,  0.0f,
                       -1.0f,  0.0f,  0.0f,
                       -1.0f,  0.0f,  0.0f,
                       -1.0f,  0.0f,  0.0f
                    },
                    TexCoords = {
                        0.0f, 0.0f,
                        1.0f, 0.0f,
                        1.0f, 1.0f,
                        1.0f, 1.0f,
                        0.0f, 1.0f,
                        0.0f, 0.0f
                    },
                    TextureId =
                    {
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex,
                        blockTypeIndex
                    }
                };
            }


        }

    }
}
