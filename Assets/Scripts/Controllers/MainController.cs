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
        [SerializeField] private Button _hintFillWordCharacterButton;
        [SerializeField] private Button _hintHideWrongKeyboardCharacterButton;

        private RandomService _randomService;

        private string _answerWord;
        private WordButton[] _wordButtons;
        private KeyboardButton[] _keyboardButtons;
        private int _currentWordCharIndex;

        private void OnEnable()
        {
            _resetWordButton.onClick.AddListener(OnResetWordButtonClicked);
            _hintFillWordCharacterButton.onClick.AddListener(OnHintFillWordCharacterButtonClicked);
            _hintHideWrongKeyboardCharacterButton.onClick.AddListener(OnHintHideWrongKeyboardCharacterButtonClicked);
        }

        private void Start()
        {
            LevelStaticData currentLevelStaticData = _levelsStaticData[0];

            _randomService = new RandomService();

            _answerWord = currentLevelStaticData.Word.ToUpper();
            char[] answerChars = _answerWord.ToCharArray();
            char[] charactersForKeyboard = _randomService.GetCharactersForKeyboard(answerChars, Constants.NumberOfKeyboardButtons);

            ClearContainers();
            CreateImages(currentLevelStaticData.Images);
            CreateWordButtons(answerChars);
            CreateKeyboardButtons(charactersForKeyboard);
        }

        private void OnDisable()
        {
            _resetWordButton.onClick.RemoveListener(OnResetWordButtonClicked);
            _hintFillWordCharacterButton.onClick.RemoveListener(OnHintFillWordCharacterButtonClicked);
            _hintHideWrongKeyboardCharacterButton.onClick.RemoveListener(OnHintHideWrongKeyboardCharacterButtonClicked);
        }

        public bool TryFillWord(KeyboardButton keyboardButton)
        {
            if (_currentWordCharIndex == -1)
            {
                return false;
            }

            _wordButtons[_currentWordCharIndex].FillByKeyboard(keyboardButton);
            UpdateCurrentWordCharIndex();
            return true;
        }

        public void UpdateCurrentWordCharIndex()
        {
            for (int i = 0; i < _wordButtons.Length; i++)
            {
                if (!_wordButtons[i].IsFilledWithCharacter)
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
                if (wordButton.IsFilledWithCharacter && !wordButton.IsLocked)
                {
                    wordButton.SetAsEmpty();
                }
            }

            UpdateCurrentWordCharIndex();
        }

        private void OnHintHideWrongKeyboardCharacterButtonClicked()
        {
            foreach (KeyboardButton keyboardButton in _keyboardButtons.OrderBy(_ => _randomService.Random.Next()))
            {
                if (!keyboardButton.IsHided && !_answerWord.Contains(keyboardButton.Character))
                {
                    keyboardButton.Hide();
                    return;
                }
            }
        }

        private void OnHintFillWordCharacterButtonClicked()
        {
            foreach (WordButton wordButton in _wordButtons.OrderBy(_ => _randomService.Random.Next()))
            {
                if (!wordButton.IsFilledWithCharacter)
                {
                    wordButton.FillByHint();
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
            _keyboardButtons = new KeyboardButton[randomCharacters.Count];

            for (int i = 0; i < randomCharacters.Count; i++)
            {
                KeyboardButton keyboardButton = Instantiate(_keyboardButtonPrefab, _keyboardContainer);
                keyboardButton.Init(randomCharacters[i], this);
                _keyboardButtons[i] = keyboardButton;
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