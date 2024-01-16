using System;
using UnityEngine;
using UnityEngine.UI;

public class WinWindow : MonoBehaviour
{
    [SerializeField] private Image _backgroundBlocker;
    [SerializeField] private Button _nextLevelButton;

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
    }

    private void OnNextLevelButtonClicked()
    {
        _nextLevelButtonClickedCallback.Invoke();

        HideWindow();
    }

    private void HideWindow()
    {
        _backgroundBlocker.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}