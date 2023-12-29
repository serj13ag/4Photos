using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs
{
    public class WordButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private MainController _mainController;
        private char _answerCharacter;

        private char? _selectedCharacter;
        private KeyboardButton _keyboardButton;

        public bool IsLocked { get; private set; }
        public bool HasCharacter => _selectedCharacter.HasValue;

        public void Init(char answerCharacter, MainController mainController)
        {
            _mainController = mainController;
            _answerCharacter = answerCharacter;

            _text.text = string.Empty;

            _image.color = Constants.EmptyButtonColor;

            _button.interactable = false;
            _button.onClick.AddListener(OnButtonClick);
        }

        public void FillWithButton(KeyboardButton keyboardButton)
        {
            _selectedCharacter = keyboardButton.Character;
            _keyboardButton = keyboardButton;

            _text.text = keyboardButton.Character.ToString();
            _image.color = Constants.FilledByKeyboardButtonColor;
            _button.interactable = true;
        }

        public void SetAnswerCharacter()
        {
            _selectedCharacter = _answerCharacter;
            IsLocked = true;

            _text.text = _selectedCharacter.ToString();

            _image.color = Constants.FilledByHintButtonColor;

            _button.interactable = false;
        }

        public void RemoveCharacter()
        {
            _selectedCharacter = null;

            _keyboardButton.TurnInteractable();
            _keyboardButton = null;

            _mainController.UpdateCurrentWordCharIndex();

            _text.text = string.Empty;
            _image.color = Constants.EmptyButtonColor;
            _button.interactable = false;
        }

        private void OnButtonClick()
        {
            RemoveCharacter();
        }
    }
}