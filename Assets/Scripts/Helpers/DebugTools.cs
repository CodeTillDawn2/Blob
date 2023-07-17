using UnityEngine;

public static class DebugTools
{
    public static void DrawLine(Vector3 position, Quaternion angle, Vector3 distance)
    {
        // set the color of the line
        PlayerController.Player.lineRenderer.startColor = Color.white;
        PlayerController.Player.lineRenderer.endColor = Color.red;

        // set width of the renderer
        PlayerController.Player.lineRenderer.startWidth = 0.3f;
        PlayerController.Player.lineRenderer.endWidth = 0.3f;

        // set the position
        PlayerController.Player.lineRenderer.SetPosition(0, position);
        PlayerController.Player.lineRenderer.SetPosition(1, position + angle * (distance * 5));
    }

    public static void DrawLine(Vector3 position1, Vector3 position2)
    {
        // set the color of the line
        PlayerController.Player.lineRenderer.startColor = Color.white;
        PlayerController.Player.lineRenderer.endColor = Color.red;

        // set width of the renderer
        PlayerController.Player.lineRenderer.startWidth = 0.1f;
        PlayerController.Player.lineRenderer.endWidth = 0.3f;

        // set the position
        PlayerController.Player.lineRenderer.positionCount = 2;
        PlayerController.Player.lineRenderer.SetPosition(0, position1);
        PlayerController.Player.lineRenderer.SetPosition(1, position2);
    }

    public static void DrawCube(Vector3 position1, Vector3 position2, Vector3 position3, Vector3 position4, Vector3 position5, Vector3 position6, Vector3 position7, Vector3 position8)
    {
        // set the color of the line
        PlayerController.Player.lineRenderer.startColor = Color.white;
        PlayerController.Player.lineRenderer.endColor = Color.red;

        // set width of the renderer
        PlayerController.Player.lineRenderer.startWidth = 0.1f;
        PlayerController.Player.lineRenderer.endWidth = 0.3f;

        // set the position
        PlayerController.Player.lineRenderer.positionCount = 8;
        PlayerController.Player.lineRenderer.SetPosition(0, position1);
        PlayerController.Player.lineRenderer.SetPosition(1, position2);
        PlayerController.Player.lineRenderer.SetPosition(1, position3);
        PlayerController.Player.lineRenderer.SetPosition(1, position4);
        PlayerController.Player.lineRenderer.SetPosition(1, position5);
        PlayerController.Player.lineRenderer.SetPosition(1, position6);
        PlayerController.Player.lineRenderer.SetPosition(1, position7);
        PlayerController.Player.lineRenderer.SetPosition(1, position8);
    }

    public static void DrawAttention(Vector3 position)
    {
        // set the color of the line
        PlayerController.Player.lineRenderer.startColor = Color.red;
        PlayerController.Player.lineRenderer.endColor = Color.red;

        // set width of the renderer
        PlayerController.Player.lineRenderer.startWidth = 0.3f;
        PlayerController.Player.lineRenderer.endWidth = 0.3f;

        // set the position
        PlayerController.Player.lineRenderer.positionCount = 2;
        PlayerController.Player.lineRenderer.SetPosition(0, position);
        PlayerController.Player.lineRenderer.SetPosition(1, position + new Vector3(0, 2, 0));
    }

    public static void ClearLine()
    {
        if (PlayerController.Player.lineRenderer != null)
        {
            PlayerController.Player.lineRenderer.positionCount = 0;
        }

    }

}
