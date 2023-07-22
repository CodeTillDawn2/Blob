using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [HideInInspector]
    public UnityEngine.Transform target;

    [Header("Stat Block")]
    public float CameraDistance;
    public float CameraAngle;
    [Serialize] public FloatVariable CubeWidth;

    private void Start()
    {

        target = transform;
    }

    void Update()
    {
        if (target != null)
        {

            float TrueCameraDistance = CubeWidth.Value * CameraDistance;


            Vector3 BackwardsVector = -target.forward;
            BackwardsVector.y = CameraAngle;
            transform.position = target.position + (BackwardsVector * TrueCameraDistance);

            //rotate us over time according to speed until we are in the required rotation
            transform.LookAt(target);
            //print("Camera Distance: " + TrueCameraDistance + " Camera Angle: " + CameraAngle + " Cube Width: " + player.CubeWidth );
        }

    }

}
