using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BaseClassRequiredAttribute))]
public class ObjectsOfBaseClassDropdown : PropertyDrawer
{
    private List<string> derivedTypeNames = new List<string> { "None" };
    private List<string> dependentTypeNames = new List<string> { "None" };
    private int selectedIndex = 0;
    private int dependentSelectedIndex = 0;

    private void Initialize(SerializedProperty property)
    {
        var attributeData = attribute as BaseClassRequiredAttribute;

        var splitValues = property.stringValue.Split(':');
        var primaryTypeSelection = splitValues[0];
        var dependentTypeSelection = splitValues.Length > 1 ? splitValues[1] : "None";

        if (AIBaker.CachedBakerSO.CharacterSystemToConfigMapping.TryGetValue(attributeData.BaseTypeName, out var primaryDict))
        {
            derivedTypeNames = primaryDict.Keys.ToList();
            if (!derivedTypeNames.Contains("None"))
            {
                derivedTypeNames.Insert(0, "None");
            }

            selectedIndex = derivedTypeNames.IndexOf(primaryTypeSelection);
            if (selectedIndex == -1) selectedIndex = 0;

            UpdateDependentDropdownList(attributeData.BaseTypeName, primaryTypeSelection, dependentTypeSelection);
        }
        else
        {
            Debug.LogError($"Couldn't find entries for base type: {attributeData.BaseTypeName} in AIBaker's dictionary.");
        }
    }

    private void UpdateDependentDropdownList(string baseType, string primaryTypeSelection, string dependentTypeSelection)
    {
        if (AIBaker.CachedBakerSO.CharacterSystemToConfigMapping.ContainsKey(baseType) && AIBaker.CachedBakerSO.CharacterSystemToConfigMapping[baseType].TryGetValue(primaryTypeSelection, out var configs))
        {
            dependentTypeNames = configs;
            if (!dependentTypeNames.Contains("None"))
            {
                dependentTypeNames.Insert(0, "None");
            }

            dependentSelectedIndex = dependentTypeNames.IndexOf(dependentTypeSelection);
            if (dependentSelectedIndex == -1) dependentSelectedIndex = 0;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        property.serializedObject.Update();

        Initialize(property);

        float singleHeight = position.height / 3.5f;
        float spacing = singleHeight * 0.25f;  // Provide a little space between GUI elements

        EditorGUI.BeginChangeCheck();
        selectedIndex = EditorGUI.Popup(
            new Rect(position.x, position.y, position.width, singleHeight),
            label.text.Split('/')[0],
            selectedIndex,
            derivedTypeNames.ToArray()
        );

        bool primaryChanged = EditorGUI.EndChangeCheck();

        if (primaryChanged)
        {
            if (derivedTypeNames[selectedIndex] == "None")
            {
                dependentSelectedIndex = 0;
                property.stringValue = "None";
            }
            else
            {
                UpdateDependentDropdownList("", derivedTypeNames[selectedIndex], "None");
            }
        }

        if (derivedTypeNames[selectedIndex] != "None")
        {
            EditorGUI.BeginChangeCheck();
            dependentSelectedIndex = EditorGUI.Popup(
                new Rect(position.x, position.y + singleHeight + spacing, position.width, singleHeight),
                $"{label.text} Configuration",
                dependentSelectedIndex,
                dependentTypeNames.ToArray()
            );

            bool dependentChanged = EditorGUI.EndChangeCheck();

            if (primaryChanged || dependentChanged)
            {
                property.stringValue = $"{derivedTypeNames[selectedIndex]}:{dependentTypeNames[dependentSelectedIndex]}";
            }

            if (dependentTypeNames[dependentSelectedIndex] == "None")
            {
                EditorGUI.HelpBox(
                    new Rect(position.x, position.y + 2 * singleHeight + 2 * spacing, position.width, singleHeight),
                    "Please select a configuration value.",
                    MessageType.Warning
                );
            }
        }
        else
        {
            property.stringValue = "None";
        }

        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float baseHeight = base.GetPropertyHeight(property, label);
        if (derivedTypeNames[selectedIndex] != "None")
        {
            // Provide space for 2 dropdowns and possible warning message
            return derivedTypeNames[selectedIndex] != "None" && dependentTypeNames[dependentSelectedIndex] == "None"
                ? baseHeight * 3.5f
                : baseHeight * 3;
        }
        return baseHeight * 2;
    }

}
