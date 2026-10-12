using UnityEngine;

namespace rf;

public static class Drawing
{
    private static Texture2D _lineTexture;

    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
    {
        if (_lineTexture == null)
        {
            _lineTexture = new Texture2D(1, 1);
            _lineTexture.SetPixel(0, 0, Color.white);
            _lineTexture.Apply();
        }

        Matrix4x4 saved = GUI.matrix;
        Color savedColor = GUI.color;
        GUI.color = color;

        float angle = Vector3.Angle(pointB - pointA, Vector2.right);
        if (pointA.y > pointB.y)
        {
            angle = -angle;
        }

        GUIUtility.RotateAroundPivot(angle, pointA);

        if (width <= 1f)
        {
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, (pointB - pointA).magnitude, width), _lineTexture);
        }
        else
        {
            GUI.DrawTexture(new Rect(pointA.x, pointA.y - width * 0.5f, (pointB - pointA).magnitude, width), _lineTexture);
        }

        GUI.matrix = saved;
        GUI.color = savedColor;
    }
}
