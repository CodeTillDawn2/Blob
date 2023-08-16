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
        if (attributeInfos.Count > 0)
        {
            string test = "";
        }

        MemberName = name;
        MemberType = type;
        declaringTypeName = declaringType;
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
