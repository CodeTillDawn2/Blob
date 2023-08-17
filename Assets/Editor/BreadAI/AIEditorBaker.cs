using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class AIEditorBaker
{


    // Flag to determine if it's the first log of a session
    private static bool isFirstLog = true;

    /// <summary>
    /// Enables or disables logging. Defaults to false.
    /// </summary>
    [SerializeField]
    private static bool enableLogging = false;

    /// <summary>
    /// Cache for detected attributes within character systems and their properties.
    /// </summary>
    public static List<ClassData> BreadMethods
    {
        get { return AIBakerData.instance.AIAttributesCache; }
        private set { AIBakerData.instance.AIAttributesCache = value; }
    }

    ///// <summary>
    ///// Cache of instances that implement the base configuration.
    ///// </summary>
    //public static ConfigurationInstanceCache ConfigurationInstances
    //{
    //    get { return AIBakerData.instance.ConfigurationInstances; }
    //    private set { AIBakerData.instance.ConfigurationInstances = value; }
    //}

    /// <summary>
    /// A nested dictionary containing mappings between classes and their interfaces.
    /// </summary>
    public static Dictionary<string, List<SimpleMemberInfo>> BreadInterfaces
    {
        get
        {
            return AIBakerData.instance.InterfaceData;
        }
        private set
        {
            AIBakerData.instance.InterfaceData = value;
        }
    }

    public static Dictionary<string, List<SimpleMemberInfo>> BreadDataMembers
    {
        get { return AIBakerData.instance.ScriptableObjectPropertiesDetection; }
        private set { AIBakerData.instance.ScriptableObjectPropertiesDetection = value; }
    }

    public static Dictionary<string, ScriptableObject> BreadConfigurations
    {
        get { return AIBakerData.instance.AllConfigInstances; }
        private set { AIBakerData.instance.AllConfigInstances = value; }
    }
    public static Dictionary<Type, List<Type>> BreadSystemInterfaces
    {
        get { return AIBakerData.instance.BreadSystemInterfaces; }
        private set { AIBakerData.instance.BreadSystemInterfaces = value; }
    }



    /// <summary>
    /// Nested dictionary meant to fill out the menu system of the dependent dropdown box on the editor UI for Nerve Systems.
    /// Should be kept fresh after every domain reload.
    /// </summary>
    public static Dictionary<string, Dictionary<string, List<string>>> SystemToConfigMapping
    {
        get { return AIBakerData.instance.CharacterSystemToConfigMapping; }
        private set { AIBakerData.instance.CharacterSystemToConfigMapping = value; }
    }



    /// <summary>
    /// Coordinates the process of baking all relevant AI and configuration data.
    /// </summary>
    public static void BakeBread()
    {


        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        isFirstLog = true;
        Debug.Log("Baking ConfigurationMappings");
        BakeConfigurationMappings();
        Debug.Log("Baked ConfigurationMappings");
        Debug.Log("Baking BakeMembers");
        BakeMembers();
        Debug.Log("Baked BakeMembers");

        Debug.Log("Baking Methods");
        BakeMethods();
        Debug.Log("Baked Methods");

        Debug.Log("Baking Interfaces");
        BakeBreadInterfaces();
        Debug.Log("Baked Interfaces");

        stopwatch.Stop();
        LogToFile($"AI Bake process completed in {stopwatch.Elapsed.TotalSeconds} seconds.");
        Debug.Log($"AI Bake process completed in {stopwatch.Elapsed.TotalSeconds} seconds.");


    }

    //public static Dictionary<(Type characterSystemType, Type configType), Func<ConfigurationBase, CharacterSystem>> 
    //    BakedMappings = new Dictionary<(Type characterSystemType, Type configType), Func<ConfigurationBase, CharacterSystem>>()
    //    public static BakedMappings BakeMappings()
    //    {

    //        foreach (var configType in AllConfigTypes)
    //        {
    //            foreach (var charType in AllCharacterSystemTypes)
    //            {dd
    //                // Check interface mappings and create Func delegates
    //                var transformationLogic = CreateTransformationLogic(configType, charType);
    //                if (transformationLogic != null)
    //                {
    //                    bakedMappings.Mappings.Add((charType, configType), transformationLogic);
    //                }
    //            }
    //        }

    //        return bakedMappings;
    //    }
    



    private static void BakeConfigurationMappings()
    {
        BreadConfigurations = GetAllInstancesOfDerived<ConfigurationBase>(); //Must happen before BakeCharacterSystemsToConfigMappings
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadConfigurations, BreadConfigurations_Path));
        SystemToConfigMapping = BakeBreadValidConfigurations();
        LogToFileIfError(SerializationUtility.WriteToDisk(SystemToConfigMapping, BreadValidConfigurations_Path));
    }

    private static void LogToFileIfError(string errorMessage)
    {
        if (errorMessage != "")
        {
            LogToFile("!!!!!!!!!!!!!!");
            LogToFile("ERROR: " + errorMessage);
            LogToFile("!!!!!!!!!!!!!!");
        }
    }


    private static void BakeMembers()
    {
        BreadDataMembers = BakeBreadMembers();
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadDataMembers, BreadDataMembersPath));
    }
    private static void BakeMethods()
    {
        BreadMethods = BakeBreadMethods();
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadMethods, BreadMethods_Path));
    }

    private static void BakeBreadInterfaces()
    {
        BreadInterfaces = BakeInterfaceData();
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadInterfaces, BreadInterfaces_Path));
        BreadSystemInterfaces = BakeBreadSystemInterfaces();
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadSystemInterfaces, BreadSystemInterfaces_Path));
    }


    public static string BreadValidConfigurations_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadValidConfigurations.json";
    public static string BreadConfigurations_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadConfigurations.json";
    public static string BreadMethods_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadMethods.json";
    public static string BreadDataMembersPath = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadDataMembers.json";
    public static string BreadInterfaces_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadInterfaces.json";
    public static string BreadSystemInterfaces_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/BreadSystemInterfaces.json";


    private static Dictionary<Type, List<Type>> BakeBreadSystemInterfaces()
    {
        var stopwatch = Stopwatch.StartNew();
        var result = new Dictionary<Type, List<Type>>();

        var characterSystemType = typeof(CharacterSystem);
        List<Type> types = new List<Type>();
        foreach (Assembly assemb in allAssemblies)
        {
            types.AddRange(assemb.GetTypes().Where(p => characterSystemType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract));
        }

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces()
                                  .Where(i => i.GetCustomAttribute<BreadInterfaceAttribute>() != null)
                                  .ToList();

            if (interfaces.Count > 0)
            {
                result[type] = interfaces;
            }
        }


        stopwatch.Stop();
        LogToFile($"Baked CharacterSystem Interface Implementations in {stopwatch.ElapsedMilliseconds}ms");

        return result;
    }

    /// <summary>
    /// Detects and caches ScriptableObject properties within classes derived from CharacterSystem.
    /// </summary>
    /// <returns>Cache of detected ScriptableObject properties.</returns>
    public static Dictionary<string, List<SimpleMemberInfo>> BakeBreadMembers()
    {
        LogToFile($"______________________________");
        LogToFile($"");
        LogToFile($"Starting {System.Reflection.MethodBase.GetCurrentMethod().Name}.");

        Dictionary<string, List<SimpleMemberInfo>> breadMembers = new Dictionary<string, List<SimpleMemberInfo>>();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var derivedTypes = allAssemblies.SelectMany(assembly => assembly.GetTypes()
                                        .Where(t => t.IsSubclassOf(typeof(CharacterSystem)) || t.IsSubclassOf(typeof(ConfigurationBase))))
                                        .ToList();

        if (!derivedTypes.Any())
        {
            LogToFile("No classes found.");
        }

        foreach (Type type in derivedTypes)
        {
            LogToFile($"Processing class: {type.Name}");

            var allScriptableObjects = new List<SimpleMemberInfo>();

            // Filter properties by ScriptableObject type
            var scriptableObjectProperties = type.GetProperties()
                .Where(prop => typeof(ScriptableObject).IsAssignableFrom(prop.PropertyType))
                .Select(prop =>
                {
                    var info = new SimplePropertyInfo(prop);
                    info.Attributes.RemoveAll(attr => !(attr is BreadAIAttributeBase));
                    return (SimpleMemberInfo)info;
                });

            // Filter fields by ScriptableObject type
            var scriptableObjectFields = type.GetFields()
                .Where(field => typeof(ScriptableObject).IsAssignableFrom(field.FieldType))
                .Select(field =>
                {
                    var info = new SimpleFieldInfo(field);
                    info.Attributes.RemoveAll(attr => !(attr is BreadAIAttributeBase));
                    return (SimpleMemberInfo)info;
                });

            allScriptableObjects.AddRange(scriptableObjectProperties);
            allScriptableObjects.AddRange(scriptableObjectFields);

            if (allScriptableObjects.Count > 0)
            {
                breadMembers[type.FullName] = allScriptableObjects;
                LogToFile($"Detected {allScriptableObjects.Count} ScriptableObject (useable) members in class {type.Name}.");
            }
        }

        // Confirmation logging at the end
        int keyCount = breadMembers.Keys.Count;
        LogToFile($"Total number of classes processed: {keyCount}");
        int totalMembers = breadMembers.Sum(kvp => kvp.Value.Count);
        LogToFile($"Total number of usable members detected: {totalMembers}");
        stopwatch.Stop();
        LogToFile($"Total time taken: {stopwatch.Elapsed.TotalSeconds} seconds.");

        if (totalMembers == 0 || keyCount == 0)
        {
            Debug.LogError($"{System.Reflection.MethodBase.GetCurrentMethod().Name} returned no results.");
        }

        return breadMembers;
    }



    /// <summary>
    /// Bakes the relationship between character systems and config instances which can be mapped to them
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, Dictionary<string, List<string>>> BakeBreadValidConfigurations()
    {
        LogToFile($"______________________________");
        LogToFile($"");
        LogToFile($"Starting {System.Reflection.MethodBase.GetCurrentMethod().Name}.");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int TotalPopulated = 0;
        int TotalClassesProcessed = 0;

        // Define our base types
        Type[] baseTypes =
        {
            typeof(Brain),
            typeof(Body),
            typeof(Nerves),
            typeof(Senses),
            typeof(Locomotion)
        };

        var systemToConfigMappings = new Dictionary<string, Dictionary<string, List<string>>>();

        foreach (var baseType in baseTypes)
        {
            List<Type> derivedTypesFromBase = new List<Type>();
            foreach (Assembly assembly in allAssemblies)
            {
                derivedTypesFromBase.AddRange(assembly.GetTypes().Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract));
            }

            LogToFile($"Found {derivedTypesFromBase.Count} derived types for base type {baseType.FullName}.");

            var configDictionary = new Dictionary<string, List<string>>();

            foreach (var characterSystem in derivedTypesFromBase)
            {
                TotalClassesProcessed++;
                var requiredInterfaces = characterSystem.GetInterfaces();
                string test = "";

                var filteredByInterfaces = BreadConfigurations
                    .Where(kv => requiredInterfaces.All(iface => iface.IsAssignableFrom(kv.Value.GetType()))).ToList();

                var configTypeInstances = filteredByInterfaces
                    .Select(kv => kv.Key)
                    .ToList();

                configTypeInstances.Insert(0, "None");

                configDictionary[characterSystem.FullName] = configTypeInstances;
                TotalPopulated = TotalPopulated + configTypeInstances.Count;
                LogToFile($"Populated {configTypeInstances.Count} ConfigurationBase types for {characterSystem.FullName} that implement all required interfaces.");
            }

            systemToConfigMappings[baseType.Name] = configDictionary;
        }

        stopwatch.Stop();
        LogToFile($"PopulateDerivedTypeDictionary completed in: {stopwatch.Elapsed.TotalSeconds} seconds.");
        if (TotalClassesProcessed == 0 || TotalPopulated == 0)
        {
            Debug.LogError($"{System.Reflection.MethodBase.GetCurrentMethod().Name} returned no results.");
        }

        return systemToConfigMappings;
    }

    public static Dictionary<string, ScriptableObject> GetAllInstancesOfDerived<T>() where T : ScriptableObject
    {
        Dictionary<string, ScriptableObject> instances = new Dictionary<string, ScriptableObject>();

        // Find all asset GUIDs
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            if (asset != null && asset is T)
            {
                instances.Add(asset.name, asset);
            }
            else if (asset != null)
            {
                Type assetType = asset.GetType();
                if (typeof(T).IsAssignableFrom(assetType))
                {
                    instances.Add(asset.name, asset);
                }
            }
        }

        return instances;
    }

    private static Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

    public static Dictionary<string, List<SimpleMemberInfo>> BakeInterfaceData()
    {
        LogToFile($"______________________________");
        LogToFile($"");
        LogToFile($"Starting {System.Reflection.MethodBase.GetCurrentMethod().Name}.");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var resultMappings = new Dictionary<string, List<SimpleMemberInfo>>();

        foreach (var assembly in allAssemblies)
        {
            var interfacesWithAttribute = assembly.GetTypes().Where(t => t.IsInterface && t.GetCustomAttributes(typeof(BreadAIAttributeBase), true).Any());

            foreach (var iface in interfacesWithAttribute)
            {
                var members = new List<SimpleMemberInfo>();

                // Extract fields
                members.AddRange(iface.GetFields().Select(field => new SimpleFieldInfo(field)));

                // Extract properties
                members.AddRange(iface.GetProperties().Select(prop => new SimplePropertyInfo(prop)));

                // Extract methods
                foreach (var method in iface.GetMethods().Where(m => !m.IsSpecialName))
                {
                    members.Add(new SimpleMethodInfo(method));
                }

                resultMappings[iface.FullName] = members;

                LogToFile($"Processed interface {iface.FullName}. Members found: {members.Count}");
            }
        }


        stopwatch.Stop();
        LogToFile($"Total time taken: {stopwatch.Elapsed.TotalSeconds} seconds.");

        return resultMappings;
    }




    // This function should return both fields and properties of a given type.
    public static List<SimpleMemberInfo> GetSimpleMembers(Type type)
    {
        List<SimpleMemberInfo> members = new List<SimpleMemberInfo>();

        // Get fields
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            members.Add(new SimpleFieldInfo(field));
        }

        // Get properties
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            members.Add(new SimplePropertyInfo(prop));
        }

        return members;
    }



    /// <summary>
    /// Constructs property mappings between a given interface and its implementing class.
    /// </summary>
    /// <param name="sourceInterface">The interface to map from.</param>
    /// <param name="targetType">The class to map to.</param>
    /// <returns>A list of property mappings.</returns>
    private static List<PropertyMapping> BuildPropertyMappings(Type sourceInterface, Type targetType)
    {
        var mappings = new List<PropertyMapping>();
        var sourceProperties = sourceInterface.GetProperties();

        foreach (var sourceProperty in sourceProperties)
        {
            var targetProperty = targetType.GetProperty(sourceProperty.Name, BindingFlags.Public | BindingFlags.Instance);
            if (targetProperty != null && targetProperty.PropertyType == sourceProperty.PropertyType)
            {
                mappings.Add(new PropertyMapping
                {
                    SourceProperty = sourceProperty,
                    DestinationProperty = targetProperty
                });
            }
        }
        return mappings;
    }






    /// <summary>
    /// Detects custom attributes within methods of classes derived from CharacterSystem.
    /// </summary>
    /// <returns>Cache of detected attributes.</returns>
    public static List<ClassData> BakeBreadMethods()
    {
        LogToFile($"______________________________");
        LogToFile($"");
        LogToFile($"Starting {System.Reflection.MethodBase.GetCurrentMethod().Name}.");

        List<ClassData> cache = new List<ClassData>();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var derivedTypes = new List<Type>();

        foreach (var assembly in allAssemblies)
        {
            derivedTypes.AddRange(assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CharacterSystem))));
        }

        if (!derivedTypes.Any())
        {
            LogToFile("No classes derived from CharacterSystem found.");
        }


        foreach (Type type in derivedTypes)
        {
            ClassData classData = new ClassData { ClassType = type.FullName };
            LogToFile($"Processing class: {type.Name}");

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var method in methods)
            {
                // Exclude methods that are property getters or setters
                if (method.IsSpecialName && (method.Name.StartsWith("get_") || method.Name.StartsWith("set_")))
                    continue;

                // Exclude Unity's special methods
                if (unitySpecialMethods.Contains(method.Name))
                    continue;

                // Exclude methods with generic parameters or a generic return type
                if (method.IsGenericMethod || method.ReturnType.IsGenericType)
                    continue;

                // Exclude methods that come from UnityEngine.MonoBehaviour or its ancestors
                if (method.DeclaringType == typeof(UnityEngine.MonoBehaviour) ||
                    method.DeclaringType == typeof(UnityEngine.Component) ||
                    method.DeclaringType == typeof(UnityEngine.MonoBehaviour) ||
                    method.DeclaringType == typeof(System.Object) ||
                    method.DeclaringType == typeof(UnityEngine.Object) ||
                    method.DeclaringType == typeof(UnityEngine.Behaviour))
                    continue;

                classData.MethodsData.Add(new SimpleMethodInfo(method));
            }

            if (classData.MethodsData.Count > 0)
            {
                cache.Add(classData);
            }
        }

        LogToFile($"Total number of classes processed: {cache.Count}");
        int totalMethods = cache.Sum(cd => cd.MethodsData.Count);
        LogToFile($"Total number of methods with custom attributes: {totalMethods}");
        int totalAttributes = cache.Sum(cd => cd.MethodsData.Sum(md => md.Attributes.Count));
        LogToFile($"Total number of custom attributes detected: {totalAttributes}");

        stopwatch.Stop();
        LogToFile($"Total time taken: {stopwatch.Elapsed.TotalSeconds} seconds.");

        return cache;
    }


    /// <summary>
    /// Detects and logs instances derived from ConfigurationBase.
    /// </summary>
    /// <returns>Cache of detected configuration instances.</returns>
    public static ConfigurationInstanceCache BakeConfigurationInstances()
    {
        LogToFile($"______________________________");
        LogToFile($"");
        LogToFile($"Starting {System.Reflection.MethodBase.GetCurrentMethod().Name}.");
        ConfigurationInstanceCache cache = new ConfigurationInstanceCache();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Detect all classes derived from ConfigurationBase that are not interfaces or abstract
        var assembly = Assembly.Load("Assembly-CSharp");
        var subclasses = assembly.GetTypes()
                                 .Where(t => t.IsSubclassOf(typeof(ConfigurationBase)) && !t.IsInterface && !t.IsAbstract);

        if (!subclasses.Any())
        {
            LogToFile("No classes derived from ConfigurationBase found.");
            return cache; // Return empty cache
        }

        foreach (Type type in subclasses)
        {
            // Find existing ScriptableObject instances in the project
            string[] guids = AssetDatabase.FindAssets($"t:{type.Name}");
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var existingInstance = AssetDatabase.LoadAssetAtPath(assetPath, type) as ConfigurationBase;

                if (existingInstance != null)
                {
                    ConfigurationData configData = new ConfigurationData
                    {
                        ConfigurationInstance = existingInstance
                    };
                    cache.Configurations.Add(configData);
                    LogToFile($"Successfully found and processed configuration instance for: {type.Name} at path: {assetPath}");
                }
            }
        }

        // Confirmation logging at the end
        LogToFile($"Total number of configuration instances processed: {cache.Configurations.Count}");
        stopwatch.Stop();
        LogToFile($"Total time taken: {stopwatch.Elapsed.TotalSeconds} seconds.");

        return cache;
    }


    /// <summary>
    /// Utility to log messages to a file. If this is the first log entry of a run, any existing log file is overwritten.
    /// </summary>
    /// <param name="message">Message to be logged.</param>
    private static void LogToFile(string message)
    {
        // If logging is disabled, simply return.
        //if (!enableLogging) return;

        // Determine the path to the log file.
        string path = "C:\\Users\\peter\\Blob\\BakingLogs\\CharacterSystemBakeLog.txt";

        // If it's the first log entry of this run, delete any existing log file.
        if (isFirstLog)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            isFirstLog = false;
        }

        // Append the message to the file.
        using (StreamWriter writer = new StreamWriter(path, true)) // true means appending
        {
            writer.WriteLine(message);
        }
    }


    private static HashSet<string> unitySpecialMethods = new HashSet<string>
{
    // Lifecycle methods
    "Awake",
    "Start",
    "Update",
    "FixedUpdate",
    "LateUpdate",
    "OnEnable",
    "OnDisable",
    "OnDestroy",
    "OnApplicationQuit",
    "OnApplicationPause",
    "OnApplicationFocus",

    // Rendering
    "OnPreCull",
    "OnPreRender",
    "OnPostRender",
    "OnRenderObject",
    "OnWillRenderObject",
    "OnBecameVisible",
    "OnBecameInvisible",
    "OnDrawGizmos",
    "OnDrawGizmosSelected",
    
    // Collision
    "OnCollisionEnter",
    "OnCollisionStay",
    "OnCollisionExit",
    "OnCollisionEnter2D",
    "OnCollisionStay2D",
    "OnCollisionExit2D",
    
    // Trigger
    "OnTriggerEnter",
    "OnTriggerStay",
    "OnTriggerExit",
    "OnTriggerEnter2D",
    "OnTriggerStay2D",
    "OnTriggerExit2D",

    // Mouse and Input
    "OnMouseDown",
    "OnMouseUp",
    "OnMouseDrag",
    "OnMouseEnter",
    "OnMouseExit",
    "OnMouseOver",
    "OnMouseUpAsButton",
    
    // UI related
    "OnRectTransformDimensionsChange",
    "OnRectTransformRemoved",
    "OnBeforeTransformParentChanged",
    "OnTransformParentChanged",
    "OnTransformChildrenChanged",
    
    // Physics
    "OnJointBreak",
    "OnJointBreak2D",
    
    // Audio
    "OnAudioFilterRead",
    "OnLevelWasLoaded",
    
    // Networking (Legacy)
    "OnPlayerConnected",
    "OnPlayerDisconnected",
    "OnServerInitialized",
    "OnNetworkInstantiate",
    "OnDisconnectedFromServer",
    "OnFailedToConnect",
    "OnFailedToConnectToMasterServer",
    "OnNetworkLoadedLevel",


};
}
