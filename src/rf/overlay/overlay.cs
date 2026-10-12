using UnityEngine;

namespace rf;

public sealed class Overlay
{
    public static readonly Overlay Instance = new Overlay();

    private static readonly Color CircleColor = new Color(1f, 1f, 1f, 0.35f);
    private static readonly Color LockColor = new Color(1f, 0.25f, 0.15f, 0.95f);
    private const float BracketSize = 52f;
    private const float LabelHeight = 18f;
    private const int CircleSegments = 64;

    public void Draw()
    {
        DrawFovCircle();

        LockTarget target = TargetLock.Instance.CurrentTarget;
        if (target.IsValid)
        {
            DrawTargetMark(target);
        }

    }

    private static void DrawFovCircle()
    {
        Camera camera = SafeCamera.Get();
        if (camera == null)
        {
            return;
        }

        float coneDegrees = Settings.FieldOfViewLimit;
        if (coneDegrees <= 0f || coneDegrees >= 360f)
        {
            return;
        }

        float halfHeight = (float)Screen.height * 0.5f;
        float tanHalfFov = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad * 0.5f);
        if (tanHalfFov <= 0.0001f)
        {
            return;
        }

        float radius = Mathf.Tan(coneDegrees * Mathf.Deg2Rad * 0.5f) / tanHalfFov * halfHeight;
        Vector2 center = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
        Color color = TargetLock.Instance.CurrentTarget.IsValid ? LockColor : CircleColor;

        float step = Mathf.PI * 2f / CircleSegments;
        Vector2 previous = center + new Vector2(radius, 0f);
        for (int i = 1; i <= CircleSegments; i++)
        {
            float angle = step * i;
            Vector2 next = center + new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            Drawing.DrawLine(previous, next, color, 1.5f);
            previous = next;
        }
    }

    private static void DrawTargetMark(LockTarget target)
    {
        Vector3 screenPos = target.ScreenPoint;
        if (screenPos.z <= 0f)
        {
            return;
        }

        Rect rect = new Rect(screenPos.x - BracketSize * 0.5f, (float)Screen.height - screenPos.y - BracketSize * 0.5f, BracketSize, BracketSize);
        DrawBracket(rect);

        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 13,
            alignment = TextAnchor.UpperCenter
        };
        style.normal.textColor = LockColor;

        float distance = TargetLock.Instance.DistanceTo(target);
        string label = target.Label + "  " + Mathf.RoundToInt(distance) + "m";
        GUI.Label(new Rect(rect.x, rect.yMax + 2f, rect.width, LabelHeight), label, style);
    }

    private static void DrawBracket(Rect rect)
    {
        float len = 14f;
        Drawing.DrawLine(rect.position + new Vector2(0f, 0f), rect.position + new Vector2(len, 0f), LockColor, 2f);
        Drawing.DrawLine(rect.position + new Vector2(rect.width, 0f), rect.position + new Vector2(rect.width - len, 0f), LockColor, 2f);
        Drawing.DrawLine(rect.position + new Vector2(0f, rect.height), rect.position + new Vector2(len, rect.height), LockColor, 2f);
        Drawing.DrawLine(rect.position + new Vector2(rect.width, rect.height), rect.position + new Vector2(rect.width - len, rect.height), LockColor, 2f);
    }
}
