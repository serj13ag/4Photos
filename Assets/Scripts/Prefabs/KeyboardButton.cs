using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs
{
    public class KeyboardButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;

        private WordService _wordService;

        private char _character;

        public void Init(char character, WordService wordService)
        {
            _character = character;
            _wordService = wordService;

            _text.text = character.ToString();

            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _wordService.AddCharacter(_character);
        }
    }
}