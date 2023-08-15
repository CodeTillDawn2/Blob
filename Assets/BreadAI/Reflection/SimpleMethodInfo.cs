using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using static UnityEditor.Profiling.FrameDataView;
using MethodInfo = System.Reflection.MethodInfo;

[Serializable]
public class SimpleMethodInfo : SimpleMemberInfo
{
    public string ReturnType { get; set; }
    public List<SimpleParameterInfo> ParameterTypes { get; set; } = new List<SimpleParameterInfo>();

    [Newtonsoft.Json.JsonIgnore]
    public MethodInfo cachedPropertyInfo { get; set; }

    public string Accessibility { get; set; }

    public SimpleMethodInfo(MethodInfo method)
        : base(method.Name, "Method", method.DeclaringType,           
            method.GetCustomAttributes().Select(x => new SimpleAttributeInfo(x)).ToList())
    {
        cachedPropertyInfo = method;
        ReturnType = method.ReturnType.FullName;
        ParameterTypes = method.GetParameters().Select(x => new SimpleParameterInfo(x)).ToList();
        Accessibility = method.IsPublic ? "Public" : method.IsPrivate ? "Private" : method.IsFamily ? "Protected" : "Unknown";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SimpleMethodInfo other = (SimpleMethodInfo)obj;
        return MemberName == other.MemberName &&
               ParameterTypes.SequenceEqual(other.ParameterTypes);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (MemberName?.GetHashCode() ?? 0);
            hash = hash * 23 + (ParameterTypes?.GetHashCode() ?? 0);
            return hash;
        }
    }
}
