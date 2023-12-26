using System.Collections.Generic;
using EventArgs;
using Prefabs;
using ScriptableObjects;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private LevelStaticData[] _levelsStaticData;

        [SerializeField] private Image _imagePrefab;
        [SerializeField] private WordButton _wordButtonPrefab;
        [SerializeField] private KeyboardButton _keyboardButtonPrefab;

        [SerializeField] private Transform _imagesContainer;
        [SerializeField] private Transform _wordContainer;
        [SerializeField] private Transform _keyboardContainer;

        private WordButton[] _wordButtons;

        private void Start()
        {
            LevelStaticData currentLevelStaticData = _levelsStaticData[0];

            RandomService randomService = new RandomService();
            WordService wordService = new WordService(currentLevelStaticData.Word.ToUpper());

            char[] charactersForKeyboard = randomService.GetCharactersForKeyboard(wordService.AnswerChars, Constants.NumberOfKeyboardButtons);

            ClearContainers();
            CreateImages(currentLevelStaticData.Images);
            CreateWordButtons(wordService.AnswerChars.Length);
            CreateKeyboardButtons(charactersForKeyboard, wordService);

            wordService.OnCharacterAdded += OnWordCharacterAdded;
        }

        private void OnWordCharacterAdded(object sender, CharacterAddedEventArgs e)
        {
            _wordButtons[e.Index].SetCharacter(e.Character);
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
            _wordButtons = new WordButton[numberOfButtons];

            for (int i = 0; i < numberOfButtons; i++)
            {
                WordButton wordButton = Instantiate(_wordButtonPrefab, _wordContainer);
                wordButton.Init();
                _wordButtons[i] = wordButton;
            }
        }

        private void CreateKeyboardButtons(IReadOnlyList<char> randomCharacters, WordService wordService)
        {
            for (int i = 0; i < Constants.NumberOfKeyboardButtons; i++)
            {
                var keyboardButtonPrefab = Instantiate(_keyboardButtonPrefab, _keyboardContainer);
                keyboardButtonPrefab.Init(randomCharacters[i], wordService);
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