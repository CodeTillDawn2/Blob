using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static HelperClasses;
using static PlayerBrain;

public class Tentacle
{
    public GameObject target;
    public GameObject tentacleObject;
    public bool HasPrey;

    public Tentacle(GameObject obj)
    {

        HasPrey = false;
        tentacleObject = obj;

    }

    

}

public class PlayerTentacles : MonoBehaviour
{
    [HideInInspector]
    public List<Tentacle> Tentacles;
    public GameObject TentaclePrefab;

    public int currentTentacles
    {
        get { return PlayerController.me.currentTentacles; }
        set { PlayerController.me.currentTentacles = value; }
    }

    public int currentMaxTentacles
    {
        get { return PlayerController.me.currentMaxTentacles; }
        set { PlayerController.me.currentMaxTentacles = value; }
    }

    public bool WithinTentacleReach(ActorController actor)
    {
        return PlayerController.me.currentTentacleReach * PlayerController.me.currentTentacleReach >= actor.SqDistanceFromPlayer;
    }

    public void CreateTentacles()
    {
        List<ActorController> victims = ActorController.Actors.Where(x => x.IsSeenByPlayer && !x.TargetedByTentacle && WithinTentacleReach(x)).OrderBy(x => x.SqDistanceFromPlayer).ToList();

        while (currentTentacles < currentMaxTentacles)
        {

            ActorController victim = victims.Where(x => x.IsSeenByPlayer && !x.TargetedByTentacle).OrderBy(x => x.SqDistanceFromPlayer).FirstOrDefault();
            if (victim != null)
            {

                //Victim location
                RaycastHit? raycast = PhysicsTools.RaycastAt(victim.gameObject.transform.position, PlayerController.me.transform.position, (int)LayerMasks.LayerMask_PlayerOnly);


                Vector3 location = raycast.Value.point;
                //Vector3 location = PlayerController.me.transform.position + new Vector3(PlayerController.me.CubeWidth * .5f, .5f, 0);
                Quaternion rotation = Quaternion.identity * Quaternion.Euler(0, 90, 0);
                LocRoc locRoc = new LocRoc(location, rotation);
                GameObject tentacleobj = Instantiate(TentaclePrefab, locRoc.location, locRoc.rotation);
                tentacleobj.transform.SetParent(PlayerController.me.transform);
                Tentacle tentacle = new Tentacle(tentacleobj);
                FixedJoint joint = PlayerController.me.rb.gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = raycast.Value.collider.gameObject.GetComponent<Rigidbody>();
                victim.TargetedByTentacle = true;
                tentacle.target = victim.gameObject;
                currentTentacles++;
            }
            else
            {
                break;
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        CreateTentacles();





    }

    private void FixedUpdate()
    {
 
    }

}
