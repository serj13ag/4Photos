using ScriptableObjects;
using Services;
using UnityEngine;

namespace Controllers
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private LevelStaticData[] _levelsStaticData;

        [SerializeField] private LevelController _levelController;
        [SerializeField] private CoinsController _coinsController;
        [SerializeField] private ImagesController _imagesController;
        [SerializeField] private WordController _wordController;
        [SerializeField] private KeyboardController _keyboardController;

        [SerializeField] private WinWindow _winWindow;

        [SerializeField] private GameObject _backgroundBlocker;

        private void Start()
        {
            RandomService randomService = new RandomService();
            StaticDataService staticDataService = new StaticDataService(_levelsStaticData);

            _levelController.Init(randomService, staticDataService, _imagesController, _wordController, _keyboardController);
            _wordController.Init(randomService, _winWindow);
            _keyboardController.Init(randomService, _levelController, _wordController);
            _imagesController.Init(_coinsController);

            _winWindow.Init(_levelController, _backgroundBlocker);

            _coinsController.SetCoinsAmount(Constants.InitialCoinsAmount);

            _levelController.CreateInitialLevel();
        }
    }
}