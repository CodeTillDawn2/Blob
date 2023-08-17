using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Provides a simplified representation of a property, along with basic reflection capabilities.
/// </summary>
public class SimplePropertyInfo : SimpleDataMemberInfo
{
    /// <summary>
    /// Gets or sets the cached reflection PropertyInfo for faster access.
    /// </summary>
    public PropertyInfo CachedPropertyInfo { get; set; }

    /// <summary>
    /// Gets or sets the accessibility level of the property (e.g., Public, Private, Mixed).
    /// </summary>
    public string Accessibility { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimplePropertyInfo"/> class.
    /// </summary>
    public SimplePropertyInfo(PropertyInfo propertyInfo)
        : base(propertyInfo.Name, "Property", propertyInfo.DeclaringType)
    {
        VariableType = propertyInfo.PropertyType;
        CachedPropertyInfo = propertyInfo;

        var cachedGetMethod = propertyInfo.GetGetMethod(true);
        var cachedSetMethod = propertyInfo.GetSetMethod(true);

        var getAccess = cachedGetMethod?.IsPublic ?? false;
        var setAccess = cachedSetMethod?.IsPublic ?? false;

        if (getAccess && setAccess) Accessibility = "Public";
        else if (!getAccess && !setAccess) Accessibility = "Private";
        else Accessibility = "Mixed";
        Attributes = ReflectAttributes();
    }

    public override object GetValue(object target)
    {
        try
        {
            
            string test = target.ToString();
            Debug.Log($"Attempting to get value from object of type: {target.GetType().Name}, member: {MemberName}");

            return CachedPropertyInfo.GetValue(target);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error getting value from {target.GetType().Name}.{CachedPropertyInfo.Name}: {ex.Message}");
            throw;
        }
    }

    public override void SetValue(object target, object value)
    {
        CachedPropertyInfo.SetValue(target, value);
    }

}
