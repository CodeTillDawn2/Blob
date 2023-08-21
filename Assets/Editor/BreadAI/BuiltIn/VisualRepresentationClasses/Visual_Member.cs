using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable]
public class Visual_Member
{
    [ReadOnly]
    [LabelWidth(75)]
    public string Name { get; set; } // Name of the member
    [ReadOnly]
    [LabelWidth(75)]
    public string ReturnType { get; set; }
    [ReadOnly]
    [LabelWidth(75)]
    public string MemberType { get; set; } // Type of the member

    [ListDrawerSettings(ShowIndexLabels = true, DraggableItems = false)]
    public List<Visual_Attribute> Attributes { get; set; } // List of attributes

    public Visual_Member(string name, string returnType, string memberType, List<Visual_Attribute> attributes)
    {
        Name = name;
        ReturnType = returnType;
        MemberType = memberType;
        Attributes = attributes;
    }
}
