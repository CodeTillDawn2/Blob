using System;
using System.Collections.Generic;
using System.Reflection;

public class BasicMemberInfo
{
    public string MemberName { get; }
    public string MemberType { get; }

    public List<SimpleAttributeInfo> Attributes { get; set; } = new List<SimpleAttributeInfo>();

    public BasicMemberInfo()
    {

    }

    public BasicMemberInfo(string name, string memberType, List<SimpleAttributeInfo> simpleAttributeInfos)
    {
        MemberName = name;
        MemberType = memberType;

        Attributes = simpleAttributeInfos;
    }
}