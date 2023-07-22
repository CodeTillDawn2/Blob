using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ActorController : MonoBehaviour
{

    [Header("Stat Block")]
    [Serialize] public FloatVariable CubeWidth;


    [HideInInspector]
    public float StartingMass;
    [HideInInspector]
    public Collider myCollider;
    [HideInInspector]
    public Rigidbody rb;
    public abstract float SqDistanceFromPlayer { get; set; }

    public Int32 FramesSinceDeclip;

    public bool IsSeenByPlayer = false;

    public bool TargetedByTentacle = false;



    // every instance registers to and removes itself from here
    private static List<ActorController> _actors = new List<ActorController>();

    public static List<ActorController> Actors
    {
        get
        {
            return _actors;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        _actors = new List<ActorController>();
    }


    protected virtual void Awake()
    {
        myCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

    }

    protected virtual void Start()
    {
        _actors.Add(this);
        StartingMass = rb.mass;
        //FramesSinceDeclip = 0;
        //collider_TopSide = topSide.GetComponent<BoxCollider>();
        //collider_BottomSide = bottomSide.GetComponent<BoxCollider>();
        //collider_LeftSide = leftSide.GetComponent<BoxCollider>();
        //collider_RightSide = rightSide.GetComponent<BoxCollider>();
        //collider_FrontSide = frontSide.GetComponent<BoxCollider>();
        //collider_BackSide = backSide.GetComponent<BoxCollider>();

    }


    private void OnDestroy()
    {
        _actors.Remove(this);
    }

    protected virtual void FixedUpdate()
    {
        FramesSinceDeclip += 1;
    }
    protected virtual void Update()
    {
        SqDistanceFromPlayer = PhysicsTools.GetSqDistanceBetweenPoints(transform.position, transform.position);
    }

    public float ColliderArea { get; private set; }

    //public void CheckBounds2()
    //{
    //    float TopDistance = 0;
    //    List<RaycastHit> FoundObjects = new List<RaycastHit>();
    //    if (InsideHits != null)
    //    {
    //        foreach (RaycastHit cast in InsideHits.Items)
    //        {
    //            if (collider_TopSide != null)
    //            {

    //                if (cast.collider != null)
    //                {
    //                    if (cast.collider.gameObject.PathMatches(collider_TopSide.gameObject))
    //                    {
    //                        TopDistance = cast.distance;
    //                        break;
    //                    }
    //                    else
    //                    {
    //                        FoundObjects.Add(cast);
    //                    }
    //                }


    //            }



    //        }
    //    }

    //    Intersects = false;
    //    Contained = false;
    //    foreach (RaycastHit cast in FoundObjects)
    //    {

    //        if (cast.collider.gameObject.layer != (int)Shortcuts.UnityLayers.Player)
    //        {


    //            if (cast.collider.gameObject.PathMatches(myCollider.gameObject) && (cast.distance <= TopDistance || TopDistance == 0))
    //            {


    //                Intersects = true;
    //                float overlapTolerance = collider_TopSide.size.x * .1f;
    //                if (PhysicsTools.ReturnColliderOverlapAmount(myCollider, collider_FrontSide) < overlapTolerance
    //                    && PhysicsTools.ReturnColliderOverlapAmount(myCollider, collider_BackSide) < overlapTolerance
    //                    && PhysicsTools.ReturnColliderOverlapAmount(myCollider, collider_LeftSide) < overlapTolerance
    //                    && PhysicsTools.ReturnColliderOverlapAmount(myCollider, collider_RightSide) < overlapTolerance
    //                    && PhysicsTools.ReturnColliderOverlapAmount(myCollider, collider_TopSide) < overlapTolerance
    //                    && PhysicsTools.ReturnColliderOverlapAmount(myCollider, collider_BottomSide) < overlapTolerance)
    //                {
    //                    Contained = true;
    //                }

    //            }
    //        }


    //    }
    //}




}
