using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class CoinsController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private Button _incrementCoinsButton;

        private int _coins;

        private void OnEnable()
        {
            _incrementCoinsButton.onClick.AddListener(OnIncrementCoinsButtonClicked);
        }

        private void OnDisable()
        {
            _incrementCoinsButton.onClick.RemoveListener(OnIncrementCoinsButtonClicked);
        }

        public void SetCoinsAmount(int coins)
        {
            _coins = coins;
            UpdateCoinsText();
        }

        public bool TrySpendCoins(int coins)
        {
            if (_coins < coins)
            {
                return false;
            }

            _coins -= coins;
            UpdateCoinsText();
            return true;
        }

        private void OnIncrementCoinsButtonClicked()
        {
            _coins++;
            UpdateCoinsText();
        }

        private void UpdateCoinsText()
        {
            _coinsText.text = _coins.ToString();
        }
    }
}