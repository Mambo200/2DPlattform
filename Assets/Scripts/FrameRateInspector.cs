#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Framerate))]
public class FrameRateInspector : Editor
{
    SerializedProperty maxFrameCount;
    SerializedProperty capFrames;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        maxFrameCount = serializedObject.FindProperty("m_MaxFrameCount");
        capFrames = serializedObject.FindProperty("m_CapFrames");
    }

    public override void OnInspectorGUI()
    {
        maxFrameCount.intValue = EditorGUILayout.IntSlider("Max Frame Count", maxFrameCount.intValue, 0, 180);
        ProgressBar(maxFrameCount.intValue / 100.0f, "Max Frame Count");

        capFrames.boolValue = EditorGUILayout.Toggle("Cap Frames", capFrames.boolValue);



    }

    // Custom GUILayout progress bar.
    void ProgressBar(float value, string label)
    {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }

    const string resourceFilename = "custom-editor-uie";

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement customInspector = new VisualElement();
        var visualTree = Resources.Load(resourceFilename) as VisualTreeAsset;
        visualTree.CloneTree(customInspector);
        customInspector.styleSheets.Add(Resources.Load($"{resourceFilename}-style") as StyleSheet);
        return customInspector;
    }
}
#endif