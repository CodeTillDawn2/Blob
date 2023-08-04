using UnityEngine;
using UnityEngine.InputSystem;
using static Shortcuts;

[RequireComponent(typeof(MomentumSensor))]
public class SmoothTentacleWhap : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        Rigidbody body = other.attachedRigidbody;
        MomentumSensor momentumSensor = GetComponent<MomentumSensor>();
        MomentumSensor otherMomentumSensor = other.GetComponent<MomentumSensor>();

        if (body != null && !body.isKinematic && momentumSensor != null && otherMomentumSensor != null)
        {
            float averageAcceleration = momentumSensor.ReturnAverageAccelerationSq();

            Vector3 relativeVelocity = otherMomentumSensor.ReturnVector() - momentumSensor.ReturnVector();
            Vector3 contactNormal = relativeVelocity.normalized;

            Rigidbody thisBody = GetComponentInParent<Rigidbody>();
            float relativeMass = (thisBody.mass * body.mass) / (thisBody.mass + body.mass);

            // Convert acceleration to force
            Vector3 force = averageAcceleration * relativeMass * contactNormal * .01f;

            // Approximate the point of contact
            Vector3 estimatedPointOfContact = (other.bounds.center + GetComponent<Collider>().bounds.center) / 2;

            ModifierLibrary.Tentacle.ApplyTentacleWhapModifer(other.gameObject, estimatedPointOfContact,
            force);
        }



        //    if (other.gameObject != null)
        //{

        //    MomentumSensor moSensor = GetComponent<MomentumSensor>();
        //    if (moSensor != null)
        //    {
        //        Vector3 estimatedPointOfContact = (other.bounds.center + GetComponent<Collider>().bounds.center) / 2;

        //        ModifierLibrary.Tentacle.ApplyTentacleWhapModifer(other.gameObject, estimatedPointOfContact, 
        //            moSensor.ReturnVector(), moSensor.ReturnAverageAcceleration());
        //        print(moSensor.ReturnAverageAcceleration());
        //    }



        //    //ModifierLibrary.OneTime.ApplyTentacleWhapModifer(other.gameObject,other.)

        //}
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer != (int)UnityLayers.Player && collision.collider.gameObject.layer != (int)UnityLayers.PlayerTentacle
            && collision.collider.gameObject.layer != (int)UnityLayers.Ground && collision.collider.gameObject.layer != (int)UnityLayers.Water)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.thisCollider.gameObject.name == "TentacleCollider")
                {


                    print("Tentacle whap! " + contact.impulse);
                }
            }


        }

    }
}
