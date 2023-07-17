using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Tentacle
{
    public GameObject gameObject;
    public BoxCollider baseCollider;
    public Vector3 LocalOrigin;
    public Vector3 WorldOrigin;
    public Vector3 LocalColliderOrigin;
    public GameObject target;
    public bool HasPrey;

    public Tentacle(GameObject go, BoxCollider baseCol)
    {
        //try
        //{
        gameObject = go;
        baseCollider = baseCol;
        HasPrey = false;
        LocalColliderOrigin = baseCollider.gameObject.transform.localPosition;
        LocalOrigin = gameObject.transform.localPosition;
        WorldOrigin = gameObject.transform.position;
        target = PlayerController.Player.detection.FindEdibleBehind();
        //}
        //catch (Exception ex)
        //{
        //    string test = "";
        //}

    }


}

public class PlayerTentacles : MonoBehaviour
{
    public List<GameObject> TentacleObjects;
    public List<Tentacle> Tentacles;
    private Tentacle BackTentacle;

    public bool UseBackTentacle;


    private void TestBone()
    {




        if (BackTentacle.target != null && !BackTentacle.HasPrey)
        {
            BackTentacle.gameObject.transform.position = Vector3.MoveTowards(BackTentacle.gameObject.transform.position, BackTentacle.target.transform.position, 10 * Time.deltaTime);
            BackTentacle.baseCollider.enabled = true;



            BackTentacle.baseCollider.transform.position = Vector3.MoveTowards(BackTentacle.baseCollider.transform.position, BackTentacle.gameObject.transform.position, 500 * Time.deltaTime);
            //print(BackBone.baseCollider.sharedMesh.bounds);

        }
        else if (BackTentacle.target != null && BackTentacle.HasPrey)
        {
            BackTentacle.gameObject.transform.position = Vector3.MoveTowards(BackTentacle.gameObject.transform.position, PlayerController.Player.transform.position, 10 * Time.deltaTime);
            BackTentacle.target.gameObject.transform.position = Vector3.MoveTowards(BackTentacle.target.gameObject.transform.position, PlayerController.Player.transform.position, 10 * Time.deltaTime);
            BackTentacle.baseCollider.enabled = false;
        }
        else
        {
            BackTentacle.gameObject.transform.position = Vector3.MoveTowards(BackTentacle.gameObject.transform.position, PlayerController.Player.transform.position, 10 * Time.deltaTime);
            BackTentacle.baseCollider.enabled = false;
        }

        if (BackTentacle.target != null)
        {
            ActorController actor = BackTentacle.target.GetComponent<ActorController>();
            if (actor.Intersects)
            {
                BackTentacle.target = null;
                BackTentacle.HasPrey = false;
                UseBackTentacle = false;
            }
        }



    }

    //private void OnTriggerStay(Collider other)
    //{
    //    CheckForPrey(other.gameObject);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    CheckForPrey(other.gameObject);
    //}

    private void CheckForPrey(GameObject prey)
    {
        foreach (Tentacle tentacle in Tentacles)
        {
            if (prey == tentacle.target)
            {
                prey.transform.SetParent(tentacle.gameObject.transform);
                tentacle.HasPrey = true;
            }
        }
    }



    //// Start is called before the first frame update
    //void Start()
    //{
    //    UseBackTentacle = false;
    //    GameObject bone = TentacleObjects.Where(x => x.name == "Back").First();

    //    BackTentacle = new Tentacle(bone, bone.GetComponentInChildren<BoxCollider>());





    //    BackTentacle.baseCollider.enabled = true;
    //    Tentacles = new List<Tentacle>();
    //    Tentacles.Add(BackTentacle);


    //}


    // Update is called once per frame
    void Update()
    {
        //print("Back bone position: " + BackBone.gameObject.transform.position);
    }

    //private void FixedUpdate()
    //{
    //    if (UseBackTentacle)
    //    {
    //        foreach (Tentacle tentacle in Tentacles)
    //        {

    //            tentacle.target = PlayerController.Player.detection.FindEdibleBehind();


    //        }

    //        TestBone();
    //    }

    //}
}
