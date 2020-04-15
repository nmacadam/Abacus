using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abacus.Internal;
using UnityEditor;
using UnityEngine;

namespace Abacus.Editor
{
    /// <summary>
    /// Custom inspector for property metrics
    /// </summary>
    [CustomEditor(typeof(PropertyMetric<>), true)]
    public class PropertyMetricEditor : UnityEditor.Editor
    {
        private SerializedProperty timeStep;

        private SerializedProperty recordGameObject;
        private SerializedProperty recordFrom;
        private SerializedProperty recordFromIndex;
        private SerializedProperty recordName;
        private SerializedProperty recordNameIndex;
        private SerializedProperty recordOnStart;

        private IRecordable _recordable;

        private bool _optionsFoldoutOpen = false;

        private void OnEnable()
        {
            // Get properties from serializedObject
            recordGameObject = serializedObject.FindProperty("recordGameObject");
            recordFrom = serializedObject.FindProperty("recordFrom");
            recordFromIndex = serializedObject.FindProperty("recordFromIndex");
            recordName = serializedObject.FindProperty("recordName");
            recordNameIndex = serializedObject.FindProperty("recordNameIndex");
            recordOnStart = serializedObject.FindProperty("_recordOnStart");

            timeStep = serializedObject.FindProperty("_timeStep");

            // Retrieve the gameobject to which the component is attached
            var component = (serializedObject.targetObject as Component);
            _recordable = (component as IRecordable);
        }

        public override void OnInspectorGUI()
        {
            // Update object
            serializedObject.Update();

            recordGameObject.objectReferenceValue = EditorGUILayout.ObjectField("Record From", recordGameObject.objectReferenceValue, typeof(GameObject), true);

            var gameObject = (GameObject)recordGameObject.objectReferenceValue;

            if (recordGameObject.objectReferenceValue != null)
            {
                // Get all components on this edtior's component's gameObject 
                var components = gameObject.GetComponents<Component>();

                string[] componentNames = components.Select(c => c.GetType().ToString()).ToArray();

                recordFromIndex.intValue = EditorGUILayout.Popup("Component", recordFromIndex.intValue, componentNames);

                //var recordable = _gameObject.GetComponent<FieldMetric<T>>();
                _recordable.SetSource(components[recordFromIndex.intValue]);

                DisplayPropertyInfo(components);

            }

            recordOnStart.boolValue = EditorGUILayout.Toggle("Record On Start", recordOnStart.boolValue);

            _optionsFoldoutOpen = EditorGUILayout.Foldout(_optionsFoldoutOpen, "Options & Debug");
            if (_optionsFoldoutOpen)
            {
                EditorGUI.indentLevel++;

                timeStep.floatValue = EditorGUILayout.FloatField("Time Step (s)", timeStep.floatValue);

                EditorGUILayout.Space();

                GUI.enabled = false;
                EditorGUILayout.PropertyField(recordFrom);
                //EditorGUILayout.PropertyField(_recordable.GetLastRecordedValue());
                GUI.enabled = true;

                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DisplayPropertyInfo(Component[] components)
        {
            PropertyInfo[] propertyInfo;
            // Get the properties of 'Type' class object.
            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            propertyInfo = components[recordFromIndex.intValue].GetType().GetProperties(bindingFlags);

            List<string> propertynames = new List<string>();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                //var get = myPropertyInfo[i].GetGetMethod();
                //if (get == null) continue;
                //else if (get.GetCustomAttribute<ObsoleteAttribute>() != null) continue;
                if (propertyInfo[i].PropertyType != _recordable.GetValueType()) continue;
                propertynames.Add(propertyInfo[i].Name);
            }

            if (propertynames.Count == 0)
            {
                GUI.enabled = false;
                EditorGUILayout.LabelField("No properties available on this component");
                GUI.enabled = true;
            }
            else
            {
                recordNameIndex.intValue = EditorGUILayout.Popup("Property", recordNameIndex.intValue, propertynames.ToArray());

                var property = propertyInfo[recordNameIndex.intValue];

                recordName.stringValue = property.Name;
            }
        }
    }
}