using System;
using UnityEngine;

public sealed class BaseClassRequiredImplementingInterfacesAttribute : PropertyAttribute
{
    public Type BaseType { get; private set; }
    public string ParentField { get; private set; }

    public BaseClassRequiredImplementingInterfacesAttribute(Type baseType, string dependentField = null)
    {
        BaseType = baseType;
        ParentField = dependentField;
    }
}
