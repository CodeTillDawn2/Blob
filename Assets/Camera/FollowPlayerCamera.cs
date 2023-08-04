using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Stat Block")]
    public GameObjectVariable target;
    public float CameraDistance;
    public float CameraAngle;
    private Camera Camera;
    [Serialize] public Vector3Variable BlobDims;

    Vector3 dragStartPosition = Vector3.zero;
    Vector3 dragCurrentPosition = Vector3.zero;
    Vector3 newPosition;
    [SerializeField] private float movementTime = 5f;

    // Smoothing factors for camera position and rotation
    public float followSpeed = 10f;
    public float rotationSpeed = 5f;

    // Smoothing variables for camera position and rotation
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private float initialCameraHeight;

    private void Start()
    {
        Camera = GetComponent<Camera>();
        newPosition = transform.position;
        Camera.main.depthTextureMode = DepthTextureMode.DepthNormals;

        if (target != null)
            initialCameraHeight = transform.position.y - target.Value.transform.position.y;
    }

    void Update()
    {
        if (target != null && BlobDims != null)
        {
            // Calculate the distance to follow the player based on the maximum dimension of the Blob
            float maxBlobDimension = Mathf.Max(BlobDims.Value.x, BlobDims.Value.y, BlobDims.Value.z);
            float distance = maxBlobDimension / 2f / Mathf.Tan(Camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            distance = Mathf.Clamp(distance, CameraDistance, distance);

            // Calculate the target position to follow the player
            Vector3 targetDirection = -target.Value.transform.forward * distance; // Reversed direction here to be behind the player
            float heightOffset = Mathf.Tan(CameraAngle * Mathf.Deg2Rad) * distance;

            // Adjust the heightOffset to raise the camera higher
            heightOffset += 1.0f; // You can change this value to increase or decrease the height

            targetPosition = target.Value.transform.position + targetDirection + Vector3.up * (initialCameraHeight + heightOffset);

            // Calculate the target rotation to align with player's heading and tilt
            Vector3 lookDirection = target.Value.transform.forward; // Use player's forward direction
            lookDirection.y = Mathf.Tan(CameraAngle * Mathf.Deg2Rad);
            targetRotation = Quaternion.LookRotation(lookDirection, target.Value.transform.up);

            // Smoothly follow the target position and rotation
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
