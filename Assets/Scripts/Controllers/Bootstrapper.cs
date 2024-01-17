using Services;
using UnityEngine;

namespace Controllers
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private LevelController _levelController;
        [SerializeField] private CoinsController _coinsController;
        [SerializeField] private WordController _wordController;
        [SerializeField] private KeyboardController _keyboardController;

        private RandomService _randomService;

        private void Start()
        {
            _randomService = new RandomService();

            _levelController.Init(_randomService);
            _wordController.Init(_randomService);
            _keyboardController.Init(_randomService);

            _coinsController.SetCoinsAmount(Constants.InitialCoinsAmount);

            _levelController.CreateInitialLevel();
        }
    }
}