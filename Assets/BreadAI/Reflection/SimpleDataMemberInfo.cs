using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using Unity.Properties;
using UnityEngine.XR;

/// <summary>
/// Represents a simplified version of a class or struct data member, providing basic reflection capabilities.
/// </summary>
[Serializable]
public abstract class SimpleDataMemberInfo : SimpleMemberInfo
{
    /// <summary>
    /// Gets or sets the data type of the member (e.g., int, string, FloatVariable).
    /// </summary>
    public Type VariableType { get; set; }

    /// <summary>
    /// Retrieves the value of the member from the specified target.
    /// </summary>
    /// <param name="target">The object from which to retrieve the value.</param>
    /// <returns>The value of the member.</returns>
    public abstract object GetValue(object target);

    /// <summary>
    /// Sets the value of the member on the specified target.
    /// </summary>
    /// <param name="target">The object on which to set the value.</param>
    /// <param name="value">The value to set.</param>
    public abstract void SetValue(object target, object value);

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleDataMemberInfo"/> class.
    /// </summary>
    public SimpleDataMemberInfo(string name, string type, Type declaringType) :
        base(name, type, declaringType)
    { }

}