using System;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs
{
    public class WordButton : MonoBehaviour
    {
        private enum WordButtonState
        {
            Empty,
            FilledByKeyboard,
            FilledByHint,
            Wrong,
        }

        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private WordController _wordController;
        private char _answerCharacter;

        private WordButtonState _state;
        private char? _filledCharacter;
        private KeyboardButton _keyboardButtonUsedToFillCharacter;

        public bool IsFilledWithCharacter => _filledCharacter.HasValue;
        public bool IsFilledWithAnswerCharacter => _filledCharacter == _answerCharacter;
        public bool IsLocked => _state == WordButtonState.FilledByHint;

        public void Init(char answerCharacter, WordController wordController)
        {
            _wordController = wordController;
            _answerCharacter = answerCharacter;

            ChangeState(WordButtonState.Empty);

            _button.onClick.AddListener(OnButtonClick);
        }

        public void FillByKeyboard(KeyboardButton keyboardButton)
        {
            _filledCharacter = keyboardButton.Character;
            _keyboardButtonUsedToFillCharacter = keyboardButton;

            ChangeState(WordButtonState.FilledByKeyboard);
        }

        public void FillByHint()
        {
            _filledCharacter = _answerCharacter;

            ChangeState(WordButtonState.FilledByHint);
        }

        public void SetAsEmpty()
        {
            _filledCharacter = null;

            if (_keyboardButtonUsedToFillCharacter != null)
            {
                _keyboardButtonUsedToFillCharacter.Activate();
                _keyboardButtonUsedToFillCharacter = null;
            }

            _wordController.UpdateCurrentWordCharIndex();

            ChangeState(WordButtonState.Empty);
        }

        public void SetAsWrong()
        {
            ChangeState(WordButtonState.Wrong);
        }

        private void OnButtonClick()
        {
            if (_state == WordButtonState.Wrong)
            {
                _wordController.ResetAllWordButtonsAfterWrong();
            }
            else
            {
                SetAsEmpty();
            }
        }

        private void ChangeState(WordButtonState newState)
        {
            _state = newState;
            UpdateView(newState);
        }

        private void UpdateView(WordButtonState newState)
        {
            string text;
            Color color;
            bool isInteractable;

            switch (newState)
            {
                case WordButtonState.Empty:
                    text = string.Empty;
                    color = Constants.EmptyButtonColor;
                    isInteractable = false;
                    break;
                case WordButtonState.FilledByKeyboard:
                    text = _filledCharacter.ToString();
                    color = Constants.FilledByKeyboardButtonColor;
                    isInteractable = true;
                    break;
                case WordButtonState.FilledByHint:
                    text = _filledCharacter.ToString();
                    color = Constants.FilledByHintButtonColor;
                    isInteractable = false;
                    break;
                case WordButtonState.Wrong:
                    text = _filledCharacter.ToString();
                    color = Constants.WrongCharacterButtonColor;
                    isInteractable = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            _text.text = text;
            _text.color = color;
            _image.color = color;
            _button.interactable = isInteractable;
        }
    }
}