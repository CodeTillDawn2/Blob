using System.Collections.Generic;
using UnityEngine;

public class AIBaker : MonoBehaviour
{


    /// <summary>
    /// Cache for detected attributes within character systems and their properties.
    /// </summary>
    public List<ClassData> AIAttributesCache { get; private set; }

    /// <summary>
    /// Cache of instances that implement the base configuration.
    /// </summary>
    public ConfigurationInstanceCache ConfigurationInstances { get; private set; }

    /// <summary>
    /// A nested dictionary containing mappings between classes and their interfaces.
    /// </summary>
    public Dictionary<string, Dictionary<string, List<PropertyMapping>>> BakedConfigurationAssignmentLogic = new Dictionary<string, Dictionary<string, List<PropertyMapping>>>();


    public List<ClassData> ScriptableObjectPropertiesDetection { get; private set; }

    /// <summary>
    /// Nested dictionary meant to fill out the menu system of the dependent dropdown box on the editor UI for Nerve Systems.
    /// Should be kept fresh after every domain reload.
    /// </summary>
    public Dictionary<string, Dictionary<string, List<string>>> CharacterSystemToConfigMapping = new Dictionary<string, Dictionary<string, List<string>>>();






    private void Awake()
    {
        //if (instance != null && instance != this)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //instance = this;
        DontDestroyOnLoad(this.gameObject); // Ensure this object persists between scenes
        AIBakerData.instance.LoadBakesFromDisk();
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
