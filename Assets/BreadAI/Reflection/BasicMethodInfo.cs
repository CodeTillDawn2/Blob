using System.Collections.Generic;
using System.Reflection;

public class BasicMethodInfo : BasicMemberInfo
{
    public string ReturnType { get; }
    public List<string> ParameterTypes { get; }

    public BasicMethodInfo(string methodName, string memberType, string returnType, 
        List<string> paramtypes, List<SimpleAttributeInfo> attributes) : base(methodName, memberType, attributes)
    {

        ReturnType = returnType;
        ParameterTypes = paramtypes;

    }
}
