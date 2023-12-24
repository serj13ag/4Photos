using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelName;
        public Sprite[] Images;
        public string Word;

        private void OnValidate()
        {
            LevelName = name;
        }
    }
}