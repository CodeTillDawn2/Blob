using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

[Serializable]
public class SimplePropertyInfo : SimpleDataMemberInfo
{
    [JsonIgnore]
    public PropertyInfo cachedPropertyInfo { get; set; }

    public string Accessibility { get; set; }

    public SimplePropertyInfo(PropertyInfo propertyInfo)
        : base(propertyInfo.Name, "Property", propertyInfo.DeclaringType,
            propertyInfo.GetCustomAttributes(typeof(BreadAIAttributeBase), true)
                       .OfType<BreadAIAttributeBase>()
                       .Select(x => new SimpleAttributeInfo(x)).ToList())
    {
        VariableType = propertyInfo.PropertyType;

        // Cache the property info and its getter and setter methods (for future uses without re-reflecting)
        cachedPropertyInfo = propertyInfo;
        var cachedGetMethod = propertyInfo.GetGetMethod(true); // 'true' allows for non-public getters
        var cachedSetMethod = propertyInfo.GetSetMethod(true); // 'true' allows for non-public setters

        // Determining accessibility
        var getAccess = cachedGetMethod?.IsPublic ?? false;
        var setAccess = cachedSetMethod?.IsPublic ?? false;

        if (getAccess && setAccess) Accessibility = "Public";
        else if (!getAccess && !setAccess) Accessibility = "Private";
        else Accessibility = "Mixed";  // for cases where one is public and the other is private

        // Bake the accessors using the cached methods
        getter = target => cachedGetMethod?.Invoke(target, null);
        setter = (target, value) => cachedSetMethod?.Invoke(target, new object[] { value });
    }
}