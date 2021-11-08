using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Tween))]
public class TweenPropertyDrawer : PropertyDrawer
{
    int lines;

    SerializedProperty mode, ease, time, delay, alpha, scale, rotation, sizeVector, scaleVector, positionVector, color, uniformScale, hideOnComplete;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        mode = property.FindPropertyRelative("mode");

        if (mode.intValue == 1)
            lines = 7;
        else
            lines = 6;

        return EditorGUI.GetPropertyHeight(property, label, false) * lines;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = GetPropertyHeight(property, GUIContent.none);

        ease = property.FindPropertyRelative("ease");
        time = property.FindPropertyRelative("time");
        delay = property.FindPropertyRelative("delay");

        alpha = property.FindPropertyRelative("alpha");
        scale = property.FindPropertyRelative("scale");
        sizeVector = property.FindPropertyRelative("sizeVector");
        color = property.FindPropertyRelative("color");
        scaleVector = property.FindPropertyRelative("scaleVector");
        positionVector = property.FindPropertyRelative("positionVector");
        rotation = property.FindPropertyRelative("rotation");

        uniformScale = property.FindPropertyRelative("uniformScale");
        hideOnComplete = property.FindPropertyRelative("hideOnComplete");

        EditorGUI.BeginProperty(position, label, property);

        Rect lineOneRect = new Rect(position.x, position.y, position.width, position.height / lines);
        Rect lineTwoRect = new Rect(position.x, position.y + (position.height / lines) * 1, position.width, position.height / lines);
        Rect lineThreeRect = new Rect(position.x, position.y + (position.height / lines) * 2, position.width, position.height / lines);
        Rect lineFourRect = new Rect(position.x, position.y + (position.height / lines) * 3, position.width, position.height / lines);
        Rect lineFiveRect = new Rect(position.x, position.y + (position.height / lines) * 4, position.width, position.height / lines);
        Rect lineSixRect = new Rect(position.x, position.y + (position.height / lines) * 5, position.width, position.height / lines);
        Rect lineSevenRect = new Rect(position.x, position.y + (position.height / lines) * 6, position.width, position.height / lines);

        // Store editor indent settings
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        EditorGUI.PropertyField(lineOneRect, mode, new GUIContent("Mode"));
        EditorGUI.PropertyField(lineTwoRect, ease, new GUIContent("Ease"));
        EditorGUI.PropertyField(lineThreeRect, time, new GUIContent("Time"));
        EditorGUI.PropertyField(lineFourRect, delay, new GUIContent("Delay"));
        EditorGUI.PropertyField(lineFiveRect, hideOnComplete, new GUIContent("Hide on complete"));
        switch (mode.intValue)
        {
            case 0: // Alpha
                EditorGUI.PropertyField(lineSixRect, alpha, new GUIContent("Alpha"));
                break;
            case 1: // Scale
                EditorGUI.PropertyField(lineSixRect, uniformScale);
                if (uniformScale.boolValue) // Show float value for scale
                    EditorGUI.PropertyField(lineSevenRect, scale, new GUIContent("Scale"));
                else                        // Show vector value for scale
                    EditorGUI.PropertyField(lineSevenRect, scaleVector, new GUIContent("Scale"));
                break;
            case 2: // Size
                EditorGUI.PropertyField(lineSixRect, sizeVector, new GUIContent("Size"));
                break;
            case 3: // Color
                EditorGUI.PropertyField(lineSixRect, color, new GUIContent("Color"));
                break;
            case 4: // Position
                EditorGUI.PropertyField(lineSixRect, positionVector, new GUIContent("Position"));
                break;
            case 5: // Rotation
                EditorGUI.PropertyField(lineSixRect, rotation, new GUIContent("Rotation"));
                break;
        }

        // Reset indent settings
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}