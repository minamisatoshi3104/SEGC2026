using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ShowIfEnumAttribute))]
public class ShowIfEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute cond = (ShowIfEnumAttribute)attribute;

        SerializedProperty enumProp = FindEnumProperty(property, cond.enumFieldName);

        if (enumProp == null)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        int enumValue = enumProp.intValue;
        string enumName = enumProp.enumNames.Length > enumProp.enumValueIndex && enumProp.enumValueIndex >= 0 ? enumProp.enumNames[enumProp.enumValueIndex] : null;

        bool show = ShouldShow(cond, enumValue, enumName);

        // Special handling: if this property is a list (isArray) we want to still draw the foldout even when hidden
        if (!show && property.isArray && property.propertyType == SerializedPropertyType.Generic)
        {
            // Draw foldout but no children
            EditorGUI.BeginProperty(position, label, property);
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
            EditorGUI.EndProperty();
            return;
        }

        if (show)
            EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute cond = (ShowIfEnumAttribute)attribute;
        SerializedProperty enumProp = FindEnumProperty(property, cond.enumFieldName);

        if (enumProp == null)
            return EditorGUI.GetPropertyHeight(property, label, true);

        int enumValue = enumProp.intValue;
        string enumName = enumProp.enumNames.Length > enumProp.enumValueIndex && enumProp.enumValueIndex >= 0 ? enumProp.enumNames[enumProp.enumValueIndex] : null;

        bool show = ShouldShow(cond, enumValue, enumName);

        if (show)
            return EditorGUI.GetPropertyHeight(property, label, true);

        // If it's an array generic property, return single line height for the foldout
        if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
            return EditorGUIUtility.singleLineHeight;

        return -EditorGUIUtility.standardVerticalSpacing;
    }

    bool ShouldShow(ShowIfEnumAttribute cond, int enumValue, string enumName)
    {
        if ((cond.enumValues != null && cond.enumValues.Length > 0))
        {
            foreach (var v in cond.enumValues)
            {
                if (v == enumValue)
                    return true;
            }
        }

        if ((cond.enumNames != null && cond.enumNames.Length > 0) && !string.IsNullOrEmpty(enumName))
        {
            foreach (var n in cond.enumNames)
            {
                if (n == enumName)
                    return true;
            }
        }

        // Default behavior: if no conditions specified, show when enumValue !=0
        if ((cond.enumValues == null || cond.enumValues.Length == 0) && (cond.enumNames == null || cond.enumNames.Length == 0))
            return enumValue != 0;

        return false;
    }

    SerializedProperty FindEnumProperty(SerializedProperty property, string enumFieldName)
    {
        string path = property.propertyPath;

        //1) Replace current property's name with enum field name
        string replaced = path.Replace(property.name, enumFieldName);
        SerializedProperty p = property.serializedObject.FindProperty(replaced);
        if (p != null)
            return p;

        //2) walk up parent paths
        string[] parts = path.Split('.');
        for (int i = parts.Length - 1; i >= 0; i--)
        {
            string parentPath = string.Join(".", parts, 0, i);
            string candidate = string.IsNullOrEmpty(parentPath) ? enumFieldName : parentPath + "." + enumFieldName;
            p = property.serializedObject.FindProperty(candidate);
            if (p != null)
                return p;
        }

        //3) root
        return property.serializedObject.FindProperty(enumFieldName);
    }
}
