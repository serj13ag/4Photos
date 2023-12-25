using System;

namespace Extensions
{
    public static class ArrayExtensions
    {
        public static void Shuffle<T>(this T[] array, Random rnd)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rnd.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
    }
}