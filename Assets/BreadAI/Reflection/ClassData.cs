using System.Collections.Generic;
using System;

[Serializable]
public class ClassData
{
    public Type ClassType;
    public List<MethodData> MethodsData = new List<MethodData>();
}