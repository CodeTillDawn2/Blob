using System.Reflection;
using System;
using System.Linq;

[Serializable]
public class SimpleFieldInfo : SimpleMemberInfo
{
    public string declaringTypeName;
    [NonSerialized]
    public FieldInfo cachedFieldInfo;

    public SimpleFieldInfo(FieldInfo fieldInfo) : base(fieldInfo.Name, fieldInfo.FieldType.FullName,
        fieldInfo.GetCustomAttributes().Select(x => new SimpleAttributeInfo(x)).ToList())
    {
        MemberKind = Kind.Field;
        declaringTypeName = fieldInfo.DeclaringType.AssemblyQualifiedName; // Save the full type name

        // Cache the field info (for future uses without re-reflecting)
        cachedFieldInfo = fieldInfo;

        foreach (var attr in fieldInfo.GetCustomAttributes())
        {
            if (attr is CustomAIAttributeBase) // Filter by your attribute
                Attributes.Add(new SimpleAttributeInfo(attr));
        }
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
            var type = Type.GetType(declaringTypeName);
            if (type == null)
            {
                throw new InvalidOperationException($"Cannot find type: {declaringTypeName}");
            }

            cachedFieldInfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                   .FirstOrDefault(f => f.Name == MemberName);
            if (cachedFieldInfo == null)
            {
                throw new InvalidOperationException($"Cannot find field: {MemberName} on type: {declaringTypeName}");
            }
        }
    }
}
