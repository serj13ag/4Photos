using TMPro;
using UnityEngine;

namespace Prefabs
{
    public class WordButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void Init(string character)
        {
            _text.text = character;
        }
    }
}