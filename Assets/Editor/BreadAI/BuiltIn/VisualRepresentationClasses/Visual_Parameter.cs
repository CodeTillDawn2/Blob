using Sirenix.OdinInspector;
using System;
[Serializable]
public class Visual_Parameter
{
    [ReadOnly]
    [LabelWidth(75)]
    public string Name; // Name of the interface
    public string ParameterType; // Name of the interface

    public Visual_Parameter(string name, string parameterType)
    {
        Name = name;
        ParameterType = parameterType;
    }

}
