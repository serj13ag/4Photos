using ScriptableObjects;
using Services;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelNumberText;

        private RandomService _randomService;
        private StaticDataService _staticDataService;
        private ImagesController _imagesController;
        private WordController _wordController;
        private KeyboardController _keyboardController;

        private int _currentLevelIndex;

        public string AnswerWord { get; private set; }

        public void Init(RandomService randomService, StaticDataService staticDataService,
            ImagesController imagesController, WordController wordController, KeyboardController keyboardController)
        {
            _randomService = randomService;
            _staticDataService = staticDataService;
            _imagesController = imagesController;
            _wordController = wordController;
            _keyboardController = keyboardController;
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
            if (_currentLevelIndex > _staticDataService.Levels.Length - 1)
            {
                _currentLevelIndex = 0;
            }

            Cleanup();
            CreateLevelWithCurrentIndex();
        }

        private void CreateLevelWithCurrentIndex()
        {
            LevelStaticData currentLevelStaticData = _staticDataService.Levels[_currentLevelIndex];

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