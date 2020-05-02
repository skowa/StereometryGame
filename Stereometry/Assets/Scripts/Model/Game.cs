using System.Collections.Generic;
using Assets.Scripts.Constants;
using Assets.Scripts.DataLoading;
using UnityEngine;

public static class Game
{
    public static List<GameObject> Actions { get; } = new List<GameObject>();

    public static List<Vector3[]> CrossSections { get; } = new List<Vector3[]>();

    public static int MaxLevel => 4;

    public static Level CurrentLevelData { get; private set; }

    public static GameObject MainShape { get; set; }

    public static bool IsAR { get; set; }

    public static bool IsLevelFilled { get; set; }

    public static void FillLevelData(int levelNumber)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(PathConstants.PathToLevelsDataFile, typeof(TextAsset));
        var levelLoader = new XmlDataLoader(textAsset.text);
        // var levelLoader = new JsonDataLoader(PathToLevelsDataFile);
        CurrentLevelData = levelLoader.LoadLevel(levelNumber);
    }
}