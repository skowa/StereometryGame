using System;
using System.Collections.Generic;
using UnityEngine;

public static class LineEquationAlgorithms
{
    private static readonly float _approximation = 1e-5f;

    public static List<Func<float, float>> CreateLineEquation(Vector3 firstPoint, Vector3 secondPoint)
    {
        return CreateLineEquationOfParallelLine(firstPoint, secondPoint, firstPoint);
    }

    public static List<Func<float, float>> CreateLineEquationOfParallelLine(Vector3 firstPoint, Vector3 secondPoint, Vector3 dot)
    {
        var equations = new List<Func<float, float>>
        {
            p => dot.x + (secondPoint.x - firstPoint.x) * p,
            p => dot.y + (secondPoint.y - firstPoint.y) * p,
            p => dot.z + (secondPoint.z - firstPoint.z) * p
        };

        return equations;
    }

    public static Vector3 FindLinesIntersection(Line firstLine, Line secondLine)
    {
        float[][] equations = CreateMatrix(firstLine, secondLine);
        float[] vector = CreateVector(firstLine, secondLine);

        int lineToRemove = GetLineToRemove(equations, vector);
        float[][] equationsForGauss = RemoveRedundantLine(equations, lineToRemove);
        float[] vectorForGauss = RemoveRedundantLine(vector, lineToRemove);

        LinearEquationsSolver.SolveWithGauss(equationsForGauss, vectorForGauss);

        var equationForFirstLine = CreateLineEquation(firstLine.StartPoint, firstLine.EndPoint);
        return new Vector3(equationForFirstLine[0](vectorForGauss[0]), equationForFirstLine[1](vectorForGauss[0]), equationForFirstLine[2](vectorForGauss[0]));
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

    private static float[] CreateVector(Line first, Line second)
    {
        return new[]
        {
            second.StartPoint.x - first.StartPoint.x,
            second.StartPoint.y - first.StartPoint.y,
            second.StartPoint.z - first.StartPoint.z
        };
    }

    private static float[][] RemoveRedundantLine(float[][] matrix, int redundantLine)
    {
        float[][] newMatrix = new float[matrix.Length - 1][];
        for (int i = 0; i < redundantLine; i++)
        {
            newMatrix[i] = matrix[i];
        }

        for (int i = redundantLine + 1; i < matrix.Length; i++)
        {
            newMatrix[i - 1] = matrix[i];
        }

        return newMatrix;
    }

    private static float[] RemoveRedundantLine(float[] vector, int redundantLine)
    {
        float[] newVector = new float[vector.Length - 1];
        for (int i = 0; i < redundantLine; i++)
        {
            newVector[i] = vector[i];
        }

        for (int i = redundantLine + 1; i < vector.Length; i++)
        {
            newVector[i - 1] = vector[i];
        }

        return newVector;
    }

    private static int GetLineToRemove(float[][] matrix, float[] vector)
    {
        if (matrix[0][0].ApproximatelyEquals(0, _approximation) && matrix[0][1].ApproximatelyEquals(0, _approximation))
        {
            return 0;
        }

        if (matrix[1][0].ApproximatelyEquals(0, _approximation) && matrix[1][1].ApproximatelyEquals(0, _approximation))
        {
            return 1;
        }

        if (matrix[2][0].ApproximatelyEquals(0, _approximation) && matrix[2][1].ApproximatelyEquals(0, _approximation))
        {
            return 2;
        }

        if (ProportionalLines(matrix, vector, 0, 1))
        {
            return 1;
        }

        return 2;
    }

    private static bool ProportionalLines(float[][] matrix, float[] vector, int i, int j)
    {
        float firstQuotient = matrix[i][0] / matrix[j][0];
        float secondQuotient = matrix[i][1] / matrix[j][1];
        float thirdQuotient = vector[i] / vector[j];
        if (firstQuotient.ApproximatelyEquals(secondQuotient, _approximation) &&
            firstQuotient.ApproximatelyEquals(thirdQuotient, _approximation))
        {
            return true;
        }

        if (float.IsNaN(firstQuotient) && secondQuotient.ApproximatelyEquals(thirdQuotient, _approximation))
        {
            return true;
        }

        if (float.IsNaN(secondQuotient) && firstQuotient.ApproximatelyEquals(thirdQuotient, _approximation))
        {
            return true;
        }

        if (float.IsNaN(thirdQuotient) && firstQuotient.ApproximatelyEquals(secondQuotient, _approximation))
        {
            return true;
        }

        return false;
    }
}