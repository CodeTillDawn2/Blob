using UnityEngine;

public class AIBaker : MonoBehaviour
{





    private void Awake()
    {
        //if (instance != null && instance != this)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //instance = this;
        DontDestroyOnLoad(this.gameObject); // Ensure this object persists between scenes
        AIBakerData.instance.LoadBakesFromDisk(true);
    }

    private void FixedUpdate()
    {

    }

    //#if UNITY_EDITOR
    //    private static AIBakerSO FindAIBakerSOInstance()
    //    {
    //        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:AIBakerSO");

    //        if (guids.Length > 0)
    //        {
    //            string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
    //            return UnityEditor.AssetDatabase.LoadAssetAtPath<AIBakerSO>(assetPath);
    //        }

    //        return null;
    //    }
    //#endif
}
