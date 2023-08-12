using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class MethodData
{
    public MethodInfo Method;
    public HashSet<Attribute> Attributes = new HashSet<Attribute>();
    public List<Func<GameObject, bool>> AttributeEvaluators { get; set; } = new List<Func<GameObject, bool>>();
}