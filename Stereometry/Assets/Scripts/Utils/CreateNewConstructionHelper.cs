using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Authorization;
using Assets.Scripts.Constants;
using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewConstructionHelper
{
    private static readonly IAuthApiService AuthApiService = new AuthApiService();
    private static readonly ObjectCreator ObjectCreator = new ObjectCreator();

    public static ActionType Action { get; set; }

    public static List<GameObject> SelectedObjects { get; set; } = new List<GameObject>();

    public static void CreateObject()
    {
        switch (Action)
        {
            case ActionType.None:
                break;
            case ActionType.Dot:
                if (SelectedObjects.Count == 2)
                {
                    CreateWithShapeRotation(CreateDot);
                }

                Clear();

                break;
            case ActionType.Line:
                if (SelectedObjects.Count == 2)
                {
                    CreateWithShapeRotation(CreateLine);
                }

                Clear();

                break;
            case ActionType.ParallelLine:
                if (SelectedObjects.Count == 2)
                {
                    CreateWithShapeRotation(CreateParallelLine);
                }

                Clear();

                break;
            case ActionType.CrossSection:
                if (SelectedObjects.Count > 2)
                {
                    CreateWithShapeRotation(t => CreateCrossSection());

                    Clear();
                }

                var okButton = GameObject.Find("OK");
                okButton.GetComponent<Image>().enabled = false;
                okButton.GetComponent<Collider2D>().enabled = false;

                break;
            case ActionType.Check:
                bool result = Check();

                Clear();
                if (result)
                {
                    SendLevelCompleted();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void CreateWithShapeRotation(Action<Transform> buildAction)
    {
        var shape = Game.MainShape.transform.GetChild(0);
        Quaternion rotation = shape.transform.rotation;
        shape.transform.localRotation = Quaternion.identity;

        buildAction(shape.transform);

        shape.transform.rotation = rotation;
    }

    private static void UnselectGameObject(GameObject gameObject)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        switch (gameObject.tag)
        {
            case Tags.Edge:
                renderer.material = Resources.Load<Material>(PathConstants.PathToLinesMaterial);
                break;
            case Tags.Dot:
                renderer.material = Resources.Load<Material>(PathConstants.PathToDotsMaterial);
                break;
        }
    }

    private static void Clear()
    {
        SelectedObjects.ForEach(UnselectGameObject);
        SelectedObjects.Clear();
        Action = ActionType.None;
    }

    private static void CreateDot(Transform transform)
    {
        LineRenderer firstLineRenderer = SelectedObjects[0].GetComponent<LineRenderer>();
        LineRenderer secondLineRenderer = SelectedObjects[1].GetComponent<LineRenderer>();
        var firstLine = new Line(firstLineRenderer.GetPosition(0), firstLineRenderer.GetPosition(1));
        var secondLine = new Line(secondLineRenderer.GetPosition(0), secondLineRenderer.GetPosition(1));

        GameObject dot = ObjectCreator.CreateDotBetweenLines(firstLine, secondLine, transform, Resources.Load<Material>(PathConstants.PathToDotsMaterial));
        Game.Actions.Add(dot);
    }

    private static void CreateLine(Transform transform)
    {
        Vector3 start = SelectedObjects[0].transform.position;
        Vector3 end = SelectedObjects[1].transform.position;

        GameObject line = ObjectCreator.CreateLongLineObject(start, end, transform, Resources.Load<Material>(PathConstants.PathToLinesMaterial), 5);
        Game.Actions.Add(line);
    }

    private static void CreateParallelLine(Transform transform)
    {
        LineRenderer line;
        Vector3 dot;
        if (SelectedObjects[0].CompareTag(Tags.Edge))
        {
            line = SelectedObjects[0].transform.GetComponent<LineRenderer>();
            dot = SelectedObjects[1].transform.position;
        }
        else
        {
            dot = SelectedObjects[0].transform.position;
            line = SelectedObjects[1].transform.GetComponent<LineRenderer>();
        }

        GameObject parallelLine = ObjectCreator.CreateParallelLineThroughDot(
            new Line(line.GetPosition(0), line.GetPosition(1)), dot, transform, Resources.Load<Material>(PathConstants.PathToLinesMaterial));
        Game.Actions.Add(parallelLine);
    }

    private static void CreateCrossSection()
    {
        var points = SelectedObjects.Select(o => o.transform.position).ToArray();

        var crossSection = MultiEdgeShapeBuilder.Create(points, true);

        crossSection.GetComponent<Renderer>().material = Resources.Load<Material>(PathConstants.PathToCrossSectionMaterial);
        crossSection.transform.SetParent(Game.MainShape.transform.GetChild(0));

        Game.Actions.Add(crossSection);
        Game.CrossSections.Add(SelectedObjects.Select(o => o.transform.localPosition).ToArray());
    }

    private static bool Check()
    {
        var result = false;
        foreach (var crossSection in Game.CrossSections)
        {
            result = crossSection.All(point => Game.CurrentLevelData.Answer.Any(answer => point == answer));

            if (result)
            {
                break;
            }
        }

        return result;
    }

    private static void SendLevelCompleted()
    {
        int maxLevelSolved = PlayerPrefs.GetInt(PlayerPrefsConstants.LevelPref);
        if (maxLevelSolved == Game.CurrentLevelData.Number)
        {
            PlayerPrefs.SetInt(PlayerPrefsConstants.LevelPref, ++maxLevelSolved);
            Game.Actions.Clear();

            AuthApiService.UpdateUserAsync(new User
                { Email = PlayerPrefs.GetString(PlayerPrefsConstants.EmailPref), LevelsPassed = maxLevelSolved });
        }

        GameObject.Find(Game.CurrentLevelData.Type.ToString()).SetActive(false);
        Resources.FindObjectsOfTypeAll<GameObject>().First(go => go.name == "RightAnswerWindow").SetActive(true);
    }
}

public enum ActionType
{
    None,
    Dot,
    Line,
    ParallelLine,
    CrossSection,
    Check
}
