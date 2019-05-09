using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static string PathToLinesMaterial = "Materials/Lines";

    public static string PathToDotsMaterial = "Materials/Dots";

    public static string PathToPrefabs = "Prefabs/";

    public static List<GameObject> Actions = new List<GameObject>();

    public static int MaxLevel = 3;

    private static string PathToLevelsDataFile = "Data/levels";

    public static Level CurrentLevelData { get; private set; }

    public static void FillLevelData(int levelNumber)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(PathToLevelsDataFile, typeof(TextAsset));
        var levelLoader = new XmlDataLoader(textAsset.text);
        CurrentLevelData = levelLoader.LoadLevel(levelNumber);
    }
}
