using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable]
public class Visual_Attribute
{
    [ReadOnly]
    [LabelWidth(75)]
    public string TypeName; // Name of the interface


    List<Visual_AttributeProperty> Properties;




    public Visual_Attribute(string typeName, List<Visual_AttributeProperty> properties)
    {
        TypeName = typeName;
        Properties = properties;
    }

}
