using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStomach : MonoBehaviour
{

    [Serialize] public GameEvent OnEnemyEnveloped;
    [Serialize] public GameEvent OnEnemyContact;


    protected void Awake()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        IAmEdible edible = other.gameObject.GetComponent<IAmEdible>();
        if (edible != null)
        {
            edible.BeingSuckedIn = true;
        }
    }
    //public void SuckInIntersectingEnemies()
    //{
    //    foreach (ActorController edible in ActorController.Actors.OfType<IAmEdible>().Where(x => x.BeingEaten == false && x.BeingSuckedIn == false).ToList())
    //    {
    //        if (edible.Intersects)
    //        {
    //            IAmEdible interf = edible as IAmEdible;
    //            interf.BeingSuckedIn = true;
    //        }
    //    }
    //}

    // Update is called once per frame
    void Update()
    {

    }



    void FixedUpdate()
    {

    }

}
