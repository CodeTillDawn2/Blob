using Sirenix.OdinInspector.Editor;
using System;
using UnityEngine;

[Serializable]
public class Visual_PropertyListDrawer : OdinValueDrawer<Visual_PropertyList>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        var value = this.ValueEntry.SmartValue;

        for (int i = 0; i < value.List.Count; i++)
        {
            var property = value.List[i];

            // Use the Visual_PropertyDrawer class to draw each property
            Visual_PropertyDrawer.DrawProperty(property);
        }
    }
}
