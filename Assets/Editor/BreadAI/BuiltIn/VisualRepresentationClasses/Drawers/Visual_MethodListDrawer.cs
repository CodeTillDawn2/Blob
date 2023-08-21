using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class Visual_MethodListDrawer : OdinValueDrawer<Visual_MethodList>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {


        // Get the value entry for the property you are drawing
        Visual_MemberList<Visual_Method> methodList = this.ValueEntry.SmartValue;

        if (methodList != null)
        {
            foreach (var method in methodList.List)
            {
                // Use the static method to draw each method
                Visual_MethodDrawer.DrawMethod(method);
            }
        }
    }
}
