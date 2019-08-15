using System.Collections.Generic;


namespace Blockgame.World
{
    public enum BlockKind : byte
    {
        Air = 0,
        Wood,
        Stone,
        Grass,
        Dirt,
        Mushroom,
        MushroomStem,
    }

    public enum BlockFace : byte
    {
        Top = 0,
        Bottom,
        Right,
        Left,
        Back,
        Front
    }

    // Information for each block
    public class BlockKindData
    {
        public readonly int[] Faces;

        public BlockKindData(int top = 0, int bottom = 0, int front = 0, int back = 0, int left = 0, int right = 0)
        {
            Faces = new int[6];
            Faces[(int)BlockFace.Top] = top;
            Faces[(int)BlockFace.Bottom] = bottom;
            Faces[(int)BlockFace.Front] = front;
            Faces[(int)BlockFace.Back] = back;
            Faces[(int)BlockFace.Left] = left;
            Faces[(int)BlockFace.Right] = right;
        }
    }
    public static class BlockRegistry
    {

        static Dictionary<BlockKind, BlockKindData> _registeredBlockTypes = new Dictionary<BlockKind, BlockKindData>();

        public static void RegisterBlockType(BlockKind id, BlockKindData data)
        {
            _registeredBlockTypes.Add(id, data);
        }

        public static BlockKindData GetData(BlockKind id)
        {
            return _registeredBlockTypes[id];
        }
    }
}
