using System;
using System.Collections.Generic;

[Serializable]
public class ClassData
{
    public string ClassType;
    public List<MethodData> MethodsData = new List<MethodData>();
}