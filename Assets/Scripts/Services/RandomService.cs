using System;
using System.Collections.Generic;
using Extensions;

namespace Services
{
    public class RandomService
    {
        private readonly Random _random;

        public RandomService()
        {
            _random = new Random();
        }

        public char[] GetCharactersForKeyboard(IReadOnlyList<char> charsToInclude, int count)
        {
            char[] randomCharacters = new char[count];

            for (int i = 0; i < randomCharacters.Length; i++)
            {
                randomCharacters[i] = i < charsToInclude.Count
                    ? charsToInclude[i]
                    : Constants.AlphabetCharacters[_random.Next(Constants.AlphabetCharacters.Length)];
            }

            randomCharacters.Shuffle(_random);
            return randomCharacters;
        }
    }
}