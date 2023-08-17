using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

[CreateAssetMenu(fileName = "BakerSO", menuName = "Baking/BakerSO")]
public class AIBakerData : ScriptableObject
{


    private static AIBakerData _instance;

    public static AIBakerData instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.FindObjectsOfTypeAll<AIBakerData>().FirstOrDefault();
                if (_instance == null)
                {
                    Debug.LogError("AIBakerSO instance not found!");
                }
            }

            return _instance;
        }
    }


    public string BreadValidConfigurations_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadValidConfigurations.json";
    public string BreadConfigurations_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadConfigurations.json";
    public string BreadMethods_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadMethods.json";
    public string BreadDataMembers_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadDataMembers.json";
    public string BreadInterfaces_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadInterfaces.json";
    public string BreadSystemInterfaces_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadSystemInterfaces.json";


    // Flag to determine if it's the first log of a session
    private bool isFirstLog = true;

    /// <summary>
    /// Enables or disables logging. Defaults to false.
    /// </summary>
    [SerializeField]
    private bool enableLogging = false;

    /// <summary>
    /// Cache for detected attributes within character systems and their properties.
    /// </summary>
    public List<ClassData> BreadMethods { get; set; }
    public Dictionary<Type, List<Type>> BreadSystemInterfaces { get; set; }

    ///// <summary>
    ///// Cache of instances that implement the base configuration.
    ///// </summary>
    //public ConfigurationInstanceCache ConfigurationInstances { get; set; }


    public Dictionary<string, List<SimpleMemberInfo>> BreadDataMembers = new Dictionary<string, List<SimpleMemberInfo>>();

    public Dictionary<string, ScriptableObject> BreadConfigurations = new Dictionary<string, ScriptableObject>();

    /// <summary>
    /// Nested dictionary meant to fill out the menu system of the dependent dropdown box on the editor UI for Nerve Systems.
    /// Should be kept fresh after every domain reload.
    /// </summary>
    public Dictionary<string, Dictionary<string, List<string>>> BreadValidConfigurations = new Dictionary<string, Dictionary<string, List<string>>>();

    public Dictionary<string, List<SimpleMemberInfo>> BreadInterfaces = new Dictionary<string, List<SimpleMemberInfo>>();

    string CharacterSystemToConfigMapping_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/CharacterSystemToConfigMapping.json";






}
