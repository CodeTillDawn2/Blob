using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public PlayerBrain Brain;

    [Header("Debug")]
    public List<MonoBehaviour> BeingEaten;
    public float CubeWidth { get; set; }
    public float CubeHalfExtents {
        get { return CubeWidth * .449f; }  
                                }
    public Vector3 Vector3_CubeHalfExtents
    {
        get { return new Vector3(CubeWidth * .449f, CubeWidth * .449f, CubeWidth * .449f); }
    }
    public Vector3 Vector3_Cube
    {
        get { return new Vector3(CubeWidth, CubeWidth, CubeWidth); }
    }

    [Header("Stat Block")]
    public float MassTarget;
    public float CubeArea;
    public float currentMoveSpeed;
    public float currentRotationSpeed;
    public float currentDigestDamage;
    public float currentTentacleReach;
    public int currentMaxTentacles;
    public int currentTentacles;
    public float currentSightDistance;
    
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

    [HideInInspector]
    public Animator Animator;
    [HideInInspector]
    public PlayerTentacles Tentacles;

    [HideInInspector]
    public BoxCollider collider_FrontSide;
    [HideInInspector]
    public BoxCollider collider_BackSide;
    [HideInInspector]
    public BoxCollider collider_LeftSide;
    [HideInInspector]
    public BoxCollider collider_RightSide;
    [HideInInspector]
    public BoxCollider collider_TopSide;
    [HideInInspector]
    public BoxCollider collider_BottomSide;
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


    //Static Variables
    public static PlayerController me;

    //Unity Functions
    protected void Awake()
    {

        rb = GetComponent<Rigidbody>();
        Brain = GetComponent<PlayerBrain>();
        lineRenderer = FindObjectOfType<Camera>().GetComponent<LineRenderer>();
        collider_FrontSide = frontSide.GetComponent<BoxCollider>();
        collider_BackSide = backSide.GetComponent<BoxCollider>();
        collider_LeftSide = leftSide.GetComponent<BoxCollider>();
        collider_RightSide = rightSide.GetComponent<BoxCollider>();
        collider_TopSide = topSide.GetComponent<BoxCollider>();
        collider_BottomSide = bottomSide.GetComponent<BoxCollider>();
        Animator = PlayerGameMesh.GetComponent<Animator>();
        Tentacles = GetComponent<PlayerTentacles>();
        me = this;

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
        currentTentacleReach = playerStats.TentacleReach;
        currentMaxTentacles = playerStats.MaxTentacles;
        currentSightDistance = playerStats.SightDistance;
        rb.mass = playerStats.Mass;
        MassTarget = playerStats.Mass;
        currentTentacles = 0;
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

    }

    private void CheckIfBelowTerrain()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);


        int layer = 3;

        int layerMask = 1 << layer;   // means take 1 and rotate it left by "layer" bit positions

        //If ground isn't below, look above and teleport
        Vector3 RayTraceStart = transform.position;
        //Vector3 RayTraceStart = transform.position - transform.rotation * new Vector3(0, topSideCollider.size.x * transform.localScale.x / 2, 0);

        if (!Physics.Raycast(RayTraceStart, down, out RaycastHit hit, 10, (int)PlayerBrain.LayerMasks.LayerMask_GroundOnly)) //Ground found
        {
            Vector3 up = transform.TransformDirection(Vector3.up);
            if (Physics.Raycast(RayTraceStart, up, out RaycastHit hit2, 10, (int)PlayerBrain.LayerMasks.LayerMask_GroundOnly)) //Ground found
            {
                transform.position += new Vector3(0, hit2.distance, 0);
            }
        }

    }

    private void SuckIn(ActorController victim)
    {
        victim.transform.position = Vector3.Slerp(victim.transform.position, 
            transform.position + new Vector3(0,PlayerController.me.CubeWidth * .5f,0), Time.deltaTime * currentSuckSpeedModifier);
    }

    private float ResetCubeWidth()
    {
        CubeWidth = collider_TopSide.size.x * transform.localScale.x;
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
