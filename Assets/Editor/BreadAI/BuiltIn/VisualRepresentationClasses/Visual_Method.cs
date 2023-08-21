using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable]
public class Visual_Method : Visual_Member
{
    [ListDrawerSettings(ShowIndexLabels = true, DraggableItems = false)]
    public List<Visual_Parameter> Parameters { get; set; }
    public bool HasDefaultImplementation { get; set; }
    public string DefaultImplementationCode { get; set; }

    public Visual_Method(string name, string returnType, string memberType, List<Visual_Attribute> attributes, List<Visual_Parameter> parameters)
        : base(name, returnType, memberType, attributes)
    {
        Parameters = parameters;
    }
}
