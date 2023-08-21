using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class Visual_PropertyDrawer : OdinValueDrawer<Visual_Property>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        Visual_Property property = this.ValueEntry.SmartValue;

        // Drawing a custom box around the property details
        GUILayout.BeginVertical(StyleHelper.BoxStyle());
        {
            DrawProperty(property);
        }
        GUILayout.EndVertical();
    }

    public static void DrawProperty(Visual_Property property)
    {
        if (property != null)
        {
            // Using the styles defined in StyleHelper



            // You can also use the predefined styles, like StyleHelper.CodeBlueStyle, if they match the color you want

            GUILayout.Label("//Placeholder description pulled from xml comments...", StyleHelper.Code_CommentsStyle, GUILayout.ExpandWidth(false));
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("abstract ", StyleHelper.Code_LightBlueStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(property.ReturnType + " ", StyleHelper.Code_FieldVariableTypeStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(property.Name + " ", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(" {", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(" get", StyleHelper.Code_LightBlueStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(";  ", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label("set", StyleHelper.Code_LightBlueStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(";  }", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));
            }
            GUILayout.EndHorizontal();
            GUILayout.Label(" ", StyleHelper.Code_LightBlueStyle, GUILayout.ExpandWidth(false));
        }
    }
}
