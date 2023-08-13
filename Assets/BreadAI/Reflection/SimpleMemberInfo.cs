using System;
using System.Collections.Generic;

[Serializable]
public class SimpleMemberInfo
{
    public enum Kind
    {
        Property,
        Field,
        Method
    }

    public string MemberName { get; set; }
    public string MemberType { get; set; }
    public Kind MemberKind { get; set; }
    public List<SimpleAttributeInfo> Attributes { get; set; } = new List<SimpleAttributeInfo>();

    // We no longer have the delegate representations here

    public SimpleMemberInfo(string name, string type, List<SimpleAttributeInfo> attributeInfos)
    {
        MemberName = name;
        MemberType = type;
        Attributes = attributeInfos;
    }

    // Introducing virtual methods
    public virtual object GetValue(object target)
    {
        throw new NotImplementedException("GetValue should be implemented in derived classes");
    }

    public virtual void SetValue(object target, object value)
    {
        throw new NotImplementedException("SetValue should be implemented in derived classes");
    }
}
