using System;
using Controllers;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs
{
    public class ImageButton : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private Image _lockImage;
        [SerializeField] private Transform _openCostCoinsContainer;
        [SerializeField] private TMP_Text _openCostCoinsText;

        private ImagesController _imagesController;

        private ImageButtonState _state;
        private Sprite _imageSprite;

        public void Init(Sprite sprite, Vector2 pivot, ImageButtonState state, ImagesController imagesController)
        {
            _imageSprite = sprite;
            _imagesController = imagesController;

            _state = state;
            _rectTransform.pivot = pivot;

            _openCostCoinsText.text = $"-{Constants.OpenImageCost}";

            UpdateView(_state);

            _button.onClick.AddListener(OnImageButtonClick);
        }

        private void UpdateView(ImageButtonState state)
        {
            bool showLockImage = false;
            bool showOpenCost = false;

            Sprite sprite;
            Color color;

            switch (state)
            {
                case ImageButtonState.Opened:
                    sprite = _imageSprite;
                    color = Color.white;
                    break;
                case ImageButtonState.Closed:
                    sprite = null;
                    color = Constants.ClosedImageButtonColor;
                    showOpenCost = true;
                    break;
                case ImageButtonState.Blocked:
                    sprite = null;
                    color = Constants.BlockedImageButtonColor;
                    showLockImage = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            _image.sprite = sprite;
            _image.color = color;

            _lockImage.gameObject.SetActive(showLockImage);
            _openCostCoinsContainer.gameObject.SetActive(showOpenCost);
        }

        private void OnImageButtonClick()
        {
            if (_state == ImageButtonState.Opened)
            {
                _imagesController.ShowScalingImage(_image.sprite, _rectTransform);
            }
        }
    }
}