using Components;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class WinWindow : MonoBehaviour
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private RectTransformMover _rectTransformMover;

        private LevelController _levelController;
        private GameObject _backgroundBlocker;

        private void OnEnable()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        }

        private void OnDisable()
        {
            _nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
        }

        public void Init(LevelController levelController, GameObject backgroundBlocker)
        {
            _levelController = levelController;
            _backgroundBlocker = backgroundBlocker;
        }

        public void ShowWindow()
        {
            _backgroundBlocker.SetActive(true);
            gameObject.SetActive(true);

            _rectTransformMover.MoveIn();
        }

        private void OnNextLevelButtonClicked()
        {
            _rectTransformMover.MoveOut(HideWindow);
        }

        private void HideWindow()
        {
            _levelController.ChangeLevelToNext();

            _backgroundBlocker.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}