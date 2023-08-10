using UnityEngine;

public class Eye : MonoBehaviour
{
    public float FOVStart = 0;
    public float FOVEnd = 360f;

    public float FOVRadius = 4f; // Distance from the eye where FOV will be drawn
    public Color FOVColor = Color.green; // Color to draw the FOV

    void OnDrawGizmosSelected()
    {
        DrawFOV();
    }
    public bool IsWithinFOV(Vector3 point)
    {
        Vector3 directionToPoint = (point - transform.position).normalized;

        // Calculate the midpoint of the FOV
        float midFOVAngle = (FOVEnd + FOVStart) * 0.5f;
        Vector3 midDirection = DirectionFromAngle(transform.eulerAngles.y, midFOVAngle);

        // Calculate the angle (in degrees) between the midDirection of the FOV and the direction to the point
        float angleToTarget = Vector3.Angle(midDirection, directionToPoint);

        // The target is within the FOV if the angle to the target is less than half the total FOV
        float halfFOV = (FOVEnd - FOVStart) * 0.5f;

        return angleToTarget <= halfFOV;
    }


    private void DrawFOV()
    {
        // Use Gizmos to draw FOV
        Gizmos.color = FOVColor;

        // Drawing the full arc (circle) for FOV
        Vector3 startPos = DirectionFromAngle(transform.eulerAngles.y, FOVStart) * FOVRadius;
        Vector3 endPos = DirectionFromAngle(transform.eulerAngles.y, FOVEnd) * FOVRadius;

        // This will loop through and draw lines between each degree of the FOV arc
        for (int i = Mathf.FloorToInt(FOVStart); i < FOVEnd; i++)
        {
            Vector3 startRay = DirectionFromAngle(transform.eulerAngles.y, i) * FOVRadius;
            Vector3 endRay = DirectionFromAngle(transform.eulerAngles.y, i + 1) * FOVRadius;
            Gizmos.DrawLine(transform.position + startRay, transform.position + endRay);
        }

        // Draw lines to connect the start and end of the FOV arc to the eye position
        Gizmos.DrawLine(transform.position, transform.position + startPos);
        Gizmos.DrawLine(transform.position, transform.position + endPos);
    }

    Vector3 DirectionFromAngle(float globalAngle, float localAngle)
    {
        float angle = globalAngle + localAngle;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
