using UnityEngine;

public static class Extensions
{
    public static void Shuffle<T>(this T[] array, System.Random rnd)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = rnd.Next(n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }

    public static void DestroyAllChildren(this Transform container)
    {
        foreach (Transform child in container)
        {
            Object.Destroy(child.gameObject);
        }
    }
}