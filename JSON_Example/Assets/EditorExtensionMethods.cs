using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// https://forum.unity.com/threads/loop-through-serializedproperty-children.435119/

public static class EditorExtensionMethods
{
    /// <summary>
    /// Gets all children of `SerializedProperty` at 1 level depth.
    /// </summary>
    /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
    /// <returns>Collection of `SerializedProperty` children.</returns>
    public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty serializedProperty)
    {
        SerializedProperty currentProperty = serializedProperty.Copy();

        if (currentProperty.Next(true))
        {
            do
            {
                yield return currentProperty;
            }
            while (currentProperty.Next(false));
        }
    }

    /// <summary>
    /// Gets visible children of `SerializedProperty` at 1 level depth.
    /// </summary>
    /// <param name="serializedProperty">Parent `SerializedProperty`.</param>
    /// <returns>Collection of `SerializedProperty` children.</returns>
    public static IEnumerable<SerializedProperty> GetVisibleChildren(this SerializedProperty serializedProperty)
    {
        SerializedProperty currentProperty = serializedProperty.Copy();

        if (currentProperty.NextVisible(true))
        {
            do
            {
                yield return currentProperty;
            }
            while (currentProperty.NextVisible(false));
        }
    }
}