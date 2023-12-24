using Prefabs;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    private const int NumberOfKeyboardButtons = 14;

    [SerializeField] private LevelStaticData[] _levelsStaticData;

    [SerializeField] private Image _imagePrefab;
    [SerializeField] private WordButton _wordButtonPrefab;
    [SerializeField] private GameObject _keyboardButtonPrefab;

    [SerializeField] private Transform _imagesContainer;
    [SerializeField] private Transform _wordContainer;
    [SerializeField] private Transform _keyboardContainer;

    private void Start()
    {
        ClearContainers();

        LevelStaticData currentLevelStaticData = _levelsStaticData[0];

        CreateImages(currentLevelStaticData);
        CreateWordButtons(currentLevelStaticData);
        CreateKeyboardButtons();
    }

    private void CreateImages(LevelStaticData currentLevelStaticData)
    {
        foreach (Sprite image in currentLevelStaticData.Images)
        {
            Image imagePrefab = Instantiate(_imagePrefab, _imagesContainer);
            imagePrefab.sprite = image;
        }
    }

    private void CreateWordButtons(LevelStaticData currentLevelStaticData)
    {
        foreach (var character in currentLevelStaticData.Word.ToUpper())
        {
            var wordButton = Instantiate(_wordButtonPrefab, _wordContainer);
            wordButton.Init(character.ToString());
        }
    }

    private void CreateKeyboardButtons()
    {
        for (int i = 0; i < NumberOfKeyboardButtons; i++)
        {
            Instantiate(_keyboardButtonPrefab, _keyboardContainer);
        }
    }

    private void ClearContainers()
    {
        DestroyAllChildren(_imagesContainer);
        DestroyAllChildren(_wordContainer);
        DestroyAllChildren(_keyboardContainer);
    }

    private static void DestroyAllChildren(Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
}