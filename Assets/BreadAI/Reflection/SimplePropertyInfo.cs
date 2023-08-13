using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class SimplePropertyInfo : SimpleMemberInfo
{
    public string declaringTypeName;
    [Newtonsoft.Json.JsonIgnore]
    public PropertyInfo cachedPropertyInfo;

    public SimplePropertyInfo(PropertyInfo propInfo) : base(propInfo.Name, propInfo.PropertyType.FullName, 
        propInfo.GetCustomAttributes().Select(x => new SimpleAttributeInfo(x)).ToList())
    {
        MemberKind = Kind.Property;
        declaringTypeName = propInfo.DeclaringType.AssemblyQualifiedName; // Save the full type name

        // Cache the PropertyInfo
        cachedPropertyInfo = propInfo;

        foreach (var attr in propInfo.GetCustomAttributes())
        {
            if (attr is CustomAIAttributeBase) // Filter by your attribute
                Attributes.Add(new SimpleAttributeInfo(attr));
        }
    }

    public override object GetValue(object target)
    {
        EnsurePropertyInfo();
        return cachedPropertyInfo.GetValue(target);
    }

    public override void SetValue(object target, object value)
    {
        EnsurePropertyInfo();
        cachedPropertyInfo.SetValue(target, value);
    }

    private void EnsurePropertyInfo()
    {
        if (cachedPropertyInfo == null)
        {
            var type = Type.GetType(declaringTypeName);
            if (type == null)
            {
                throw new InvalidOperationException($"Cannot find type: {declaringTypeName}");
            }

            cachedPropertyInfo = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                     .FirstOrDefault(p => p.Name == MemberName);
            if (cachedPropertyInfo == null)
            {
                throw new InvalidOperationException($"Cannot find property: {MemberName} on type: {declaringTypeName}");
            }
        }
    }
}
