using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using Abacus.Internal;

/// <summary>
/// Editor window for modifying the settings of the Abacus plugin
/// </summary>
class AbacusSettingsEditorWindow : EditorWindow
{
    private SerializedObject _serializedSettings;

    private SerializedProperty _defaultTimeStep;
    private SerializedProperty _bindingFlags;
    private SerializedProperty _fileOutputType;
    private SerializedProperty _formatOutputFile;
    private SerializedProperty _path;

    private bool _isDirty = false;

    private BindingFlags _flags;

    [MenuItem("Window/Abacus Settings")]
    public static void ShowWindow()
    {
        AbacusSettingsEditorWindow window = EditorWindow.GetWindow<AbacusSettingsEditorWindow>(false, "Abacus Settings");
    }

    private void OnEnable()
    {
        var abacusSettings = AbacusSettings.Instance;

        _isDirty = false;

        if (abacusSettings == null)
        {
            Debug.LogWarning("No AbacusWriter Settings object was found in this project's Assets.");
            return;
        }

        _serializedSettings = new SerializedObject(abacusSettings);

        _defaultTimeStep = _serializedSettings.FindProperty("DefaultTimeStep");
        _bindingFlags = _serializedSettings.FindProperty("BindingFlags");
        //_fileOutputType = _serializedSettings.FindProperty("DefaultTimeStep");
        _formatOutputFile = _serializedSettings.FindProperty("FormatOutput");
        _path = _serializedSettings.FindProperty("WritePath");
    }

    private void OnGUI()
    {
        if (_serializedSettings == null)
        {
            if (GUILayout.Button("Create a New Settings File"))
            {
                AbacusSettings asset = ScriptableObject.CreateInstance<AbacusSettings>();

                AssetDatabase.CreateAsset(asset, "Assets/Abacus Settings.asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = asset;

                _isDirty = true;
            }

            if (_isDirty)
            {
                EditorGUILayout.LabelField("Please close and re-open this window to refresh");
            }

            return;
        }

        _serializedSettings.Update();
        
        _defaultTimeStep.floatValue = EditorGUILayout.FloatField(new GUIContent("Default Time Step", "Default interval between value recording"), _defaultTimeStep.floatValue);
        _flags = (BindingFlags)EditorGUILayout.EnumFlagsField(new GUIContent("Binding Flags", "What fields/properties should be made readable by Abacus?"), _flags);
        _formatOutputFile.boolValue = EditorGUILayout.Toggle(new GUIContent("Format Output", "Should the output file be indented or minimized for space?"), _formatOutputFile.boolValue);

        EditorGUILayout.BeginHorizontal();

        _path.stringValue = EditorGUILayout.TextField(new GUIContent("Save Path", "Where should the output file be saved?"), _path.stringValue);
        if (GUILayout.Button("..", GUILayout.Width(20), GUILayout.Height(15)))
        {
            _path.stringValue = EditorUtility.OpenFolderPanel("Abacus Output Path", _path.stringValue, "");
        }

        EditorGUILayout.EndHorizontal();

        _serializedSettings.ApplyModifiedProperties();
    }
}
