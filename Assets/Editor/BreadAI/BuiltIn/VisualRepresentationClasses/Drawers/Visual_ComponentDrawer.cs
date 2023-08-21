using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class Visual_ComponentDrawer : OdinValueDrawer<Visual_Component_Base>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        Visual_Component_Base component = this.ValueEntry.SmartValue;

        if (component != null)
        {
            // Draw the default properties of Visual_Component using Odin
            this.CallNextDrawer(label);

            // Draw the Unity Editor for the component
            component.DrawComponentInspector();
        }
    }


    public static void DrawVisualComponent(Visual_Component_Base component)
    {
        if (component != null)
        {
            GUILayout.Label("//Placeholder description pulled from xml comments...", StyleHelper.Code_CommentsStyle, GUILayout.ExpandWidth(false));
            // Add save button with save icon
            GUIContent saveButtonContent = new GUIContent(EditorGUIUtility.IconContent("SaveActive").image);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("abstract ", StyleHelper.Code_LightBlueStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(component.ReturnType + " ", StyleHelper.Code_FieldVariableTypeStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(component.Name + " ", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(" {", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(" get", StyleHelper.Code_LightBlueStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(";  ", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label("set", StyleHelper.Code_LightBlueStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(";  }", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));


            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button(saveButtonContent, GUILayout.Width(25), GUILayout.Height(25)))
            {
                // Check if the component has a SaveComponentToPrefab method
                var saveMethod = component.GetType().GetMethod("SaveComponentToPrefab");
                if (saveMethod != null)
                {
                    saveMethod.Invoke(component, null);
                }
                else
                {
                    Debug.LogWarning("SaveComponentToPrefab method not found on component!");
                }
            }

            GUILayout.Label(" ", StyleHelper.Code_LightBlueStyle, GUILayout.ExpandWidth(false));

            // Draw the Unity Editor for the component
            component.DrawComponentInspector();
        }
    }

}
