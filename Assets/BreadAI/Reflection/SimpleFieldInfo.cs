using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

[Serializable]
public class SimpleFieldInfo : SimpleDataMemberInfo
{
    [JsonIgnore]
    public FieldInfo cachedFieldInfo { get; set; }

    public string Accessibility { get; set; }

    public SimpleFieldInfo(FieldInfo fieldInfo) : base(fieldInfo.Name, "Field", fieldInfo.DeclaringType,
        fieldInfo.GetCustomAttributes(typeof(BreadAIAttributeBase), true)
                       .OfType<BreadAIAttributeBase>()
                       .Select(x => new SimpleAttributeInfo(x)).ToList())
    {
        VariableType = fieldInfo.FieldType;

        // Cache the field info (for future uses without re-reflecting)
        cachedFieldInfo = fieldInfo;

        Accessibility = fieldInfo.IsPublic ? "Public" :
                        fieldInfo.IsPrivate ? "Private" :
                        fieldInfo.IsFamily ? "Protected" :
                        "Unknown";

        // Bake the accessors using the cached field
        getter = target => cachedFieldInfo.GetValue(target);
        setter = (target, value) => cachedFieldInfo.SetValue(target, value);
    }
}