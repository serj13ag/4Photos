using TMPro;
using UnityEngine;

namespace Prefabs
{
    public class WordButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void Init()
        {
            _text.text = string.Empty;
        }

        public void SetCharacter(char character)
        {
            _text.text = character.ToString();
        }
    }
}