using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
[Serializable]
public class Visual_Interface
{
    [ReadOnly]
    [LabelWidth(75)]
    public string Name; // Name of the interface
    [HorizontalGroup("NameGroup")]
    [LabelWidth(75)]
    public string EditName;

    [Button("Edit", ButtonSizes.Small, ButtonAlignment = 1, Stretch = false)]
    [HorizontalGroup("NameGroup", 0.15f)] // Allocates a percentage of the width for the button
    public void AttemptApplyName()
    {
        // Edit name code here
    }

    [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false, ShowFoldout = true, OnTitleBarGUI = "DrawAddAttributeButton", IsReadOnly = true)]
    public List<string> Attributes; // List of attributes

    [HideInInspector]
    public List<Visual_Property> Properties; // List of properties
    [HideInInspector]
    public List<Visual_Method> Methods; // List of methods
    [HideInInspector]
    public List<Visual_Component_Base> Components; // List of methods

    [HorizontalGroup("Interface Counts"), ShowInInspector, ReadOnly, LabelText("Components")]
    public int ComponentsCount
    {
        get { return Components.Count; }
    }


    [HorizontalGroup("Interface Counts"), ShowInInspector, ReadOnly, LabelText("Properties")]
    public int PropertiesCount
    {
        get { return Properties.Count; }
    }

    [HorizontalGroup("Interface Counts"), ShowInInspector, ReadOnly, LabelText("Methods")]
    public int MethodsCount
    {
        get { return Methods.Count; }
    }


    public Visual_Interface(string name)
    {
        Name = name;
        EditName = name;
        Attributes = new List<string>();
        Properties = new List<Visual_Property>();
        Methods = new List<Visual_Method>();
        Components = new List<Visual_Component_Base>();
    }

    public Visual_Interface(string name, List<SimpleMemberInfo> members)
    {
        Debug.LogWarning("???");
        Name = name;
        EditName = name;
        Attributes = new List<string>();
        Properties = new List<Visual_Property>();
        Methods = new List<Visual_Method>();
        Components = new List<Visual_Component_Base>();

        foreach (SimpleMemberInfo member in members)
        {
            if (member is SimpleMethodInfo smi)
            {
                Visual_Method visual_Method = new Visual_Method(smi.MemberName, smi.ReturnType, smi.MemberType,
                    smi.Attributes.ConvertAll(attr =>
                        new Visual_Attribute(attr.AttributeTypeName,
                            attr.AttributeProperties.Select(prop =>
                                new Visual_AttributeProperty(new Dictionary<string, List<string>> { { prop.Key, prop.Value } })).ToList()
                        )
                    ),
                    smi.ParameterTypes.ConvertAll(x => new Visual_Parameter(x.Name, x.ParameterType.Name))
                );

                Methods.Add(visual_Method);
            }
            else if (member is SimplePropertyInfo spi)
            {
                List<Visual_Attribute> visualAttributes = spi.Attributes.ConvertAll(attr =>
                    new Visual_Attribute(attr.AttributeTypeName,
                        attr.AttributeProperties.Select(prop =>
                            new Visual_AttributeProperty(new Dictionary<string, List<string>> { { prop.Key, prop.Value } })).ToList()
                    )
                );

                if (typeof(Component).IsAssignableFrom(spi.VariableType))
                {
                    Debug.LogWarning("?!?");
                    Type genericVisualComponentType = typeof(Visual_Component<>);
                    Type constructedVisualComponentType = genericVisualComponentType.MakeGenericType(spi.VariableType);
                    object visualComponentInstance = Activator.CreateInstance(constructedVisualComponentType, spi.MemberName, spi.VariableType.Name, spi.MemberType, visualAttributes);
                    Components.Add(visualComponentInstance as Visual_Component_Base);
                    
                }

                else
                {
                    Visual_Property visual_Property = new Visual_Property(spi.MemberName, spi.VariableType.Name, spi.MemberType, visualAttributes);
                    Properties.Add(visual_Property);
                }
            }
            else
            {
                Debug.LogWarning("Seeing fields here in the Visual Interface");
            }
        }
    }


    private void DrawAddAttributeButton()
    {
        if (GUILayout.Button("+"))
        {
            // Open the search box here
            string searchString = "Type to search..."; // This is a placeholder for your search logic
            EditorUtility.DisplayDialog("Search Attributes", searchString, "OK");
        }
    }

    private void SearchForDuplicateInterface()
    {
        // Implementation for searching duplicate interface
    }
}
