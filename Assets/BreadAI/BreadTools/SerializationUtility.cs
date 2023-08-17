using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class SerializationUtility
{



    public static string SerializeObject(object obj, bool humanReadable, bool useRefPreserveReferencesHandlingObjects)
    {
        var settings = new JsonSerializerSettings
        {
            // Determines whether to preserve object references.
            PreserveReferencesHandling = useRefPreserveReferencesHandlingObjects ? PreserveReferencesHandling.Objects : PreserveReferencesHandling.None,

            // Auto-detects object's type during serialization.
            TypeNameHandling = TypeNameHandling.Auto,

            // Custom converters to handle specific serialization cases.
            Converters = new List<JsonConverter>
    {
        new InstanceScriptableObjectConverter(),
        new SimpleMemberConverter(),
        new TypeJsonConverter()
    },

            // Produces human-readable JSON if the flag is set, otherwise it's compact.
            Formatting = humanReadable ? Formatting.Indented : Formatting.None
        };

        // Return the serialized JSON string.
        return JsonConvert.SerializeObject(obj, settings);
    }






    /// <summary>
    /// This is a list of all known configs which can be serialized. Allows me not to have to test check every time 
    /// since I know there are good configurations
    /// </summary>
    private static HashSet<Type> GoodConfigs = new HashSet<Type>() { typeof(List<ClassData>),
        typeof(Dictionary<String, List<SimpleMemberInfo>>),
        typeof(Dictionary<String, Dictionary<String, List<String>>>),
        typeof(Dictionary<String, ScriptableObject>),
        typeof(Dictionary<Type,Type>)
    };

    public static string GeneratePathForObjectName(string objectName)
    {
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("Object name cannot be null or whitespace.", nameof(objectName));

        return Application.dataPath + @"/BreadAI/BreadBake/BakedData/" + objectName + ".json";
    }


    public static string WriteToDisk<T>(T dataObject, string filePath)
    {

        Debug.Log("Logging file to " + filePath);

        string returnedError = "";
        try
        {
            // Use the new SerializationUtility to get the JSON string.
            string json = SerializationUtility.SerializeObject(dataObject, false, true);
            string json2 = SerializationUtility.SerializeObject(dataObject, true, false);
            File.WriteAllText(filePath, json);
            File.WriteAllText(filePath.Replace(".json", "_readable.json"), json2);


            string typeo = typeof(T).Name;
            if (!GoodConfigs.Contains(typeof(T)))
            {
                string testingJson = File.ReadAllText(filePath);
                T testingObject = SerializationUtility.DeserializeObject<T>(json, false);
                var comparer = new UnityObjectComparer<T>();
                if (comparer.Equals(dataObject, testingObject))
                {
                    returnedError = "";
                    Debug.LogWarning($"Type successfully tested. Please add 'typeof({GetFriendlyTypeName(typeof(T))})' to GoodConfigs. {Environment.StackTrace}");
                }
                else
                {
                    returnedError = $"Serialization failed, something did not match when tested. Object signature:  '{GetFriendlyTypeName(typeof(T))}' Stack Trace: {Environment.StackTrace}";
                    Debug.LogError(returnedError);
                }
            }
        }
        catch (Exception ex)
        {
            returnedError = $"Serialization encountered issues for type '{GetFriendlyTypeName(typeof(T))}'.  Reason for error: {ex.Message} Stack Trace: {ex.StackTrace}";
            Debug.LogError(returnedError);
        }
        return returnedError;
    }



    public static T DeserializeObject<T>(string json, bool useRefPreserveReferencesHandlingObjects)
    {
        var settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = useRefPreserveReferencesHandlingObjects
                                         ? PreserveReferencesHandling.Objects
                                         : PreserveReferencesHandling.None,
            TypeNameHandling = TypeNameHandling.Auto,
            Converters = new List<JsonConverter>
        {
            new InstanceScriptableObjectConverter(),
            new SimpleMemberConverter(),
            new TypeJsonConverter()
        }
        };
        return JsonConvert.DeserializeObject<T>(json, settings);
    }


    private static void TestSerializabilityOfObject<T>(T obj, string parentPath = "", int depth = 0)
    {

        if (obj is string str)
        {
            return;
        }

        const int MAX_DEPTH = 4; // this is about how deep a custom object might get

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
            string json = SerializationUtility.SerializeObject(obj, false , true);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Serialization failed for path '{parentPath}' in type '{GetFriendlyTypeName(typeof(T))}'. Reason: {ex.Message}");
        }
    }


    public class SimpleMemberConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(SimpleMemberInfo)) || objectType == typeof(SimpleMemberInfo);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            // Validate the existence of required properties
            if (!obj.ContainsKey("MemberType") || !obj.ContainsKey("MemberName") || !obj.ContainsKey("declaringTypeName"))
            {
                throw new JsonSerializationException("Required properties missing in the JSON data.");
            }

            var memberType = obj["MemberType"].ToString();
            string MemberName = obj["MemberName"].ToString();
            string declaringTypeName = obj["declaringTypeName"].ToString();
            Type declaringType = Type.GetType(declaringTypeName);

            if (memberType == "Property")
            {
                var property = declaringType.GetProperty(MemberName);
                var propInfo = new SimplePropertyInfo(property);
                return propInfo;
            }
            else if (memberType == "Field")
            {
                var field = declaringType.GetField(MemberName);
                var fieldInfo = new SimpleFieldInfo(field);
                return fieldInfo;
            }
            else if (memberType == "Method")
            {
                Type[] parameterTypes = new Type[0];

                if (obj.TryGetValue("ParameterTypes", out JToken parameterTypesToken) && parameterTypesToken is JArray parameterTypesArray)
                {
                    int ParameterCount = parameterTypesArray.Count;
                    parameterTypes = new Type[ParameterCount];

                    for (int i = 0; i < ParameterCount; i++)
                    {
                        var paramTypeString = parameterTypesArray[i].ToString();
                        if (!string.IsNullOrEmpty(paramTypeString))
                        {
                            parameterTypes[i] = Type.GetType(paramTypeString);
                        }
                    }
                }

                var method = declaringType.GetMethod(MemberName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
                    null, parameterTypes.Where(type => type != null).ToArray(), null);

                if (method == null)
                {
                    throw new JsonSerializationException($"Failed to find method: {MemberName} in type: {declaringTypeName}.");
                }

                var methodInfo = new SimpleMethodInfo(method);
                return methodInfo;
            }
            else
            {
                throw new JsonSerializationException("Unknown member type: " + memberType);
            }
        }



        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject obj = new JObject();
            JArray attributesArray;

            if (value is SimplePropertyInfo propInfo)
            {
                obj["MemberName"] = propInfo.MemberName;
                obj["MemberType"] = propInfo.MemberType;
                obj["VariableType"] = propInfo.VariableType.AssemblyQualifiedName;
                obj["declaringTypeName"] = propInfo.declaringTypeName.AssemblyQualifiedName;

                attributesArray = JArray.FromObject(propInfo.Attributes, serializer);

                if (attributesArray.Count > 0)
                    obj["Attributes"] = attributesArray;
            }
            else if (value is SimpleFieldInfo fieldInfo)
            {
                obj["MemberName"] = fieldInfo.MemberName;
                obj["MemberType"] = fieldInfo.MemberType;
                obj["Accessibility"] = fieldInfo.Accessibility;
                obj["VariableType"] = fieldInfo.VariableType.AssemblyQualifiedName;
                obj["declaringTypeName"] = fieldInfo.declaringTypeName.AssemblyQualifiedName;

                attributesArray = JArray.FromObject(fieldInfo.Attributes, serializer);

                if (attributesArray.Count > 0)
                    obj["Attributes"] = attributesArray;
            }
            else if (value is SimpleMethodInfo methodInfo)
            {
                obj["MemberName"] = methodInfo.MemberName;
                obj["MemberType"] = methodInfo.MemberType;
                obj["Accessibility"] = methodInfo.Accessibility;
                obj["declaringTypeName"] = methodInfo.declaringTypeName.AssemblyQualifiedName;

                List<string> parameterTypeNames = new List<string>();

                foreach (SimpleParameterInfo param in methodInfo.ParameterTypes)
                {
                    if (param.IsGeneric)
                    {
                        parameterTypeNames.Add("T");
                    }
                    else
                    {
                        parameterTypeNames.Add(param.ParameterType.AssemblyQualifiedName);
                    }
                }

                if (parameterTypeNames.Count > 0)
                    obj["ParameterTypes"] = JArray.FromObject(parameterTypeNames, serializer);

                attributesArray = JArray.FromObject(methodInfo.Attributes, serializer);

                if (attributesArray.Count > 0)
                    obj["Attributes"] = attributesArray;
            }
            else
            {
                throw new JsonSerializationException("Unknown member type: " + obj["MemberType"].ToString());
            }

            obj.WriteTo(writer);
        }


        public override bool CanRead => true;
    }


    public class TypeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Type).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = (Type)value;
            serializer.Serialize(writer, type.AssemblyQualifiedName);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var typeName = serializer.Deserialize<string>(reader);
            return Type.GetType(typeName);
        }
    }

    public class SimpleParameterInfoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SimpleParameterInfo);
        }

        private int DebugReadInt = 0;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var Name = obj["Name"].ToString();
            bool IsOut = (bool)obj["IsOut"];
            bool IsGeneric = (bool)obj["IsGeneric"];
            Type ParameterType = null;

            // If it's not generic, get the ParameterType.
            if (!IsGeneric)
            {
                ParameterType = Type.GetType(obj["ParameterType"].ToString());
            }

            // Get the GenericTypeParameterName; default to an empty string if it's not provided.
            string genericTypeName = obj["GenericTypeParameterName"]?.ToString() ?? "";

            // Instantiate and return SimpleParameterInfo based on the generic status.
            if (IsGeneric)
            {
                return new SimpleParameterInfo(Name, ParameterType, IsOut, true, genericTypeName);
            }
            else
            {
                return new SimpleParameterInfo(Name, ParameterType, IsOut, false);
            }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject obj = new JObject();

            if (value is SimpleParameterInfo paramInfo)
            {
                obj["Name"] = paramInfo.Name;
                obj["IsOut"] = paramInfo.IsOut;
                obj["IsGeneric"] = paramInfo.IsGeneric;

                if (paramInfo.IsGeneric)
                {
                    obj["ParameterType"] = null; // It's better to explicitly set null than "T", because "T" can be misleading
                    obj["GenericTypeParameterName"] = paramInfo.GenericTypeParameterName;
                }
                else
                {
                    obj["ParameterType"] = paramInfo.ParameterType.AssemblyQualifiedName;
                    obj["GenericTypeParameterName"] = "";
                }
            }

            obj.WriteTo(writer);
        }



        public override bool CanRead => true;
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



        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return AreObjectsEqual(x, y);
        }



        private bool AreObjectsEqual(object x, object y)
        {


            if (x.GetType().IsValueType && !x.GetType().IsPrimitive && !(x is Enum))
            {
                return AreStructsEqual(x, y);
            }

            foreach (FieldInfo field in x.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {

                // Skip the _items field of List<T>
                if (field.DeclaringType.IsGenericType
                    && field.DeclaringType.GetGenericTypeDefinition() == typeof(List<>)
                    && field.Name == "_items")
                {
                    continue;
                }


                if (field.GetValue(x) == null && field.GetValue(y) == null)
                {
                    return true;
                }

            }

            return true;
        }


        private bool AreStructsEqual(object x, object y)
        {

            if (x is IList listX && y is IList listY)
            {
                if (listX.Count != listY.Count) return false;
                for (int i = 0; i < listX.Count; i++)
                {
                    if (!AreObjectsEqual(listX[i], listY[i])) return false;
                }
                return true; // If we reached here, all items in the lists are equal.
            }


            return true;
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
