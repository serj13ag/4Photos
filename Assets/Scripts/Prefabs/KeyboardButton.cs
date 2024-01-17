using System;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs
{
    public class KeyboardButton : MonoBehaviour
    {
        private enum KeyboardButtonState
        {
            Active,
            Inactive,
            Hided,
        }

        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private WordController _wordController;
        private KeyboardButtonState _state;

        public char Character { get; private set; }
        public bool IsHided => _state == KeyboardButtonState.Hided;

        public void Init(char character, WordController wordController)
        {
            _wordController = wordController;

            Character = character;

            ChangeState(KeyboardButtonState.Active);

            _button.onClick.AddListener(OnButtonClick);
        }

        public void Activate()
        {
            if (!IsHided)
            {
                ChangeState(KeyboardButtonState.Active);
            }
        }

        public void Hide()
        {
            ChangeState(KeyboardButtonState.Hided);
        }

        private void OnButtonClick()
        {
            if (!IsHided && _wordController.TryFillWord(this))
            {
                ChangeState(KeyboardButtonState.Inactive);
            }
        }

        private void ChangeState(KeyboardButtonState newState)
        {
            _state = newState;

            UpdateView(newState);
        }

        private void UpdateView(KeyboardButtonState newState)
        {
            string text;
            bool isInteractable;

            switch (newState)
            {
                case KeyboardButtonState.Active:
                    text = Character.ToString();
                    isInteractable = true;
                    break;
                case KeyboardButtonState.Inactive:
                    text = Character.ToString();
                    isInteractable = false;
                    break;
                case KeyboardButtonState.Hided:
                    text = string.Empty;
                    isInteractable = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            _text.text = text;
            _button.interactable = isInteractable;
        }
    }
}