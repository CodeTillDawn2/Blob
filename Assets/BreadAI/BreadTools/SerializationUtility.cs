using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public static class SerializationUtility
{
    // Custom ContractResolver
    private class SerializeFieldContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            // Use ShouldSkipMember to determine if we should serialize this member
            if (ShouldSkipMember(member))
            {
                prop.Ignored = true;
            }
            else if (member is FieldInfo fieldInfo)
            {
                prop.Ignored = false; // Include field by default
                prop.Readable = true; // Make sure it has a getter
                if (fieldInfo.GetCustomAttribute<SerializeField>() == null && fieldInfo.IsPrivate)
                {
                    prop.Ignored = true; // Ignore private fields unless they have [SerializeField]
                }
            }

            return prop;
        }

        private bool ShouldSkipMember(MemberInfo member)
        {
            if (member.GetCustomAttribute<DoNotSerializeAttribute>() != null)
                return true;

            // Check for specific problematic properties from earlier conversation
            if (member.DeclaringType == typeof(GameObject) || member.DeclaringType == typeof(ScriptableObject))
            {
                var propertiesToSkip = new List<string> { "hideFlags" }; // Add more properties as needed
                if (propertiesToSkip.Contains(member.Name))
                    return true;
            }

            // Handle dictionary
            if (member.DeclaringType.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
            {
                // Only allow Keys, Values, or Item properties
                if (member.Name != "Keys" && member.Name != "Values" && member.Name != "Item")
                    return true;
            }

            // Handle lists and collections
            if (member.DeclaringType.GetInterfaces().Any(x => x == typeof(IList)))
            {
                // Skip properties like Count, Capacity, etc.
                return member.Name != "Item";
            }

            // Handle HashSets
            if (member.DeclaringType.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(HashSet<>)))
            {
                // Skip properties like Count for HashSet
                // HashSet does not have 'Keys' or 'Values', so you're mainly dealing with 'Item'
                return member.Name != "Item";
            }

            return false;
        }
    }

    // Static method to serialize an object using the custom resolver
    public static string SerializeObject(object obj)
    {
        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new InstanceScriptableObjectConverter() },
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
                                    typeof(Dictionary<String, Dictionary<String, List<String>>>),
                                    typeof(Dictionary<String, ScriptableObject>)};

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
                T testingObject = SerializationUtility.DeserializeObject<T>(json);
                var comparer = new UnityObjectComparer<T>();
                if (comparer.Equals(dataObject, testingObject))
                {
                    Debug.LogWarning($"Type successfully tested. Please add 'typeof({GetFriendlyTypeName(typeof(T))})' to GoodConfigs. {Environment.StackTrace}");
                }
                else
                {
                    Debug.LogError($"Serialization failed, too complex of an object. Object signature:  '{GetFriendlyTypeName(typeof(T))}' Stack Trace: {Environment.StackTrace}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Serialization encountered issues for type '{GetFriendlyTypeName(typeof(T))}'.  Reason for error: {ex.Message} Stack Trace: {ex.StackTrace}");
        }
    }


    public static T DeserializeObject<T>(string json)
    {
        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new InstanceScriptableObjectConverter() }
        };
        return JsonConvert.DeserializeObject<T>(json, settings);
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


    public class InstanceScriptableObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(ScriptableObject).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            var so = value as ScriptableObject;
            if (so != null)
            {
                JObject obj = new JObject();
#if UNITY_EDITOR
                obj["asset"] = so.name;
#else
            obj["asset"] = so.name;
#endif
                obj.WriteTo(writer);
            }
            else
            {
                serializer.Serialize(writer, null);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var obj = JObject.Load(reader);
            var assetName = obj["asset"].Value<string>();

#if UNITY_EDITOR
            // This will only work in the Editor! 
            // It looks for all assets of the given type and filters by name.
            var assets = UnityEditor.AssetDatabase.FindAssets($"t:{objectType.Name}");
            foreach (var guid in assets)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath(path, objectType);
                if (asset.name == assetName)
                {
                    return asset;
                }
            }
#else
        // Load the instance from resources
        return Resources.Load<ScriptableObject>(assetName);
#endif

            return null;
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


    public class UnityObjectComparer<T> : IEqualityComparer<T>
    {

        int DebugCount = 0;

        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return AreObjectsEqual(x, y);
        }

        private bool AreObjectsEqual(object x, object y)
        {
            foreach (FieldInfo field in x.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (!AreObjectsFieldOrPropEqual(field.GetValue(x), field.GetValue(y), field.FieldType))
                {
                    return false;
                }
                    
            }

            foreach (PropertyInfo prop in x.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (!prop.CanRead) continue;  // Skip properties without a getter.

                // Skip the properties we've deemed unnecessary using a similar logic
                if (ShouldSkipMember(prop)) continue;

                try
                {
                    if (!AreObjectsFieldOrPropEqual(prop.GetValue(x), prop.GetValue(y), prop.PropertyType))
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
            return true;
        }



        private bool ShouldSkipMember(PropertyInfo property)
        {
            // Skip properties with the JsonIgnoreAttribute
            if (property.IsDefined(typeof(JsonIgnoreAttribute), true))
            {
                return true;
            }

            // Skip indexed properties
            if (property.GetIndexParameters().Length > 0)
            {
                return true;
            }

            // Skip properties without a getter
            if (!property.CanRead)
            {
                return true;
            }

            return false;
        }

        private bool AreObjectsFieldOrPropEqual(object valueX, object valueY, Type type)
        {

            if (type == typeof(object))
            {
                return true;
            }
            // Exclude string's indexer property
            if (type == typeof(char) && valueX is string && valueY is string)
            {
                return true; // or however you'd like to handle this case
            }
            // Exclude properties of scriptable object or game object
            if (typeof(ScriptableObject).IsAssignableFrom(type) || typeof(GameObject).IsAssignableFrom(type))
            {
                return true; // or however you'd like to handle this case
            }
            if (valueX == null && valueY == null) return true;
            if (typeof(ScriptableObject).IsAssignableFrom(type) || typeof(GameObject).IsAssignableFrom(type))
            {
                return AreUnityObjectsEqual((UnityEngine.Object)valueX, (UnityEngine.Object)valueY);
            }

            if (typeof(IList).IsAssignableFrom(type)) // Older non-generic lists
            {
                IList listX = (IList)valueX;
                IList listY = (IList)valueY;

                if (listX.Count != listY.Count) return false;
                for (int i = 0; i < listX.Count; i++)
                {
                    if (!AreObjectsEqual(listX[i], listY[i]))  // Recursively handle nested collections
                        return false;
                }
                return true;
            }

            if (typeof(IDictionary).IsAssignableFrom(type)) // Older non-generic dictionaries
            {
                IDictionary dictX = (IDictionary)valueX;
                IDictionary dictY = (IDictionary)valueY;

                if (dictX.Count != dictY.Count) return false;
                foreach (var key in dictX.Keys)
                {
                    if (!dictY.Contains(key) || !AreObjectsEqual(dictX[key], dictY[key]))  // Recursively handle nested collections
                        return false;
                }
                return true;
            }

            if (valueX is IEnumerable && valueY is IEnumerable) // General case for all IEnumerable objects
            {
                return AreEnumerablesEqual(valueX as IEnumerable, valueY as IEnumerable);
            }

            Debug.Log(type);


            // Check for unhandled collections
            if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
            {
                if (type.IsGenericType)
                {
                    Type genericTypeDefinition = type.GetGenericTypeDefinition();

                    // If we have support for this collection, it should have been handled above.
                    if (genericTypeDefinition == typeof(List<>) ||
                        genericTypeDefinition == typeof(Dictionary<,>) ||
                        genericTypeDefinition == typeof(HashSet<>))
                    {
                        throw new InvalidOperationException($"Unhandled collection type '{type.FullName}' detected. Logic seems to support it but didn't process it.");
                    }
                }
                // Throw an error for unsupported collection.
                throw new NotSupportedException($"Unsupported collection type '{type.FullName}' detected.");
            }

            Debug.Log(DebugCount);
            DebugCount += 1;
            if (DebugCount == 374)
            {
                string test = "";
            }
            return object.Equals(valueX, valueY);
        }


        private List<object> ConvertToList(IEnumerable collection)
        {
            List<object> list = new List<object>();
            foreach (var item in collection)
            {
                list.Add(item);
            }
            return list;
        }

        private bool AreEnumerablesEqual(IEnumerable collection1, IEnumerable collection2)
        {
            var list1 = ConvertToList(collection1);
            var list2 = ConvertToList(collection2);

            if (list1.Count != list2.Count) return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!AreObjectsEqual(list1[i], list2[i]))
                    return false;
            }

            return true;
        }

        private bool AreUnityObjectsEqual(UnityEngine.Object x, UnityEngine.Object y)
        {
#if UNITY_EDITOR
            string assetPathX = UnityEditor.AssetDatabase.GetAssetPath(x);
            string assetPathY = UnityEditor.AssetDatabase.GetAssetPath(y);
            return assetPathX == assetPathY;
#else
        return x == y; // At runtime, you can just compare references or use another suitable method.
#endif
        }

        // You might need to implement GetHashCode as well for completeness.
        public int GetHashCode(T obj)
        {
            // Leaving it simple for now. Depending on the usage, you may want to enhance this.
            return obj.GetHashCode();
        }

    }



}
