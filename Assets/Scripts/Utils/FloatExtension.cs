using System;

public static class FloatExtension
{
    public static bool ApproximatelyEquals(this float a, float b, float approximation)
    {
        return Math.Abs(a - b) < approximation;
    }
}
