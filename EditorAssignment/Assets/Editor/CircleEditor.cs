using UnityEngine;
using UnityEditor;

public class CircleEditor : EditorWindow
{
    public LineRenderer renderer = null;

    public int segments = 16;
    public float radius = 5f;

    private Vector3[] points;

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    [MenuItem("Window/Circle Editor")]
    static void Init()
    {
        CircleEditor window = (CircleEditor)GetWindow(typeof(CircleEditor));
        window.Show();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (renderer == null)
        {
            renderer = GameObject.Find("RenderObject").GetComponent<LineRenderer>();
        }

        HandleRadius();
        HandleSegments();

        points = new Vector3[segments + 1];

        float deltaTheta = (Mathf.PI * 2) / segments;
        float theta = 0;

        for (int i = 0; i < segments + 1; i++)
        {
            theta += deltaTheta;
            points[i] = new Vector3(radius * Mathf.Cos(theta), 0f, radius * Mathf.Sin(theta));
        }

        renderer.SetPositions(points);
        renderer.positionCount = points.Length;
    }

    private void OnGUI()
    {
        if (renderer == null)
        {
            renderer = GameObject.Find("RenderObject").GetComponent<LineRenderer>();
        }

        renderer = (LineRenderer)EditorGUILayout.ObjectField(renderer, (typeof(LineRenderer)), true);

        segments = EditorGUILayout.IntField("Segments", segments);
        radius = EditorGUILayout.FloatField("Radius", radius);

        segments = Mathf.Clamp(segments, 4, 64);

        points = new Vector3[segments + 1];

        float deltaTheta = (Mathf.PI * 2) / segments;
        float theta = 0;

        for (int i = 0; i < segments + 1; i++)
        {
            theta += deltaTheta;
            points[i] = new Vector3(radius * Mathf.Cos(theta), 0f, radius * Mathf.Sin(theta));
        }

        renderer.SetPositions(points);
        renderer.positionCount = points.Length;
    }

    private void HandleSegments()
    {
        Color oldColor = Handles.color;

        EditorGUI.BeginChangeCheck();

        Handles.color = Color.red;
        int tmpSegments = (int)Handles.ScaleSlider(
            segments,
            Vector3.zero,
            Vector3.back,
            Quaternion.identity,
            radius,
            1f
            );

        segments = Mathf.Clamp(segments, 4, 64);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(this, "change segments");
            segments = tmpSegments;
        }

        Handles.color = oldColor;
    }

    private void HandleRadius()
    {
        Color oldColor = Handles.color;

        Handles.color = new Color(0f, 1.0f, 0f, 0.15f);
        Handles.DrawSolidDisc(Vector3.zero, Vector3.up, radius);

        EditorGUI.BeginChangeCheck();

        Handles.color = Color.blue;
        float tmpRadius = Handles.ScaleSlider(
            radius,
            Vector3.zero,
            Vector3.forward,
            Quaternion.identity,
            radius,
            1f
            );

        tmpRadius = Mathf.Clamp(tmpRadius, 1f, float.MaxValue);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(this, "change radius");
            radius = tmpRadius;
        }

        Handles.color = oldColor;
    }
}