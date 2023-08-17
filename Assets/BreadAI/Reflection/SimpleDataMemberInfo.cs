using System.Collections.Generic;
using System;

[Serializable]
public abstract class SimpleDataMemberInfo : SimpleMemberInfo
{
    public Type VariableType { get; set; }
    protected Func<object, object> getter;
    protected Action<object, object> setter;

    public SimpleDataMemberInfo(string name, string type, Type declaringType, List<SimpleAttributeInfo> attributeInfos) :
        base(name, type, declaringType, attributeInfos)
    { }

    public virtual object GetValue(object target)
    {
        if (getter == null)
            throw new NotImplementedException("Getter not implemented for this member.");
        return getter(target);
    }

    public virtual void SetValue(object target, object value)
    {
        if (setter == null)
            throw new NotImplementedException("Setter not implemented for this member.");
        setter(target, value);
    }
}
