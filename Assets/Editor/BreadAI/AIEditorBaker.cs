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
        get { return AIBakerData.Instance.BreadMethods; }
        private set { AIBakerData.Instance.BreadMethods = value; }
    }

    ///// <summary>
    ///// Cache of instances that implement the base configuration.
    ///// </summary>
    //public static ConfigurationInstanceCache ConfigurationInstances
    //{
    //    get { return AIBakerData.Instance.ConfigurationInstances; }
    //    private set { AIBakerData.Instance.ConfigurationInstances = value; }
    //}

    /// <summary>
    /// A nested dictionary containing mappings between classes and their interfaces.
    /// </summary>
    public static Dictionary<string, List<SimpleMemberInfo>> BreadInterfaces
    {
        get
        {
            return AIBakerData.Instance.BreadInterfaces;
        }
        private set
        {
            AIBakerData.Instance.BreadInterfaces = value;
        }
    }

    public static Dictionary<string, List<SimpleMemberInfo>> BreadDataMembers
    {
        get { return AIBakerData.Instance.BreadDataMembers; }
        private set { AIBakerData.Instance.BreadDataMembers = value; }
    }

    public static Dictionary<string, ScriptableObject> BreadConfigurations
    {
        get { return AIBakerData.Instance.BreadConfigurations; }
        private set { AIBakerData.Instance.BreadConfigurations = value; }
    }
    public static Dictionary<Type, List<Type>> BreadSystemInterfaces
    {
        get { return AIBakerData.Instance.BreadSystemInterfaces; }
        private set { AIBakerData.Instance.BreadSystemInterfaces = value; }
    }



    /// <summary>
    /// Nested dictionary meant to fill out the menu system of the dependent dropdown box on the editor UI for Nerve Systems.
    /// Should be kept fresh after every domain reload.
    /// </summary>
    public static Dictionary<string, Dictionary<string, List<string>>> BreadValidConfigurations
    {
        get { return AIBakerData.Instance.BreadValidConfigurations; }
        private set { AIBakerData.Instance.BreadValidConfigurations = value; }
    }

    public static void StartBake()
    {
        ProofBread();
        BakeBread();
        StoreBread();
    }

    private static void ProofBread()
    {

    }

    /// <summary>
    /// Coordinates the process of baking all relevant AI and configuration data.
    /// </summary>
    private static void BakeBread()
    {


        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        isFirstLog = true;
        Debug.Log("Proofing ConfigurationMappings");
        BakeConfigurationMappings();
        Debug.Log("Proofed ConfigurationMappings");
        Debug.Log("Proofing BakeMembers");
        BakeMembers();
        Debug.Log("Proofed BakeMembers");

        Debug.Log("Proofing Methods");
        BakeMethods();
        Debug.Log("Proofed Methods");

        Debug.Log("Proofing Interfaces");
        BakeBreadInterfaces();
        Debug.Log("Proofed Interfaces");

        stopwatch.Stop();
        LogToFile($"AI proof process completed in {stopwatch.Elapsed.TotalSeconds} seconds.");
        Debug.Log($"AI proof process completed in {stopwatch.Elapsed.TotalSeconds} seconds.");


    }

    private static string BreadValidConfigurations_Path = Application.dataPath + @"/BreadAI/BakedData/BreadValidConfigurations.json";
    private static string BreadConfigurations_Path = Application.dataPath + @"/BreadAI/BakedData/BreadConfigurations.json";
    private static string BreadMethods_Path = Application.dataPath + @"/BreadAI/BakedData/BreadMethods.json";
    private static string BreadDataMembers_Path = Application.dataPath + @"/BreadAI/BakedData/BreadDataMembers.json";
    private static string BreadInterfaces_Path = Application.dataPath + @"/BreadAI/BakedData/BreadInterfaces.json";
    private static string BreadSystemInterfaces_Path = Application.dataPath + @"/BreadAI/BakedData/BreadSystemInterfaces.json";

    private static void StoreBread()
    {
        Debug.Log("Baking Bread");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadConfigurations, BreadConfigurations_Path));
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadValidConfigurations, BreadValidConfigurations_Path));
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadDataMembers, BreadDataMembers_Path));
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadMethods, BreadMethods_Path));
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadInterfaces, BreadInterfaces_Path));
        LogToFileIfError(SerializationUtility.WriteToDisk(BreadSystemInterfaces, BreadSystemInterfaces_Path));
        LogToFile($"AI Bake process completed in {stopwatch.Elapsed.TotalSeconds} seconds.");
        Debug.Log($"AI Bake process completed in {stopwatch.Elapsed.TotalSeconds} seconds.");
    }



    private static void BakeConfigurationMappings()
    {
        BreadConfigurations = GetAllInstancesOfDerived<ConfigurationBase>(); //Must happen before BakeCharacterSystemsToConfigMappings
        BreadValidConfigurations = BakeBreadValidConfigurations();
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
        BreadDataMembers = BakeBreadDataMembers();

    }
    private static void BakeMethods()
    {
        BreadMethods = BakeBreadMethods();

    }

    private static void BakeBreadInterfaces()
    {
        BreadInterfaces = BakeInterfaceData();

        BreadSystemInterfaces = BakeBreadSystemInterfaces();

    }



    private static Dictionary<Type, List<Type>> BakeBreadSystemInterfaces()
    {
        var stopwatch = Stopwatch.StartNew();
        var result = new Dictionary<Type, List<Type>>();

        var characterSystemType = typeof(CharacterSystem);
        var characterSystemType2 = typeof(ConfigurationBase);
        List<Type> types = new List<Type>();
        foreach (Assembly assemb in allAssemblies)
        {
            types.AddRange(assemb.GetTypes().Where(p => (characterSystemType.IsAssignableFrom(p)
            || characterSystemType2.IsAssignableFrom(p)) && !p.IsInterface && !p.IsAbstract));
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
    public static Dictionary<string, List<SimpleMemberInfo>> BakeBreadDataMembers()
    {
        LogToFile($"______________________________");
        LogToFile($"");
        LogToFile($"Starting {System.Reflection.MethodBase.GetCurrentMethod().Name}.");

        Dictionary<string, List<SimpleMemberInfo>> breadMembers = new Dictionary<string, List<SimpleMemberInfo>>();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
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

            var allMembers = new List<SimpleMemberInfo>();

            // Identify interfaces that have the AIInterfaceAttribute.
            var interfacesWithAIAttribute = type.GetInterfaces()
                .Where(intf => intf.GetCustomAttribute<BreadInterfaceAttribute>() != null)
                .ToList();

            if (!interfacesWithAIAttribute.Any())
                continue; // skip types that don't implement interfaces with AIInterfaceAttribute

            foreach (var intf in interfacesWithAIAttribute)
            {
                // Filter properties based on the identified interfaces
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop => intf.GetProperties().Any(iprop => iprop.Name == prop.Name && iprop.PropertyType == prop.PropertyType))
                    .Select(prop =>
                    {
                        SimplePropertyInfo info = new SimplePropertyInfo(prop);
                        info.Attributes.RemoveAll(attr => (!(attr is BreadAIAttributeBase) && !(attr is BasicComponentAttribute)));
                        var correspondingInterfaceProperty = intf.GetProperties().FirstOrDefault(iprop => iprop.Name == prop.Name && iprop.PropertyType == prop.PropertyType);
                        if (correspondingInterfaceProperty != null)
                        {
                            var interfaceAttributes = correspondingInterfaceProperty.GetCustomAttributes(true)
                                                                                  .OfType<Attribute>()
                                                                                  .Select(attr => new SimpleAttributeInfo(attr))
                                                                                  .ToList();
                            info.InterfaceAttributes[intf] = interfaceAttributes;
                        }
                        return info;
                    });

                // Filter fields based on the identified interfaces
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Where(field => intf.GetFields().Any(ifield => ifield.Name == field.Name && ifield.FieldType == field.FieldType))
                    .Select(field =>
                    {
                        SimpleFieldInfo info = new SimpleFieldInfo(field);
                        info.Attributes.RemoveAll(attr => (!(attr is BreadAIAttributeBase) && !(attr is BasicComponentAttribute)));
                        var correspondingInterfaceField = intf.GetFields().FirstOrDefault(ifield => ifield.Name == field.Name && ifield.FieldType == field.FieldType);
                        if (correspondingInterfaceField != null)
                        {
                            var interfaceAttributes = correspondingInterfaceField.GetCustomAttributes(true)
                                                                                  .OfType<Attribute>()
                                                                                  .Select(attr => new SimpleAttributeInfo(attr))
                                                                                  .ToList();
                            info.InterfaceAttributes[intf] = interfaceAttributes;
                        }
                        return info;
                    });

                allMembers.AddRange(properties);
                allMembers.AddRange(fields);
            }

            if (allMembers.Count > 0)
            {
                breadMembers[type.FullName] = allMembers;
                LogToFile($"Detected {allMembers.Count} members (usable) in class {type.Name}.");
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
        var derivedTypes = allAssemblies.SelectMany(assembly => assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CharacterSystem))))
                                        .ToList();

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

                SimpleMethodInfo info = new SimpleMethodInfo(method);
                info.Attributes.RemoveAll(attr => (!(attr is BreadAIAttributeBase) && !(attr is BasicComponentAttribute)));

                foreach (var intf in type.GetInterfaces().Where(intf => intf.GetCustomAttribute<BreadInterfaceAttribute>() != null))
                {
                    var correspondingInterfaceMethod = intf.GetMethods().FirstOrDefault(imethod => imethod.Name == method.Name && imethod.ReturnType == method.ReturnType);

                    if (correspondingInterfaceMethod != null)
                    {
                        var interfaceAttributes = correspondingInterfaceMethod.GetCustomAttributes(true)
                                                                              .OfType<Attribute>()
                                                                              .Select(attr => new SimpleAttributeInfo(attr))
                                                                              .ToList();
                        info.InterfaceAttributes[intf] = interfaceAttributes;
                    }
                }

                classData.MethodsData.Add(info);
            }

            if (classData.MethodsData.Count > 0)
            {
                cache.Add(classData);
            }
        }

        LogToFile($"Total number of classes processed: {cache.Count}");
        int totalMethods = cache.Sum(cd => cd.MethodsData.Count);
        LogToFile($"Total number of methods with custom attributes: {totalMethods}");
        int totalAttributes = cache.Sum(cd => cd.MethodsData.Sum(md => md.InterfaceAttributes.Values.Sum(interfAttr => interfAttr.Count)));
        LogToFile($"Total number of custom attributes detected: {totalAttributes}");

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
