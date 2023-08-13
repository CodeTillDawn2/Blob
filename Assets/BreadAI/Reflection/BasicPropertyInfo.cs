using System.Collections.Generic;
using System.Reflection;

public class BasicPropertyInfo : BasicMemberInfo
{
    public PropertyInfo Property { get; }
    public bool CanRead { get; }
    public bool CanWrite { get; }

    public BasicPropertyInfo(PropertyInfo property, List<SimpleAttributeInfo> attributes) : base(property.Name, property.PropertyType.FullName, attributes)
    {
        Property = property;
        CanRead = property.CanRead;
        CanWrite = property.CanWrite;
    }
}