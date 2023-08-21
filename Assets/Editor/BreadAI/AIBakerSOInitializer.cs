#if UNITY_EDITOR
using System;
using UnityEditor;
[InitializeOnLoad]
public static class AIBakerSOInitializer
{

    static AIBakerSOInitializer()
    {
        // This will be called every time the domain reloads
        EditorApplication.delayCall += LogDomainDirtyTime;
    }

    private static void LogDomainDirtyTime()
    {


        AIBaker.EatBread();
        AIEditorBaker.DomainLastRefreshed = DateTime.Now;

    }
}
#endif
