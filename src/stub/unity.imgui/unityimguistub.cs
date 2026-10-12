// Compile-time API stub for UnityEngine.IMGUIModule (identity: Version=0.0.0.0,
// unsigned). GUI.DrawTexture takes Texture (not Texture2D) in real Unity -- the
// parameter type is part of the runtime memberref signature, so it must match.

using UnityEngine;

namespace UnityEngine
{
    public static class GUI
    {
        public static Matrix4x4 matrix { get { return default(Matrix4x4); } set { } }
        public static Color color { get { return default(Color); } set { } }
        public static GUISkin skin { get { return null; } }

        public static void Label(Rect position, string text, GUIStyle style) { }
        public static void DrawTexture(Rect position, Texture image) { }
    }

    public class GUISkin : Object
    {
        public GUIStyle label { get { return null; } }
    }

    public class GUIStyle
    {
        // Properties in real Unity, not fields (reflection-verified).
        public int fontSize { get; set; }
        public TextAnchor alignment { get; set; }
        public GUIStyleState normal { get { return null; } }

        public GUIStyle() { }
        public GUIStyle(GUIStyle other) { }
    }

    public class GUIStyleState
    {
        public Color textColor { get { return default(Color); } set { } }
    }

    public static class GUIUtility
    {
        public static void RotateAroundPivot(float angle, Vector2 pivotPoint) { }
    }

    // Verified shapes: static get-only `current`, get-only `type`.
    // rf reads `type` only via ToString(), so no enum ordinal is baked into rf IL.
    public class Event
    {
        public static Event current { get { return null; } }
        public EventType type { get { return EventType.Ignore; } }
    }

    public enum EventType
    {
        Ignore = 0
    }
}
