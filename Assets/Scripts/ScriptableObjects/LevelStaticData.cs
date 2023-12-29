using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
    public class LevelStaticData : ScriptableObject
    {
        public string Code;
        public Sprite[] Images;
        public string Word;

        private void OnValidate()
        {
            Code = name;

            if (Images.Length < Constants.NumberOfImages)
            {
                Debug.LogError($"{nameof(LevelStaticData)}-{Code}: {nameof(Images)} count is less than {Constants.NumberOfImages}");
            }

            for (int i = 0; i < Images.Length; i++)
            {
                if (Images[i] == null)
                {
                    Debug.LogError($"{nameof(LevelStaticData)}-{Code}: {nameof(Images)}[{i}] is null");
                }
            }

            if (Word.Length == 0)
            {
                Debug.LogError($"{nameof(LevelStaticData)}-{Code}: {nameof(Word)} is empty");
            }

            if (Word.Length > Constants.MaxNumberOfWordCharacters)
            {
                Debug.LogError($"{nameof(LevelStaticData)}-{Code}: {nameof(Word)} has more than {Constants.MaxNumberOfWordCharacters} characters");
            }
        }
    }
}