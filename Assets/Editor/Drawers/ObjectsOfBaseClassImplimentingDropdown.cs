using System;
using System.Collections.Generic;  // For the List<T>
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BaseClassRequiredImplementingInterfacesAttribute))]
public class ObjectsOfBaseClassImplimentingDropdown : PropertyDrawer
{
    private Type[] derivedTypes;
    private List<string> derivedTypeNames = new List<string> { "None" };
    private int selectedIndex = 0;
    private string lastParentDropdownValue = string.Empty;


    private void RefreshDerivedTypes(SerializedProperty property)
    {
        var baseClassAttribute = attribute as BaseClassRequiredImplementingInterfacesAttribute;
        var baseType = baseClassAttribute.BaseType;

        derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
            .ToArray();

        derivedTypeNames = derivedTypes.Select(t => t.FullName ?? "Unknown").ToList();
        derivedTypeNames.Insert(0, "None");

        if (derivedTypeNames.Any(string.IsNullOrEmpty))
        {
            Debug.LogError("Null or empty names found in derivedTypeNames!");
        }

        if (!string.IsNullOrEmpty(baseClassAttribute.ParentField))
        {
            var dependentProperty = property.serializedObject.FindProperty(baseClassAttribute.ParentField);
            if (dependentProperty != null)
            {
                if (dependentProperty.stringValue == "None" || string.IsNullOrEmpty(dependentProperty.stringValue))
                {
                    // If parent is set to None, reset the dependent dropdown to None
                    property.stringValue = "None";
                    selectedIndex = 0;
                }
                else
                {
                    var dependentType = FindType(dependentProperty.stringValue);
                    if (dependentType != null)
                    {
                        var expectedInterfacesField = dependentType.GetField("_expectedStatsInterfaces", BindingFlags.Public | BindingFlags.Static);
                        if (expectedInterfacesField != null)
                        {
                            var expectedInterfaces = expectedInterfacesField.GetValue(null) as Type[];
                            if (expectedInterfaces != null)
                            {
                                derivedTypes = derivedTypes.Where(t => t != baseType && expectedInterfaces.All(ei => ei.IsAssignableFrom(t))).ToArray();
                                derivedTypeNames = derivedTypes.Select(t => t.FullName ?? "Unknown").ToList();
                                selectedIndex = 0; // set the dependent dropdown to the first item
                            }
                        }
                        else
                        {
                            derivedTypes = new Type[0];
                            derivedTypeNames.Clear();
                            derivedTypeNames.Add("None");
                            selectedIndex = 0;
                        }
                    }
                }
            }
        }
    }



    private bool IsValidSelection(SerializedProperty property, BaseClassRequiredImplementingInterfacesAttribute baseClassAttribute)
    {

        if (!string.IsNullOrEmpty(baseClassAttribute.ParentField))
        {
            var parentProperty = property.serializedObject.FindProperty(baseClassAttribute.ParentField);
            if (parentProperty != null)
            {

                // If parent dropdown is set to "None", child should also be "None" and this is valid.
                if (parentProperty.stringValue == "None")
                {
                    if (property.stringValue == "None")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                // If parent dropdown has a value other than "None" and child is "None", this is invalid.
                if (!string.IsNullOrEmpty(parentProperty.stringValue) && property.stringValue == "None")
                {
                    return false;
                }

                // If both parent and child have values other than "None", this is valid.
                if (!string.IsNullOrEmpty(parentProperty.stringValue) && !string.IsNullOrEmpty(property.stringValue))
                {
                    return true;
                }
            }
        }

        return true; // If no conditions are met, assume valid by default
    }





    private Type FindType(string typeName)
    {
        if (string.IsNullOrEmpty(typeName) || typeName == "None")
        {
            return null;
        }

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }
        return null;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var baseClassAttribute = (BaseClassRequiredImplementingInterfacesAttribute)attribute;

        if (!string.IsNullOrEmpty(baseClassAttribute.ParentField))
        {
            var dependentProperty = property.serializedObject.FindProperty(baseClassAttribute.ParentField);
            var currentParentDropdownValue = dependentProperty.stringValue;

            if (currentParentDropdownValue != lastParentDropdownValue || derivedTypeNames.Count == 0)
            {
                RefreshDerivedTypes(property);
                lastParentDropdownValue = currentParentDropdownValue;
            }
        }

        // Get the current value from the SerializedProperty and ensure it matches the displayed GUI
        var currentValue = property.stringValue;
        selectedIndex = derivedTypeNames.IndexOf(currentValue);
        if (selectedIndex == -1)
        {
            selectedIndex = 0; // Default to "None"
        }

        // Display the dropdown with the correct value
        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, derivedTypeNames.ToArray());

        // If the user changes the dropdown value, update the SerializedProperty
        if (selectedIndex < derivedTypeNames.Count)
        {
            property.stringValue = derivedTypeNames[selectedIndex];
        }

        // If any changes have been made, apply them
        property.serializedObject.ApplyModifiedProperties();

        EditorGUI.EndProperty();
    }




    private const float helpBoxHeight = 40f; // Define a constant height for the HelpBox.
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var baseClassAttribute = (BaseClassRequiredImplementingInterfacesAttribute)attribute;

        if (!IsValidSelection(property, baseClassAttribute))
        {
            return base.GetPropertyHeight(property, label) + helpBoxHeight; // Add the height of the help box if validation fails.
        }

        return base.GetPropertyHeight(property, label); // Default height.
    }

}
