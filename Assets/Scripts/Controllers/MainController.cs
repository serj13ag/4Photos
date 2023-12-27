using System.Collections.Generic;
using Prefabs;
using ScriptableObjects;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private LevelStaticData[] _levelsStaticData;

        [SerializeField] private Image _imagePrefab;
        [SerializeField] private WordButton _wordButtonPrefab;
        [SerializeField] private KeyboardButton _keyboardButtonPrefab;

        [SerializeField] private Transform _imagesContainer;
        [SerializeField] private Transform _wordContainer;
        [SerializeField] private Transform _keyboardContainer;

        private char[] _answerChars;
        private WordButton[] _wordButtons;
        private int _currentWordCharIndex;

        private void Start()
        {
            LevelStaticData currentLevelStaticData = _levelsStaticData[0];

            RandomService randomService = new RandomService();

            _answerChars = currentLevelStaticData.Word.ToUpper().ToCharArray();
            _currentWordCharIndex = 0;

            char[] charactersForKeyboard = randomService.GetCharactersForKeyboard(_answerChars, Constants.NumberOfKeyboardButtons);

            ClearContainers();
            CreateImages(currentLevelStaticData.Images);
            CreateWordButtons(_answerChars.Length);
            CreateKeyboardButtons(charactersForKeyboard);
        }

        public void KeyboardButtonClicked(KeyboardButton keyboardButton)
        {
            _wordButtons[_currentWordCharIndex++].FillWithButton(keyboardButton);
        }

        private void CreateImages(IEnumerable<Sprite> images)
        {
            foreach (Sprite image in images)
            {
                Image imagePrefab = Instantiate(_imagePrefab, _imagesContainer);
                imagePrefab.sprite = image;
            }
        }

        private void CreateWordButtons(int numberOfButtons)
        {
            _wordButtons = new WordButton[numberOfButtons];

            for (int i = 0; i < numberOfButtons; i++)
            {
                WordButton wordButton = Instantiate(_wordButtonPrefab, _wordContainer);
                wordButton.Init();
                _wordButtons[i] = wordButton;
            }
        }

        private void CreateKeyboardButtons(IReadOnlyList<char> randomCharacters)
        {
            for (int i = 0; i < Constants.NumberOfKeyboardButtons; i++)
            {
                var keyboardButtonPrefab = Instantiate(_keyboardButtonPrefab, _keyboardContainer);
                keyboardButtonPrefab.Init(randomCharacters[i], this);
            }
        }

        private void ClearContainers()
        {
            DestroyAllChildren(_imagesContainer);
            DestroyAllChildren(_wordContainer);
            DestroyAllChildren(_keyboardContainer);
        }

        private static void DestroyAllChildren(Transform container)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
    }
}