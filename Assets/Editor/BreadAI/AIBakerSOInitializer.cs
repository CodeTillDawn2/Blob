#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
public static class AIBakerSOInitializer
{
    static AIBakerSOInitializer()
    {
        // This will be called every time the domain reloads
        EditorApplication.delayCall += ReBakeData;
    }

    private static void ReBakeData()
    {
        // Locate your AIBakerSO instance. Assuming you have only one in your project:
        string[] guids = AssetDatabase.FindAssets("t:AIBakerSO");
        if (guids.Length > 0)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            AIBakerSO baker = AssetDatabase.LoadAssetAtPath<AIBakerSO>(assetPath);
            if (baker != null)
            {
                baker.BakeAI(); // This will re-bake your data.
            }
        }
    }
}
#endif
