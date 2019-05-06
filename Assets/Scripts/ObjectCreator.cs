using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator
{
    private readonly float _approximation = 1e-10f;

    public GameObject CreateLineObject(Vector3 start, Vector3 end, Color color)
    {
        var line = new GameObject();

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = color;
        lineRenderer.positionCount = 2;
        lineRenderer.SetWidth(0.03f, 0.03f);
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.useWorldSpace = false;

        return line;
    }

    public GameObject CreateLongLineObject(Vector3 start, Vector3 end, Color color, float parameter)
    {
        List<Func<float, float>> equations = CreateLineEquation(start, end);

        var convertedStart = new Vector3(equations[0](-parameter), equations[1](-parameter), equations[2](-parameter));
        var convertedEnd = new Vector3(equations[0](parameter), equations[1](parameter), equations[2](parameter));

        GameObject line = new GameObject();
        LineBehaviour scriptableLine = line.AddComponent<LineBehaviour>();
        scriptableLine.InstantiateFields(convertedStart, convertedEnd, color, true);

        return line;
    }

    public GameObject CreateDotBetweenLines(Line first, Line second, out Vector3 pointBetweenLines)
    {
        float[][] equations = CreateMatrix(first, second);
        int removedLine;
        float[][] equationsForGauss = RemoveUnnecessaryLine(equations, out removedLine);

        float[] vector = CreateVector(first, second, removedLine);
        LinearEquationsSolver.SolveWithGauss(equationsForGauss, vector);

        var equationForFirstLine = CreateLineEquation(first.StartPoint, first.EndPoint);
        pointBetweenLines = new Vector3(equationForFirstLine[0](vector[0]), equationForFirstLine[1](vector[0]), equationForFirstLine[2](vector[0]));

        GameObject dot = CreateSphereObject(new Color(0.3f, 0.3f, 0.3f));

        return dot;
    }

    public GameObject CreateTextObject(string text, Color color)
    {
        var textObject = new GameObject();
        TextMesh t = textObject.AddComponent<TextMesh>();
        t.color = color;
        t.text = text;
        t.fontSize = 300;
        t.characterSize = 0.009f;

        return textObject;
    }

    public GameObject CreateSphereObject(Color color)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        var renderer = sphere.GetComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = new Color(0.3f, 0.3f, 0.3f);

        return sphere;
    }

    private List<Func<float, float>> CreateLineEquation(Vector3 firstPoint, Vector3 secondPoint)
    {
        var equations = new List<Func<float, float>>
        {
            p => firstPoint.x + (secondPoint.x - firstPoint.x) * p,
            p => firstPoint.y + (secondPoint.y - firstPoint.y) * p,
            p => firstPoint.z + (secondPoint.z - firstPoint.z) * p
        };

        return equations;
    }

    private float[][] CreateMatrix(Line first, Line second)
    {
        return new[]
        {
            new[] {first.EndPoint.x - first.StartPoint.x, second.StartPoint.x - second.EndPoint.x},
            new[] {first.EndPoint.y - first.StartPoint.y, second.StartPoint.y - second.EndPoint.y},
            new[] {first.EndPoint.z - first.StartPoint.z, second.StartPoint.z - second.EndPoint.z}
        };
    }

    private float[] CreateVector(Line first, Line second, int lineNotToInclude)
    {
        switch (lineNotToInclude)
        {
            case 0:
                return new[]
                {
                    second.StartPoint.y - first.StartPoint.y,
                    second.StartPoint.z - first.StartPoint.z
                };
            case 1:
                return new[]
                {
                    second.StartPoint.x - first.StartPoint.x,
                    second.StartPoint.z - first.StartPoint.z
                };
            case 2:
                return new[]
                {
                    second.StartPoint.x - first.StartPoint.x,
                    second.StartPoint.y - first.StartPoint.y
                };
            default: throw new ArgumentException();
        }
    }

    private float[][] RemoveUnnecessaryLine(float[][] matrix, out int removedLine)
    {
        var result = new float[2][];

        if (matrix[0][0].ApproximatelyEquals(0, _approximation) && matrix[0][1].ApproximatelyEquals(0, _approximation))
        {
            AddTwoLinesFromMatrixToAnother(matrix, result, 1, 2);
            removedLine = 0;
        } 
        else if (matrix[1][0].ApproximatelyEquals(0, _approximation) && matrix[1][1].ApproximatelyEquals(0, _approximation))
        {
            AddTwoLinesFromMatrixToAnother(matrix, result, 0, 2);
            removedLine = 1;
        }
        else if (matrix[2][0].ApproximatelyEquals(0, _approximation) && matrix[2][1].ApproximatelyEquals(0, _approximation))
        {
            AddTwoLinesFromMatrixToAnother(matrix, result, 0, 1);
            removedLine = 2;
        }
        else if ((matrix[0][0] / matrix[1][0]).ApproximatelyEquals(matrix[0][1] / matrix[1][1], _approximation))
        {
            AddTwoLinesFromMatrixToAnother(matrix, result, 0, 2);
            removedLine = 1;
        }
        else 
        {
            AddTwoLinesFromMatrixToAnother(matrix, result, 0, 1);
            removedLine = 2;
        }

        return result;
    }

    private void AddTwoLinesFromMatrixToAnother(float[][] matrix, float[][] anotherMatrix, int i, int j)
    {
        anotherMatrix[0] = matrix[i];
        anotherMatrix[1] = matrix[j];
    }
}

