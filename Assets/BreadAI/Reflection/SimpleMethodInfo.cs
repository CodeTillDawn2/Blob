using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

[Serializable]
public class SimpleMethodInfo : SimpleMemberInfo
{
    public List<string> ParameterTypes { get; set; } = new List<string>();



    public SimpleMethodInfo(string methodName, string MethodType, List<string> parameters, List<SimpleAttributeInfo> attributes) 
        : base(methodName, MethodType, attributes)
    {
        MemberKind = Kind.Method;
        ParameterTypes = parameters;
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