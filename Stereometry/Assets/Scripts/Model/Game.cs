using System.Collections.Generic;
using Assets.Scripts.DataLoading;
using UnityEngine;

public static class Game
{
    private static string PathToLevelsDataFile = "Data/levels";
    public static string PathToLinesMaterial => "Materials/Lines";
    public static string PathToDotsMaterial => "Materials/Dots";
    public static string PathToFilledMaterial => "Materials/MainShapeFilled";
    public static string PathToTransparentMaterial => "Materials/Transparent";
    public static string PathToPrefabs => "Prefabs/";
    public static List<GameObject> Actions { get; } = new List<GameObject>();
    public static int MaxLevel => 3;
    public static Level CurrentLevelData { get; private set; }
    public static GameObject MainShape { get; set; }
    public static bool IsAR { get; set; }
    public static bool IsLevelFilled { get; set; }

    public static void FillLevelData(int levelNumber)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(PathToLevelsDataFile, typeof(TextAsset));
        var levelLoader = new XmlDataLoader(textAsset.text);
        CurrentLevelData = levelLoader.LoadLevel(levelNumber);
    }
}
