using System;
using System.Linq;
using System.Reflection;

public class SimplePropertyInfo : SimpleMemberInfo
{
    public Type VariableType { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public PropertyInfo cachedPropertyInfo { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public MethodInfo cachedGetMethod { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public MethodInfo cachedSetMethod { get; set; }

    public SimplePropertyInfo(PropertyInfo propertyInfo)
        : base(propertyInfo.Name, "Property", propertyInfo.DeclaringType,
            propertyInfo.GetCustomAttributes(typeof(BreadAIAttributeBase), true)
                       .OfType<BreadAIAttributeBase>()
                       .Select(x => new SimpleAttributeInfo(x)).ToList())
    {
        VariableType = propertyInfo.PropertyType;

        // Cache the property info and its getter and setter methods (for future uses without re-reflecting)
        cachedPropertyInfo = propertyInfo;
        cachedGetMethod = propertyInfo.GetGetMethod(true); // 'true' allows for non-public getters
        cachedSetMethod = propertyInfo.GetSetMethod(true); // 'true' allows for non-public setters
    }

    public override object GetValue(object target)
    {
        EnsurePropertyInfo();
        if (cachedGetMethod == null) throw new InvalidOperationException($"The property {MemberName} does not have a get method.");
        return cachedGetMethod.Invoke(target, null);
    }

    public override void SetValue(object target, object value)
    {
        EnsurePropertyInfo();
        if (cachedSetMethod == null) throw new InvalidOperationException($"The property {MemberName} does not have a set method.");
        cachedSetMethod.Invoke(target, new object[] { value });
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

            cachedGetMethod = cachedPropertyInfo.GetGetMethod(true);
            cachedSetMethod = cachedPropertyInfo.GetSetMethod(true);
        }
    }
}
