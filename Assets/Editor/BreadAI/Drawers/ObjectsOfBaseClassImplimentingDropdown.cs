using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BaseClassRequiredImplementingInterfacesAttribute))]
public class ObjectsOfBaseClassImplementingDropdown : PropertyDrawer
{
    private List<string> derivedTypeNames = new List<string> { "None" };
    private int selectedIndex = 0;
    private string lastParentDropdownValue = string.Empty;

    private void RefreshDerivedTypesFromDictionary(SerializedProperty property)
    {
        var baseClassAttribute = attribute as BaseClassRequiredImplementingInterfacesAttribute;

        // Fetch the parent dropdown value, which should be the specific derived type like "PigBrain" or "HumanBrain"
        var parentDropdownValue = property.serializedObject.FindProperty(baseClassAttribute.ParentField).stringValue;

        // Find the matching type for the parent field string representation
        var matchingType = AIBaker.CachedBakerSO.CharacterSystemToConfigMapping.Keys.FirstOrDefault(t => t.ToString() == baseClassAttribute.ParentField);

        if (matchingType != null)
        {
            if (AIBaker.CachedBakerSO.CharacterSystemToConfigMapping.TryGetValue(matchingType, out var innerDictionary))
            {
                if (!innerDictionary.TryGetValue(parentDropdownValue, out derivedTypeNames))
                {
                    derivedTypeNames = new List<string> { "None" };
                }
            }
            else
            {
                derivedTypeNames = new List<string> { "None" };
            }
        }
        else
        {
            derivedTypeNames = new List<string> { "None" };
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var baseClassAttribute = (BaseClassRequiredImplementingInterfacesAttribute)attribute;

        if (!string.IsNullOrEmpty(baseClassAttribute.ParentField))
        {
            var dependentProperty = property.serializedObject.FindProperty(baseClassAttribute.ParentField);
            var currentParentDropdownValue = dependentProperty.stringValue;

            if (currentParentDropdownValue != lastParentDropdownValue || !derivedTypeNames.Any())
            {
                RefreshDerivedTypesFromDictionary(property);
                lastParentDropdownValue = currentParentDropdownValue;
            }
        }

        var currentValue = property.stringValue;
        selectedIndex = derivedTypeNames.IndexOf(currentValue);
        if (selectedIndex == -1)
        {
            selectedIndex = 0; // Default to "None"
        }

        selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, derivedTypeNames.ToArray());

        if (selectedIndex < derivedTypeNames.Count)
        {
            property.stringValue = derivedTypeNames[selectedIndex];
        }

        property.serializedObject.ApplyModifiedProperties();

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var baseClassAttribute = (BaseClassRequiredImplementingInterfacesAttribute)attribute;
        return !IsValidSelection(property, baseClassAttribute)
            ? base.GetPropertyHeight(property, label) + 40f
            : base.GetPropertyHeight(property, label);
    }

    private bool IsValidSelection(SerializedProperty property, BaseClassRequiredImplementingInterfacesAttribute baseClassAttribute)
    {
        if (string.IsNullOrEmpty(baseClassAttribute.ParentField))
            return true;

        var parentProperty = property.serializedObject.FindProperty(baseClassAttribute.ParentField);
        if (parentProperty == null)
            return true;

        if (parentProperty.stringValue == "None")
            return property.stringValue == "None";

        return !string.IsNullOrEmpty(parentProperty.stringValue) && !string.IsNullOrEmpty(property.stringValue);
    }
}
