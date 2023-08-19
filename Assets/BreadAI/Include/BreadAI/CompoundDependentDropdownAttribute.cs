using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class CompountDependentDropdownAttribute : PropertyAttribute
{
    public string Key { get; private set; }

    public CompountDependentDropdownAttribute(string key)
    {
        Key = key;
    }
}
