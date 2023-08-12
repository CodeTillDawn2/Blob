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

        AIEditorBaker.BakeAI();

    }
}
#endif
