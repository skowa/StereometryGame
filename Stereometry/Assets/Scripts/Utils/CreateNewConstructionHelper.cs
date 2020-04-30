using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Authorization;
using Assets.Scripts.Behaviour.ButtonsActions;
using Assets.Scripts.Constants;
using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using UnityEngine;

public class CreateNewConstructionHelper
{
    private static readonly IAuthApiService AuthApiService = new AuthApiService();
    private static readonly ObjectCreator ObjectCreator = new ObjectCreator();
    private static readonly Material LineMaterial = Resources.Load<Material>(PathConstants.PathToLinesMaterial);
    private static readonly Material DotMaterial = Resources.Load<Material>(PathConstants.PathToDotsMaterial);

    public static ActionType Action { get; set; }

    public static List<GameObject> SelectedObjects { get; set; } = new List<GameObject>();

    public static void CreateObject()
    {
        switch (Action)
        {
            case ActionType.Dot:
                if (SelectedObjects.Count == 2)
                {
                    CreateWithShapeRotation(CreateDot);
                }

                ResetAction();

                break;
            case ActionType.Line:
                if (SelectedObjects.Count == 2)
                {
                    CreateWithShapeRotation(CreateLine);
                }

                ResetAction();

                break;
            case ActionType.ParallelLine:
                if (SelectedObjects.Count == 2)
                {
                    CreateWithShapeRotation(CreateParallelLine);
                }

                ResetAction();

                break;
            case ActionType.CrossSection:
                if (SelectedObjects.Count > 2)
                {
                    CreateWithShapeRotation(t => CreateCrossSection());

                    ResetAction();
                }

                GameOkButton.Enable(false);

                break;
            case ActionType.Check:
                bool result = Check();

                ResetAction();
                if (result)
                {
                    SendLevelCompleted();
                }

                break;
        }
    }

    public static void ResetAction()
    {
        var buildButtonsManager = GameObject.Find(GameObjectNames.GameScene.BuildButtonsManager);
        buildButtonsManager.GetComponent<BuildButtonsManager>().ResetButtonByActionType(Action);

        SelectedObjects.ForEach(UnselectGameObject);
        SelectedObjects.Clear();
        Action = ActionType.None;
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
                renderer.material = LineMaterial;
                break;
            case Tags.Dot:
                renderer.material = DotMaterial;
                break;
        }
    }

    private static void CreateDot(Transform transform)
    {
        LineRenderer firstLineRenderer = SelectedObjects[0].GetComponent<LineRenderer>();
        LineRenderer secondLineRenderer = SelectedObjects[1].GetComponent<LineRenderer>();
        var firstLine = new Line(firstLineRenderer.GetPosition(0), firstLineRenderer.GetPosition(1));
        var secondLine = new Line(secondLineRenderer.GetPosition(0), secondLineRenderer.GetPosition(1));

        GameObject dot = ObjectCreator.CreateDotBetweenLines(firstLine, secondLine, transform, DotMaterial);
        Game.Actions.Add(dot);
    }

    private static void CreateLine(Transform transform)
    {
        Vector3 start = SelectedObjects[0].transform.position;
        Vector3 end = SelectedObjects[1].transform.position;

        GameObject line = ObjectCreator.CreateLongLineObject(start, end, transform, LineMaterial, 5);
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
            new Line(line.GetPosition(0), line.GetPosition(1)), dot, transform, LineMaterial);
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
        Resources.FindObjectsOfTypeAll<GameObject>().First(go => go.name == GameObjectNames.GameScene.RightAnswerWindow).SetActive(true);
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
