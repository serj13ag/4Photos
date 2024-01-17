using System.Collections.Generic;
using System.Linq;
using Extensions;
using Prefabs;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class WordController : MonoBehaviour
    {
        [SerializeField] private WordButton _wordButtonPrefab;
        [SerializeField] private Transform _wordContainer;

        [SerializeField] private Button _resetWordButton;
        [SerializeField] private Button _hintFillWordCharacterButton;

        private RandomService _randomService;
        private WinWindow _winWindow;

        private WordButton[] _wordButtons;
        private int _currentWordCharIndex;

        private void OnEnable()
        {
            _resetWordButton.onClick.AddListener(OnResetWordButtonClicked);
            _hintFillWordCharacterButton.onClick.AddListener(OnHintFillWordCharacterButtonClicked);
        }

        private void OnDisable()
        {
            _resetWordButton.onClick.RemoveListener(OnResetWordButtonClicked);
            _hintFillWordCharacterButton.onClick.RemoveListener(OnHintFillWordCharacterButtonClicked);
        }

        public void Init(RandomService randomService, WinWindow winWindow)
        {
            _randomService = randomService;
            _winWindow = winWindow;
        }

        public void CreateWordButtons(IReadOnlyList<char> answerChars)
        {
            _currentWordCharIndex = 0;

            _wordButtons = new WordButton[answerChars.Count];

            for (int i = 0; i < _wordButtons.Length; i++)
            {
                WordButton wordButton = Instantiate(_wordButtonPrefab, _wordContainer);
                wordButton.Init(answerChars[i], this);
                _wordButtons[i] = wordButton;
            }
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

        public void ResetAllWordButtonsAfterWrong()
        {
            ResetWordButtons();
        }

        public void ClearWordButtons()
        {
            _wordContainer.DestroyAllChildren();
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

        private void OnResetWordButtonClicked()
        {
            ResetWordButtons();
        }

        private void CheckAnswer()
        {
            if (_currentWordCharIndex == -1)
            {
                if (AnswerIsRight())
                {
                    _winWindow.ShowWindow();
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

        private void HighlightWordAsWrong()
        {
            foreach (WordButton wordButton in _wordButtons)
            {
                wordButton.SetAsWrong();
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
    }
}