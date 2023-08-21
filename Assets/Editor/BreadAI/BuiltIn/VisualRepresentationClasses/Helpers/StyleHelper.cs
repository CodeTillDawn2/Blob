using UnityEngine;

public static class StyleHelper
{
    // Helper method to create a texture with rounded corners
    private static Texture2D MakeRoundedTexture(int radius, Color color)
    {
        Texture2D texture = new Texture2D(radius * 2, radius * 2);
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (Vector2.Distance(new Vector2(radius, radius), new Vector2(x, y)) > radius)
                {
                    color.a = 0; // Transparent outside the radius
                }
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }


    public static GUIStyle BoxStyle()
    {
        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.normal.background = MakeRoundedTexture(2, Color.grey);
        return boxStyle;
    }

    public static GUIStyle AttributeBoxStyle()
    {
        GUIStyle attributeBoxStyle = new GUIStyle(GUI.skin.box);
        attributeBoxStyle.normal.background = MakeRoundedTexture(1, Color.gray);
        return attributeBoxStyle;
    }

    public static Color CodeBlueColor = new Color(0, 0, 1);
    public static Color CodeYellowOrangeColor = new Color(1, 0.65f, 0);
    public static Color CodeLightBlueColor = new Color(62f / 255, 144f / 255, 214f / 255);
    public static Color Code_FieldVariableTypeColor = new Color(58f / 255, 107f / 255, 45f / 255);
    public static Color Code_CommentGreen = new Color(64f / 255, 155f / 255, 74f / 255);

    public static GUIStyle Code_BlueStyle = new GUIStyle(GUI.skin.label) { 
        normal = { textColor = CodeBlueColor },
        margin = new RectOffset(0, 0, 0, 0),
        padding = new RectOffset(1, 1, 1, 1)
    };
    public static GUIStyle Code_YellowOrangeStyle = new GUIStyle(GUI.skin.label) { 
        normal = { textColor = CodeYellowOrangeColor },
        margin = new RectOffset(0, 0, 0, 0),
        padding = new RectOffset(1, 1, 1, 1)
    };
    public static GUIStyle Code_LightBlueStyle = new GUIStyle(GUI.skin.label) { 
        normal = { textColor = CodeLightBlueColor },
        margin = new RectOffset(0, 0, 0, 0),
        padding = new RectOffset(1, 1, 1, 1)
    };

    public static GUIStyle Code_FieldVariableTypeStyle = new GUIStyle(GUI.skin.label) { 
        normal = { textColor = Code_FieldVariableTypeColor },
        margin = new RectOffset(0, 0, 0, 0),
        padding = new RectOffset(1, 1, 1, 1)
    };
    public static GUIStyle Code_WhiteStyle = new GUIStyle(GUI.skin.label)
    {
        normal = { textColor = Color.white },
        margin = new RectOffset(0, 0, 0, 0),
        padding = new RectOffset(1, 1, 1, 1)
    };

    public static GUIStyle Code_CommentsStyle = new GUIStyle(GUI.skin.label)
    {
        normal = { textColor = Code_CommentGreen },
        margin = new RectOffset(0, 0, 0, 0),
        padding = new RectOffset(1, 1, 1, 1)
    };

}
