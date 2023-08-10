using System.Reflection;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;

[CreateAssetMenu(fileName = "BakerSO", menuName = "Baking/BakerSO")]
public class AIBakerSO : ScriptableObject
{
    public AttributesCache AIAttributesCache { get; private set; }
    public ConfigurationsCache ConfigurationDataCache { get; private set; }

    public Dictionary<string, Dictionary<string, List<PropertyMapping>>> BakedMappings = new Dictionary<string, Dictionary<string, List<PropertyMapping>>>();
    public void BakeAI()
    {
        isFirstLog = true;

        BakeMappings();
        ConfigurationDataCache = DetectClassesImplementingConfigurationBase();
        AIAttributesCache = DetectAttributesInCharacterSystems();
    }

    public void BakeMappings()
    {
        LogToFile("");
        LogToFile("");
        LogToFile("");
        LogToFile("Starting BakeMappings.");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Fetch only interfaces
        var allInterfaces = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsInterface);
        int totalMappings = 0;
        int totalTypesProcessed = 0;

        foreach (var iface in allInterfaces)
        {
            // Find classes that implement this interface
            var implementingTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => iface.IsAssignableFrom(t) && !t.IsInterface);

            foreach (var type in implementingTypes)
            {
                // Initialize the nested dictionary
                if (!BakedMappings.ContainsKey(type.FullName))
                {
                    BakedMappings[type.FullName] = new Dictionary<string, List<PropertyMapping>>();
                }

                var mappings = BuildPropertyMappings(iface, type);

                BakedMappings[type.FullName][iface.FullName] = mappings;
                totalMappings += mappings.Count;

                LogToFile($"Processed type {type.FullName} implementing {iface.FullName}. Mappings created: {mappings.Count}");
                totalTypesProcessed++;
            }
        }

        stopwatch.Stop();
        LogToFile($"Processed {totalTypesProcessed} types and created {totalMappings} mappings. Total time taken: {stopwatch.Elapsed.TotalSeconds} seconds.");
    }


    private List<PropertyMapping> BuildPropertyMappings(Type sourceInterface, Type targetType)
    {
        var mappings = new List<PropertyMapping>();
        var sourceProperties = sourceInterface.GetProperties();
        foreach (var sourceProp in sourceProperties)
        {
            var targetProp = targetType.GetProperty(sourceProp.Name);
            if (targetProp != null && targetProp.PropertyType == sourceProp.PropertyType)
            {
                mappings.Add(new PropertyMapping
                {
                    SourceProperty = sourceProp,
                    DestinationProperty = targetProp
                });
            }
        }
        return mappings;
    }



    public AttributesCache DetectAttributesInCharacterSystems()
    {
        LogToFile("");
        LogToFile("");
        LogToFile("");
        LogToFile("Starting DetectAttributesInCharacterSystems.");

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

    public ConfigurationsCache DetectClassesImplementingConfigurationBase()
    {
        LogToFile("");
        LogToFile("");
        LogToFile("");
        LogToFile("Starting DetectClassesImplementingConfigurationBase.");
        ConfigurationsCache cache = new ConfigurationsCache();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var subclasses = AppDomain.CurrentDomain.GetAssemblies() // Get all loaded assemblies
                                       .SelectMany(a => a.GetTypes())
                                       .Where(t => t.IsSubclassOf(typeof(ConfigurationBase)) && !t.IsInterface && !t.IsAbstract);

        if (!subclasses.Any())
        {
            LogToFile("No classes derived from ConfigurationBase found.");
        }

        foreach (Type type in subclasses)
        {
            ConfigurationData configData = new ConfigurationData { ConfigurationType = type };
            LogToFile($"Processing configuration: {type.Name}");

            cache.Configurations.Add(configData);
        }

        // Confirmation logging at the end
        LogToFile($"Total number of configurations processed: {cache.Configurations.Count}");
        stopwatch.Stop();
        LogToFile($"Total time taken: {stopwatch.Elapsed.TotalSeconds} seconds.");

        return cache;
    }



    private bool isFirstLog = true;

    private void LogToFile(string message)
    {
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
