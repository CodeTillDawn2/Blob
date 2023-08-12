using System;
using System.Reflection;

public class FieldMapping
{
    public PropertyInfo SourceProperty { get; set; }
    public PropertyInfo DestinationProperty { get; set; }
    public Type SourceInstanceType { get; set; }

}
