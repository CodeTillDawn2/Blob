using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle
{

    public GameObject target;
    public GameObject tentacleObject;
    public GameObject cubeSide;
    public bool HasPrey;
    public Vector3 AttachPosition;
    public Vector3 AttachOrientation;
    internal float tentacleWidth;



    public Tentacle(GameObject obj, GameObject cubeSide, Vector3 AttachPosition)
    {

        HasPrey = false;
        tentacleObject = obj;
        this.cubeSide = cubeSide;
        this.AttachPosition = AttachPosition;
        target = obj.GetComponent<TentacleController>().target;

    }



    public Rigidbody rb
    {
        get
        {
            Rigidbody returnRB = tentacleObject.GetComponentInChildren<Rigidbody>();
            return returnRB;
        }
    }



}