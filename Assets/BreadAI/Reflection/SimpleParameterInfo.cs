using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class SimpleParameterInfo
{
    public string Name { get; set; }

    // The actual type if it's not generic or if it's a constructed generic type.
    public Type ParameterType { get; set; }

    // Name of the generic type parameter (e.g., "T" in List<T>). 
    // This will be non-null only if IsGeneric is true and ParameterType is null.
    public string GenericTypeParameterName { get; set; }

    public bool IsOut { get; set; }
    public bool IsGeneric { get; set; }

    public SimpleParameterInfo(ParameterInfo parameterInfo)
    {
        Name = parameterInfo.Name;
        IsOut = parameterInfo.IsOut;

        if (parameterInfo.ParameterType.IsGenericParameter)
        {
            IsGeneric = true;
            ParameterType = null;
            GenericTypeParameterName = parameterInfo.ParameterType.Name;
        }
        else
        {
            IsGeneric = false;
            ParameterType = parameterInfo.ParameterType;
            GenericTypeParameterName = null;  // Not a generic parameter, so set to null.
        }
    }

    public SimpleParameterInfo(string name, Type paramType, bool isOut, bool isGeneric, string genericTypeParameterName = null)
    {
        Name = name;
        IsOut = isOut;
        ParameterType = paramType;
        IsGeneric = isGeneric;
        GenericTypeParameterName = genericTypeParameterName;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SimpleParameterInfo other = (SimpleParameterInfo)obj;
        return Name == other.Name && ParameterType == other.ParameterType;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (Name?.GetHashCode() ?? 0);
            hash = hash * 23 + (ParameterType?.GetHashCode() ?? 0);
            return hash;
        }
    }

   
}
