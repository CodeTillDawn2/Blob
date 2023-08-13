using System.Collections.Generic;
using System.Reflection;

public class BasicFieldInfo : BasicMemberInfo
{
    public FieldInfo Field { get; }

    public BasicFieldInfo(FieldInfo field, List<SimpleAttributeInfo> attributes) : base(field.Name, field.FieldType.FullName, attributes)
    {
        Field = field;
    }
}