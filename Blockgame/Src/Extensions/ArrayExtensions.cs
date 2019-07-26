namespace Blockgame.Extensions
{
    public static class ArrayExtensions
    {
        public static bool TryGetValue<T>(this T[,,] array, int index1, int index2, int index3, out T value)
        {
            if (index1 >= 0 && index2 >= 0 && index3 >= 0 && index1 < array.Length && index2 < array.Length && index3 < array.Length)
            {
                value = array[index1, index2, index3];
                return true;
            }
            value = default;
            return false;
        }
    }
}
