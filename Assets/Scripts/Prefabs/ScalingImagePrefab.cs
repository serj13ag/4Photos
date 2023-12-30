using UnityEngine;
using UnityEngine.UI;

namespace Prefabs
{
    public class ScalingImagePrefab : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;

        private Vector2 _initialSize;
        private Vector2 _targetSize;
        private Vector2 _currentTargetSize;

        public void Init(Sprite sprite, Vector3 position, Vector2 size, Vector2 pivot, Vector2 targetSize)
        {
            _initialSize = size;
            _targetSize = targetSize;

            _image.sprite = sprite;
            _rectTransform.pivot = pivot;
            _rectTransform.position = position;
            _rectTransform.sizeDelta = size;

            _currentTargetSize = targetSize;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void Update()
        {
            _rectTransform.sizeDelta = Vector2.Lerp(_rectTransform.sizeDelta, _currentTargetSize, Time.deltaTime * 10f);

            if (Vector2.Distance(_rectTransform.sizeDelta, _currentTargetSize) < 1f)
            {
                _rectTransform.sizeDelta = _currentTargetSize;

                if (_currentTargetSize == _initialSize)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnClick()
        {
            if (_currentTargetSize == _initialSize)
            {
                _currentTargetSize = _targetSize;
            }
            else if (_currentTargetSize == _targetSize)
            {
                _currentTargetSize = _initialSize;
            }
        }
    }
}