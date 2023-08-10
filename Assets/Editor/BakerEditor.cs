using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIBaker))]
public class BakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Reference to our script
        AIBaker bakerScript = (AIBaker)target;

        // Add a button in the inspector
        if (GUILayout.Button("Bake AI"))
        {
            if (bakerScript.BakerSO != null)
            {
                bakerScript.BakerSO.BakeAI();
            }
            else
            {
                Debug.LogWarning("bakerSO is null! Please assign a BakerSO instance first.");
            }
        }
    }
}
