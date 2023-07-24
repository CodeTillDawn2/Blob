using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;





public class BlobSenses : MonoBehaviour
{

    [Header("Stat Block")]
    [Tooltip("")]
    [Serialize] public GameObjectRuntimeSet IntersectsPlayer;
    [Serialize] public GameObjectRuntimeSet ContainedInStomach;
    [Serialize] public GameObjectRuntimeSet AllEnemies;
    [Serialize] public Dict_GameObjectToLastSeen ThingsSeen;
    [Serialize] public GameObjectRuntimeSet ThingsNearby;
    [Serialize] public FloatVariable CurrentSightDistance;
    [Serialize] public Vector3Variable BlobDims;
    [Serialize] public BooleanVariable PlayerIsAlive;

    [Serialize] public PlayerScriptableObject StartingStats;

    [Serialize] public GameObject frontSide;
    [Serialize] public GameObject backSide;
    [Serialize] public GameObject leftSide;
    [Serialize] public GameObject rightSide;
    [Serialize] public GameObject topSide;
    [Serialize] public GameObject bottomSide;
    [Serialize] public RaycastInfoRuntimeSet InsideHits;

    private BoxCollider collider_TopSide;
    private BoxCollider collider_FrontSide;
    private BoxCollider collider_BackSide;
    private BoxCollider collider_LeftSide;
    private BoxCollider collider_RightSide;
    private BoxCollider collider_BottomSide;

    private BoxCollider SightBox;

    protected Rigidbody rb;


    private float CurrentSightBoxSize;


    private void Awake()
    {



    }
    protected void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == null) return;

        if (col.gameObject.layer != (int)Shortcuts.UnityLayers.Player &&
            col.gameObject.layer != (int)Shortcuts.UnityLayers.PlayerTentacle &&
            col.gameObject.layer != (int)Shortcuts.UnityLayers.Ground)
        {
            ThingsNearby.Add(col.gameObject);
        }
    }

    protected void OnTriggerExit(Collider col)
    {
        if (col.gameObject == null) return;
        ThingsNearby.Remove(col.gameObject);
    }




    // Start is called before the first frame update
    void Start()
    {
        InsideHits.RemoveAll();
        rb = GetComponent<Rigidbody>();
        collider_TopSide = topSide.GetComponent<BoxCollider>();
        collider_BottomSide = bottomSide.GetComponent<BoxCollider>();
        collider_LeftSide = leftSide.GetComponent<BoxCollider>();
        collider_RightSide = rightSide.GetComponent<BoxCollider>();
        collider_FrontSide = frontSide.GetComponent<BoxCollider>();
        collider_BackSide = backSide.GetComponent<BoxCollider>();
        CurrentSightDistance.Value = StartingStats.SightDistance;
        SightBox = GetComponent<BoxCollider>();
        CurrentSightBoxSize = SightBox.size.x;
        StartCoroutine(CheckCubeBounds());
    }
    

    void Update()
    {
        SeeThings();

    }

    private void FixedUpdate()
    {
        if (CurrentSightBoxSize <= CurrentSightDistance.Value)
        {
            SightBox.size = new Vector3(CurrentSightDistance.Value, CurrentSightDistance.Value, CurrentSightDistance.Value);
        }
    }

    public IEnumerator CheckCubeBounds()
    {
        while (PlayerIsAlive.Value)
        {
            CheckBounds();
            yield return new WaitForEndOfFrame();
        }


    }

    public bool WithinVisionRange(float SqDistance)
    {
        if (SqDistance < CurrentSightDistance.Value * CurrentSightDistance.Value)
        {
            return true;
        }
        return false;
    }

    public Vector3 Vector3_CubeHalfExtents
    {
        get { return new Vector3(BlobDims.Value.x * .449f, BlobDims.Value.y * .449f, BlobDims.Value.z * .449f); }
    }


    private void SeeThings()
    {

        InsideHits.Items = Physics.BoxCastAll(gameObject.transform.position, Vector3_CubeHalfExtents,
            transform.up, Quaternion.identity, BlobDims.Value.y / 2f, (int)Shortcuts.LayerMasks.LayerMask_NotGround).ToList().ConvertAll(x => new RaycastHitInfo(x));


        //Remove anything which isn't nearby anymore 
        for (int i = ThingsSeen.Value.Keys.Count-1; i >= 0; i--)
        {
            GameObject OldSeen = ThingsSeen.Value.Keys.ToList()[i];
            if (!ThingsNearby.Items.Contains(OldSeen) || OldSeen == null)
            {
                ThingsSeen.Value.Remove(OldSeen);
            }
        }


        //See things
        foreach (GameObject ThingNearby in ThingsNearby.Items.Where(x => x != null))
        {
            if ((ThingNearby.transform.position - transform.position).sqrMagnitude <= CurrentSightDistance.Value * CurrentSightDistance.Value)
            {
                Collider ThingCollider= ThingNearby.GetComponent<Collider>();
                
                if (ThingCollider == null) ThingCollider = ThingNearby.GetComponentInChildren<Collider>();

                if (ThingCollider == null) continue;
               

                Bounds ThingBounds = ThingCollider.bounds;

                bool Spotted = false;

                Vector3 BottomCenter = gameObject.transform.position;
                Vector3 MiddleCenter = BottomCenter + gameObject.transform.up * BlobDims.Value.y / 2;
                Vector3 TopCenter = BottomCenter + gameObject.transform.up * BlobDims.Value.y;

                Vector3 GoForward = gameObject.transform.forward * BlobDims.Value.z / 2f;
                Vector3 GoRight = gameObject.transform.right * BlobDims.Value.x / 2f;

                List<Vector3> positionsToLookFrom = new List<Vector3>()
                {
                    MiddleCenter, //Center
                    TopCenter + -GoRight + GoForward, //Top Left Forward Corner
                    TopCenter + GoRight + GoForward, //Top Right Forward Corner
                    TopCenter + -GoRight + -GoForward, //Top Left Backward Corner
                    TopCenter + GoRight + -GoForward, //Top Right Backward Corner
                    BottomCenter // Bottom Center
                };

                BottomCenter = ThingCollider.transform.position;
                MiddleCenter = BottomCenter + ThingCollider.transform.up * ThingBounds.size.y / 2f;
                TopCenter = BottomCenter + ThingCollider.transform.up * ThingBounds.size.y;

                GoForward = ThingCollider.transform.forward * ThingBounds.size.z / 2f;
                GoRight = ThingCollider.transform.right * ThingBounds.size.x / 2f;

                List<Vector3> LookTargets = new List<Vector3>()
                {
                    TopCenter, // Top of bounds
                    //MiddleCenter + GoRight, //Middle Right
                    //MiddleCenter + -GoRight, // Middle Left
                    //MiddleCenter + GoForward, // Middle Forward
                    MiddleCenter + -GoForward, // Middle Backward
                    //BottomCenter //Bottom center
                };

                foreach (Vector3 LookPosition in  positionsToLookFrom)
                {
                    foreach(Vector3 LookTarget in LookTargets)
                    {
                      RaycastHit? rayCast = PhysicsTools.RaycastAt(LookPosition,
                      LookTarget, CurrentSightDistance.Value,
                      (int)Shortcuts.LayerMasks.LayerMask_NotPlayerOrTentacles);
                        if (rayCast != null)
                        {
                            if (rayCast.Value.collider != null && rayCast.Value.collider.gameObject != null)
                            {
                                if (rayCast.Value.collider.gameObject == ThingNearby)
                                {
                                    ThingsSeen.Value.AddUpdate(ThingNearby,
                                        new LastSeenData()
                                        {
                                            WhenSeen = Time.timeAsDouble,
                                            LastSeen = rayCast.Value.collider.gameObject.transform.position,
                                            Distance = rayCast.Value.distance
                                        });
                                    Spotted = true;
                                    Debug.DrawLine(LookPosition, LookTarget);
                                    //break;
                                }
                            }
                        }
                    }
                    //if (Spotted) break;

                }
                if (!Spotted)
                {
                    ThingsSeen.Value.Remove(ThingNearby);
                }

            }



        }


    }


    public void CheckBounds()
    {
        float TopDistance = 0;
        List<RaycastHitInfo> FoundObjects = new List<RaycastHitInfo>();
        if (InsideHits != null)
        {
            foreach (RaycastHitInfo hit in InsideHits.Items)
            {
                if (collider_TopSide != null)
                {

                    if (hit.collider != null)
                    {
                        if (hit.collider.gameObject.PathMatches(collider_TopSide.gameObject))
                        {
                            TopDistance = hit.distance;
                        }
                        else if (hit.collider.gameObject.layer != (int)Shortcuts.UnityLayers.Player)
                        {
                            FoundObjects.Add(hit);
                        }
                    }


                }



            }
        }


        IntersectsPlayer.RemoveAll();
        ContainedInStomach.RemoveAll();

        foreach (RaycastHitInfo hit in FoundObjects)
        {

            bool Contained = false;

            IntersectsPlayer.Add(hit.collider.gameObject);
            if (hit.distance <= TopDistance || TopDistance == 0)
            {



                float overlapTolerance = collider_TopSide.size.x * .05f;
                if (PhysicsTools.ReturnColliderOverlapAmount(hit.collider, collider_FrontSide) < overlapTolerance
                    && PhysicsTools.ReturnColliderOverlapAmount(hit.collider, collider_BackSide) < overlapTolerance
                    && PhysicsTools.ReturnColliderOverlapAmount(hit.collider, collider_LeftSide) < overlapTolerance
                    && PhysicsTools.ReturnColliderOverlapAmount(hit.collider, collider_RightSide) < overlapTolerance
                    && PhysicsTools.ReturnColliderOverlapAmount(hit.collider, collider_TopSide) < overlapTolerance
                    && PhysicsTools.ReturnColliderOverlapAmount(hit.collider, collider_BottomSide) < overlapTolerance)
                {
                    Contained = true;
                }
                else
                {
                    string test = "";
                }
            }

            if (Contained)
            {
                ContainedInStomach.Add(hit.collider.gameObject);
            }

        }
    }


}
