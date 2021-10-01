using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UITween))]
public class UITweenEditor : Editor
{
    UITween tweenInterface;

    SerializedProperty tweenType;
    SerializedProperty tweenMode;
    SerializedProperty tweenTime;

    SerializedProperty alphaOut;
    SerializedProperty alphaIn;

    SerializedProperty uniformScale;
    SerializedProperty scaleOut;
    SerializedProperty scaleIn;
    SerializedProperty scaleOutVector;
    SerializedProperty scaleInVector;

    SerializedProperty positionOut;
    SerializedProperty positionIn;

    SerializedProperty rotationOut;
    SerializedProperty rotationIn;

    public void OnEnable()
    {
        tweenInterface = (UITween)target;

        tweenType = serializedObject.FindProperty("tweenType");
        tweenMode = serializedObject.FindProperty("tweenMode");
        tweenTime = serializedObject.FindProperty("tweenTime");

        alphaOut = serializedObject.FindProperty("alphaOut");
        alphaIn = serializedObject.FindProperty("alphaIn");
        scaleOutVector = serializedObject.FindProperty("scaleOutVector");
        scaleInVector = serializedObject.FindProperty("scaleInVector");

        uniformScale = serializedObject.FindProperty("uniformScale");
        scaleOut = serializedObject.FindProperty("scaleOut");
        scaleIn = serializedObject.FindProperty("scaleIn");

        positionOut = serializedObject.FindProperty("positionOut");
        positionIn = serializedObject.FindProperty("positionIn");

        rotationOut = serializedObject.FindProperty("rotationOut");
        rotationIn = serializedObject.FindProperty("rotationIn");
    }

    public override void OnInspectorGUI()
    {
        Property(tweenType);
        Property(tweenMode);
        Property(tweenTime);

        Space();
        Indent();
        if (tweenInterface.tweenMode == UITween.TweenMode.Alpha)
        {
                Property(alphaOut);
                Property(alphaIn);
        }
        if (tweenInterface.tweenMode == UITween.TweenMode.Scale)
        {
            Property(uniformScale);
            if (tweenInterface.uniformScale)
            {
                Property(scaleOut);
                Property(scaleIn);
            }
            else
            {
                Property(scaleOutVector);
                Property(scaleInVector);
            }
        }
        if (tweenInterface.tweenMode == UITween.TweenMode.Position)
        {
            Property(positionOut);
            Property(positionIn);
        }
        if (tweenInterface.tweenMode == UITween.TweenMode.Rotation)
        {
            Property(rotationOut);
            Property(rotationIn);
        }
        Outdent();
        serializedObject.ApplyModifiedProperties();
    }

    #region Wrappers
    void Header(string header)
    {
        GUILayout.Label(header, EditorStyles.boldLabel);
    }
    void Space()
    {
        EditorGUILayout.Space();
    }
    void Property(SerializedProperty property)
    {
        EditorGUILayout.PropertyField(property);
    }
    // Increase the GUI's indent level
    void Indent()
    {
        EditorGUI.indentLevel++;
    }
    // Outdent! Decreases the indent level
    void Outdent()
    {
        EditorGUI.indentLevel--;
    }
    void BeginHorizontal()
    {
        EditorGUILayout.BeginHorizontal();
    }
    void EndHorizontal()
    {
        EditorGUILayout.EndHorizontal();
    }
    #endregion
}