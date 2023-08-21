using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class Visual_ComponentListDrawer : OdinValueDrawer<Visual_ComponentList>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        Visual_ComponentList componentList = this.ValueEntry.SmartValue;

        if (componentList != null)
        {
            // Iterate through the list and use the static method to draw each component
            foreach (var component in componentList.List)
            {
                Visual_ComponentDrawer.DrawVisualComponent(component);
            }
        }
    }
}
