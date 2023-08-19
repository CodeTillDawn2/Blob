using System;
using UnityEngine;

/// <summary>
/// Utilized by CompoundDependentDropdown to show dual dropdowns sourced 
/// from a Dictionary<string, Dictionary<string, string>>. 
/// The input is the primary key, enabling multiple instances on one page to share data. 
/// The secondary dropdown derives from the inner string list, determined by the inner key.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class CompoundDependentDropdownAttribute : PropertyAttribute
{
    public string Key { get; private set; }

    public CompoundDependentDropdownAttribute(string key)
    {
        Key = key;
    }
}
