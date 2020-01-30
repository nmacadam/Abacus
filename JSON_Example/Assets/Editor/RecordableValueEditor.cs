using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// todo: =============================================================
// + pretty iffy using the selection instead of serialized properties!\
// + current editor prefs usage means i only get to have one recordable; they need uids or something

public abstract class RecordableValueEditor<T> : Editor
{
    private SerializedProperty recordFrom;
    private SerializedProperty propertyName;
    
    private int _selectedComponent;
    private int _selectedProperty;

    private void OnEnable()
    {
        recordFrom = serializedObject.FindProperty("recordFrom");
        propertyName = serializedObject.FindProperty("propertyNamePlaceholder");

        _selectedComponent = EditorPrefs.GetInt("abacus_component", 0);
        _selectedProperty = EditorPrefs.GetInt("abacus_property", 0);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        

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

        _selectedComponent = EditorGUILayout.Popup("Component", _selectedComponent, componentnames.ToArray());

        var recordable = Selection.activeGameObject.GetComponent<RecordableValue<T>>();
        recordable.recordFrom = components[_selectedComponent];
        //recordFrom = components[_selectedComponent];

        PropertyInfo[] myPropertyInfo;
        // Get the properties of 'Type' class object.
        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        myPropertyInfo = components[_selectedComponent].GetType().GetProperties(bindingFlags);

        List<string> propertynames = new List<string>();
        for (int i = 0; i < myPropertyInfo.Length; i++)
        {
            //var get = myPropertyInfo[i].GetGetMethod();
            //if (get == null) continue;
            //else if (get.GetCustomAttribute<ObsoleteAttribute>() != null) continue;
            if (myPropertyInfo[i].PropertyType != typeof(T)) continue;
            propertynames.Add(myPropertyInfo[i].ToString());
        }

        _selectedProperty = EditorGUILayout.Popup("Property", _selectedProperty, propertynames.ToArray());

        var property = myPropertyInfo[_selectedProperty];

        propertyName.stringValue = property.Name;

        GUI.enabled = false;
        EditorGUILayout.PropertyField(recordFrom);
        //EditorGUILayout.PropertyField(val);
        GUI.enabled = true;

        EditorPrefs.SetInt("abacus_component", _selectedComponent);
        EditorPrefs.SetInt("abacus_property", _selectedProperty);

        serializedObject.ApplyModifiedProperties();
    }
}