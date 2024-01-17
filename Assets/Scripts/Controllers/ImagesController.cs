using System;
using System.Collections.Generic;
using Enums;
using Extensions;
using Prefabs;
using UnityEngine;

namespace Controllers
{
    public class ImagesController : MonoBehaviour
    {
        [SerializeField] private ImageButton _imageButtonPrefab;
        [SerializeField] private ScalingImage _scalingImagePrefab;

        [SerializeField] private RectTransform _imagesGridRectTransform;
        [SerializeField] private Transform _scalingImageContainer;

        private CoinsController _coinsController;

        private int _lastOpenedImageIndex;
        private ScalingImage _scalingImage;
        private ImageButton[] _imageButtons;

        public void Init(CoinsController coinsController)
        {
            _coinsController = coinsController;
        }

        public void CreateImageButtons(IReadOnlyList<Sprite> images, int numberOfInitiallyOpenedImages)
        {
            _lastOpenedImageIndex = numberOfInitiallyOpenedImages - 1;

            _imageButtons = new ImageButton[images.Count];

            for (int i = 0; i < images.Count; i++)
            {
                ImageButton imageButton = Instantiate(_imageButtonPrefab, _imagesGridRectTransform);
                imageButton.Init(images[i], GetImagePivotByIndex(i), GetImageStateByIndex(i), this);

                _imageButtons[i] = imageButton;
            }
        }

        public void ShowScalingImage(Sprite sprite, RectTransform imageRectTransform)
        {
            _scalingImage = Instantiate(_scalingImagePrefab, _scalingImageContainer);
            _scalingImage.Init(sprite, imageRectTransform.position, imageRectTransform.sizeDelta,
                imageRectTransform.pivot, _imagesGridRectTransform.rect.size);
        }

        public void TryOpenImage()
        {
            if (_coinsController.TrySpendCoins(Constants.OpenImageCost))
            {
                _lastOpenedImageIndex++;

                UpdateImageButtonsState();
            }
        }

        public void ClearImages()
        {
            _imageButtons = null;
            _imagesGridRectTransform.DestroyAllChildren();
        }

        private void UpdateImageButtonsState()
        {
            for (int i = 0; i < _imageButtons.Length; i++)
            {
                _imageButtons[i].ChangeState(GetImageStateByIndex(i));
            }
        }

        private ImageButtonState GetImageStateByIndex(int index)
        {
            if (index <= _lastOpenedImageIndex)
            {
                return ImageButtonState.Opened;
            }

            if (index == _lastOpenedImageIndex + 1)
            {
                return ImageButtonState.Closed;
            }

            return ImageButtonState.Blocked;
        }

        private static Vector2 GetImagePivotByIndex(int index)
        {
            return index switch
            {
                0 => new Vector2(0, 1),
                1 => new Vector2(1, 1),
                2 => new Vector2(0, 0),
                3 => new Vector2(1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null),
            };
        }
    }
}