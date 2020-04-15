using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abacus.Internal;
using UnityEditor;
using UnityEngine;

namespace Abacus.Editor
{
    /// <summary>
    /// Custom inspector for field metrics
    /// </summary>
    [CustomEditor(typeof(FieldMetric<>), true)]
    public class FieldMetricEditor : UnityEditor.Editor
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

                DisplayFieldInfo(components);
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

        private void DisplayFieldInfo(Component[] components)
        {
            FieldInfo[] fieldInfo;
            // Get the properties of 'Type' class object.
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            fieldInfo = components[recordFromIndex.intValue].GetType().GetFields(bindingFlags);

            List<string> fieldNames = new List<string>();
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                //var get = myPropertyInfo[i].GetGetMethod();
                //if (get == null) continue;
                //else if (get.GetCustomAttribute<ObsoleteAttribute>() != null) continue;
                if (fieldInfo[i].FieldType != _recordable.GetValueType()) continue;
                fieldNames.Add(fieldInfo[i].Name);
            }

            if (fieldNames.Count == 0)
            {
                GUI.enabled = false;
                EditorGUILayout.LabelField("No fields available on this component");
                GUI.enabled = true;
            }
            else
            {
                recordNameIndex.intValue = EditorGUILayout.Popup("Field", recordNameIndex.intValue, fieldNames.ToArray());

                var field = fieldInfo[recordNameIndex.intValue];

                recordName.stringValue = field.Name;
            }
        }
    }
}