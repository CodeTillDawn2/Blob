using System;
using System.Collections.Generic;
using System.Reflection;

public class ScriptableObjectCache
{
    public Dictionary<Type, List<PropertyInfo>> ClassToScriptableObjectProperties { get; set; } = new Dictionary<Type, List<PropertyInfo>>();
}