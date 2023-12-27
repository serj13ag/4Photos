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

        private KeyboardButton _keyboardButton;

        public void Init()
        {
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

            _text.text = string.Empty;
            _image.color = Constants.EmptyButtonColor;
            _button.interactable = false;
        }
    }
}