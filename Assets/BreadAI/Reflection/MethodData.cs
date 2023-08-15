using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class MethodData2
{

    public SimpleMethodInfo Method { get; set; }
    public HashSet<SimpleAttributeInfo> Attributes { get; set; } = new HashSet<SimpleAttributeInfo>();

    public List<Func<GameObject, bool>> AttributeEvaluators { get; set; } = new List<Func<GameObject, bool>>();
}