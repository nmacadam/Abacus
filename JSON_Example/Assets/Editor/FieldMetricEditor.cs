using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class FieldMetricEditor<T> : Editor
{
    private SerializedProperty timeStep;

    private SerializedProperty recordFrom;
    private SerializedProperty recordName;
    private SerializedProperty recordOnStart;

    private int _selectedComponent;
    private int _selectedRecord;

    private string _selectedComponentKey;
    private string _selectedRecordKey;

    private GameObject _gameObject;

    private bool _optionsFoldoutOpen = false;

    private void OnEnable()
    {
        // Get properties from serializedObject
        recordFrom = serializedObject.FindProperty("recordFrom");
        recordName = serializedObject.FindProperty("recordName");
        recordOnStart = serializedObject.FindProperty("_recordOnStart");

        timeStep = serializedObject.FindProperty("_timeStep");

        // Retrieve the gameobject to which the component is attached
        _gameObject = (serializedObject.targetObject as Component).gameObject;
        var instanceID = serializedObject.targetObject.GetHashCode();

        // Use the gameobject's instance id to create a key for the component and recordable value
        _selectedComponentKey = $"abacus_component_id:{instanceID}";
        _selectedRecordKey = $"abacus_property_id:{instanceID}";

        // Use keys to store popup indicies in EditorPrefs
        _selectedComponent = EditorPrefs.GetInt(_selectedComponentKey, 0);
        _selectedRecord = EditorPrefs.GetInt(_selectedRecordKey, 0);
    }

    public override void OnInspectorGUI()
    {
        // Update object
        serializedObject.Update();

        // Get all components on this edtior's component's gameObject 
        var components = _gameObject.GetComponents<Component>();

        string[] componentNames = components.Select(c => c.GetType().ToString()).ToArray();

        _selectedComponent = EditorGUILayout.Popup("Component", _selectedComponent, componentNames);

        var recordable = _gameObject.GetComponent<FieldMetric<T>>();
        recordable.SetSource(components[_selectedComponent]);

        FieldInfo[] fieldInfo;
        // Get the properties of 'Type' class object.
        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        fieldInfo = components[_selectedComponent].GetType().GetFields(bindingFlags);

        List<string> fieldNames = new List<string>();
        for (int i = 0; i < fieldInfo.Length; i++)
        {
            //var get = myPropertyInfo[i].GetGetMethod();
            //if (get == null) continue;
            //else if (get.GetCustomAttribute<ObsoleteAttribute>() != null) continue;
            if (fieldInfo[i].FieldType != typeof(T)) continue;
            fieldNames.Add(fieldInfo[i].Name);
        }

        _selectedRecord = EditorGUILayout.Popup("Field", _selectedRecord, fieldNames.ToArray());

        var field = fieldInfo[_selectedRecord];

        recordName.stringValue = field.Name;

        recordOnStart.boolValue = EditorGUILayout.Toggle("Record On Start", recordOnStart.boolValue);

        _optionsFoldoutOpen = EditorGUILayout.Foldout(_optionsFoldoutOpen, "Options & Debug");
        if (_optionsFoldoutOpen)
        {
            EditorGUI.indentLevel++;

            timeStep.floatValue = EditorGUILayout.FloatField("Time Step (s)", timeStep.floatValue);

            EditorGUILayout.Space();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(recordFrom);
            //EditorGUILayout.PropertyField(recordable.GetLastRecordedValue());
            GUI.enabled = true;

            EditorGUI.indentLevel--;
        }

        EditorPrefs.SetInt(_selectedComponentKey, _selectedComponent);
        EditorPrefs.SetInt(_selectedRecordKey, _selectedRecord);

        serializedObject.ApplyModifiedProperties();
    }
}
