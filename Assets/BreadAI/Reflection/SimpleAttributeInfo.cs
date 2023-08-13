using System.Collections.Generic;
using System;
using System.Reflection;
using Sirenix.OdinInspector.Editor;

[Serializable]
public class SimpleAttributeInfo
{
    public string AttributeTypeName { get; set; }
    public Dictionary<string, List<string>> AttributeProperties { get; set; } = new Dictionary<string, List<string>>();

    public SimpleAttributeInfo() { }

    public SimpleAttributeInfo(Attribute attribute)
    {
        AttributeTypeName = attribute.GetType().Name;

        // Fetch properties and their values
        foreach (var property in attribute.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.GetCustomAttribute<NonSerializedAttribute>() != null)
                continue;

            var propertyValue = property.GetValue(attribute);
            if (propertyValue != null)
            {
                if (!AttributeProperties.ContainsKey(property.Name))
                {
                    AttributeProperties[property.Name] = new List<string>();
                }

                if (propertyValue is Type[] types)
                {
                    foreach (var typeFound in types)
                    {
                        AttributeProperties[property.Name].Add(typeFound.ToString());
                    }
                }
                else
                {
                    AttributeProperties[property.Name].Add(propertyValue.ToString());
                }
            }
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SimpleAttributeInfo other = (SimpleAttributeInfo)obj;
        if (AttributeTypeName != other.AttributeTypeName)
            return false;

        return AreDictionariesEqual(AttributeProperties, other.AttributeProperties);
    }

    private bool AreDictionariesEqual(Dictionary<string, List<string>> dict1, Dictionary<string, List<string>> dict2)
    {
        if (dict1 == null && dict2 == null)
            return true;

        if (dict1 == null || dict2 == null)
            return false;

        if (dict1.Count != dict2.Count)
            return false;

        foreach (var key in dict1.Keys)
        {
            if (!dict2.ContainsKey(key) || !AreListsEqual(dict1[key], dict2[key]))
                return false;
        }

        return true;
    }

    private bool AreListsEqual(List<string> list1, List<string> list2)
    {
        if (list1 == null && list2 == null)
            return true;

        if (list1 == null || list2 == null || list1.Count != list2.Count)
            return false;

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (AttributeTypeName?.GetHashCode() ?? 0);
            hash = hash * 23 + (AttributeProperties?.Count ?? 0);  // Simplifying, might not be best for all cases.
            return hash;
        }
    }
}
