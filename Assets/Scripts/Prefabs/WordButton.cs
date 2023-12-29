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
        }

        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private MainController _mainController;
        private char _answerCharacter;

        private WordButtonState _state;
        private char? _selectedCharacter;
        private KeyboardButton _keyboardButton;

        public bool HasCharacter => _selectedCharacter.HasValue;
        public bool IsLocked => _state == WordButtonState.FilledByHint;

        public void Init(char answerCharacter, MainController mainController)
        {
            _mainController = mainController;
            _answerCharacter = answerCharacter;

            ChangeState(WordButtonState.Empty);

            _button.onClick.AddListener(OnButtonClick);
        }

        public void FillByKeyboard(KeyboardButton keyboardButton)
        {
            _selectedCharacter = keyboardButton.Character;
            _keyboardButton = keyboardButton;

            ChangeState(WordButtonState.FilledByKeyboard);
        }

        public void FillByHint()
        {
            _selectedCharacter = _answerCharacter;

            ChangeState(WordButtonState.FilledByHint);
        }

        public void SetAsEmpty()
        {
            _selectedCharacter = null;

            _keyboardButton.TurnInteractable();
            _keyboardButton = null;

            _mainController.UpdateCurrentWordCharIndex();

            ChangeState(WordButtonState.Empty);
        }

        private void OnButtonClick()
        {
            SetAsEmpty();
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
                    text = _selectedCharacter.ToString();
                    color = Constants.FilledByKeyboardButtonColor;
                    isInteractable = true;
                    break;
                case WordButtonState.FilledByHint:
                    text = _selectedCharacter.ToString();
                    color = Constants.FilledByHintButtonColor;
                    isInteractable = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            _text.text = text;
            _image.color = color;
            _button.interactable = isInteractable;
        }
    }
}