using TMPro;
using UnityEngine;

namespace Services
{
    public class WordService
    {
        private readonly char[] _wordChars;
        private int _currentWordCharIndex;

        public char[] AnswerChars { get; }

        public WordService(string answer)
        {
            AnswerChars = answer.ToCharArray();

            _wordChars = new char[answer.Length];
            _currentWordCharIndex = 0;
        }

        public void AddCharacter(char character)
        {
            if (_currentWordCharIndex < _wordChars.Length)
            {
                _wordChars[_currentWordCharIndex++] = character;
                Debug.LogError(_wordChars.ArrayToString()); // TODO: remove
            }
        }
    }
}