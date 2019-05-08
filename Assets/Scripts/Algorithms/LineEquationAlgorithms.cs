using System;
using System.Collections.Generic;
using UnityEngine;

public static class LineEquationAlgorithms
{
    private static readonly float _approximation = 1e-5f;

    public static List<Func<float, float>> CreateLineEquation(Vector3 firstPoint, Vector3 secondPoint)
    {
        var equations = new List<Func<float, float>>
        {
            p => firstPoint.x + (secondPoint.x - firstPoint.x) * p,
            p => firstPoint.y + (secondPoint.y - firstPoint.y) * p,
            p => firstPoint.z + (secondPoint.z - firstPoint.z) * p
        };

        return equations;
    }

    public static Vector3 FindLinesIntersection(Line firstLine, Line secondLine)
    {
        float[][] equations = CreateMatrix(firstLine, secondLine);
        int removedLine;
        float[][] equationsForGauss = RemoveUnnecessaryLine(equations, out removedLine);

        float[] vector = CreateVector(firstLine, secondLine, removedLine);
        LinearEquationsSolver.SolveWithGauss(equationsForGauss, vector);

        var equationForFirstLine = CreateLineEquation(firstLine.StartPoint, firstLine.EndPoint);
        return new Vector3(equationForFirstLine[0](vector[0]), equationForFirstLine[1](vector[0]), equationForFirstLine[2](vector[0]));
    }

    private static float[][] CreateMatrix(Line firstLine, Line secondLine)
    {
        return new[]
        {
            new[] {firstLine.EndPoint.x - firstLine.StartPoint.x, secondLine.StartPoint.x - secondLine.EndPoint.x},
            new[] {firstLine.EndPoint.y - firstLine.StartPoint.y, secondLine.StartPoint.y - secondLine.EndPoint.y},
            new[] {firstLine.EndPoint.z - firstLine.StartPoint.z, secondLine.StartPoint.z - secondLine.EndPoint.z}
        };
    }

    private static float[] CreateVector(Line first, Line second, int lineNotToInclude)
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

    private static float[][] RemoveUnnecessaryLine(float[][] matrix, out int removedLine)
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

    private static void AddTwoLinesFromMatrixToAnother(float[][] matrix, float[][] anotherMatrix, int i, int j)
    {
        anotherMatrix[0] = matrix[i];
        anotherMatrix[1] = matrix[j];
    }
}