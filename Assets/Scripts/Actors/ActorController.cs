using System;
using System.Collections.Generic;
using UnityEngine;

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

    public Int32 FramesSinceDeclip;

    // every instance registers to and removes itself from here
    private static List<ActorController> _actors = new List<ActorController>();

    // Readonly property, I would return a new HashSet so nobody on the outside can alter the content
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
        float magnitude = (transform.position - PlayerController.Player.transform.position).sqrMagnitude;
        SqDistanceFromPlayer = magnitude;
    }

    public float ColliderArea { get; private set; }



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
            rotatedLocalPoint1 = PlayerController.Player.transform.rotation * localPoint1;
            rotatedLocalPoint2 = PlayerController.Player.transform.rotation * localPoint2;
            rotatedLocalPoint3 = PlayerController.Player.transform.rotation * localPoint3;
            rotatedLocalPoint4 = PlayerController.Player.transform.rotation * localPoint4;
            rotatedLocalPoint5 = PlayerController.Player.transform.rotation * localPoint5;
            rotatedLocalPoint6 = PlayerController.Player.transform.rotation * localPoint6;
            rotatedLocalPoint7 = PlayerController.Player.transform.rotation * localPoint7;
            rotatedLocalPoint8 = PlayerController.Player.transform.rotation * localPoint8;

            //Local To Player Bounds
            Vector3 localizedPoint1 = rotatedLocalPoint1 + transform.position - PlayerController.Player.transform.position;
            Vector3 localizedPoint2 = rotatedLocalPoint2 + transform.position - PlayerController.Player.transform.position;
            Vector3 localizedPoint3 = rotatedLocalPoint3 + transform.position - PlayerController.Player.transform.position;
            Vector3 localizedPoint4 = rotatedLocalPoint4 + transform.position - PlayerController.Player.transform.position;
            Vector3 localizedPoint5 = rotatedLocalPoint5 + transform.position - PlayerController.Player.transform.position;
            Vector3 localizedPoint6 = rotatedLocalPoint6 + transform.position - PlayerController.Player.transform.position;
            Vector3 localizedPoint7 = rotatedLocalPoint7 + transform.position - PlayerController.Player.transform.position;
            Vector3 localizedPoint8 = rotatedLocalPoint8 + transform.position - PlayerController.Player.transform.position;

            Bounds playerBounds = new Bounds(Vector3.zero, new Vector3(PlayerController.Player.CubeWidth, PlayerController.Player.CubeWidth, PlayerController.Player.CubeWidth));

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








        //Old attempt

        //float xSize = PlayerController.Player.topSideCollider.size.x;
        //Vector3 playerSize = new Vector3(xSize, xSize, xSize);
        //Vector3 Player_boundPoint1 = PlayerController.Player.transform.position - (playerSize / 2f);
        //Vector3 Player_boundPoint2 = PlayerController.Player.transform.position + (playerSize / 2f);
        //Vector3 Player_boundPoint3 = new Vector3(Player_boundPoint1.x, Player_boundPoint1.y, Player_boundPoint2.z);
        //Vector3 Player_boundPoint4 = new Vector3(Player_boundPoint1.x, Player_boundPoint2.y, Player_boundPoint1.z);
        //Vector3 Player_boundPoint5 = new Vector3(Player_boundPoint2.x, Player_boundPoint1.y, Player_boundPoint1.z);
        //Vector3 Player_boundPoint6 = new Vector3(Player_boundPoint1.x, Player_boundPoint2.y, Player_boundPoint2.z);
        //Vector3 Player_boundPoint7 = new Vector3(Player_boundPoint2.x, Player_boundPoint1.y, Player_boundPoint2.z);
        //Vector3 Player_boundPoint8 = new Vector3(Player_boundPoint2.x, Player_boundPoint2.y, Player_boundPoint1.z);

        //float maxX = Player_boundPoint1.x;
        //float minX = Player_boundPoint1.x;
        //float maxY = Player_boundPoint1.y;
        //float minY = Player_boundPoint1.y;
        //float maxZ = Player_boundPoint1.z;
        //float minZ = Player_boundPoint1.z;

        //foreach (Vector3 vector3 in new List<Vector3>() { Player_boundPoint2, Player_boundPoint3, Player_boundPoint4,
        //                                                Player_boundPoint5, Player_boundPoint6, Player_boundPoint7, Player_boundPoint8})
        //{
        //    if (vector3.x > maxX)
        //        maxX = vector3.x;
        //    if (vector3.x < minX)
        //        minX = vector3.x;

        //    if (vector3.y > maxY)
        //        maxY = vector3.y;
        //    if (vector3.y < minY)
        //        minY = vector3.y;

        //    if (vector3.z > maxZ)
        //        maxZ = vector3.z;
        //    if (vector3.z < minZ)
        //        minZ = vector3.z;
        //}

        //Vector3 boundPoint1 = worldBounds.center - (worldBounds.size / 2f); 
        //Vector3 boundPoint2 = worldBounds.center + (worldBounds.size / 2f);
        //Vector3 boundPoint3 = new Vector3(boundPoint1.x, boundPoint1.y, boundPoint2.z);
        //Vector3 boundPoint4 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint1.z);
        //Vector3 boundPoint5 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint1.z);
        //Vector3 boundPoint6 = new Vector3(boundPoint1.x, boundPoint2.y, boundPoint2.z);
        //Vector3 boundPoint7 = new Vector3(boundPoint2.x, boundPoint1.y, boundPoint2.z);
        //Vector3 boundPoint8 = new Vector3(boundPoint2.x, boundPoint2.y, boundPoint1.z);

        //bool IsContained = true;
        //bool DoesIntersects = false;

        //foreach (Vector3 vector3 in new List<Vector3>() { boundPoint1, boundPoint2, boundPoint3, boundPoint4,
        //                                                boundPoint5, boundPoint6, boundPoint7, boundPoint8})
        //{
        //    if (vector3.x >= minX && vector3.x <= maxX && vector3.y >= minY && vector3.y <= maxY && vector3.z >= minZ && vector3.z <= maxZ)
        //    {
        //        DoesIntersects = true;
        //    }
        //    else
        //    {
        //        IsContained = false;
        //    }
        //}

        //Intersects = DoesIntersects;
        //Contained = IsContained;

        //Even older attempt

        //Vector3[] verticies = worldBounds.;
        //Vector3[] verticies2 = PlayerController.Player.myMesh.vertices;
        //bool Contained = BoundsExtension.Contains(PlayerController.Player.PlayerObject.colliderTrigger.bounds, myCollider.bounds);

        //return Contained;
        //return Contained;
    }



    //public Boolean DoesIntersect()
    //{
    //    return false;
    //    //return myCollider.bounds.Intersects(PlayerController.Player.colliderTrigger.bounds);

    //}

}
