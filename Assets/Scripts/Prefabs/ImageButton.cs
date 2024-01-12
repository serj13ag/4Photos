using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs
{
    public class ImageButton : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        private ImagesController _imagesController;

        public void Init(Sprite sprite, Vector2 pivot, ImagesController imagesController)
        {
            _imagesController = imagesController;
            _image.sprite = sprite;
            _rectTransform.pivot = pivot;

            _button.onClick.AddListener(OnImageButtonClick);
        }

        private void OnImageButtonClick()
        {
            _imagesController.ShowScalingImage(_image.sprite, _rectTransform);
        }
    }
}