using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs
{
    public class KeyboardButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private MainController _mainController;

        public char Character { get; private set; }

        public void Init(char character, MainController mainController)
        {
            _mainController = mainController;

            Character = character;

            _text.text = character.ToString();

            _button.interactable = true;
            _button.onClick.AddListener(OnButtonClick);
        }

        public void Activate()
        {
            _button.interactable = true;
        }

        private void OnButtonClick()
        {
            if (_mainController.TryFillWord(this))
            {
                _button.interactable = false;

            }
        }
    }
}