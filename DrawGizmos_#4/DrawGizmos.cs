﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class DrawGizmos : MonoBehaviour
{
    public enum Shape { Box = 0, Sphere = 1, Capsule = 2 }

    [SerializeField] bool _draw = true;
    public bool Draw { get => _draw; set => _draw = value; }

    [SerializeField] Shape _shape;
    public Shape m_Shape { get => _shape; set => _shape = value; }

    [SerializeField] Color _color = Color.white;
    public Color Color { get => _color; set => _color = value; }

    [SerializeField] bool _wireFrame;
    public bool WireFrame { get => _wireFrame; set => _wireFrame = value; }

    [SerializeField] bool _freeSync;
    public bool FreeSycn { get => _freeSync; set => _freeSync = value; }

    //Box Parameters
    [SerializeField] Vector3 _cubeSize = Vector3.one;
    public Vector3 CubeSize { get => _cubeSize; set => _cubeSize = value; }

    private void OnDrawGizmos()
    {
        if (!Draw) return;

        DrawShape();
    }
    void DrawShape()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color;

        switch (m_Shape)
        {
            case Shape.Box:
                DrawCube();
                break;
            case Shape.Sphere:
                DrawSphere();
                break;
            case Shape.Capsule:
                DrawCap();
                break;
        }
    }
    void DrawCube()
    {
        if (!WireFrame)
            Gizmos.DrawCube(Vector3.zero, CubeSize);
        else
            Gizmos.DrawWireCube(Vector3.zero, CubeSize);
    }
    void DrawSphere()
    {
       
    }
    void DrawCap()
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
[CanEditMultipleObjects,CustomEditor(typeof(DrawGizmos))]
public class DrawGizmosEditor : Editor
{
    DrawGizmos script { get => target as DrawGizmos; }

    SerializedObject s_Script;

    SerializedProperty p_Shape;
    SerializedProperty p_Color;
    SerializedProperty p_WireFrame;
    SerializedProperty p_FreeSync;

    private void OnEnable()
    {
        s_Script = new SerializedObject(script);
        p_Shape = s_Script.FindProperty("_shape");
        p_Color = s_Script.FindProperty("_color");
        p_WireFrame = s_Script.FindProperty("_wireFrame");
        p_FreeSync = s_Script.FindProperty("_freeSync");
    }
    public override void OnInspectorGUI()
    {
        s_Script.Update();

        EditorGUILayout.BeginVertical("Box");

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        script.Draw = EditorGUILayout.Toggle(script.Draw, GUILayout.Width(20));
        EditorGUILayout.LabelField("Draw", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
            SceneView.lastActiveSceneView.Repaint();

        if (script.Draw)
        {
            EditorGUILayout.PropertyField(p_Shape);
            EditorGUILayout.PropertyField(p_Color);
            EditorGUILayout.PropertyField(p_WireFrame);
            EditorGUILayout.PropertyField(p_FreeSync);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        s_Script.ApplyModifiedProperties();
    }
    private void OnSceneGUI()
    {
        
    }
}
#endif
