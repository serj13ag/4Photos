using ScriptableObjects;
using Services;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelStaticData[] _levelsStaticData;

        [SerializeField] private ImagesController _imagesController;
        [SerializeField] private WordController _wordController;
        [SerializeField] private KeyboardController _keyboardController;

        [SerializeField] private TMP_Text _levelNumberText;

        private RandomService _randomService;

        private int _currentLevelIndex;

        public string AnswerWord { get; private set; }

        public void Init(RandomService randomService)
        {
            _randomService = randomService;
        }

        public void CreateInitialLevel()
        {
            Cleanup();

            _currentLevelIndex = 0;

            CreateLevelWithCurrentIndex();
        }

        public void ChangeLevelToNext()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex > _levelsStaticData.Length - 1)
            {
                _currentLevelIndex = 0;
            }

            Cleanup();
            CreateLevelWithCurrentIndex();
        }

        private void CreateLevelWithCurrentIndex()
        {
            LevelStaticData currentLevelStaticData = _levelsStaticData[_currentLevelIndex];

            AnswerWord = currentLevelStaticData.Word.ToUpper();
            char[] answerChars = AnswerWord.ToCharArray();
            char[] charactersForKeyboard = _randomService.GetCharactersForKeyboard(answerChars, Constants.NumberOfKeyboardButtons);

            _imagesController.CreateImageButtons(currentLevelStaticData.Images, Constants.NumberOfInitiallyOpenedImages);
            _wordController.CreateWordButtons(answerChars);
            _keyboardController.CreateKeyboardButtons(charactersForKeyboard);

            UpdateLevelNumberText();
        }

        private void Cleanup()
        {
            _imagesController.ClearImages();
            _wordController.ClearWordButtons();
            _keyboardController.ClearKeyboardButtons();
        }

        private void UpdateLevelNumberText()
        {
            _levelNumberText.text = (_currentLevelIndex + 1).ToString();
        }
    }
}