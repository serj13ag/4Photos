using Services;
using UnityEngine;

namespace Controllers
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private LevelController _levelController;
        [SerializeField] private CoinsController _coinsController;
        [SerializeField] private ImagesController _imagesController;
        [SerializeField] private WordController _wordController;
        [SerializeField] private KeyboardController _keyboardController;

        [SerializeField] private WinWindow _winWindow;

        [SerializeField] private GameObject _backgroundBlocker;

        private RandomService _randomService;

        private void Start()
        {
            _randomService = new RandomService();

            _levelController.Init(_randomService, _imagesController, _wordController, _keyboardController);
            _wordController.Init(_randomService, _winWindow);
            _keyboardController.Init(_randomService, _levelController, _wordController);
            _imagesController.Init(_coinsController);

            _winWindow.Init(_levelController, _backgroundBlocker);

            _coinsController.SetCoinsAmount(Constants.InitialCoinsAmount);

            _levelController.CreateInitialLevel();
        }
    }
}