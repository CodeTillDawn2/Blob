



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
    public static AttributesCache AIAttributesCache
    {
        get { return AIBakerData.instance.AIAttributesCache; }
        private set { AIBakerData.instance.AIAttributesCache = value; }
    }

    /// <summary>
    /// Cache of instances that implement the base configuration.
    /// </summary>
    public static ConfigurationInstanceCache ConfigurationInstances
    {
        get { return AIBakerData.instance.ConfigurationInstances; }
        private set { AIBakerData.instance.ConfigurationInstances = value; }
    }

    /// <summary>
    /// A nested dictionary containing mappings between classes and their interfaces.
    /// </summary>
    public static Dictionary<string, Dictionary<string, List<PropertyMapping>>> BakedConfigurationAssignmentLogic = new Dictionary<string, Dictionary<string, List<PropertyMapping>>>();


    public static ScriptableObjectCache ScriptableObjectPropertiesDetection
    {
        get { return AIBakerData.instance.ScriptableObjectPropertiesDetection; }
        private set { AIBakerData.instance.ScriptableObjectPropertiesDetection = value; }
    }

    public static Dictionary<string, ScriptableObject> AllConfigInstances
    {
        get { return AIBakerData.instance.AllConfigInstances; }
        private set { AIBakerData.instance.AllConfigInstances = value; }
    }

    /// <summary>
    /// Nested dictionary meant to fill out the menu system of the dependent dropdown box on the editor UI for Nerve Systems.
    /// Should be kept fresh after every domain reload.
    /// </summary>
    public static Dictionary<string, Dictionary<string, List<string>>> CharacterSystemToConfigMapping
    {
        get { return AIBakerData.instance.CharacterSystemToConfigMapping; }
        set { AIBakerData.instance.CharacterSystemToConfigMapping = value; }
    }

    /// <summary>
    /// Coordinates the process of baking all relevant AI and configuration data.
    /// </summary>
    public static void BakeAI()
    {
        Debug.Log("Baking...");
        isFirstLog = true;
        BakeConfigurationAssignmentLogic();
        AllConfigInstances = GetAllInstancesOfDerived<ConfigurationBase>(); //Must happen before BakeCharacterSystemsToConfigMappings
        SerializationUtility.WriteToDisk<Dictionary<string, ScriptableObject>>(AllConfigInstances, AllConfigInstances_Path);
        //CharacterSystemToConfigMapping = BakeCharacterSystemsToConfigMappings();
        //SerializationUtility.WriteToDisk<Dictionary<string, Dictionary<string, List<string>>>>(CharacterSystemToConfigMapping, CharacterSystemToConfigMapping_Path);
        //ConfigurationInstances = BakeConfigurationInstances();
        //AIAttributesCache = BakeAttributesInCharacterSystems();
        //ScriptableObjectPropertiesDetection = BakeScriptableObjectPropertiesDetection();
    }

    public static string CharacterSystemToConfigMapping_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/CharacterSystemToConfigMapping.json";
    public static string AllConfigInstances_Path = Application.dataPath + @"/BreadAI/BreadBake/BakedData/AllConfigInstances.json";


    /// <summary>
    /// Detects and caches ScriptableObject properties within classes derived from CharacterSystem.
    /// </summary>
    /// <returns>Cache of detected ScriptableObject properties.</returns>
    public static ScriptableObjectCache BakeScriptableObjectPropertiesDetection()
    {
        LogToFile("");
        LogToFile("");
        LogToFile("");
        LogToFile("Starting BakeScriptableObjectPropertiesDetection.");

        ScriptableObjectCache cache = new ScriptableObjectCache();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var derivedTypes = Assembly.GetExecutingAssembly().GetTypes()
                                   .Where(t => t.IsSubclassOf(typeof(CharacterSystem)));

        if (!derivedTypes.Any())
        {
            LogToFile("No classes derived from CharacterSystem found.");
        }

        foreach (Type type in derivedTypes)
        {
            LogToFile($"Processing class: {type.Name}");

            var scriptableObjectProperties = type.GetInterfaces()
                .SelectMany(iface => iface.GetProperties())
                .Where(prop => typeof(ScriptableObject).IsAssignableFrom(prop.PropertyType))
                .ToList();

            if (scriptableObjectProperties.Count > 0)
            {
                cache.ClassToScriptableObjectProperties[type] = scriptableObjectProperties;
                LogToFile($"Detected {scriptableObjectProperties.Count} ScriptableObject properties in class {type.Name}.");
            }
        }

        // Confirmation logging at the end
        LogToFile($"Total number of classes processed: {cache.ClassToScriptableObjectProperties.Keys.Count}");
        int totalProperties = cache.ClassToScriptableObjectProperties.Sum(kvp => kvp.Value.Count);
        LogToFile($"Total number of ScriptableObject properties detected: {totalProperties}");
        stopwatch.Stop();
        LogToFile($"Total time taken: {stopwatch.Elapsed.TotalSeconds} seconds.");

        return cache;
    }

    /// <summary>
    /// Bakes the relationship between character systems and config instances which can be mapped to them
    /// </summary>
    /// <returns></returns>
    public static Dictionary<string, Dictionary<string, List<string>>> BakeCharacterSystemsToConfigMappings()
    {
        LogToFile("");
        LogToFile("");
        LogToFile("");
        LogToFile("Starting BakeCharacterSystemsToConfigMappings.");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Define our base types
        Type[] baseTypes =
        {
            typeof(Brain),
            typeof(Body),
            typeof(Nerves),
            typeof(Senses),
            typeof(Locomotion)
        };

        var mappings = new Dictionary<string, Dictionary<string, List<string>>>();

        foreach (var baseType in baseTypes)
        {
            var derivedTypesFromBase = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                .ToArray();

            LogToFile($"Found {derivedTypesFromBase.Length} derived types for base type {baseType.FullName}.");

            var innerDictionary = new Dictionary<string, List<string>>();

            foreach (var derivedType in derivedTypesFromBase)
            {
                var requiredInterfaces = derivedType.GetInterfaces();
                string test = "";

                var filteredByInterfaces = AllConfigInstances
                    .Where(kv => requiredInterfaces.All(iface => iface.IsAssignableFrom(kv.Value.GetType()))).ToList();

                var configTypeInstances = filteredByInterfaces
                    .Select(kv => kv.Key)
                    .ToList();

                //var configTypeInstances = AllConfigInstances
                //    .Where(kv => derivedType.IsAssignableFrom(kv.Value.GetType())
                //            && requiredInterfaces.All(iface => iface.IsAssignableFrom(kv.Value.GetType())))
                //    .Select(kv => kv.Key)
                //    .ToList();

                configTypeInstances.Insert(0, "None");

                innerDictionary[derivedType.FullName] = configTypeInstances;

                LogToFile($"Populated {configTypeInstances.Count} ConfigurationBase types for {derivedType.FullName} that implement all required interfaces.");
            }

            mappings[baseType.Name] = innerDictionary;
        }

        stopwatch.Stop();
        LogToFile($"PopulateDerivedTypeDictionary completed in: {stopwatch.Elapsed.TotalSeconds} seconds.");

        return mappings;
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


    /// <summary>
    /// Constructs mappings between ConfigurationBase-derived class properties and classes derived from CharacterSystem that implement the same interfaces.
    /// </summary>
    public static void BakeConfigurationAssignmentLogic()
    {
        LogToFile("");
        LogToFile("");
        LogToFile("");
        LogToFile("Starting BakeMappings for BakedMappings.");

        BakedConfigurationAssignmentLogic.Clear();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Fetch all classes derived from ConfigurationBase
        var derivedConfigBaseTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(ConfigurationBase)));

        foreach (var sourceType in derivedConfigBaseTypes)
        {
            // Get all the properties of the derived ConfigurationBase type
            //var properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //foreach (var property in properties)
            //{
            // Check if the Configuration implements any interfaces
            var interfaces = sourceType.GetInterfaces().Where(i => i.GetCustomAttributes(typeof(BreadAIInterfaceAttribute), false).Length > 0);


            foreach (var iface in interfaces)
            {
                // Find classes that derive from CharacterSystem and implement the same interface as the configuration
                var potentialDestTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(CharacterSystem)) && iface.IsAssignableFrom(t));

                foreach (var destType in potentialDestTypes)
                {
                    // Initialize the nested dictionary
                    if (!BakedConfigurationAssignmentLogic.ContainsKey(destType.FullName))
                    {
                        BakedConfigurationAssignmentLogic[destType.FullName] = new Dictionary<string, List<PropertyMapping>>();
                    }

                    var mappings = BuildPropertyMappings(iface, destType);

                    foreach (var mapping in mappings)
                    {
                        mapping.SourceInstanceType = sourceType;  // Set the derived ConfigurationBase type as the source
                    }

                    BakedConfigurationAssignmentLogic[destType.FullName][iface.Name] = mappings;

                    LogToFile($"Processed type interface {iface.FullName} for mappings from {sourceType.FullName} to {destType.FullName}. Mappings created: {mappings.Count}");
                }
            }
            //}
        }

        stopwatch.Stop();
        LogToFile($"Total time taken: {stopwatch.Elapsed.TotalSeconds} seconds.");
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
    public static AttributesCache BakeAttributesInCharacterSystems()
    {
        LogToFile("");
        LogToFile("");
        LogToFile("");
        LogToFile("Starting BakeAttributesInCharacterSystems for AIAttributesCache.");

        AttributesCache cache = new AttributesCache();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var derivedTypes = Assembly.GetExecutingAssembly().GetTypes()
                                   .Where(t => t.IsSubclassOf(typeof(CharacterSystem)));

        if (!derivedTypes.Any())
        {
            LogToFile("No classes derived from CharacterSystem found.");
        }

        foreach (Type type in derivedTypes)
        {
            ClassData classData = new ClassData { ClassType = type };
            LogToFile($"Processing class: {type.Name}");

            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            if (methods.Length == 0)
            {
                LogToFile($"Class {type.Name} has no public instance methods.");
            }

            foreach (var method in methods)
            {
                var customAttributes = method.GetCustomAttributes()
                             .Where(attr => attr is CustomAIAttributeBase)
                             .ToArray();

                if (customAttributes.Length > 0)
                {
                    MethodData methodData = new MethodData
                    {
                        Method = method,
                        Attributes = new HashSet<Attribute>(customAttributes)
                    };

                    foreach (var attribute in customAttributes)
                    {
                        LogToFile($"Attribute {attribute.GetType().Name} detected for method {method.Name} in class {type.Name}.");

                        // Baking evaluators
                        if (attribute is AIRequiredFieldTrueAttribute requiredTrueAttr)
                        {
                            Type requiredType = requiredTrueAttr.FieldToEvaluate;
                            PropertyInfo propInfo = requiredType.GetProperty("Value");
                            Func<GameObject, bool> evaluator = (go) =>
                            {
                                var component = go.GetComponent(requiredType);
                                return component != null && (bool)propInfo.GetValue(component);
                            };
                            methodData.AttributeEvaluators.Add(evaluator);
                        }

                        // Add similar blocks for other attribute types...
                    }

                    classData.MethodsData.Add(methodData);
                }
            }

            if (classData.MethodsData.Count > 0)
            {
                cache.ClassesData.Add(classData);
            }
        }

        // Confirmation logging at the end
        LogToFile($"Total number of classes processed: {cache.ClassesData.Count}");
        int totalMethods = cache.ClassesData.Sum(cd => cd.MethodsData.Count);
        LogToFile($"Total number of methods with custom attributes: {totalMethods}");
        int totalAttributes = cache.ClassesData.Sum(cd => cd.MethodsData.Sum(md => md.Attributes.Count));
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
        LogToFile("");
        LogToFile("");
        LogToFile("");
        LogToFile("Starting BakeConfigurationInstances for ConfigurationInstances.");
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
        if (!enableLogging) return;

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


}
