using System.Collections.Generic;
using System.Linq;
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

    private void Start()
    {
        Camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (target != null)
        {

            //float TrueCameraDistance = new List<float>() { BlobDims.Value.x, BlobDims.Value.y, BlobDims.Value.z }.Max() * CameraDistance;

            float distance = new List<float>() { BlobDims.Value.x, BlobDims.Value.y, BlobDims.Value.z }.Max() / 2
                / Mathf.Abs(Mathf.Sin(Camera.fieldOfView * Mathf.Deg2Rad / 2));
            Vector3 BackwardsVector = -target.Value.transform.forward;
            BackwardsVector.y = CameraAngle;
            if (distance < CameraDistance)
            {
                distance = CameraDistance;
            }
            transform.position = target.Value.transform.position + (BackwardsVector * distance);



            //rotate us over time according to speed until we are in the required rotation
            transform.LookAt(target.Value.transform.position + new Vector3(0, BlobDims.Value.y / 2, 0));
            //print("Camera Distance: " + TrueCameraDistance + " Camera Angle: " + CameraAngle + " Cube Width: " + player.CubeWidth );
        }

    }

}
