using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Shortcuts;
using static UnityEngine.GraphicsBuffer;

public class BlobTentacles : MonoBehaviour
{

    [Header("Stat Block")]
    [Serialize] public IntegerVariable TentacleCount;
    [Serialize] public IntegerVariable CurrentMaxTentacles;
    [Serialize] public GameObjectVariable parentRB;
    [Serialize] public FloatVariable MaxTentacleReach;
    [Serialize] public FloatVariable MinTentacleReach;
    [Serialize] public Vector3Variable BlobDims;
    [Serialize] public PlayerStatsBase StartingStats;
    [Serialize] public Dict_GameObjectToLastSeen ObjectsSeen;

    public GameObject TentaclePrefab;

    private int TentacleID = 1;

    private List<Bounds> TentacleSpawnRegions;

    private List<SmoothTentacle> existingTentacles;

    public delegate string TentacleCreatedAction(object sender, EventArgs args);
    public static event TentacleCreatedAction TentacleCreatedEvent;
    private void OnTentacleCreatedEvent(EventArgs e)
    {
        if (TentacleCreatedEvent != null)
        {
            string myString = TentacleCreatedEvent(this, e);
        }


    }

    private void OnEnable()
    {
        TentacleCount.Value = 0;
    }

    private void Start()
    {
        MaxTentacleReach.Value = StartingStats.MaxTentacleReach;
        MinTentacleReach.Value = StartingStats.MinTentacleReach;
        CurrentMaxTentacles.Value = StartingStats.MaxTentacles;
        SetTentacleSpawnRegions();

    }

    private void SetTentacleSpawnRegions()
    {
        TentacleSpawnRegions = new List<Bounds>();
        foreach (GameObject side in new List<GameObject>() { FrontSide, BackSide, LeftSide, RightSide })
        {
            BoxCollider boxcol = side.GetComponent<BoxCollider>(); 
            if (boxcol != null)
            {

                Bounds bound = new Bounds(transform.position + new Vector3(0,BlobDims.Value.y / 2f, 0), boxcol.bounds.size);
                TentacleSpawnRegions.Add(boxcol.bounds);


            }
        }



    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CreateTentacles();
        //MoveTentacleTarget();
    }




    //public bool WithinTentacleReach(ActorController actor)
    //{
    //    return CurrentTentacleReach.Value * CurrentTentacleReach.Value >= actor.SqDistanceFromPlayer;
    //}

    public GameObject FrontSide;
    public GameObject BackSide;
    public GameObject LeftSide;
    public GameObject RightSide;
    public GameObject TopSide;
    public GameObject BottomSide;

    private Vector3 FindRotationFromCubeSide(GameObject cubeSide)
    {
        if (cubeSide == FrontSide)
        {
            return parentRB.Value.transform.forward;
        }
        else if (cubeSide == BackSide)
        {
            return -parentRB.Value.transform.forward;
        }
        else if (cubeSide == RightSide)
        {
            return parentRB.Value.transform.right;
        }
        else //Left side
        {
            return -parentRB.Value.transform.right;
        }
    }

    public GameObject FindClosestCubeSide(GameObject target)
    {

        Vector3 _direction = (target.transform.position - transform.position).normalized;

        float FrontSideDot = Vector3.Dot(_direction, transform.forward);
        float RightSideDot = Vector3.Dot(_direction, transform.right);

        if (Math.Abs(FrontSideDot) > Math.Abs(RightSideDot))
        {
            if (FrontSideDot > 0)
            {
                return FrontSide;
            }
            else
            {
                return BackSide;
            }
        }
        else
        {
            if (RightSideDot > 0)
            {
                return RightSide;
            }
            else
            {
                return LeftSide;
            }
        }
    }


    public void CreateTentacles()
    {

        List<GameObject> potentialvictims = ObjectsSeen.Value.Keys.Where(x => x != null).ToList();
        for (int i = potentialvictims.Count - 1; i >= 0; i--)
        {
            if (potentialvictims[i] != null)
            {
                GameObject potential = potentialvictims[i];
                float targetSq = (potential.transform.position - gameObject.transform.position).sqrMagnitude;
                float TentacleReach = MaxTentacleReach.Value * MaxTentacleReach.Value;
                float MinReach = MinTentacleReach.Value * MinTentacleReach.Value;

                if (targetSq > TentacleReach || targetSq < MinReach)
                {
                    potentialvictims.Remove(potential);
                }
            }

        }

        existingTentacles = GetComponentsInChildren<SmoothTentacle>().ToList();


        //Existing tentacles
        //for (int tentacleID = existingTentacles.Count() - 1; tentacleID >= 0; tentacleID--)
        //{

        //    SmoothTentacle tentacle = existingTentacles[tentacleID];
        //    if (tentacle.IsAlive)
        //    {
        //        GameObject existingTarget = tentacle.target;
        //        tentacle.tentacleOrientation = FindRotationFromCubeSide(tentacle.cubeSide);

        //        float dot = Vector3.Dot((existingTarget.transform.position - transform.position).normalized,
        //            tentacle.tentacleOrientation);

        //        if (dot < 0 || (tentacleID > CurrentMaxTentacles.Value - 1 && existingTarget == null))
        //        {
        //            DespawnTentacle(existingTentacles, tentacle);
        //            continue;
        //        }
        //        if (!potentialvictims.Contains(existingTarget))
        //        {

        //            //if (TryTargetNewVictims(potentialvictims, out GameObject newTentacle))
        //            //{
        //            //    SmoothTentacle t = newTentacle.GetComponent<SmoothTentacle>();
        //            //    existingTentacles.Add(t);


        //            //}
        //            //else
        //            //{
        //            DespawnTentacle(existingTentacles, tentacle);
        //            continue;
        //            //}
        //        }
        //    }

        //}

        //Remove excess
        for (int i = CurrentMaxTentacles.Value; i < existingTentacles.Count(); i++)
        {
            DespawnTentacle(existingTentacles, existingTentacles[i]);
        }


        //List<GameObject> existingTargets = existingTentacles.Select(x => x.).ToList();
        //New Tentacles

        if (potentialvictims.Count > existingTentacles.Count())
        {

        }
        for (int tentacleID = existingTentacles.Count(); tentacleID < CurrentMaxTentacles.Value && tentacleID < potentialvictims.Count(); tentacleID++)
        {
      
            CreateTentacle(tentacleID);

        //    List<GameObject> ActualPotentialVictims = potentialvictims.Where(x => !existingTargets.Contains(x)).ToList();
        //    if (ActualPotentialVictims.Count() > 0)
        //    {
        //        if (TryCreateTentacle(ActualPotentialVictims, tentacleID,  out GameObject newTentacle))
        //        {
        //            SmoothTentacle tentacle = newTentacle.GetComponent<SmoothTentacle>();
        //            existingTentacles.Add(tentacle);
        //            existingTargets.Add(tentacle.target);
        //            if (existingTentacles.Count() > 1)
        //            {
        //                string test = "";
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //        tentacleID++;
        //    }
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        string test = "";
    }

    
    private void CreateTentacle(int TentacleID)
    {

        GameObject tentacleobj = Instantiate(TentaclePrefab, parentRB.Value.transform.position + new Vector3(0,-1000,0), Quaternion.identity);
        SmoothTentacle smoothTentacle = tentacleobj.GetComponent<SmoothTentacle>();
        if (smoothTentacle != null)
        {
            existingTentacles.Add(smoothTentacle);
            tentacleobj.name = "Tentacle" + UnityEngine.Random.Range(1, 100); // TentacleID;
            Rigidbody rb = parentRB.Value.GetComponent<Rigidbody>();
            smoothTentacle.Go(gameObject, rb, TentacleSpawnRegions, ObjectsSeen);
        }
        else
        {
            Destroy(tentacleobj);
        }


        //Vector3 TentacleOrientation = Vector3.forward;
        //bool Success = false;
        //int Attempts = 0;

        //    GameObject CubeSide = FindClosestCubeSide(target);
        //if (CubeSide != null)
        //{
        //    TentacleOrientation = FindRotationFromCubeSide(CubeSide);
        //    float dot = Vector3.Dot((target.transform.position - transform.position).normalized,
        //    TentacleOrientation);

        //    if (dot > 0)
        //    {

        //        BoxCollider cubeSideCollider = CubeSide.GetComponent<BoxCollider>();


        //        float SideMargin = .8f; //Space on side of blob minus space to not spawn tentacle in
        //        float Inset = (1 - SideMargin) / 2f; //Space on side of blob minus space to not spawn tentacle in

        //        while (!Success && Attempts < 15)
        //        {
        //            Attempts++;
        //            Vector3 randomPoint = new Vector3(
        //                UnityEngine.Random.Range(0f, cubeSideCollider.size.x * SideMargin) - cubeSideCollider.size.x / 2f + cubeSideCollider.size.x * 1.5f * Inset,
        //               UnityEngine.Random.Range(0f, cubeSideCollider.size.y * SideMargin) - cubeSideCollider.size.y / 2f + cubeSideCollider.size.y * Inset,
        //               UnityEngine.Random.Range(0f, cubeSideCollider.size.z * SideMargin) - cubeSideCollider.size.z / 2f + cubeSideCollider.size.z * Inset);

        //            randomPoint = cubeSideCollider.transform.TransformPoint(randomPoint);

        //            RaycastHit? hit = PhysicsTools.RaycastAt(GetMidPointOfObject(target), randomPoint, Mathf.Infinity, TentaclePositionRaycast);
        //            if (hit != null && hit.Value.collider.gameObject.layer != (int)UnityLayers.PlayerTentacle)
        //            {
        //                Quaternion rotationToSpawn;
        //                Vector3 locationToSpawn = hit.Value.point;
        //                TentacleOrientation = FindRotationFromCubeSide(CubeSide);
        //                if (CubeSide == FrontSide)
        //                {
        //                    rotationToSpawn = Quaternion.LookRotation(RigidBodyObject.Value.transform.forward);
        //                }
        //                else if (CubeSide == BackSide)
        //                {
        //                    rotationToSpawn = Quaternion.LookRotation(-RigidBodyObject.Value.transform.forward);
        //                }
        //                else if (CubeSide == RightSide)
        //                {
        //                    rotationToSpawn = Quaternion.LookRotation(RigidBodyObject.Value.transform.right);
        //                }
        //                else //Left side
        //                {
        //                    rotationToSpawn = Quaternion.LookRotation(-RigidBodyObject.Value.transform.right);
        //                }

        //                GameObject tentacleobj = Instantiate(TentaclePrefab, locationToSpawn, Quaternion.identity * rotationToSpawn);
        //                SmoothTentacle smoothTentacle = tentacleobj.GetComponent<SmoothTentacle>();
        //                if (smoothTentacle != null)
        //                {
        //                    if (TentacleID > 1)
        //                    {
        //                        string test = "";
        //                    }
        //                    tentacleobj.name = "Tentacle" + TentacleID;
        //                    smoothTentacle.name = "Tentacle" + TentacleID;
        //                    smoothTentacle.target = target;
        //                    smoothTentacle.cubeSide = CubeSide;
        //                    smoothTentacle.tentacleOrientation = TentacleOrientation;
        //                    tentacleobj.transform.SetParent(transform, true);
        //                    tentacle = tentacleobj;
        //                    Success = true;
        //                    break;
        //                }




        //            }
        //        }

        //    }


        //}
            

        

        //return Success;
    }

    private Vector3 GetMidPointOfObject(GameObject target)
    {
        Vector3 targetCenter;
        Renderer renderer = target.GetComponent<Renderer>();

        if (renderer != null)
        {
            targetCenter = renderer.bounds.center;

            return targetCenter;

        }
        Vector3 sumVector = new Vector3(0f, 0f, 0f);

        foreach (Transform child in target.transform)
        {
            sumVector += child.position;
        }

        targetCenter = sumVector / target.transform.childCount;
        return targetCenter;
    }

    private void DespawnTentacle(List<SmoothTentacle> tentacles, SmoothTentacle tentacle)
    {

        tentacle.IsAlive.Value = false;
        tentacles.Remove(tentacle);
        Destroy(tentacle.gameObject);
    }



    //private bool TryTargetNewVictims(List<GameObject> potentialvictims, int TentacleID, out GameObject tentacle)
    //{
    //    Vector3 TentacleOrientation = Vector3.forward;
    //    tentacle = null;
    //    bool Success = false;
    //    int Attempts = 0;
    //    foreach (GameObject target in potentialvictims)
    //    {


    //        GameObject CubeSide = FindClosestCubeSide(target);
    //        TentacleOrientation = FindRotationFromCubeSide(CubeSide);
    //        float dot = Vector3.Dot((target.transform.position - transform.position).normalized,
    //        TentacleOrientation);

    //        if (CubeSide != null && dot > 0)
    //        {

    //            BoxCollider cubeSideCollider = CubeSide.GetComponent<BoxCollider>();


    //            float SideMargin = .8f; //Space on side of blob minus space to not spawn tentacle in
    //            float Inset = (1 - SideMargin) / 2f; //Space on side of blob minus space to not spawn tentacle in

    //            while (!Success && Attempts < 15)
    //            {
    //                Attempts++;
    //                Vector3 randomPoint = new Vector3(
    //                    UnityEngine.Random.Range(0f, cubeSideCollider.size.x * SideMargin) - cubeSideCollider.size.x / 2f + cubeSideCollider.size.x * 1.5f * Inset,
    //                   UnityEngine.Random.Range(0f, cubeSideCollider.size.y * SideMargin) - cubeSideCollider.size.y / 2f + cubeSideCollider.size.y * Inset,
    //                   UnityEngine.Random.Range(0f, cubeSideCollider.size.z * SideMargin) - cubeSideCollider.size.z / 2f + cubeSideCollider.size.z * Inset);

    //                randomPoint = cubeSideCollider.transform.TransformPoint(randomPoint);

    //                RaycastHit? hit = PhysicsTools.RaycastAt(GetMidPointOfObject(target), randomPoint, Mathf.Infinity, TentaclePositionRaycast);
    //                if (hit != null && hit.Value.collider.gameObject.layer != (int)UnityLayers.PlayerTentacle)
    //                {
    //                    Quaternion rotationToSpawn;
    //                    Vector3 locationToSpawn = hit.Value.point;
    //                    TentacleOrientation = FindRotationFromCubeSide(CubeSide);
    //                    if (CubeSide == FrontSide)
    //                    {
    //                        rotationToSpawn = Quaternion.LookRotation(RigidBodyObject.Value.transform.forward);
    //                    }
    //                    else if (CubeSide == BackSide)
    //                    {
    //                        rotationToSpawn = Quaternion.LookRotation(-RigidBodyObject.Value.transform.forward);
    //                    }
    //                    else if (CubeSide == RightSide)
    //                    {
    //                        rotationToSpawn = Quaternion.LookRotation(RigidBodyObject.Value.transform.right);
    //                    }
    //                    else //Left side
    //                    {
    //                        rotationToSpawn = Quaternion.LookRotation(-RigidBodyObject.Value.transform.right);
    //                    }

    //                    GameObject tentacleobj = Instantiate(TentaclePrefab, locationToSpawn, Quaternion.identity * rotationToSpawn);
    //                    SmoothTentacle smoothTentacle = tentacleobj.GetComponent<SmoothTentacle>();
    //                    if (smoothTentacle != null)
    //                    {
    //                        if (TentacleID > 1)
    //                        {
    //                            string test = "";
    //                        }
    //                        tentacleobj.name = "Tentacle" + TentacleID;
    //                        smoothTentacle.name = "Tentacle" + TentacleID;
    //                        smoothTentacle.target = target;
    //                        smoothTentacle.cubeSide = CubeSide;
    //                        smoothTentacle.tentacleOrientation = TentacleOrientation;
    //                        tentacleobj.transform.SetParent(transform, true);
    //                        tentacle = tentacleobj;
    //                        Success = true;
    //                        break;
    //                    }




    //                }
    //            }

    //        }

    //        if (Success) break;

    //    }

    //    return Success;
    //}


}
