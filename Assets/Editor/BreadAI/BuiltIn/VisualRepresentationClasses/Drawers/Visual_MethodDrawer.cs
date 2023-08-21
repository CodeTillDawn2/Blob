using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class Visual_MethodDrawer : OdinValueDrawer<Visual_Method>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        Visual_Method method = this.ValueEntry.SmartValue;

        // Drawing a custom box around the method details
        GUILayout.BeginVertical(StyleHelper.BoxStyle());
        {
            DrawMethod(method);
        }
        GUILayout.EndVertical();
    }

    public static void DrawMethod(Visual_Method method)
    {
        if (method != null)
        {
            GUILayout.Label("//Placeholder description pulled from xml comments...", StyleHelper.Code_CommentsStyle, GUILayout.ExpandWidth(false));
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(method.ReturnType + " ", StyleHelper.Code_BlueStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(method.Name, StyleHelper.Code_YellowOrangeStyle, GUILayout.ExpandWidth(false));
                GUILayout.Label(" (", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));

                for (int i = 0; i < method.Parameters.Count; i++)
                {
                    var parameter = method.Parameters[i];
                    GUILayout.Label(parameter.ParameterType + " ", StyleHelper.Code_BlueStyle, GUILayout.ExpandWidth(false));
                    GUILayout.Label(parameter.Name, StyleHelper.Code_LightBlueStyle, GUILayout.ExpandWidth(false));

                    if (i < method.Parameters.Count - 1)
                    {
                        GUILayout.Label(", ", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));
                    }
                }

                GUILayout.Label(")", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));
            }
            GUILayout.EndHorizontal();

            method.HasDefaultImplementation = EditorGUILayout.Toggle(
                new GUIContent("Has Default Implementation?"),
                method.HasDefaultImplementation,
                GUILayout.MinWidth(500));

            if (method.HasDefaultImplementation)
            {
                GUILayout.Label("{", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));

                float width = EditorGUIUtility.currentViewWidth - 40;
                float height = GUI.skin.textArea.CalcHeight(new GUIContent(method.DefaultImplementationCode), width);

                // Increase the height of the text area by the height of two lines
                float extraLineHeight = GUI.skin.textArea.lineHeight * 2;
                height += extraLineHeight;

                method.DefaultImplementationCode = EditorGUILayout.TextArea(method.DefaultImplementationCode, GUILayout.Height(height));

                GUILayout.Label("}", StyleHelper.Code_WhiteStyle, GUILayout.ExpandWidth(false));
            }
        }
    }
}
