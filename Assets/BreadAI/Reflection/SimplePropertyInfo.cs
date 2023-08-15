using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class SimplePropertyInfo : SimpleMemberInfo
{

    public Type VariableType { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public PropertyInfo cachedPropertyInfo { get; set; }

    public SimplePropertyInfo(PropertyInfo propertyInfo)
        : base(propertyInfo.Name, "Property", propertyInfo.DeclaringType,
            propertyInfo.GetCustomAttributes().Select(x => new SimpleAttributeInfo(x)).ToList())
    {

        VariableType = propertyInfo.PropertyType;

        // Cache the field info (for future uses without re-reflecting)
        cachedPropertyInfo = propertyInfo;
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
            if (declaringTypeName == null)
            {
                throw new InvalidOperationException($"Cannot find type: {declaringTypeName}");
            }

            cachedPropertyInfo = declaringTypeName.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                     .FirstOrDefault(p => p.Name == MemberName);
            if (cachedPropertyInfo == null)
            {
                throw new InvalidOperationException($"Cannot find property: {MemberName} on type: {declaringTypeName}");
            }
        }
    }
}
