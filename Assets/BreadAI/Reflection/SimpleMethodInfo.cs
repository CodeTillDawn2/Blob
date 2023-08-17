using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;

/// <summary>
/// Provides a simplified representation of a method, along with basic reflection capabilities.
/// </summary>
[Serializable]
public class SimpleMethodInfo : SimpleMemberInfo
{
    /// <summary>
    /// Gets or sets the return type of the method.
    /// </summary>
    public string ReturnType { get; set; }

    /// <summary>
    /// Gets or sets the parameters of the method.
    /// </summary>
    public List<SimpleParameterInfo> ParameterTypes { get; set; } = new List<SimpleParameterInfo>();

    /// <summary>
    /// Gets or sets the cached reflection MethodInfo for faster access.
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public MethodInfo CachedMethodInfo { get; set; }

    /// <summary>
    /// Gets or sets the accessibility level of the method (e.g., Public, Private, Protected).
    /// </summary>
    public string Accessibility { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleMethodInfo"/> class.
    /// </summary>
    public SimpleMethodInfo(MethodInfo method)
        : base(method.Name, "Method", method.DeclaringType)
    {
        CachedMethodInfo = method;
        ReturnType = method.ReturnType.FullName;
        ParameterTypes = method.GetParameters().Select(x => new SimpleParameterInfo(x)).ToList();
        Accessibility = method.IsPublic ? "Public" :
                        method.IsPrivate ? "Private" :
                        method.IsFamily ? "Protected" : "Unknown";
        Attributes = ReflectAttributes();
    }


    public object Invoke(object obj, params object[] parameters)
    {
        if (obj is ConfigurationBase || obj is CharacterSystem)
        {
            return CachedMethodInfo.Invoke(obj, parameters);
        }

        throw new NotImplementedException("The target object type is not supported.");
    }



}
