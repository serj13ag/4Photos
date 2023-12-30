using UnityEngine;
using UnityEngine.UI;

namespace Prefabs
{
    public class ImageButton : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        public void Init(Sprite sprite, Vector2 pivot)
        {
            _image.sprite = sprite;
            _rectTransform.pivot = pivot;

            _button.onClick.AddListener(OnImageButtonClick);
        }

        private void OnImageButtonClick()
        {
            // Scale image
        }
    }
}