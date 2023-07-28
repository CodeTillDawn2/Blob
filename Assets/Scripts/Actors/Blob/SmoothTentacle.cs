using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmoothTentacle : MonoBehaviour
{
    [Serialize] public GameObject targetBall;

    public string name;
    public GameObject target;
    public bool IsAlive = true;
    private bool IsReady = false;

    private List<Vector3> tentacleRegions = new List<Vector3>();
 
    private Rigidbody parentRB;
    private Dict_GameObjectToLastSeen ObjectsSeen;


    public void Begin(Rigidbody parentRB, List<Vector3> tentacleRegions, Dict_GameObjectToLastSeen ObjectsSeen)
    {
        this.parentRB = parentRB;
        this.tentacleRegions = tentacleRegions;
        this.ObjectsSeen = ObjectsSeen;
        if (parentRB != null && tentacleRegions.Count >= 1 && ObjectsSeen != null)
        {
            IsReady = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsReady)
        {
            Retarget();
        }
    }



    private void Retarget()
    {
        Vector3 TentacleRotation = FindTentacleRotation(target);
        float dot = Vector3.Dot((target.transform.position - transform.position).normalized,
            TentacleRotation);

        if (dot > 0)
        {
            float SideMargin = .8f; //Space on side of blob minus space to not spawn tentacle in
            float Inset = (1 - SideMargin) / 2f; //Space on side of blob minus space to not spawn tentacle in


        }

    }
    public Vector3 FindTentacleRotation(GameObject target)
    {

        Vector3 _direction = (target.transform.position - transform.position).normalized;

        float FrontSideDot = Vector3.Dot(_direction, transform.forward);
        float RightSideDot = Vector3.Dot(_direction, transform.right);

        if (Math.Abs(FrontSideDot) > Math.Abs(RightSideDot))
        {
            if (FrontSideDot > 0)
            {
                return parentRB.transform.forward;
            }
            else
            {
                return -parentRB.transform.forward;
            }
        }
        else
        {
            if (RightSideDot > 0)
            {
                return parentRB.transform.right;
            }
            else
            {
                return -parentRB.transform.right;
            }
        }
    }

   
}
