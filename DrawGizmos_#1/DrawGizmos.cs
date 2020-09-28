using UnityEditor;
using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    public enum Shape { Box = 0, Sphere = 1, Capsule = 2 }

    [SerializeField] Shape _shape;
    public Shape m_Shape { get => _shape; set => _shape = value; }

    [SerializeField] Color _color = Color.white;
    public Color Color { get => _color; set => _color = value; }

    private void OnDrawGizmos()
    {

    }
#if UNITY_EDITOR
    void DrawCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color), bool _wireFrame = false)
    {
        if (_color != default(Color))
            Handles.color = _color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            var pointOffset = (_height - (_radius * 2)) / 2;

            //draw sideways
            if (_wireFrame)
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
            else
                Handles.DrawSolidArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
            Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
            Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
            if (_wireFrame)
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
            else
                Handles.DrawSolidArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
            //draw frontways
            if (_wireFrame)
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
            else
                Handles.DrawSolidArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
            Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
            Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
            if (_wireFrame)
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
            else
                Handles.DrawSolidArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
            //draw center
            if (_wireFrame)
            {
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);
            }
            else
            {
                Handles.DrawSolidDisc(Vector3.up * pointOffset, Vector3.up, _radius);
                Handles.DrawSolidDisc(Vector3.down * pointOffset, Vector3.up, _radius);
            }
        }
    }
#endif
}
#if UNITY_EDITOR
[CustomEditor(typeof(DrawGizmos))]
public class DrawGizmosEditor : Editor
{
    DrawGizmos script { get => target as DrawGizmos; }

    SerializedObject s_Script;
    SerializedProperty p_Shape;
    SerializedProperty p_Color;

    private void OnEnable()
    {
        s_Script = new SerializedObject(script);
        p_Shape = s_Script.FindProperty("_shape");
        p_Color = s_Script.FindProperty("_color");
    }
    public override void OnInspectorGUI()
    {
        s_Script.Update();

        EditorGUILayout.PropertyField(p_Shape);
        EditorGUILayout.PropertyField(p_Color);

        s_Script.ApplyModifiedProperties();
    }
}
#endif
