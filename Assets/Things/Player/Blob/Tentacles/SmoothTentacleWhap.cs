using UnityEngine;
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

        if (other.gameObject != null)
        {

            MomentumSensor moSensor = GetComponent<MomentumSensor>();
            if (moSensor != null)
            {
                ModifierLibrary.Tentacle.ApplyTentacleWhapModifer(other.gameObject, gameObject, 5f, moSensor.ReturnVector(), moSensor.ReturnSpeed());
            }



            //ModifierLibrary.OneTime.ApplyTentacleWhapModifer(other.gameObject,other.)

        }
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
