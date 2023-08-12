using System;
using System.Reflection;

public class PropertyMapping
{
    public PropertyInfo SourceProperty { get; set; }
    public PropertyInfo DestinationProperty { get; set; }

    public Type SourceInstanceType { get; set; }
}