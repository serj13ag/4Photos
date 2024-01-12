using System;
using System.Collections.Generic;
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

        private ScalingImage _scalingImage;

        public void CreateImages(IReadOnlyList<Sprite> images)
        {
            for (int i = 0; i < images.Count; i++)
            {
                ImageButton imagePrefab = Instantiate(_imageButtonPrefab, _imagesGridRectTransform);
                imagePrefab.Init(images[i], GetImagePivotByIndex(i), this);
            }
        }

        public void ShowScalingImage(Sprite sprite, RectTransform imageRectTransform)
        {
            _scalingImage = Instantiate(_scalingImagePrefab, _scalingImageContainer);
            _scalingImage.Init(sprite, imageRectTransform.position, imageRectTransform.sizeDelta,
                imageRectTransform.pivot, _imagesGridRectTransform.rect.size);
        }

        public void ClearContainer()
        {
            _imagesGridRectTransform.DestroyAllChildren();
        }

        private static Vector2 GetImagePivotByIndex(int i)
        {
            return i switch
            {
                0 => new Vector2(0, 1),
                1 => new Vector2(1, 1),
                2 => new Vector2(0, 0),
                3 => new Vector2(1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
            };
        }
    }
}