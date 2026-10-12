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
        public int fontSize;
        public TextAnchor alignment;
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
}
