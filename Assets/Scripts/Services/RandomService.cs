using System;
using System.Collections.Generic;

namespace Services
{
    public class RandomService
    {
        public Random Random { get; }

        public RandomService()
        {
            Random = new Random();
        }

        public char[] GetCharactersForKeyboard(IReadOnlyList<char> charsToInclude, int count)
        {
            char[] randomCharacters = new char[count];

            for (int i = 0; i < randomCharacters.Length; i++)
            {
                randomCharacters[i] = i < charsToInclude.Count
                    ? charsToInclude[i]
                    : Constants.AlphabetCharacters[Random.Next(Constants.AlphabetCharacters.Length)];
            }

            randomCharacters.Shuffle(Random);
            return randomCharacters;
        }
    }
}