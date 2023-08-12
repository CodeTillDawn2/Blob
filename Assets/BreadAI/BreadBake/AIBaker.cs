using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class AIBaker : MonoBehaviour
{
    [SerializeField] private AIBakerSO bakerSO;

    public static AIBaker instance { get; private set; }

    // Static reference for AIBakerSO

    private static AIBakerSO _cachedBakerSO;
    public static AIBakerSO CachedBakerSO
    {
        get
        {
#if UNITY_EDITOR
            if (_cachedBakerSO == null)
            {
                _cachedBakerSO = FindAIBakerSOInstance();
            }
#endif
            return _cachedBakerSO;
        }
    }

    public AttributesCache AIAttributesCache => instance.bakerSO.AIAttributesCache;
    public ConfigurationInstanceCache ConfigurationDataCache => instance.bakerSO.ConfigurationInstances;
    public Dictionary<string, Dictionary<string, List<PropertyMapping>>> BakedConfigurationAssignmentLogic => instance.bakerSO.BakedConfigurationAssignmentLogic;
    public Dictionary<string, Dictionary<string, List<string>>> ConfigurationTypeDictionary => instance.bakerSO.CharacterSystemToConfigMapping;

    public AIBakerSO BakerSO
    {
        get => bakerSO;
        set => bakerSO = value;
    }




    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject); // Ensure this object persists between scenes
        _cachedBakerSO = bakerSO;  // Cache the bakerSO on Awake

        BakerSO.LoadBakesFromDisk();
    }

    private void FixedUpdate()
    {
        
    }





#if UNITY_EDITOR
    private static AIBakerSO FindAIBakerSOInstance()
    {
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:AIBakerSO");

        if (guids.Length > 0)
        {
            string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            return UnityEditor.AssetDatabase.LoadAssetAtPath<AIBakerSO>(assetPath);
        }

        return null;
    }
#endif
}
