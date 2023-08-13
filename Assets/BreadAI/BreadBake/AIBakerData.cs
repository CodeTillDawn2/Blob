using Newtonsoft.Json;
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
            string test = "";
            return _instance;
        }
    }

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
    public AttributesCache AIAttributesCache { get; set; }

    ///// <summary>
    ///// Cache of instances that implement the base configuration.
    ///// </summary>
    //public ConfigurationInstanceCache ConfigurationInstances { get; set; }

    /// <summary>
    /// A nested dictionary containing mappings between classes and their interfaces.
    /// </summary>
    public Dictionary<string, Dictionary<string, List<PropertyMapping>>> BakedConfigurationAssignmentLogic = new Dictionary<string, Dictionary<string, List<PropertyMapping>>>();


    public ScriptableObjectCache ScriptableObjectPropertiesDetection { get; set; }

    public Dictionary<string, ScriptableObject> AllConfigInstances { get; set; }

    /// <summary>
    /// Nested dictionary meant to fill out the menu system of the dependent dropdown box on the editor UI for Nerve Systems.
    /// Should be kept fresh after every domain reload.
    /// </summary>
    public Dictionary<string, Dictionary<string, List<string>>> CharacterSystemToConfigMapping = new Dictionary<string, Dictionary<string, List<string>>>();



    string CharacterSystemToConfigMapping_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/CharacterSystemToConfigMapping.json";



    internal void LoadBakesFromDisk()
    {
        CharacterSystemToConfigMapping = ReadFromDisk<Dictionary<string, Dictionary<string, List<string>>>>(CharacterSystemToConfigMapping_Path);
    }

    public T ReadFromDisk<T>(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return SerializationUtility.DeserializeObject<T>(json);
        }
        else
        {
            Debug.LogError("File not found at: " + filePath);
            return default(T); // Returns the default value for the type (e.g., null for reference types)
        }
    }




}
