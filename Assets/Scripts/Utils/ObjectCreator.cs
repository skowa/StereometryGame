using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator
{
    public void CreateLineObjectForButtons(Vector3 start, Vector3 end, Transform transform, Color color)
    {
        var lineObject = new GameObject();

        lineObject.transform.SetParent(transform);
        var scriptableObject = lineObject.AddComponent<LineBehaviour>();

        var material = new Material(Shader.Find("Unlit/Color")) {color = color};

        scriptableObject.InstantiateFields(start, end, material, false);
    }

    public GameObject CreateLongLineObject(Vector3 start, Vector3 end, Transform transform, Material material, float parameter)
    {
        List<Func<float, float>> equations = LineEquationAlgorithms.CreateLineEquation(start, end);

        return CreateLongLineObjectCore(equations, transform, material, parameter);
    }

    public GameObject CreateDotBetweenLines(Line first, Line second, Transform transform, Material material)
    {
        Vector3 pointBetweenLines = LineEquationAlgorithms.FindLinesIntersection(first, second);

        return CreateDot(pointBetweenLines, transform, material);
    }

    public GameObject CreateParallelLineThroughDot(Line line, Vector3 dot, Transform transform, Material material)
    {
        List<Func<float, float>> parallelLineEquations = LineEquationAlgorithms.CreateLineEquationOfParallelLine(line.StartPoint, line.EndPoint, dot);

        float parameter = 5;
        return CreateLongLineObjectCore(parallelLineEquations, transform, material, parameter);
    }

    public GameObject CreateLine(Line line, Transform transform, Material material)
    {
        var lineObject = new GameObject();

        lineObject.transform.SetParent(transform);
        var scriptableObject = lineObject.AddComponent<LineBehaviour>();
        scriptableObject.InstantiateFields(line.StartPoint, line.EndPoint, material, false);

        lineObject.tag = "Edge";
        lineObject.name = "Line";

        scriptableObject.AddColliderToLineObject();
        lineObject.transform.GetChild(0).tag = "Edge";

        return lineObject;
    }

    public GameObject CreateDot(Vector3 vector3, Transform transform, Material material)
    {
        GameObject sphere = CreateSphereObject(material);

        sphere.name = "Dot";
        sphere.tag = "Dot";
        sphere.transform.SetParent(transform);
        sphere.transform.position = new Vector3(vector3.x, vector3.y, vector3.z);
        sphere.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        sphere.GetComponent<SphereCollider>().radius = 0.75f;

        return sphere;
    }

    public void CreateLetter(Letter letter, Transform transform, Color color)
    {
        GameObject text = CreateTextObject(letter.Name, color);
        text.name = "Letter" + letter;
        text.transform.SetParent(transform);
        text.transform.localPosition += new Vector3(letter.Position.x, letter.Position.y, letter.Position.z);
    }

    private GameObject CreateLongLineObjectCore(List<Func<float, float>> equations, Transform transform, Material material, float parameter)
    {
        var convertedStart = new Vector3(equations[0](-parameter), equations[1](-parameter), equations[2](-parameter));
        var convertedEnd = new Vector3(equations[0](parameter), equations[1](parameter), equations[2](parameter));

        return CreateLine(new Line(convertedStart, convertedEnd), transform, material);
    }

    private GameObject CreateTextObject(string text, Color color)
    {
        var textObject = new GameObject();
        TextMesh t = textObject.AddComponent<TextMesh>();
        t.color = color;
        t.text = text;
        t.fontSize = 300;
        t.characterSize = 0.009f;

        return textObject;
    }

    private GameObject CreateSphereObject(Material material)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        var renderer = sphere.GetComponent<MeshRenderer>();
        renderer.material = material;

        return sphere;
    }
}

