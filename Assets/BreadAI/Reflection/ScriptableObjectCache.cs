using System;
using System.Collections.Generic;
using System.Reflection;

public class ScriptableObjectCache
{

    public Dictionary<string, List<SimpleMemberInfo>> ClassToScriptableObjectProperties { get; set; } = new Dictionary<string, List<SimpleMemberInfo>>();
}