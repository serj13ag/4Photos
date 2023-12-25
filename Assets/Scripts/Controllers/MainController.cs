using System.Collections.Generic;
using Extensions;
using Prefabs;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Controllers
{
    public class MainController : MonoBehaviour
    {
        private const string AlphabetCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const int NumberOfKeyboardButtons = 14;

        [SerializeField] private LevelStaticData[] _levelsStaticData;

        [SerializeField] private Image _imagePrefab;
        [SerializeField] private WordButton _wordButtonPrefab;
        [SerializeField] private KeyboardButton _keyboardButtonPrefab;

        [SerializeField] private Transform _imagesContainer;
        [SerializeField] private Transform _wordContainer;
        [SerializeField] private Transform _keyboardContainer;

        private string _answerWord;

        private void Start()
        {
            Random randomService = new Random();

            ClearContainers();

            LevelStaticData currentLevelStaticData = _levelsStaticData[0];

            _answerWord = currentLevelStaticData.Word.ToUpper();
            string[] charactersForKeyboard = GetCharactersForKeyboard(_answerWord, randomService);

            CreateImages(currentLevelStaticData.Images);
            CreateWordButtons(_answerWord.Length);
            CreateKeyboardButtons(charactersForKeyboard);
        }

        private static string[] GetCharactersForKeyboard(string answerWord, Random randomService)
        {
            string[] randomCharacters = new string[NumberOfKeyboardButtons];

            for (int i = 0; i < randomCharacters.Length; i++)
            {
                char character = i < answerWord.Length
                    ? answerWord[i]
                    : AlphabetCharacters[randomService.Next(AlphabetCharacters.Length)];

                randomCharacters[i] = character.ToString().ToUpper();
            }

            randomCharacters.Shuffle(randomService);
            return randomCharacters;
        }

        private void CreateImages(IEnumerable<Sprite> images)
        {
            foreach (Sprite image in images)
            {
                Image imagePrefab = Instantiate(_imagePrefab, _imagesContainer);
                imagePrefab.sprite = image;
            }
        }

        private void CreateWordButtons(int numberOfButtons)
        {
            for (int i = 0; i < numberOfButtons; i++)
            {
                WordButton wordButton = Instantiate(_wordButtonPrefab, _wordContainer);
                wordButton.Init();
            }
        }

        private void CreateKeyboardButtons(IReadOnlyList<string> randomCharacters)
        {
            for (int i = 0; i < NumberOfKeyboardButtons; i++)
            {
                var keyboardButtonPrefab = Instantiate(_keyboardButtonPrefab, _keyboardContainer);
                keyboardButtonPrefab.Init(randomCharacters[i]);
            }
        }

        private void ClearContainers()
        {
            DestroyAllChildren(_imagesContainer);
            DestroyAllChildren(_wordContainer);
            DestroyAllChildren(_keyboardContainer);
        }

        private static void DestroyAllChildren(Transform container)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
    }
}