using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{


    [Header("Stat Block")]
    public GameObject target;
    public float CameraDistance;
    public float CameraAngle;
    [Serialize] public FloatVariable CubeWidth;

    private void Start()
    {

    }

    void Update()
    {
        if (target != null)
        {

            float TrueCameraDistance = CubeWidth.Value * CameraDistance;


            Vector3 BackwardsVector = -target.transform.forward;
            BackwardsVector.y = CameraAngle;
            transform.position = target.transform.position + (BackwardsVector * TrueCameraDistance);

            //rotate us over time according to speed until we are in the required rotation
            transform.LookAt(target.transform);
            //print("Camera Distance: " + TrueCameraDistance + " Camera Angle: " + CameraAngle + " Cube Width: " + player.CubeWidth );
        }

    }

}
