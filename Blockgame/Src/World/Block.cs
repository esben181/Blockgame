namespace Blockgame.World
{
    
    public class Block
    {
        public BlockKind Kind { get; set; }

        public Block(BlockKind kind = BlockKind.Air)
        {
            Kind = kind;
        }

        public bool Equals(Block other)
        {
            return Kind == other.Kind;
        }

        public bool IsAir()
        {
            return Kind == BlockKind.Air;
        }

    }
}
