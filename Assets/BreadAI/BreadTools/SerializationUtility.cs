using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System.Linq;

public static class SerializationUtility
{
    // Custom ContractResolver
    private class SerializeFieldContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (member is FieldInfo fieldInfo)
            {
                prop.Ignored = false;  // Include field by default
                prop.Readable = true;  // Make sure it has a getter
                if (fieldInfo.GetCustomAttribute<SerializeField>() == null && fieldInfo.IsPrivate)
                {
                    prop.Ignored = true; // Ignore private fields unless they have [SerializeField]
                }
            }

            return prop;
        }
    }

    // Static method to serialize an object using the custom resolver
    public static string SerializeObject(object obj)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new SerializeFieldContractResolver(),
            Formatting = Formatting.Indented
        };
        return JsonConvert.SerializeObject(obj, settings);
    }

    /// <summary>
    /// This is a list of all known configs which can be serialized. Allows me not to have to test check every time 
    /// since I know there are good configurations
    /// </summary>
    private static HashSet<Type> GoodConfigs = new HashSet<Type>() { typeof(Dictionary<string, string>),
                                    typeof(Dictionary<String, Dictionary<String, List<String>>>)};

    public static void WriteToDisk<T>(T dataObject, string filePath)
    {
        try
        {
            // Use the new SerializationUtility to get the JSON string.
            string json = SerializationUtility.SerializeObject(dataObject);
            File.WriteAllText(filePath, json);

            if (!GoodConfigs.Contains(typeof(T)))
            {
                string testingJson = File.ReadAllText(filePath);
                T testingObject = JsonConvert.DeserializeObject<T>(json);
                Assert.AreEqual(dataObject, testingObject);
                Debug.LogWarning($"Type successfully tested. Please add 'typeof({GetFriendlyTypeName(typeof(T))})' to GoodConfigs. {Environment.StackTrace}");
            }
        }
        catch (AssertionException ex)
        {
            Debug.LogError($"Serialization failed, too complex of an object. {ex.Message}. Initiating detailed check. Object signature:  '{GetFriendlyTypeName(typeof(T))}' Stack Trace: {Environment.StackTrace}");
            TestSerializabilityOfObject(dataObject);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Serialization encountered issues for type '{GetFriendlyTypeName(typeof(T))}'. Initiating detailed check. Reason for error: {ex.Message} Stack Trace: {Environment.StackTrace}");
            TestSerializabilityOfObject(dataObject);
        }
    }

    private static void TestSerializabilityOfObject<T>(T obj, string parentPath = "", int depth = 0)
    {
        const int MAX_DEPTH = 10; // Change this value based on what's reasonable for your structures.

        if (depth > MAX_DEPTH)
        {
            Debug.LogWarning($"Reached maximum depth at path: {parentPath}. Exiting...");
            return;
        }

        if (obj == null) return;

        if (obj is IDictionary)
        {
            IDictionary dictionary = obj as IDictionary;
            foreach (DictionaryEntry entry in dictionary)
            {
                TestSerializabilityOfObject(entry.Value, $"{parentPath}/{entry.Key}", depth + 1);
            }
        }
        else if (obj is IList)
        {
            IList list = obj as IList;
            for (int i = 0; i < list.Count; i++)
            {
                TestSerializabilityOfObject(list[i], $"{parentPath}/[{i}]", depth + 1);
            }
        }
        else if (obj is IEnumerable && !(obj is string))
        {
            // Handle other enumerable types like arrays or custom collections
            int i = 0;
            foreach (var item in (IEnumerable)obj)
            {
                TestSerializabilityOfObject(item, $"{parentPath}/[{i}]", depth + 1);
                i++;
            }
        }

        try
        {
            // Use the new SerializationUtility to get the JSON string.
            string json = SerializationUtility.SerializeObject(obj);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Serialization failed for path '{parentPath}' in type '{GetFriendlyTypeName(typeof(T))}'. Reason: {ex.Message}");
        }
    }




    public static string GetFriendlyTypeName(Type t)
    {
        if (!t.IsGenericType)
            return t.Name;

        string name = t.Name.Substring(0, t.Name.IndexOf('`'));
        string typeParameters = string.Join(", ", t.GetGenericArguments().Select(GetFriendlyTypeName).ToArray());
        return $"{name}<{typeParameters}>";
    }
}
