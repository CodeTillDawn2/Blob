using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[Serializable]
public class SimpleMethodInfo : SimpleMemberInfo
{
    public string ReturnType { get; set; }
    public List<SimpleParameterInfo> ParameterTypes { get; set; } = new List<SimpleParameterInfo>();

    [Newtonsoft.Json.JsonIgnore]
    public MethodInfo cachedMethodInfo { get; set; }

    public string Accessibility { get; set; }

    public SimpleMethodInfo(MethodInfo method)
        : base(method.Name, "Method", method.DeclaringType,
            method.GetCustomAttributes(typeof(BreadAIAttributeBase), true)
                       .OfType<BreadAIAttributeBase>()
                       .Select(x => new SimpleAttributeInfo(x)).ToList())
    {
        cachedMethodInfo = method;
        ReturnType = method.ReturnType.FullName;
        ParameterTypes = method.GetParameters().Select(x => new SimpleParameterInfo(x)).ToList();
        Accessibility = method.IsPublic ? "Public" :
                        method.IsPrivate ? "Private" :
                        method.IsFamily ? "Protected" : "Unknown";
    }

    public object Invoke(object obj, params object[] parameters)
    {
        return cachedMethodInfo.Invoke(obj, parameters);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (MemberName?.GetHashCode() ?? 0);
            hash = hash * 23 + ParameterTypes.Select(p => p.GetHashCode()).Sum();
            return hash;
        }
    }

    public static bool operator ==(SimpleMethodInfo left, SimpleMethodInfo right)
    {
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
        }
        return left.Equals(right);
    }

    public static bool operator !=(SimpleMethodInfo left, SimpleMethodInfo right)
    {
        return !(left == right);
    }
}
