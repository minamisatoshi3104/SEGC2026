using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ShowIfEnumAttribute : PropertyAttribute
{
    public readonly string enumFieldName;
    public readonly int[] enumValues;
    public readonly string[] enumNames;

    // Numeric constructor
    public ShowIfEnumAttribute(string enumFieldName, params int[] enumValues)
    {
        this.enumFieldName = enumFieldName;
        this.enumValues = enumValues ?? Array.Empty<int>();
        this.enumNames = Array.Empty<string>();
    }

    // Name-based constructor
    public ShowIfEnumAttribute(string enumFieldName, params string[] enumNames)
    {
        this.enumFieldName = enumFieldName;
        this.enumNames = enumNames ?? Array.Empty<string>();
        this.enumValues = Array.Empty<int>();
    }
}
