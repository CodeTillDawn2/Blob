using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Provides a simplified representation of a field, along with basic reflection capabilities.
/// </summary>
public class SimpleFieldInfo : SimpleDataMemberInfo
{
    /// <summary>
    /// Gets or sets the cached reflection FieldInfo for faster access.
    /// </summary>
    public FieldInfo CachedFieldInfo { get; set; }

    /// <summary>
    /// Gets or sets the accessibility level of the field (e.g., Public, Private, Protected).
    /// </summary>
    public string Accessibility { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleFieldInfo"/> class.
    /// </summary>
    public SimpleFieldInfo(FieldInfo fieldInfo)
        : base(fieldInfo.Name, "Field", fieldInfo.DeclaringType)
    {
        VariableType = fieldInfo.FieldType;
        CachedFieldInfo = fieldInfo;
        Accessibility = fieldInfo.IsPublic ? "Public" :
                        fieldInfo.IsPrivate ? "Private" :
                        fieldInfo.IsFamily ? "Protected" :
                        "Unknown";
        Attributes = ReflectAttributes();
    }


    public override object GetValue(object target)
    {
        return CachedFieldInfo.GetValue(target);
    }

    public override void SetValue(object target, object value)
    {
        CachedFieldInfo.SetValue(target, value);
    }


}