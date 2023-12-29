using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private Button _resetWordButton;
        [SerializeField] private Button _hintShowWordCharacterButton;
        [SerializeField] private Button _hintHideWrongKeyboardCharacterButton;

        private RandomService _randomService;

        private char[] _answerChars;
        private WordButton[] _wordButtons;
        private int _currentWordCharIndex;

        private void OnEnable()
        {
            _resetWordButton.onClick.AddListener(OnResetWordButtonClicked);
            _hintShowWordCharacterButton.onClick.AddListener(OnHintShowWordCharacterButtonClicked);
            _hintHideWrongKeyboardCharacterButton.onClick.AddListener(OnHintHideWrongKeyboardCharacterButtonClicked);
        }

        private void Start()
        {
            LevelStaticData currentLevelStaticData = _levelsStaticData[0];

            _randomService = new RandomService();

            _answerChars = currentLevelStaticData.Word.ToUpper().ToCharArray();
            _currentWordCharIndex = 0;

            char[] charactersForKeyboard = _randomService.GetCharactersForKeyboard(_answerChars, Constants.NumberOfKeyboardButtons);

            ClearContainers();
            CreateImages(currentLevelStaticData.Images);
            CreateWordButtons(_answerChars);
            CreateKeyboardButtons(charactersForKeyboard);
        }

        private void OnDisable()
        {
            _resetWordButton.onClick.RemoveListener(OnResetWordButtonClicked);
            _hintShowWordCharacterButton.onClick.RemoveListener(OnHintShowWordCharacterButtonClicked);
            _hintHideWrongKeyboardCharacterButton.onClick.RemoveListener(OnHintHideWrongKeyboardCharacterButtonClicked);
        }

        public bool TryFillWord(KeyboardButton keyboardButton)
        {
            if (_currentWordCharIndex == -1)
            {
                return false;
            }

            _wordButtons[_currentWordCharIndex].FillWithButton(keyboardButton);
            UpdateCurrentWordCharIndex();
            return true;
        }

        public void UpdateCurrentWordCharIndex()
        {
            for (int i = 0; i < _wordButtons.Length; i++)
            {
                if (!_wordButtons[i].HasCharacter)
                {
                    _currentWordCharIndex = i;
                    return;
                }
            }

            _currentWordCharIndex = -1;
        }

        private void OnResetWordButtonClicked()
        {
            foreach (WordButton wordButton in _wordButtons)
            {
                if (wordButton.HasCharacter && !wordButton.IsLocked)
                {
                    wordButton.RemoveCharacter();
                }
            }

            UpdateCurrentWordCharIndex();
        }

        private void OnHintHideWrongKeyboardCharacterButtonClicked()
        {
        }

        private void OnHintShowWordCharacterButtonClicked()
        {
            foreach (WordButton wordButton in _wordButtons.OrderBy(_ => _randomService.Random.Next()))
            {
                if (!wordButton.HasCharacter)
                {
                    wordButton.SetAnswerCharacter();
                    UpdateCurrentWordCharIndex();
                    return;
                }
            }
        }

        private void CreateImages(IEnumerable<Sprite> images)
        {
            foreach (Sprite image in images)
            {
                Image imagePrefab = Instantiate(_imagePrefab, _imagesContainer);
                imagePrefab.sprite = image;
            }
        }

        private void CreateWordButtons(char[] answerChars)
        {
            _wordButtons = new WordButton[answerChars.Length];

            for (int i = 0; i < _wordButtons.Length; i++)
            {
                WordButton wordButton = Instantiate(_wordButtonPrefab, _wordContainer);
                wordButton.Init(answerChars[i], this);
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