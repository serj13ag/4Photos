using UnityEngine;

namespace Extensions
{
    public static class UnityExtensions
    {
        public static void DestroyAllChildren(this Transform container)
        {
            foreach (Transform child in container)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}