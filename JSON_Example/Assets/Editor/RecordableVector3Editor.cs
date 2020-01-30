using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[CustomEditor(typeof(RecordableVector3))]
public class RecordableVector3Editor : Editor
{
    SerializedProperty lookAtPoint;
    SerializedProperty val;

    int selectedComponent = 0;
    int selectedProperty = 0;

    void OnEnable()
    {
        lookAtPoint = serializedObject.FindProperty("recordFrom");
        val = serializedObject.FindProperty("val");
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        GUI.enabled = false;
            EditorGUILayout.PropertyField(lookAtPoint);
            EditorGUILayout.PropertyField(val);
        GUI.enabled = true;

        var components = Selection.activeGameObject.GetComponents<Component>();

        List<string> componentnames = new List<string>();
        for (int i = 0; i < components.Length; i++)
        {
            componentnames.Add(components[i].GetType().ToString());
        }

        //string[] options = new string[]
        //{
        //    "Option1", "Option2", "Option3",
        //};
        selectedComponent = EditorGUILayout.Popup("Component", selectedComponent, componentnames.ToArray());

        var recordable = Selection.activeGameObject.GetComponent<RecordableVector3>();
        recordable.recordFrom = components[selectedComponent];

        PropertyInfo[] myPropertyInfo;
        // Get the properties of 'Type' class object.
        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        myPropertyInfo = components[selectedComponent].GetType().GetProperties(bindingFlags);

        List<string> propertynames = new List<string>();
        for (int i = 0; i < myPropertyInfo.Length; i++)
        {
            //var get = myPropertyInfo[i].GetGetMethod();
            //if (get == null) continue;
            //else if (get.GetCustomAttribute<ObsoleteAttribute>() != null) continue;
            if (myPropertyInfo[i].PropertyType != typeof(Vector3)) continue;
            propertynames.Add(myPropertyInfo[i].ToString());
        }

        selectedProperty = EditorGUILayout.Popup("Property", selectedProperty, propertynames.ToArray());

        var property = myPropertyInfo[selectedProperty];
        //recordable.propertyInfo = property;
        recordable.propertyNamePlaceholder = property.Name;
        //recordable.propertyType = property.PropertyType;
        //Debug.Log(property.PropertyType);
        //property.GetValue

        //serializedObject.ApplyModifiedProperties();
    }
}