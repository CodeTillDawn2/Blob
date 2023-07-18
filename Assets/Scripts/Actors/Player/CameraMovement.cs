using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [HideInInspector]
    public UnityEngine.Transform target;

    public float CameraDistance;
    public float CameraAngle;

    private void Start()
    {

        target = PlayerController.Player.transform;
    }

    void Update()
    {
        if (target != null)
        {

            float TrueCameraDistance = PlayerController.Player.CubeWidth * CameraDistance;


            Vector3 BackwardsVector = -target.forward;
            BackwardsVector.y = CameraAngle;
            transform.position = target.position + (BackwardsVector * TrueCameraDistance);

            //rotate us over time according to speed until we are in the required rotation
            transform.LookAt(target);
            //print("Camera Distance: " + TrueCameraDistance + " Camera Angle: " + CameraAngle + " Cube Width: " + player.CubeWidth );
        }

    }

}
