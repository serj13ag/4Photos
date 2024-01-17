using System.Collections.Generic;
using System.Linq;
using Extensions;
using Prefabs;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class KeyboardController : MonoBehaviour
    {
        [SerializeField] private MainController _mainController;
        [SerializeField] private WordController _wordController;

        [SerializeField] private KeyboardButton _keyboardButtonPrefab;
        [SerializeField] private Transform _keyboardContainer;

        [SerializeField] private Button _hintHideWrongKeyboardCharacterButton;

        private RandomService _randomService;

        private KeyboardButton[] _keyboardButtons;

        private void OnEnable()
        {
            _hintHideWrongKeyboardCharacterButton.onClick.AddListener(OnHintHideWrongKeyboardCharacterButtonClicked);
        }

        private void OnDisable()
        {
            _hintHideWrongKeyboardCharacterButton.onClick.RemoveListener(OnHintHideWrongKeyboardCharacterButtonClicked);
        }

        public void Init(RandomService randomService)
        {
            _randomService = randomService;
        }

        public void CreateKeyboardButtons(IReadOnlyList<char> randomCharacters)
        {
            _keyboardButtons = new KeyboardButton[randomCharacters.Count];

            for (int i = 0; i < randomCharacters.Count; i++)
            {
                KeyboardButton keyboardButton = Instantiate(_keyboardButtonPrefab, _keyboardContainer);
                keyboardButton.Init(randomCharacters[i], _wordController);
                _keyboardButtons[i] = keyboardButton;
            }
        }

        public void ClearKeyboardButtons()
        {
            _keyboardContainer.DestroyAllChildren();
        }

        private void OnHintHideWrongKeyboardCharacterButtonClicked()
        {
            foreach (KeyboardButton keyboardButton in _keyboardButtons.OrderBy(_ => _randomService.Random.Next()))
            {
                if (!keyboardButton.IsHided && !_mainController.AnswerWord.Contains(keyboardButton.Character))
                {
                    keyboardButton.Hide();
                    return;
                }
            }
        }
    }
}