using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using UnityEngine;

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
}
