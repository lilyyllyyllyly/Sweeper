using Godot;
using System;

public static class Utils
{

    /// <summary>
    /// Rotates a vector <c>vec</c> by <c>angle</c> (in radians).
    /// </summary>
    public static Vector2 RotateVector(Vector2 vec, float angle) 
    {
        Vector2 a = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 b = new Vector2(-Mathf.Sin(angle), Mathf.Cos(angle));
        Vector2 rotatedVector = vec.x * a + vec.y * b;
        return rotatedVector;
    }

    /// <summary>
    /// Returns the angle (in radians) between one point (origin) and another (point).
    /// </summary>
    public static float AngleBetweenPoints(Vector2 origin, Vector2 point)
    {
        Vector2 diff = origin - point;
        float newRotation = Mathf.Atan2(diff.y, diff.x);
        return newRotation;
    }

    /// <summary>
    /// Returns the angle (in radians) between one point (origin) and another (point), with an offset applied.
    /// </summary>
    /// <param name="offsetDeg"> The offset to be applied, in degrees. </param>
    public static float AngleBetweenPoints(Vector2 origin, Vector2 point, float offsetDeg)
    {
        Vector2 diff = origin - point;
        float newRotation = Mathf.Atan2(diff.y, diff.x);
        return newRotation + Mathf.Deg2Rad(offsetDeg);
    }
}
