using System;
using System.Collections.Generic;  // For the List<T>
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BaseClassRequiredAttribute))]
public class ObjectsOfBaseClassDropdown : PropertyDrawer
{
    private Type[] derivedTypes;
    private List<string> derivedTypeNames = new List<string> { "None" };
    private int selectedIndex = 0;

    private void Initialize(SerializedProperty property)
    {
        var baseClassAttribute = attribute as BaseClassRequiredAttribute;
        var baseType = baseClassAttribute.BaseType;

        derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
            .ToArray();

        // After populating derivedTypes, create derivedTypeNames
        derivedTypeNames = derivedTypes.Select(t => t.FullName ?? "Unknown").ToList();
        derivedTypeNames.Insert(0, "None");

        // Debug check: Print out if any null or empty names are found
        if (derivedTypeNames.Any(string.IsNullOrEmpty))
        {
            Debug.LogError("Null or empty names found in derivedTypeNames!");
        }

        var currentValue = property.stringValue;
        selectedIndex = derivedTypeNames.IndexOf(currentValue);
        if (selectedIndex == -1) selectedIndex = 0; // Default to "None"
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Initialize(property); // Make sure you're always updating the derived types/names.

        // Now, properly update the selectedIndex based on current property value.
        var currentValue = property.stringValue;
        selectedIndex = derivedTypeNames.IndexOf(currentValue);
        if (selectedIndex == -1) selectedIndex = 0; // Default to "None"

        var previousIndex = selectedIndex;
        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, derivedTypeNames.ToArray());

        if (previousIndex != selectedIndex) // Check if the dropdown value has changed
        {
            property.serializedObject.Update();
        }

        if (selectedIndex == 0) // "None" selected
        {
            property.stringValue = string.Empty;
        }
        else if (selectedIndex > 0 && selectedIndex < derivedTypeNames.Count)
        {
            property.stringValue = derivedTypeNames[selectedIndex];
        }

        EditorGUI.EndProperty();
    }

}
