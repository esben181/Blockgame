
namespace Blockgame.World
{
    public enum BlockMaterial : byte
    {
        Empty = 0,
        Construction,
        Stone,
        Grass,
        Dirt,
    }

    public class Block
    {
        public Block(BlockMaterial material = BlockMaterial.Empty)
        {
            Material = material;
        }

        public bool Equals(Block other)
        {
            return Material == other.Material;
        }

        public bool IsEmpty()
        {
            return Material == BlockMaterial.Empty;
        }
        public BlockMaterial Material { get; set; }

    }
}
