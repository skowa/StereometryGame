using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static string PathToLinesMaterial = "Materials/Lines";

    public static string PathToDotsMaterial = "Materials/Dots";

    public static string PathToPrefabs = "Prefabs/";

    public static List<GameObject> Actions = new List<GameObject>();

    private static string PathToLevelsDataFile = "Data/levels";

    public static Level CurrentLevelData { get; private set; }

    public static void FillLevelData(int levelNumber)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(PathToLevelsDataFile);
        var levelLoader = new XmlDataLoader("Assets/Resources/" + PathToLevelsDataFile + ".xml");
        CurrentLevelData = levelLoader.LoadLevel(levelNumber);
    }
}
