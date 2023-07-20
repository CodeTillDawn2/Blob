using System;
using System.Collections.Generic;
using UnityEngine;
using static PlayerBrain;

public abstract class ActorController : MonoBehaviour
{
    [HideInInspector]
    public float StartingMass;
    [HideInInspector]
    public Collider myCollider;
    [HideInInspector]
    public Rigidbody rb;
    public abstract float SqDistanceFromPlayer { get; set; }

    public Bounds localBounds;
    public Bounds worldBounds;

    public bool Intersects;
    public bool Contained;

    public bool TouchingFront = false;
    public bool TouchingBack = false;
    public bool TouchingTop = false;
    public bool TouchingBottom = false;
    public bool TouchingLeft = false;
    public bool TouchingRight = false;

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
        FramesSinceDeclip = 0;


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



        float x = myCollider.bounds.size.x;
        float y = myCollider.bounds.size.y;
        float z = myCollider.bounds.size.z;

        BoxCollider box = (BoxCollider)myCollider;
        if (box != null)
        {
            x = box.size.x;
            y = box.size.y;
            z = box.size.z;
        }

        ColliderArea = x * y * z;
        float magnitude = (transform.position - PlayerController.me.transform.position).sqrMagnitude;
        SqDistanceFromPlayer = magnitude;
    }

    public float ColliderArea { get; private set; }

    public void CheckBounds2()
    {
        float TopDistance = 0;
        List<RaycastHit> FoundObjects = new List<RaycastHit>();
        if (PlayerController.me.Brain.InsideHits != null)
        {
            foreach (RaycastHit cast in PlayerController.me.Brain.InsideHits)
            {
                if (cast.collider.gameObject.Matches(PlayerController.me.collider_TopSide.gameObject))
                {
                    TopDistance = cast.distance;
                    break;
                }
                else
                {
                    FoundObjects.Add(cast);
                }
            }
        }

        Intersects = false;
        Contained = false;
        foreach (RaycastHit cast in FoundObjects)
        {

            if (cast.collider.gameObject.layer != (int)UnityLayers.Player)
            {


                if (cast.collider.gameObject.Matches(myCollider.gameObject) && (cast.distance <= TopDistance || TopDistance == 0))
                {
                    Intersects = true;
                    float overlapTolerance = PlayerController.me.collider_TopSide.size.x * .1f;
                    if (PhysicsTools.ReturnColliderOverlapAmount(myCollider, PlayerController.me.collider_FrontSide) < overlapTolerance
                        && PhysicsTools.ReturnColliderOverlapAmount(myCollider, PlayerController.me.collider_BackSide) < overlapTolerance
                        && PhysicsTools.ReturnColliderOverlapAmount(myCollider, PlayerController.me.collider_LeftSide) < overlapTolerance
                        && PhysicsTools.ReturnColliderOverlapAmount(myCollider, PlayerController.me.collider_RightSide) < overlapTolerance
                        && PhysicsTools.ReturnColliderOverlapAmount(myCollider, PlayerController.me.collider_TopSide) < overlapTolerance
                        && PhysicsTools.ReturnColliderOverlapAmount(myCollider, PlayerController.me.collider_BottomSide) < overlapTolerance)
                    {
                        Contained = true;
                    }
                    //if (!myCollider.bounds.Intersects(PlayerController.me.collider_FrontSide.)
                    //    && !myCollider.bounds.Intersects(PlayerController.me.collider_BackSide.bounds)
                    //    && !myCollider.bounds.Intersects(PlayerController.me.collider_LeftSide.bounds)
                    //    && !myCollider.bounds.Intersects(PlayerController.me.collider_RightSide.bounds)
                    //    && !myCollider.bounds.Intersects(PlayerController.me.collider_TopSide.bounds)
                    //    && !myCollider.bounds.Intersects(PlayerController.me.collider_BottomSide.bounds))
                    //{
                    //    Contained = true;
                    //}

                }
            }

           
        }
    }

    public void CheckBounds()
    {


        if (myCollider is BoxCollider)
        {



            Quaternion rotation = Quaternion.Inverse(transform.rotation);
            Vector3 myVector = Vector3.one;
            Vector3 rotateVector = rotation * myVector;


            //Local Bounds
            Vector3 localPoint1 = (localBounds.center - (localBounds.size / 2f));
            Vector3 localPoint2 = (localBounds.center + (localBounds.size / 2f));

            Vector3 localPoint3 = new Vector3(localPoint1.x, localPoint1.y, localPoint2.z);
            Vector3 localPoint4 = new Vector3(localPoint1.x, localPoint2.y, localPoint1.z);
            Vector3 localPoint6 = new Vector3(localPoint1.x, localPoint2.y, localPoint2.z);


            Vector3 localPoint5 = new Vector3(localPoint2.x, localPoint1.y, localPoint1.z);
            Vector3 localPoint7 = new Vector3(localPoint2.x, localPoint1.y, localPoint2.z);
            Vector3 localPoint8 = new Vector3(localPoint2.x, localPoint2.y, localPoint1.z);


            //Remove any existing rotation
            Vector3 rotatedLocalPoint1 = rotation * localPoint1;
            Vector3 rotatedLocalPoint2 = rotation * localPoint2;
            Vector3 rotatedLocalPoint3 = rotation * localPoint3;
            Vector3 rotatedLocalPoint4 = rotation * localPoint4;
            Vector3 rotatedLocalPoint5 = rotation * localPoint5;
            Vector3 rotatedLocalPoint6 = rotation * localPoint6;
            Vector3 rotatedLocalPoint7 = rotation * localPoint7;
            Vector3 rotatedLocalPoint8 = rotation * localPoint8;

            //Rotated
            rotatedLocalPoint1 = PlayerController.me.transform.rotation * localPoint1;
            rotatedLocalPoint2 = PlayerController.me.transform.rotation * localPoint2;
            rotatedLocalPoint3 = PlayerController.me.transform.rotation * localPoint3;
            rotatedLocalPoint4 = PlayerController.me.transform.rotation * localPoint4;
            rotatedLocalPoint5 = PlayerController.me.transform.rotation * localPoint5;
            rotatedLocalPoint6 = PlayerController.me.transform.rotation * localPoint6;
            rotatedLocalPoint7 = PlayerController.me.transform.rotation * localPoint7;
            rotatedLocalPoint8 = PlayerController.me.transform.rotation * localPoint8;

            //Local To Player Bounds
            Vector3 localizedPoint1 = rotatedLocalPoint1 + transform.position - PlayerController.me.transform.position;
            Vector3 localizedPoint2 = rotatedLocalPoint2 + transform.position - PlayerController.me.transform.position;
            Vector3 localizedPoint3 = rotatedLocalPoint3 + transform.position - PlayerController.me.transform.position;
            Vector3 localizedPoint4 = rotatedLocalPoint4 + transform.position - PlayerController.me.transform.position;
            Vector3 localizedPoint5 = rotatedLocalPoint5 + transform.position - PlayerController.me.transform.position;
            Vector3 localizedPoint6 = rotatedLocalPoint6 + transform.position - PlayerController.me.transform.position;
            Vector3 localizedPoint7 = rotatedLocalPoint7 + transform.position - PlayerController.me.transform.position;
            Vector3 localizedPoint8 = rotatedLocalPoint8 + transform.position - PlayerController.me.transform.position;

            Bounds playerBounds = new Bounds(Vector3.zero, new Vector3(PlayerController.me.CubeWidth, PlayerController.me.CubeWidth, PlayerController.me.CubeWidth));

            bool DoesIntersect = false;
            bool IsContained = true;

            foreach (Vector3 worldPoint in new List<Vector3>() { localizedPoint1, localizedPoint2, localizedPoint3, localizedPoint4,
                                                            localizedPoint5, localizedPoint6, localizedPoint7, localizedPoint8})
            {
                if (playerBounds.Contains(worldPoint))
                {
                    DoesIntersect = true;
                }
                else
                {
                    IsContained = false;
                }

            }


            Intersects = DoesIntersect;
            Contained = IsContained;
        }
        else
        {
            throw new NotImplementedException();
        }

    }



}
