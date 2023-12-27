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

        private KeyboardButton _keyboardButton;

        public bool HasCharacter => _keyboardButton != null;

        public void Init(MainController mainController)
        {
            _mainController = mainController;

            _text.text = string.Empty;

            _image.color = Constants.EmptyButtonColor;

            _button.interactable = false;
            _button.onClick.AddListener(OnButtonClick);
        }

        public void FillWithButton(KeyboardButton keyboardButton)
        {
            _keyboardButton = keyboardButton;

            _text.text = keyboardButton.Character.ToString();
            _image.color = Constants.FilledButtonColor;
            _button.interactable = true;
        }

        private void OnButtonClick()
        {
            _keyboardButton.TurnInteractable();
            _keyboardButton = null;
            
            _mainController.WordButtonClicked();

            _text.text = string.Empty;
            _image.color = Constants.EmptyButtonColor;
            _button.interactable = false;
        }
    }
}