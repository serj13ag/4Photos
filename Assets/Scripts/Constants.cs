using UnityEngine;

public static class Constants
{
    public const string AlphabetCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const int NumberOfImages = 4;
    public const int NumberOfKeyboardButtons = 14;
    public const int MaxNumberOfWordCharacters = 7;

    public static Color EmptyButtonColor = Color.white;
    public static Color FilledByKeyboardButtonColor = new Color32(233, 196, 106, 255);
    public static Color FilledByHintButtonColor = new Color32(42, 157, 143, 255);
    public static Color WrongCharacterButtonColor = new Color32(231, 111, 81, 255);
}