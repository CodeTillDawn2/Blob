using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public PlayerDetection detection;

    [Header("Debug")]
    public List<MonoBehaviour> BeingEaten;
    public float CubeWidth { get; set; }
    public float CubeArea { get; set; }
    public float currentMoveSpeed;
    public float currentRotationSpeed;
    public float currentDigestDamage;
    public float MassTarget;
    [HideInInspector]
    public LineRenderer lineRenderer;


    [Header("Designer Setup")]
    public PlayerScriptableObject playerStats;
    public GameObject frontSide;
    public GameObject backSide;
    public GameObject leftSide;
    public GameObject rightSide;
    public GameObject topSide;
    public GameObject bottomSide;
    public GameObject PlayerObject;
    public GameObject PlayerEatingObject;
    public GameObject PlayerGameMesh;
    public GameObject TentacleNub;
    public GameObject TentaclePrefab;

    [HideInInspector]
    public Animator Animator;
    [HideInInspector]
    public PlayerTentacles Tentacles;

    [HideInInspector]
    public BoxCollider frontSideCollider;
    [HideInInspector]
    public BoxCollider backSideCollider;
    [HideInInspector]
    public BoxCollider leftSideCollider;
    [HideInInspector]
    public BoxCollider rightSideCollider;
    [HideInInspector]
    public BoxCollider topSideCollider;
    [HideInInspector]
    public BoxCollider bottomSideCollider;
    [HideInInspector]
    public BoxCollider PlayerObjectCollider;
    [HideInInspector]
    public BoxCollider PlayerEatingObjectCollider;
    [HideInInspector]
    public BoxCollider BoxTriggerObjectCollider;



    [HideInInspector]
    public float currentMassPerCubicFoot;
    [HideInInspector]
    public float currentAngularDragInsideStomach;
    [HideInInspector]
    public float currentDragInsideStomach;
    [HideInInspector]
    public float currentGrowthSpeedModifier;
    [HideInInspector]
    public float currentSuckSpeedModifier;
    [HideInInspector]
    public int currentTentacleSegments;

    private GameObject[] TentacleParts = new GameObject[10];

    //Static Variables
    public static PlayerController Player;

    //Unity Functions
    protected void Awake()
    {

        rb = GetComponent<Rigidbody>();
        detection = GetComponent<PlayerDetection>();
        lineRenderer = FindObjectOfType<Camera>().GetComponent<LineRenderer>();
        frontSideCollider = frontSide.GetComponent<BoxCollider>();
        backSideCollider = backSide.GetComponent<BoxCollider>();
        leftSideCollider = leftSide.GetComponent<BoxCollider>();
        rightSideCollider = rightSide.GetComponent<BoxCollider>();
        topSideCollider = topSide.GetComponent<BoxCollider>();
        bottomSideCollider = bottomSide.GetComponent<BoxCollider>();
        Animator = PlayerGameMesh.GetComponent<Animator>();
        Tentacles = GetComponent<PlayerTentacles>();
        Player = this;

    }

    protected void Start()
    {


        DebugTools.ClearLine();
        BeingEaten = new List<MonoBehaviour>();
        currentRotationSpeed = playerStats.RotateSpeed;
        currentMoveSpeed = playerStats.MoveSpeed;
        currentDigestDamage = playerStats.DigestDamage;
        currentMassPerCubicFoot = playerStats.MassPerCubicFoot;
        currentAngularDragInsideStomach = playerStats.AngularDragInsideStomach;
        currentDragInsideStomach = playerStats.DragInsideStomach;
        currentGrowthSpeedModifier = playerStats.GrowthSpeedModifier;
        currentSuckSpeedModifier = playerStats.SuckSpeedModifier;
        rb.mass = playerStats.Mass;
        MassTarget = playerStats.Mass;
        currentTentacleSegments = 0;
        ResetCubeWidth();

    }

    protected void Update()
    {

    }

    protected void FixedUpdate()
    {

        for (int i = 0; i < BeingEaten.Count; i++)
        {
            IAmEdible m = (IAmEdible)BeingEaten[i];
            m.BeEaten(currentDigestDamage);
        }

        for (int i = 0; i < ActorController.Actors.Count; i++)
        {
            ActorController a = (ActorController)ActorController.Actors[i];

            if (a.SqDistanceFromPlayer < CubeWidth * CubeWidth * 10)
            {
                IAmEdible m = (IAmEdible)ActorController.Actors[i];

                if (m.BeingSuckedIn)
                {
                    SuckIn(ActorController.Actors[i]);
                }
            }

        }

        if (rb.mass != MassTarget)
        {
            StartCoroutine(ChangeSize());
        }

        CheckIfBelowTerrain();
        //CheckTentacle();
    }

    private void CheckTentacle()
    {
        if (currentTentacleSegments < 1)
        {

            if (currentTentacleSegments == 0)
            {
                TentacleNub.SetActive(true);
                TentacleParts[0] = TentacleNub.transform.GetChild(0).gameObject;
                currentTentacleSegments += 1;
            }
            else
            {
                Vector3 previousScale = TentacleNub.transform.localScale;
                TentacleNub.transform.localScale = Vector3.one;
                float zJointOffset = -.01f;
                float zTentacleOffset = zJointOffset - .001f;


                GameObject emptyFather = new GameObject();
                
                emptyFather.transform.position = TentacleParts[currentTentacleSegments - 1].transform.position;
                emptyFather.transform.position += new Vector3(0, 0, zJointOffset);
                emptyFather.transform.SetParent(TentacleParts[currentTentacleSegments - 1].transform);
                emptyFather.transform.localScale = Vector3.one;
                GameObject newTentacle = Instantiate(TentaclePrefab, emptyFather.transform.position, Quaternion.identity);
                TentacleParts[currentTentacleSegments] = newTentacle.transform.GetChild(0).gameObject;
                newTentacle.transform.localScale = Vector3.one;

                newTentacle.transform.position += new Vector3(0, 0, zTentacleOffset);
                
                
                HingeJoint hingeJoint = TentacleParts[currentTentacleSegments].AddComponent<HingeJoint>();
                hingeJoint.autoConfigureConnectedAnchor = false;
                hingeJoint.connectedBody = TentacleParts[currentTentacleSegments-1].GetComponent<Rigidbody>();
                hingeJoint.anchor = (TentacleParts[currentTentacleSegments - 1].transform.position - TentacleParts[currentTentacleSegments].transform.position)  * 10;
                hingeJoint.connectedAnchor = TentacleParts[currentTentacleSegments-1].transform.position - TentacleParts[currentTentacleSegments].transform.position;

                hingeJoint.axis = new Vector3(1, 0, 0);
                hingeJoint.useLimits = true;
                JointLimits limits = hingeJoint.limits;
                limits.min = -40;
                limits.max = 40;
                limits.bounciness = 0;
                limits.bounceMinVelocity = 0;

                hingeJoint.limits = limits;
                newTentacle.transform.SetParent(emptyFather.transform);
                newTentacle.transform.localScale = Vector3.one;

                //HingeJoint hingeJoint = newTentacle.AddComponent<HingeJoint>();
                //hingeJoint.connectedBody = TentacleParts[currentTentacleSegments-1].GetComponent<Rigidbody>();


                currentTentacleSegments += 1;
                TentacleNub.transform.localScale = previousScale;
            }

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        string test = "";
    }

    private void CheckIfBelowTerrain()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);


        int layer = 3;

        int layerMask = 1 << layer;   // means take 1 and rotate it left by "layer" bit positions

        //If ground isn't below, look above and teleport
        Vector3 RayTraceStart = transform.position;
        //Vector3 RayTraceStart = transform.position - transform.rotation * new Vector3(0, topSideCollider.size.x * transform.localScale.x / 2, 0);

        if (!Physics.Raycast(RayTraceStart, down, out RaycastHit hit, 10, layerMask)) //Ground found
        {
            Vector3 up = transform.TransformDirection(Vector3.up);
            if (Physics.Raycast(RayTraceStart, up, out RaycastHit hit2, 10, layerMask)) //Ground found
            {
                transform.position += new Vector3(0, hit2.distance, 0);
            }
        }

    }

    private void SuckIn(ActorController victim)
    {
        victim.transform.position = Vector3.Slerp(victim.transform.position, transform.position + new Vector3(0,PlayerController.Player.CubeWidth * .75f,0), Time.deltaTime * currentSuckSpeedModifier);
    }

    private float ResetCubeWidth()
    {
        CubeWidth = topSideCollider.size.x * transform.localScale.x;
        CubeArea = CubeWidth * CubeWidth * CubeWidth;
        return CubeWidth;

    }


    public void ChangeMass(float mass)
    {
        if (mass < 0)
        {
            string test = "";
        }
        MassTarget += mass;
    }

    private IEnumerator ChangeSize()
    {
        float MassDifferential;

        float TargetVolume = MassTarget / currentMassPerCubicFoot;

        float TargetSideSize = (float)Math.Pow(TargetVolume, 1.0 / 3.0);

        float ratio = TargetSideSize / CubeWidth;

        if (MassTarget > rb.mass)
        {
            MassDifferential = ratio - 1;
        }
        else
        {
            MassDifferential = (rb.mass / MassTarget) - 1;
        }

        if (MassDifferential > 0.001)
        {

            if (MassDifferential < .01)
            {

                UnparentEdibles();
                transform.localScale = new Vector3(transform.localScale.x * ratio, transform.localScale.y * ratio, transform.localScale.z * ratio);

                ParentEdibles();
                print("Set mas to target " + MassTarget);
                rb.mass = MassTarget;
                ResetCubeWidth();
                yield return null;
            }
            else
            {
                UnparentEdibles();
                float TargetScale = transform.localScale.x * ratio;
                transform.localScale += new Vector3(TargetScale - transform.localScale.x, TargetScale - transform.localScale.y, TargetScale - transform.localScale.z) * Time.deltaTime * currentGrowthSpeedModifier;
                ParentEdibles();

                float CubeWidth = ResetCubeWidth();
                print("Set mass to " + CubeWidth * CubeWidth * CubeWidth * currentMassPerCubicFoot);
                rb.mass = CubeWidth * CubeWidth * CubeWidth * currentMassPerCubicFoot;
                yield return new WaitForEndOfFrame();

            }




        }

    }

    private void UnparentEdibles()
    {
        foreach (MonoBehaviour edible in BeingEaten)
        {
            edible.transform.parent = null;
        }
    }

    private void ParentEdibles()
    {
        foreach (MonoBehaviour edible in BeingEaten)
        {
            edible.transform.parent = PlayerEatingObject.transform;
        }
    }

}
