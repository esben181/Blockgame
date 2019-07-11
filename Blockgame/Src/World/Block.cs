using System;

namespace Blockgame.World
{
    public enum BlockType
    {
        Debug = 0,
        Stone,
        Grass,
        Dirt
    }

    public class Block
    {
        public Block(BlockType type = BlockType.Debug, bool disabled = true)
        {
            Type = type;
            Disabled = disabled;
            
        }
        public bool Disabled { get; set; } = false;

        public BlockType Type { get; set; } = BlockType.Debug;

    }
}
