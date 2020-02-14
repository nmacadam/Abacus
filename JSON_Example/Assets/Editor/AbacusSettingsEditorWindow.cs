using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

class AbacusSettingsEditorWindow : EditorWindow
{
    private AbacusSettings _abacusSettings;
    private SerializedObject _serializedSettings;

    private SerializedProperty _defaultTimeStep;
    private SerializedProperty _bindingFlags;
    private SerializedProperty _fileOutputType;

    private BindingFlags _flags;

    [MenuItem("Window/Abacus Settings")]
    public static void ShowWindow()
    {
        AbacusSettingsEditorWindow window = EditorWindow.GetWindow<AbacusSettingsEditorWindow>();
    }

    public void OnEnable()
    {
        var _abacusSettings = Resources.FindObjectsOfTypeAll<AbacusSettings>().FirstOrDefault();

        if (_abacusSettings == null)
        {
            Debug.LogWarning("No Abacus Settings object was found in this project's Assets.");
            return;
        }

        _serializedSettings = new SerializedObject(_abacusSettings);

        _defaultTimeStep = _serializedSettings.FindProperty("DefaultTimeStep");
        _bindingFlags = _serializedSettings.FindProperty("BindingFlags");
        //_fileOutputType = _serializedSettings.FindProperty("DefaultTimeStep");
    }

    void OnGUI()
    {
        if (_serializedSettings == null)
            return;

        _serializedSettings.Update();
        
        _defaultTimeStep.floatValue = EditorGUILayout.FloatField("Default Time Step", _defaultTimeStep.floatValue);
        _flags = (BindingFlags)EditorGUILayout.EnumFlagsField(_flags);

        _serializedSettings.ApplyModifiedProperties();
    }
}
