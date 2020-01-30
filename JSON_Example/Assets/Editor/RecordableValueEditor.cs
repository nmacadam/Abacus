using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// todo: =============================================================
// + cache some of the stuff like component names

public abstract class RecordableValueEditor<T> : Editor
{
    private SerializedProperty recordFrom;
    private SerializedProperty propertyName;
    
    private int _selectedComponent;
    private int _selectedProperty;

    private string _selectedComponentKey;
    private string _selectedPropertyKey;

    private GameObject _gameObject;

    private void OnEnable()
    {
        recordFrom = serializedObject.FindProperty("recordFrom");
        propertyName = serializedObject.FindProperty("propertyNamePlaceholder");

        _gameObject = (serializedObject.targetObject as Component).gameObject;
        var instanceID = serializedObject.targetObject.GetHashCode();

        _selectedComponentKey = $"abacus_component_id:{instanceID}";
        _selectedPropertyKey = $"abacus_property_id:{instanceID}";

        _selectedComponent = EditorPrefs.GetInt(_selectedComponentKey, 0);
        _selectedProperty = EditorPrefs.GetInt(_selectedPropertyKey, 0);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Get all components on this edtior's component's gameObject 
        var components = _gameObject.GetComponents<Component>();

        // Get the names of all components so we can display them in a popup
        List<string> componentnames = new List<string>();
        for (int i = 0; i < components.Length; i++)
        {
            componentnames.Add(components[i].GetType().ToString());
        }

        _selectedComponent = EditorGUILayout.Popup("Component", _selectedComponent, componentnames.ToArray());

        var recordable = _gameObject.GetComponent<RecordableValue<T>>();
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

        EditorPrefs.SetInt(_selectedComponentKey, _selectedComponent);
        EditorPrefs.SetInt(_selectedPropertyKey, _selectedProperty);

        serializedObject.ApplyModifiedProperties();
    }
}