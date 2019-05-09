using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateButtonsHelper
{
    public static ActionType Action; 
    public static List<GameObject> SelectedObjects = new List<GameObject>();

    public static void CreateObject()
    {
        var creator = new ObjectCreator();

        switch (Action)
        {
            case ActionType.None:
                break;
            case ActionType.Dot:
                if (SelectedObjects.Count == 2)
                {
                    var shape = GameObject.Find(Game.CurrentLevelData.Type.ToString());
                    Quaternion rotation = shape.transform.rotation;
                    shape.transform.localRotation = Quaternion.identity;

                    LineRenderer firstLineRenderer = SelectedObjects[0].GetComponent<LineRenderer>();
                    LineRenderer secondLineRenderer = SelectedObjects[1].GetComponent<LineRenderer>();

                    Line firstLine = new Line(firstLineRenderer.GetPosition(0), firstLineRenderer.GetPosition(1));
                    Line secondLine = new Line(secondLineRenderer.GetPosition(0), secondLineRenderer.GetPosition(1));

                    GameObject dot = creator.CreateDotBetweenLines(firstLine, secondLine, shape.transform, Resources.Load<Material>(Game.PathToDotsMaterial));
                    Game.Actions.Add(dot);

                    shape.transform.rotation = rotation;
                }

                Clear();

                break;
            case ActionType.Line:
                if (SelectedObjects.Count == 2)
                {
                    var shape = GameObject.Find(Game.CurrentLevelData.Type.ToString());
                    Quaternion rotation = shape.transform.rotation;
                    shape.transform.localRotation = Quaternion.identity;

                    Vector3 start = SelectedObjects[0].transform.position;
                    Vector3 end = SelectedObjects[1].transform.position;

                    GameObject line = creator.CreateLongLineObject(start, end, shape.transform, Resources.Load<Material>(Game.PathToLinesMaterial), 5);
                    Game.Actions.Add(line);

                    shape.transform.rotation = rotation;
                }

                Clear();

                break;
            case ActionType.ParallelLine:
                if (SelectedObjects.Count == 2)
                {
                    var shape = GameObject.Find(Game.CurrentLevelData.Type.ToString());
                    Quaternion rotation = shape.transform.rotation;
                    shape.transform.localRotation = Quaternion.identity;

                    LineRenderer line;
                    Vector3 dot;
                    if (SelectedObjects[0].tag == "Edge")
                    {
                        line = SelectedObjects[0].transform.GetComponent<LineRenderer>();
                        dot = SelectedObjects[1].transform.position;
                    }
                    else
                    {
                        dot = SelectedObjects[0].transform.position;
                        line = SelectedObjects[1].transform.GetComponent<LineRenderer>();
                    }

                    GameObject parallelLine = creator.CreateParallelLineThroughDot(
                        new Line(line.GetPosition(0), line.GetPosition(1)), dot, shape.transform,
                        Resources.Load<Material>(Game.PathToLinesMaterial));
                    Game.Actions.Add(parallelLine);

                    shape.transform.rotation = rotation;
                }

                Clear();

                break;
            case ActionType.Check:
                if (SelectedObjects.Count == Game.CurrentLevelData.Answer.Count)
                {
                    var shape = GameObject.Find(Game.CurrentLevelData.Type.ToString());
                    Quaternion rotation = shape.transform.rotation;
                    shape.transform.localRotation = Quaternion.identity;

                    bool result = SelectedObjects.All(o =>
                        Game.CurrentLevelData.Answer.Any(t => o.transform.localPosition == t));

                    if (!result)
                    {
                        Clear();
                        shape.transform.rotation = rotation;
                    }
                    else
                    {
                        int maxLevelSolved = PlayerPrefs.GetInt("level");
                        if (maxLevelSolved == Game.CurrentLevelData.Number)
                        {
                            PlayerPrefs.SetInt("level", ++maxLevelSolved);
                            Game.Actions.Clear();
                        }
                        
                        GameObject.Find(Game.CurrentLevelData.Type.ToString()).SetActive(false);
                        Resources.FindObjectsOfTypeAll<GameObject>().First(go => go.name == "RightAnswerWindow").SetActive(true);
                    }
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var okButton = GameObject.Find("OK");
        okButton.GetComponent<Renderer>().enabled = false;
        okButton.GetComponent<Collider2D>().enabled = false;
    }

    private static void UnselectGameObject(GameObject gameObject)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        switch (gameObject.tag)
        {
            case "Edge":
                renderer.material = Resources.Load<Material>(Game.PathToLinesMaterial);
                break;
            case "Dot":
                renderer.material = Resources.Load<Material>(Game.PathToDotsMaterial);
                break;
            default:
                break;
        }
    }

    private static void Clear()
    {
        SelectedObjects.ForEach(UnselectGameObject);
        SelectedObjects.Clear();
        Action = ActionType.None;
    }
}

public enum ActionType
{
    None,
    Dot,
    Line,
    ParallelLine,
    Check
}
