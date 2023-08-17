using System;
using System.Collections.Generic;

[Serializable]
public abstract class SimpleMemberInfo
{
    public string MemberName { get; set; }
    public string MemberType { get; set; }
    public Type declaringTypeName { get; set; }
    public List<SimpleAttributeInfo> Attributes { get; set; } = new List<SimpleAttributeInfo>();

    public SimpleMemberInfo(string name, string type, Type declaringType, List<SimpleAttributeInfo> attributeInfos)
    {
        MemberName = name;
        MemberType = type;
        declaringTypeName = declaringType;
        Attributes = attributeInfos;
    }
}