using System.Reflection;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AIBaker : MonoBehaviour
{

    [SerializeField] private AIBakerSO bakerSO;

    public static AIBaker instance { get; private set; }

    public AttributesCache AIAttributesCache { get { return instance.bakerSO.AIAttributesCache; } }
    public ConfigurationsCache ConfigurationDataCache { get { return instance.bakerSO.ConfigurationDataCache; } }
    public Dictionary<string, Dictionary<string, List<PropertyMapping>>> BakedMappings { get { return instance.bakerSO.BakedMappings; } }

    public AIBakerSO BakerSO
    {
        get { return bakerSO; }
        set { bakerSO = value; }
    }
    private void Awake()
    {
        // Singleton pattern check
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject); // Ensure this object persists between scenes
    }





}
