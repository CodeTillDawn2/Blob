using System;
using UnityEngine;

public sealed class BaseClassRequiredAttribute : PropertyAttribute
{
    public Type BaseType { get; private set; }

    public BaseClassRequiredAttribute(Type baseType)
    {
        BaseType = baseType;
    }
}
