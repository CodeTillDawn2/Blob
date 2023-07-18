using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

public class ScriptedImporterEditorScript : ScriptedImporterEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw custom GUI

        serializedObject.ApplyModifiedProperties();

        ApplyRevertGUI();
    }
}
