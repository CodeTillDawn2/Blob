using System;
using System.Collections.Generic;

[Serializable]
public class ClassData
{
    public Type ClassType;
    public List<MethodData> MethodsData = new List<MethodData>();
}