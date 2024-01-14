using System.Collections.Generic;
using System.Linq;
using Extensions;
using Prefabs;
using ScriptableObjects;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private LevelStaticData[] _levelsStaticData;

        [SerializeField] private ImagesController _imagesController;

        [SerializeField] private WordButton _wordButtonPrefab;
        [SerializeField] private KeyboardButton _keyboardButtonPrefab;

        [SerializeField] private Transform _wordContainer;
        [SerializeField] private Transform _keyboardContainer;

        [SerializeField] private TMP_Text _levelNumberText;
        [SerializeField] private TMP_Text _coinsText;

        [SerializeField] private Button _resetWordButton;
        [SerializeField] private Button _hintFillWordCharacterButton;
        [SerializeField] private Button _hintHideWrongKeyboardCharacterButton;
        [SerializeField] private Button _incrementCoinsButton;

        private RandomService _randomService;

        private int _coins;

        private int _currentLevelIndex;
        private string _answerWord;
        private WordButton[] _wordButtons;
        private KeyboardButton[] _keyboardButtons;
        private int _currentWordCharIndex;

        private void OnEnable()
        {
            _resetWordButton.onClick.AddListener(OnResetWordButtonClicked);
            _hintFillWordCharacterButton.onClick.AddListener(OnHintFillWordCharacterButtonClicked);
            _hintHideWrongKeyboardCharacterButton.onClick.AddListener(OnHintHideWrongKeyboardCharacterButtonClicked);
            _incrementCoinsButton.onClick.AddListener(OnIncrementCoinsButtonClicked);
        }

        private void Start()
        {
            _randomService = new RandomService();

            _coins = 4;
            _currentLevelIndex = 0;

            InitLevelWithCurrentIndex();
            UpdateCoinsText();
        }

        private void OnDisable()
        {
            _resetWordButton.onClick.RemoveListener(OnResetWordButtonClicked);
            _hintFillWordCharacterButton.onClick.RemoveListener(OnHintFillWordCharacterButtonClicked);
            _hintHideWrongKeyboardCharacterButton.onClick.RemoveListener(OnHintHideWrongKeyboardCharacterButtonClicked);
            _incrementCoinsButton.onClick.RemoveListener(OnIncrementCoinsButtonClicked);
        }

        private void InitLevelWithCurrentIndex()
        {
            LevelStaticData currentLevelStaticData = _levelsStaticData[_currentLevelIndex];

            _answerWord = currentLevelStaticData.Word.ToUpper();
            char[] answerChars = _answerWord.ToCharArray();
            char[] charactersForKeyboard = _randomService.GetCharactersForKeyboard(answerChars, Constants.NumberOfKeyboardButtons);

            _currentWordCharIndex = 0;

            _levelNumberText.text = (_currentLevelIndex + 1).ToString();

            ClearContainers();
            _imagesController.CreateImageButtons(currentLevelStaticData.Images, Constants.NumberOfInitiallyOpenedImages);
            CreateWordButtons(answerChars);
            CreateKeyboardButtons(charactersForKeyboard);
        }

        public bool TryFillWord(KeyboardButton keyboardButton)
        {
            if (_currentWordCharIndex == -1)
            {
                return false;
            }

            _wordButtons[_currentWordCharIndex].FillByKeyboard(keyboardButton);
            UpdateCurrentWordCharIndex();
            CheckAnswer();
            return true;
        }

        public bool TrySpendCoins(int coins)
        {
            if (_coins < coins)
            {
                return false;
            }

            _coins -= coins;
            UpdateCoinsText();
            return true;
        }

        public void ResetAllWordButtonsAfterWrong()
        {
            ResetWordButtons();
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

        private void CheckAnswer()
        {
            if (_currentWordCharIndex == -1)
            {
                if (AnswerIsRight())
                {
                    ChangeLevelToNext();
                }
                else
                {
                    HighlightWordAsWrong();
                }
            }
        }

        private bool AnswerIsRight()
        {
            foreach (WordButton wordButton in _wordButtons)
            {
                if (!wordButton.IsFilledWithAnswerCharacter)
                {
                    return false;
                }
            }

            return true;
        }

        private void ChangeLevelToNext()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex > _levelsStaticData.Length - 1)
            {
                _currentLevelIndex = 0;
            }

            InitLevelWithCurrentIndex();
        }

        private void HighlightWordAsWrong()
        {
            foreach (WordButton wordButton in _wordButtons)
            {
                wordButton.SetAsWrong();
            }
        }

        private void OnResetWordButtonClicked()
        {
            ResetWordButtons();
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
                    CheckAnswer();
                    return;
                }
            }
        }

        private void OnIncrementCoinsButtonClicked()
        {
            _coins++;
            UpdateCoinsText();
        }

        private void UpdateCoinsText()
        {
            _coinsText.text = _coins.ToString();
        }

        private void ResetWordButtons()
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

        private void CreateWordButtons(IReadOnlyList<char> answerChars)
        {
            _wordButtons = new WordButton[answerChars.Count];

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
            _imagesController.ClearImages();

            _wordContainer.DestroyAllChildren();
            _keyboardContainer.DestroyAllChildren();
        }
    }
}