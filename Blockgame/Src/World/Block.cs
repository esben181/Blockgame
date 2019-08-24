
using System;
namespace Blockgame.World
{
    
    public class Block
    {
        BlockKind _kind;
        public BlockKind Kind {
            get => _kind;
            set
            {
                _kind = value;
                CurrentHP = 0;
                if (!IsAir())
                    CurrentHP = BlockRegistry.GetData(Kind).Health;
            }
        }
        public float CurrentHP { get; set; } = 0;

        public Block(BlockKind kind = BlockKind.Air)
        {
            Kind = kind;
        }

        public bool Equals(Block other)
        {
            return Kind == other.Kind && (int)CurrentHP == (int)other.CurrentHP;
        }

        public bool IsAir()
        {
            return Kind == BlockKind.Air;
        }

    }
}
