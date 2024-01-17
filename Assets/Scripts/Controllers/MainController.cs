using System.Collections.Generic;
using System.Linq;
using Extensions;
using Prefabs;
using ScriptableObjects;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private LevelStaticData[] _levelsStaticData;

        [SerializeField] private CoinsController _coinsController;
        [SerializeField] private ImagesController _imagesController;
        [SerializeField] private WordController _wordController;

        [SerializeField] private KeyboardButton _keyboardButtonPrefab;
        [SerializeField] private Transform _keyboardContainer;

        [SerializeField] private TMP_Text _levelNumberText;

        [SerializeField] private Button _hintHideWrongKeyboardCharacterButton;

        private RandomService _randomService;

        private int _currentLevelIndex;
        private string _answerWord;
        private KeyboardButton[] _keyboardButtons;

        private void Start()
        {
            Cleanup();

            _randomService = new RandomService();

            _currentLevelIndex = 0;

            _wordController.Init(_randomService);
            _coinsController.SetCoinsAmount(Constants.InitialCoinsAmount);

            InitLevelWithCurrentIndex();
        }

        private void OnEnable()
        {
            _hintHideWrongKeyboardCharacterButton.onClick.AddListener(OnHintHideWrongKeyboardCharacterButtonClicked);
        }

        private void OnDisable()
        {
            _hintHideWrongKeyboardCharacterButton.onClick.RemoveListener(OnHintHideWrongKeyboardCharacterButtonClicked);
        }

        private void InitLevelWithCurrentIndex()
        {
            LevelStaticData currentLevelStaticData = _levelsStaticData[_currentLevelIndex];

            _answerWord = currentLevelStaticData.Word.ToUpper();
            char[] answerChars = _answerWord.ToCharArray();
            char[] charactersForKeyboard = _randomService.GetCharactersForKeyboard(answerChars, Constants.NumberOfKeyboardButtons);

            _levelNumberText.text = (_currentLevelIndex + 1).ToString();

            _imagesController.CreateImageButtons(currentLevelStaticData.Images, Constants.NumberOfInitiallyOpenedImages);
            _wordController.CreateWordButtons(answerChars);
            CreateKeyboardButtons(charactersForKeyboard);
        }

        public void ChangeLevelToNext()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex > _levelsStaticData.Length - 1)
            {
                _currentLevelIndex = 0;
            }

            Cleanup();
            InitLevelWithCurrentIndex();
        }

        private void OnHintHideWrongKeyboardCharacterButtonClicked()
        {
            foreach (KeyboardButton keyboardButton in _keyboardButtons.OrderBy(_ => _randomService.Random.Next()))
            {
                if (!keyboardButton.IsHided && !_answerWord.Contains(keyboardButton.Character))
                {
                    keyboardButton.Hide();
                    return;
                }
            }
        }

        private void CreateKeyboardButtons(IReadOnlyList<char> randomCharacters)
        {
            _keyboardButtons = new KeyboardButton[randomCharacters.Count];

            for (int i = 0; i < randomCharacters.Count; i++)
            {
                KeyboardButton keyboardButton = Instantiate(_keyboardButtonPrefab, _keyboardContainer);
                keyboardButton.Init(randomCharacters[i], _wordController);
                _keyboardButtons[i] = keyboardButton;
            }
        }

        private void Cleanup()
        {
            _imagesController.ClearImages();
            _wordController.ClearWordButtons();
            _keyboardContainer.DestroyAllChildren();
        }
    }
}