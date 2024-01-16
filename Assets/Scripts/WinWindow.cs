using System;
using Components;
using UnityEngine;
using UnityEngine.UI;

public class WinWindow : MonoBehaviour
{
    [SerializeField] private Image _backgroundBlocker;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private RectTransformMover _rectTransformMover;

    private Action _nextLevelButtonClickedCallback;

    private void OnEnable()
    {
        _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
    }

    private void OnDisable()
    {
        _nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
    }

    public void ShowWindow(Action nextLevelButtonClickedCallback)
    {
        _nextLevelButtonClickedCallback = nextLevelButtonClickedCallback;

        _backgroundBlocker.gameObject.SetActive(true);
        gameObject.SetActive(true);

        _rectTransformMover.MoveIn();
    }

    private void OnNextLevelButtonClicked()
    {
        _rectTransformMover.MoveOut(HideWindow);
    }

    private void HideWindow()
    {
        _nextLevelButtonClickedCallback.Invoke();
        _nextLevelButtonClickedCallback = null;

        _backgroundBlocker.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}