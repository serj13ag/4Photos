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

            UpdateView();

            _button.onClick.AddListener(OnImageButtonClick);
        }

        public void ChangeState(ImageButtonState newState)
        {
            if (_state != newState)
            {
                _state = newState;
                UpdateView();
            }
        }

        private void UpdateView()
        {
            bool showLockImage = false;
            bool showOpenCost = false;
            Sprite sprite = null;
            Color color;

            switch (_state)
            {
                case ImageButtonState.Opened:
                    sprite = _imageSprite;
                    color = Color.white;
                    break;
                case ImageButtonState.Closed:
                    color = Constants.ClosedImageButtonColor;
                    showOpenCost = true;
                    break;
                case ImageButtonState.Blocked:
                    color = Constants.BlockedImageButtonColor;
                    showLockImage = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
            }

            _image.sprite = sprite;
            _image.color = color;

            _lockImage.gameObject.SetActive(showLockImage);
            _openCostCoinsContainer.gameObject.SetActive(showOpenCost);
        }

        private void OnImageButtonClick()
        {
            switch (_state)
            {
                case ImageButtonState.Opened:
                    _imagesController.ShowScalingImage(_image.sprite, _rectTransform);
                    break;
                case ImageButtonState.Closed:
                    _imagesController.TryOpenImage();
                    break;
            }
        }
    }
}