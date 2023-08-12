using System;
using UnityEngine;

public sealed class BaseClassRequiredAttribute : PropertyAttribute
{
    public string BaseTypeName { get; private set; }

    public BaseClassRequiredAttribute(string baseTypeName)
    {
        BaseTypeName = baseTypeName;
    }
}
