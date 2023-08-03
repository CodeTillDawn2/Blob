using UnityEngine;

public class MomentumSensor : MonoBehaviour
{

    Vector3 oldPosition;
    Vector3 newPosition;



    // Update is called once per frame
    void FixedUpdate()
    {

        if (oldPosition == null)
        {
            oldPosition = transform.position;
        }

        newPosition = transform.position;

    }

    public Vector3 ReturnVector()
    {
        return (newPosition - oldPosition).normalized;
    }

    public float ReturnSpeed()
    {
        return (newPosition - oldPosition).magnitude;
    }

    public float ReturnSpeedSq()
    {
        return (newPosition - oldPosition).sqrMagnitude;
    }

}
