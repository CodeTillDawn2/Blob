using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable]
public class Visual_AttributeProperty
{
    [ReadOnly]
    [LabelWidth(75)]
    public Dictionary<string, List<string>> Property; // Name of the interface


    public Visual_AttributeProperty(Dictionary<string, List<string>> property)
    {
        Property = property;
    }

}
