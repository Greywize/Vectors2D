using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Tween))]
public class TweenPropertyDrawer : PropertyDrawer
{
    int lines;

    SerializedProperty mode, ease, time, delay, alpha, scale, rotation, sizeVector, scaleVector, positionVector, color, lightIntensity, lightColor, value, textColor, uniformScale, cameraSize, continuous;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        mode = property.FindPropertyRelative("mode");

        if (mode.intValue == 1 || mode.intValue == 7)
            lines = 8;
        else
            lines = 7;

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
        textColor = property.FindPropertyRelative("textColor");
        lightIntensity = property.FindPropertyRelative("lightIntensity");
        lightColor = property.FindPropertyRelative("lightColor");
        value = property.FindPropertyRelative("value");
        scaleVector = property.FindPropertyRelative("scaleVector");
        positionVector = property.FindPropertyRelative("positionVector");
        rotation = property.FindPropertyRelative("rotation");
        cameraSize = property.FindPropertyRelative("cameraSize");

        uniformScale = property.FindPropertyRelative("uniformScale");
        continuous = property.FindPropertyRelative("continuous");

        EditorGUI.BeginProperty(position, label, property);

        Rect lineOneRect = new Rect(position.x, position.y, position.width, position.height / lines);
        Rect lineTwoRect = new Rect(position.x, position.y + (position.height / lines) * 1, position.width, position.height / lines);
        Rect lineThreeRect = new Rect(position.x, position.y + (position.height / lines) * 2, position.width, position.height / lines);
        Rect lineFourRect = new Rect(position.x, position.y + (position.height / lines) * 3, position.width, position.height / lines);
        Rect lineFiveRect = new Rect(position.x, position.y + (position.height / lines) * 4, position.width, position.height / lines);
        Rect lineSixRect = new Rect(position.x, position.y + (position.height / lines) * 5, position.width, position.height / lines);
        Rect lineSevenRect = new Rect(position.x, position.y + (position.height / lines) * 6, position.width, position.height / lines);
        Rect lineEightRect = new Rect(position.x, position.y + (position.height / lines) * 7, position.width, position.height / lines);

        EditorGUI.LabelField(lineOneRect, "Tween");
        // Store editor indent settings
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        EditorGUI.PropertyField(lineTwoRect, mode, new GUIContent("Mode"));
        EditorGUI.PropertyField(lineThreeRect, ease, new GUIContent("Ease"));
        EditorGUI.PropertyField(lineFourRect, time, new GUIContent("Time"));
        EditorGUI.PropertyField(lineFiveRect, delay, new GUIContent("Delay"));
        EditorGUI.PropertyField(lineSixRect, continuous, new GUIContent("Continous"));
        switch (mode.intValue)
        {
            case 0: // Alpha
                EditorGUI.PropertyField(lineSevenRect, alpha, new GUIContent("Alpha"));
                break;
            case 1: // Scale
                EditorGUI.PropertyField(lineSevenRect, uniformScale);
                if (uniformScale.boolValue) // Show float value for scale
                    EditorGUI.PropertyField(lineEightRect, scale, new GUIContent("Scale"));
                else                        // Show vector value for scale
                    EditorGUI.PropertyField(lineEightRect, scaleVector, new GUIContent("Scale"));
                break;
            case 2: // Size
                EditorGUI.PropertyField(lineSevenRect, sizeVector, new GUIContent("Size"));
                break;
            case 3: // Color
                EditorGUI.PropertyField(lineSevenRect, color, new GUIContent("Color"));
                break;
            case 4: // Text Color
                EditorGUI.PropertyField(lineSevenRect, textColor, new GUIContent("Text Color"));
                break;
            case 5: // Position
                EditorGUI.PropertyField(lineSevenRect, positionVector, new GUIContent("Position"));
                break;
            case 6: // Rotation
                EditorGUI.PropertyField(lineSevenRect, rotation, new GUIContent("Rotation"));
                break;
            case 7: // Light
                EditorGUI.PropertyField(lineSevenRect, lightIntensity, new GUIContent("Intensity"));
                EditorGUI.PropertyField(lineEightRect, lightColor, new GUIContent("Color"));
                break;
            case 8: // Camera Size
                EditorGUI.PropertyField(lineSevenRect, cameraSize, new GUIContent("Camera Size"));
                break;
            case 9: // Custom Value
                EditorGUI.PropertyField(lineSevenRect, value, new GUIContent("Custom Value"));
                break;
        }

        // Reset indent settings
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}