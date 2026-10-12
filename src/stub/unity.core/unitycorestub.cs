// Compile-time API stub for UnityEngine.CoreModule (identity: Version=0.0.0.0,
// unsigned). Only the members rf touches; runtime binds to the game's real modules.
// Member kinds and signatures must match real Unity (properties stay properties,
// static readonly fields stay fields, const values stay exact because consts are
// inlined into our IL at compile time).

namespace UnityEngine
{
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 right { get { return default(Vector2); } }

        public float magnitude { get { return 0f; } }

        public static Vector2 operator +(Vector2 a, Vector2 b) { return default(Vector2); }
        public static Vector2 operator -(Vector2 a, Vector2 b) { return default(Vector2); }
        public static Vector2 operator -(Vector2 a) { return default(Vector2); }

        public static implicit operator Vector3(Vector2 v) { return default(Vector3); }
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 zero { get { return default(Vector3); } }

        public float magnitude { get { return 0f; } }
        public float sqrMagnitude { get { return 0f; } }
        public Vector3 normalized { get { return default(Vector3); } }

        public static Vector3 operator +(Vector3 a, Vector3 b) { return default(Vector3); }
        public static Vector3 operator -(Vector3 a, Vector3 b) { return default(Vector3); }
        public static Vector3 operator -(Vector3 a) { return default(Vector3); }
        public static Vector3 operator *(Vector3 a, float d) { return default(Vector3); }
        public static Vector3 operator *(float d, Vector3 a) { return default(Vector3); }
        public static bool operator ==(Vector3 a, Vector3 b) { return false; }
        public static bool operator !=(Vector3 a, Vector3 b) { return false; }

        public override bool Equals(object other) { return false; }
        public override int GetHashCode() { return 0; }

        public static float Distance(Vector3 a, Vector3 b) { return 0f; }
        public static float Dot(Vector3 a, Vector3 b) { return 0f; }
        public static float Angle(Vector3 from, Vector3 to) { return 0f; }
        public static Vector3 RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta) { return default(Vector3); }
    }

    public struct Quaternion
    {
        public static Quaternion LookRotation(Vector3 forward) { return default(Quaternion); }
    }

    public struct Matrix4x4
    {
        public static Matrix4x4 identity { get { return default(Matrix4x4); } }
    }

    public struct Color
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static Color white { get { return default(Color); } }
    }

    public struct Rect
    {
        // Properties in real Unity, not fields.
        public float x { get; set; }
        public float y { get; set; }
        public float width { get; set; }
        public float height { get; set; }

        public Rect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public float xMax { get { return x + width; } }
        public float yMax { get { return y + height; } }
        public Vector2 position { get { return new Vector2(x, y); } set { x = value.x; y = value.y; } }
    }

    public static class Mathf
    {
        // Exact Unity values: consts are inlined into our IL, wrong literals would
        // silently corrupt the math.
        public const float PI = 3.14159274f;
        public const float Deg2Rad = 0.0174532924f;

        public static float Tan(float f) { return 0f; }
        public static float Sin(float f) { return 0f; }
        public static float Cos(float f) { return 0f; }
        public static float Max(float a, float b) { return 0f; }
        public static int RoundToInt(float f) { return 0; }
    }

    public static class Time
    {
        public static float time { get { return 0f; } }
        public static float deltaTime { get { return 0f; } }
    }

    public static class Screen
    {
        public static int width { get { return 0; } }
        public static int height { get { return 0; } }
    }

    public class Object
    {
        public string name { get; set; }

        public static void Destroy(Object obj) { }

        public static bool operator ==(Object x, Object y) { return false; }
        public static bool operator !=(Object x, Object y) { return false; }
        public override bool Equals(object other) { return false; }
        public override int GetHashCode() { return 0; }
    }

    public class Texture : Object
    {
    }

    public class Texture2D : Texture
    {
        public Texture2D(int width, int height) { }
        public void SetPixel(int x, int y, Color color) { }
        public void Apply() { }
    }

    public class Component : Object
    {
        public Transform transform { get { return null; } }
        public GameObject gameObject { get { return null; } }
        public T GetComponent<T>() { return default(T); }
    }

    public class Transform : Component
    {
        public Vector3 position { get { return default(Vector3); } set { } }
        public Vector3 forward { get { return default(Vector3); } }
        public Quaternion rotation { get { return default(Quaternion); } set { } }
    }

    public class GameObject : Object
    {
        public T AddComponent<T>() where T : Component { return default(T); }
        public T GetComponent<T>() { return default(T); }
    }

    public class MonoBehaviour : Component
    {
    }

    public class Camera : Component
    {
        public float fieldOfView { get { return 0f; } set { } }
        public Vector3 WorldToScreenPoint(Vector3 position) { return default(Vector3); }
    }
}
