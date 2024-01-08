using System;
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

        [SerializeField] private ImageButton _imageButtonPrefab;
        [SerializeField] private WordButton _wordButtonPrefab;
        [SerializeField] private KeyboardButton _keyboardButtonPrefab;
        [SerializeField] private ScalingImagePrefab _scalingImagePrefab;

        [SerializeField] private RectTransform _imagesGridRectTransform;
        [SerializeField] private Transform _wordContainer;
        [SerializeField] private Transform _keyboardContainer;
        [SerializeField] private Transform _scalingImageContainer;

        [SerializeField] private Button _resetWordButton;
        [SerializeField] private Button _hintFillWordCharacterButton;
        [SerializeField] private Button _hintHideWrongKeyboardCharacterButton;

        private RandomService _randomService;

        private int _currentLevelIndex;
        private string _answerWord;
        private WordButton[] _wordButtons;
        private KeyboardButton[] _keyboardButtons;
        private int _currentWordCharIndex;
        private ScalingImagePrefab _scalingImage;

        private void OnEnable()
        {
            _resetWordButton.onClick.AddListener(OnResetWordButtonClicked);
            _hintFillWordCharacterButton.onClick.AddListener(OnHintFillWordCharacterButtonClicked);
            _hintHideWrongKeyboardCharacterButton.onClick.AddListener(OnHintHideWrongKeyboardCharacterButtonClicked);
        }

        private void Start()
        {
            _randomService = new RandomService();

            _currentLevelIndex = 0;

            InitLevelWithCurrentIndex();
        }

        private void OnDisable()
        {
            _resetWordButton.onClick.RemoveListener(OnResetWordButtonClicked);
            _hintFillWordCharacterButton.onClick.RemoveListener(OnHintFillWordCharacterButtonClicked);
            _hintHideWrongKeyboardCharacterButton.onClick.RemoveListener(OnHintHideWrongKeyboardCharacterButtonClicked);
        }

        private void InitLevelWithCurrentIndex()
        {
            LevelStaticData currentLevelStaticData = _levelsStaticData[_currentLevelIndex];

            _answerWord = currentLevelStaticData.Word.ToUpper();
            char[] answerChars = _answerWord.ToCharArray();
            char[] charactersForKeyboard = _randomService.GetCharactersForKeyboard(answerChars, Constants.NumberOfKeyboardButtons);

            ClearContainers();
            CreateImages(currentLevelStaticData.Images);
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

        private void CreateImages(IReadOnlyList<Sprite> images)
        {
            for (int i = 0; i < images.Count; i++)
            {
                ImageButton imagePrefab = Instantiate(_imageButtonPrefab, _imagesGridRectTransform);
                imagePrefab.Init(images[i], GetImagePivotByIndex(i), this);
            }
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
            DestroyAllChildren(_imagesGridRectTransform);
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

        private static Vector2 GetImagePivotByIndex(int i)
        {
            return i switch
            {
                0 => new Vector2(0, 1),
                1 => new Vector2(1, 1),
                2 => new Vector2(0, 0),
                3 => new Vector2(1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
            };
        }

        public void ShowScalingImage(Sprite sprite, RectTransform imageRectTransform)
        {
            _scalingImage = Instantiate(_scalingImagePrefab, _scalingImageContainer);
            _scalingImage.Init(sprite, imageRectTransform.position, imageRectTransform.sizeDelta, imageRectTransform.pivot, _imagesGridRectTransform.rect.size);
        }
    }
}