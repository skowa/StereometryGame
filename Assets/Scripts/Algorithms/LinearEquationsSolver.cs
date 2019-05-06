public static class LinearEquationsSolver
{
    public static void SolveWithGauss(float[][] matrix, float[] vector)
    {
        SwapLinesIfFirstElementIsZero(matrix, vector);

        DirectStep(matrix, vector);
        ReverseStep(matrix, vector);
        DivideElements(matrix, vector);
    }

    private static void DirectStep(float[][] matrix, float[] vector)
    {
        int dimension = vector.Length;
        for (int k = 0; k < dimension - 1; k++)
        {
            for (int i = k + 1; i < dimension; i++)
            {
                float multiplier = matrix[i][k] / matrix[k][k];

                for (int j = k; j < dimension; j++)
                {
                    matrix[i][j] -= matrix[k][j] * multiplier;
                }
                vector[i] -= vector[k] * multiplier;
            }
        }
    }

    private static void ReverseStep(float[][] matrix, float[] vector)
    {
        int dimension = vector.Length;
        for (int k = dimension - 1; k >= 0; k--)
        {

            for (int i = k - 1; i >= 0; i--)
            {
                float multiplier = matrix[i][k] / matrix[k][k];

                for (int j = dimension - 1; j >= k; j--)
                {
                    matrix[i][j] -= matrix[k][j] * multiplier;
                }
                vector[i] -= vector[k] * multiplier;
            }
        }
    }

    private static void DivideElements(float[][] matrix, float[] vector)
    {
        for (int i = 0; i < vector.Length; i++)
        {
            vector[i] /= matrix[i][i];
        }
    }

    private static void SwapLinesIfFirstElementIsZero(float[][] matrix, float[] vector)
    {
        float approximation = 1e-10f;

        if (matrix[0][0].ApproximatelyEquals(0, approximation))
        {
            Swap(ref matrix[0], ref matrix[1]);
            Swap(ref vector[0], ref vector[1]);
        }
    }

    private static void Swap(ref float[] a, ref float[] b)
    {
        float[] tmp = a;
        a = b;
        b = tmp;
    }

    private static void Swap(ref float a, ref float b)
    {
        float tmp = a;
        a = b;
        b = tmp;
    }
}
