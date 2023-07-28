using UnityEngine;
using static Shortcuts;

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
            if (other.gameObject.layer == (int)UnityLayers.PlayerTentacle)
            {
                string test = other.gameObject.name;
                ModifierLibrary.OneTime.ApplyTentacleWhapModifer(gameObject, other.gameObject, 10f, Vector3.up, 10f);
                //ModifierLibrary.OneTime.ApplyTentacleWhapModifer(other.gameObject,other.)
            }
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
