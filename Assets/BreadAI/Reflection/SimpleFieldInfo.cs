using System.Reflection;
using System;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class SimpleFieldInfo : SimpleMemberInfo
{
    
    [JsonIgnore]
    public FieldInfo cachedFieldInfo { get; set; }

    public Type VariableType { get; set; }
    public string Accessibility { get; set; }
    
    public SimpleFieldInfo(FieldInfo fieldInfo) : base(fieldInfo.Name, "Field", fieldInfo.DeclaringType,
        fieldInfo.GetCustomAttributes().Select(x => new SimpleAttributeInfo(x)).ToList())
    {

        VariableType = fieldInfo.FieldType;

        // Cache the field info (for future uses without re-reflecting)
        cachedFieldInfo = fieldInfo;

        Accessibility = fieldInfo.IsPublic ? "Public" : fieldInfo.IsPrivate ? "Private" : fieldInfo.IsFamily ? "Protected" : "Unknown";
    }



    public override object GetValue(object target)
    {
        EnsureFieldInfo();
        return cachedFieldInfo.GetValue(target);
    }

    public override void SetValue(object target, object value)
    {
        EnsureFieldInfo();
        cachedFieldInfo.SetValue(target, value);
    }

    private void EnsureFieldInfo()
    {
        if (cachedFieldInfo == null)
        {
            if (declaringTypeName == null)
            {
                throw new InvalidOperationException($"Cannot find type: {declaringTypeName}");
            }

            cachedFieldInfo = declaringTypeName.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                   .FirstOrDefault(f => f.Name == MemberName);
            if (cachedFieldInfo == null)
            {
                throw new InvalidOperationException($"Cannot find field: {MemberName} on type: {declaringTypeName}");
            }
        }
    }
}
