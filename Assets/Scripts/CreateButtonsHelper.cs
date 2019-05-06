using System;
using System.Collections.Generic;
using UnityEditor;
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
                    var cube = GameObject.Find("Cube");
                    Quaternion rotation = cube.transform.rotation;
                    cube.transform.localRotation = Quaternion.identity;

                    LineRenderer firstLineRenderer = SelectedObjects[0].GetComponent<LineRenderer>();
                    LineRenderer secondLineRenderer = SelectedObjects[1].GetComponent<LineRenderer>();

                    Line firstLine = new Line(firstLineRenderer.GetPosition(0), firstLineRenderer.GetPosition(1));
                    Line secondLine = new Line(secondLineRenderer.GetPosition(0), secondLineRenderer.GetPosition(1));

                    GameObject parentObject = GameObject.Find("Cube");
                    Vector3 pointBetweenLines;
                    GameObject dot = creator.CreateDotBetweenLines(firstLine, secondLine, out pointBetweenLines);
                    dot.transform.SetParent(parentObject.transform);
                    dot.transform.position = new Vector3(pointBetweenLines.x, pointBetweenLines.y, pointBetweenLines.z);
                    dot.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
                    dot.tag = "Dot";

                    cube.transform.rotation = rotation;
                }

                SelectedObjects.ForEach(UnselectGameObject);
                SelectedObjects.Clear();
                Action = ActionType.None;

                break;
            case ActionType.Line:
                if (SelectedObjects.Count == 2)
                {
                    var cube = GameObject.Find("Cube");
                    Quaternion rotation = cube.transform.rotation;
                    cube.transform.localRotation = Quaternion.identity;

                    Vector3 start = SelectedObjects[0].transform.position;
                    Vector3 end = SelectedObjects[1].transform.position;

                    GameObject parentObject = GameObject.Find("Cube");
                    GameObject line = creator.CreateLongLineObject(start, end, Color.gray, 5);
                    line.transform.SetParent(parentObject.transform);
                    line.GetComponent<LineBehaviour>().AddColliderToLineObject();
                    line.tag = "Edge";
                    line.transform.GetChild(0).tag = "Edge";

                    cube.transform.rotation = rotation;
                }

                SelectedObjects.ForEach(UnselectGameObject);
                SelectedObjects.Clear();
                Action = ActionType.None;

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
                renderer.material.color = Color.gray;
                break;
            case "Dot":
                renderer.material.color = new Color(0.3f, 0.3f, 0.3f);
                break;
            default:
                break;
        }
    }
}

public enum ActionType
{
    None,
    Dot,
    Line
}
