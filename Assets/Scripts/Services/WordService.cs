using System;
using EventArgs;

namespace Services
{
    public class WordService
    {
        private readonly char[] _wordChars;
        private int _currentWordCharIndex;

        public char[] AnswerChars { get; }

        public event EventHandler<CharacterAddedEventArgs> OnCharacterAdded;

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
                int index = _currentWordCharIndex++;
                _wordChars[index] = character;
                OnCharacterAdded?.Invoke(this, new CharacterAddedEventArgs(character, index));
            }
        }
    }
}