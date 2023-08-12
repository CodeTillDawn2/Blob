using UnityEditor;

[CustomEditor(typeof(RuntimeSet<>), true)]
public class RuntimeSetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw the list
        EditorGUILayout.PropertyField(serializedObject.FindProperty("items"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
