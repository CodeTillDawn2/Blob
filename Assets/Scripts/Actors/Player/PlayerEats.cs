using System.Linq;
using UnityEngine;

public class PlayerEats : MonoBehaviour
{

    PlayerController playerBase;

    protected void Awake()
    {
        playerBase = GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        IAmEdible edible = other.gameObject.GetComponent<IAmEdible>();
        if (edible != null)
        {
            edible.BeingSuckedIn = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        foreach (ActorController edible in ActorController.Actors.OfType<IAmEdible>().Where(x => x.BeingEaten == false && x.BeingSuckedIn == false).ToList())
        {
            if (edible.Intersects)
            {
                IAmEdible interf = edible as IAmEdible;
                interf.BeingSuckedIn = true;
            }
        }
    }

    void FixedUpdate()
    {

    }

}
