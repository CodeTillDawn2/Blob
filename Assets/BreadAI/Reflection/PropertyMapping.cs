using System;
using System.Reflection;

public class PropertyMapping
{
    public PropertyInfo SourceProperty { get; set; }
    public PropertyInfo DestinationProperty { get; set; }
    public string SourceInstanceType { get; set; }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        PropertyMapping other = (PropertyMapping)obj;
        return
            Equals(SourceProperty, other.SourceProperty) &&
            Equals(DestinationProperty, other.DestinationProperty) &&
            Equals(SourceInstanceType, other.SourceInstanceType);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (SourceProperty?.GetHashCode() ?? 0);
            hash = hash * 23 + (DestinationProperty?.GetHashCode() ?? 0);
            hash = hash * 23 + (SourceInstanceType?.GetHashCode() ?? 0);
            return hash;
        }
    }

    private static bool Equals(PropertyInfo prop1, PropertyInfo prop2)
    {
        if (ReferenceEquals(prop1, prop2)) return true;
        if (prop1 == null || prop2 == null) return false;
        return prop1.MetadataToken == prop2.MetadataToken &&
               prop1.Module == prop2.Module;
    }
}
