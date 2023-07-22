using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static HelperClasses;





public class PlayerVision : MonoBehaviour
{

    [Header("Stat Block")]
    [Serialize] public FloatVariable CurrentSightDistance;
    [Serialize] public FloatVariable CubeWidth;
    [Serialize] public Vector3Variable PlayerPosition;
    [Serialize] public GameObjectRuntimeSet IntersectsPlayer;
    [Serialize] public GameObjectRuntimeSet ContainedInStomach;

    protected BoxCollider collider_TopSide;
    protected BoxCollider collider_FrontSide;
    protected BoxCollider collider_BackSide;
    protected BoxCollider collider_LeftSide;
    protected BoxCollider collider_RightSide;
    protected BoxCollider collider_BottomSide;

    public GameObject frontSide;
    public GameObject backSide;
    public GameObject leftSide;
    public GameObject rightSide;
    public GameObject topSide;
    public GameObject bottomSide;

    public Rigidbody rb;

    public List<GameObject> Eyes;
    public float sightDistance;

    public Eye Eye_FrontBottomLeft;
    public Eye Eye_FrontBottomMid;
    public Eye Eye_FrontBottomRight;
    public Eye Eye_FrontMiddleLeft;
    public Eye Eye_FrontMiddleMid;
    public Eye Eye_FrontMiddleRight;
    public Eye Eye_FrontTopLeft;
    public Eye Eye_FrontTopMid;
    public Eye Eye_FrontTopRight;

    public Eye Eye_BackBottomLeft;
    public Eye Eye_BackBottomMid;
    public Eye Eye_BackBottomRight;
    public Eye Eye_BackMiddleLeft;
    public Eye Eye_BackMiddleMid;
    public Eye Eye_BackMiddleRight;
    public Eye Eye_BackTopLeft;
    public Eye Eye_BackTopMid;
    public Eye Eye_BackTopRight;

    public Eye Eye_RightBottomBack;
    public Eye Eye_RightBottomMid;
    public Eye Eye_RightBottomFront;
    public Eye Eye_RightMiddleBack;
    public Eye Eye_RightMiddleMid;
    public Eye Eye_RightMiddleFront;
    public Eye Eye_RightTopBack;
    public Eye Eye_RightTopMid;
    public Eye Eye_RightTopFront;

    public Eye Eye_LeftBottomFront;
    public Eye Eye_LeftBottomMid;
    public Eye Eye_LeftBottomBack;
    public Eye Eye_LeftMiddleFront;
    public Eye Eye_LeftMiddleMid;
    public Eye Eye_LeftMiddleBack;
    public Eye Eye_LeftTopFront;
    public Eye Eye_LeftTopMid;
    public Eye Eye_LeftTopBack;

    public static List<Eye> AllEyes;
    public static List<Eye> CenterEyes;
    public static List<Eye> TopCornerEyes;

    public RaycastRuntimeSet InsideHits;



    private void Awake()
    {
        Eye_FrontBottomLeft = new Eye(Eyes.Where(x => x.name == "FrontBottomLeft").First());
        Eye_FrontBottomMid = new Eye(Eyes.Where(x => x.name == "FrontBottomMid").First());
        Eye_FrontBottomRight = new Eye(Eyes.Where(x => x.name == "FrontBottomRight").First());
        Eye_FrontMiddleLeft = new Eye(Eyes.Where(x => x.name == "FrontMiddleLeft").First());
        Eye_FrontMiddleMid = new Eye(Eyes.Where(x => x.name == "FrontMiddleMid").First());
        Eye_FrontMiddleRight = new Eye(Eyes.Where(x => x.name == "FrontMiddleRight").First());
        Eye_FrontTopLeft = new Eye(Eyes.Where(x => x.name == "FrontTopLeft").First());
        Eye_FrontTopMid = new Eye(Eyes.Where(x => x.name == "FrontTopMid").First());
        Eye_FrontTopRight = new Eye(Eyes.Where(x => x.name == "FrontTopRight").First());

        Eye_BackBottomLeft = new Eye(Eyes.Where(x => x.name == "BackBottomLeft").First());
        Eye_BackBottomMid = new Eye(Eyes.Where(x => x.name == "BackBottomMid").First());
        Eye_BackBottomRight = new Eye(Eyes.Where(x => x.name == "BackBottomRight").First());
        Eye_BackMiddleLeft = new Eye(Eyes.Where(x => x.name == "BackMiddleLeft").First());
        Eye_BackMiddleMid = new Eye(Eyes.Where(x => x.name == "BackMiddleMid").First());
        Eye_BackMiddleRight = new Eye(Eyes.Where(x => x.name == "BackMiddleRight").First());
        Eye_BackTopLeft = new Eye(Eyes.Where(x => x.name == "BackTopLeft").First());
        Eye_BackTopMid = new Eye(Eyes.Where(x => x.name == "BackTopMid").First());
        Eye_BackTopRight = new Eye(Eyes.Where(x => x.name == "BackTopRight").First());

        Eye_LeftBottomFront = new Eye(Eyes.Where(x => x.name == "LeftBottomFront").First());
        Eye_LeftBottomMid = new Eye(Eyes.Where(x => x.name == "LeftBottomMid").First());
        Eye_LeftBottomBack = new Eye(Eyes.Where(x => x.name == "LeftBottomBack").First());
        Eye_LeftMiddleFront = new Eye(Eyes.Where(x => x.name == "LeftMiddleFront").First());
        Eye_LeftMiddleMid = new Eye(Eyes.Where(x => x.name == "LeftMiddleMid").First());
        Eye_LeftMiddleBack = new Eye(Eyes.Where(x => x.name == "LeftMiddleBack").First());
        Eye_LeftTopFront = new Eye(Eyes.Where(x => x.name == "LeftTopFront").First());
        Eye_LeftTopMid = new Eye(Eyes.Where(x => x.name == "LeftTopMid").First());
        Eye_LeftTopBack = new Eye(Eyes.Where(x => x.name == "LeftTopBack").First());

        Eye_RightBottomBack = new Eye(Eyes.Where(x => x.name == "RightBottomBack").First());
        Eye_RightBottomMid = new Eye(Eyes.Where(x => x.name == "RightBottomMid").First());
        Eye_RightBottomFront = new Eye(Eyes.Where(x => x.name == "RightBottomFront").First());
        Eye_RightMiddleBack = new Eye(Eyes.Where(x => x.name == "RightMiddleBack").First());
        Eye_RightMiddleMid = new Eye(Eyes.Where(x => x.name == "RightMiddleMid").First());
        Eye_RightMiddleFront = new Eye(Eyes.Where(x => x.name == "RightMiddleFront").First());
        Eye_RightTopBack = new Eye(Eyes.Where(x => x.name == "RightTopBack").First());
        Eye_RightTopMid = new Eye(Eyes.Where(x => x.name == "RightTopMid").First());
        Eye_RightTopFront = new Eye(Eyes.Where(x => x.name == "RightTopFront").First());

        AllEyes = new List<Eye> { Eye_FrontBottomLeft, Eye_FrontBottomMid, Eye_FrontBottomRight, Eye_FrontMiddleLeft, Eye_FrontMiddleMid, Eye_FrontMiddleRight, Eye_FrontTopLeft, Eye_FrontTopMid, Eye_FrontTopRight,
                                        Eye_BackBottomLeft, Eye_BackBottomMid, Eye_BackBottomRight, Eye_BackMiddleLeft, Eye_BackMiddleMid, Eye_BackMiddleRight, Eye_BackTopLeft, Eye_BackTopMid, Eye_BackTopRight,
                                        Eye_LeftBottomFront, Eye_LeftBottomMid, Eye_LeftBottomBack, Eye_LeftMiddleFront, Eye_LeftMiddleMid, Eye_LeftMiddleBack, Eye_LeftTopFront, Eye_LeftTopMid, Eye_LeftTopBack,
                                        Eye_RightBottomBack, Eye_RightBottomMid, Eye_RightBottomFront, Eye_RightMiddleBack, Eye_RightMiddleMid, Eye_RightMiddleFront, Eye_RightTopBack, Eye_RightTopMid, Eye_RightTopFront};

        CenterEyes = new List<Eye> { Eye_FrontMiddleMid, Eye_BackMiddleMid, Eye_LeftMiddleMid, Eye_RightMiddleMid };
        TopCornerEyes = new List<Eye> { Eye_FrontTopLeft, Eye_FrontTopRight, Eye_BackTopLeft, Eye_BackTopRight };


    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider_TopSide = topSide.GetComponent<BoxCollider>();
        collider_BottomSide = bottomSide.GetComponent<BoxCollider>();
        collider_LeftSide = leftSide.GetComponent<BoxCollider>();
        collider_RightSide = rightSide.GetComponent<BoxCollider>();
        collider_FrontSide = frontSide.GetComponent<BoxCollider>();
        collider_BackSide = backSide.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        SeeThings();
        
    }

    private void FixedUpdate()
    {
        StartCoroutine(CheckCubeBounds());
    }

    public IEnumerator CheckCubeBounds()
    {
        CheckBounds();
        yield return new WaitForFixedUpdate();
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
        get { return new Vector3(CubeWidth.Value * .449f, CubeWidth.Value * .449f, CubeWidth.Value * .449f); }
    }

    private void SeeThings()
    {

        InsideHits.SetFromArray(Physics.BoxCastAll(PlayerPosition.Value, Vector3_CubeHalfExtents,
            transform.up, Quaternion.identity, CubeWidth.Value / 2f, (int)Shortcuts.LayerMasks.LayerMask_NotGround));

        foreach (ActorController actorController in ActorController.Actors.Where(x => WithinVisionRange(x.SqDistanceFromPlayer)))
        {
            if (WithinVisionRange(actorController.SqDistanceFromPlayer))
            {
                foreach (Eye eye in TopCornerEyes)
                {
                    RaycastHit? rayCast = PhysicsTools.RaycastAt(eye.gameObject.transform.position, actorController.gameObject.transform.position, CurrentSightDistance.Value);
                    if (rayCast != null)
                    {
                        if (rayCast.Value.collider.gameObject.PathMatches(actorController.gameObject))
                        {
                            actorController.IsSeenByPlayer = true;
                            break;
                        }
                    }


                }
            }

        }


    }


    public void CheckBounds()
    {
        float TopDistance = 0;
        List<RaycastHit> FoundObjects = new List<RaycastHit>();
        if (InsideHits != null)
        {
            foreach (RaycastHit cast in InsideHits.Items)
            {
                if (collider_TopSide != null)
                {

                    if (cast.collider != null)
                    {
                        if (cast.collider.gameObject.PathMatches(collider_TopSide.gameObject))
                        {
                            TopDistance = cast.distance;
                        }
                        else if (cast.collider.gameObject.layer != (int)Shortcuts.UnityLayers.Player)
                        {
                            FoundObjects.Add(cast);
                        }
                    }


                }



            }
        }


        foreach (RaycastHit cast in FoundObjects)
        {
            
            bool Intersects = false;
            bool Contained = false;


            if (cast.distance <= TopDistance || TopDistance == 0)
            {


                    Intersects = true;
                    float overlapTolerance = collider_TopSide.size.x * .1f;
                    if (PhysicsTools.ReturnColliderOverlapAmount(cast.collider, collider_FrontSide) < overlapTolerance
                        && PhysicsTools.ReturnColliderOverlapAmount(cast.collider, collider_BackSide) < overlapTolerance
                        && PhysicsTools.ReturnColliderOverlapAmount(cast.collider, collider_LeftSide) < overlapTolerance
                        && PhysicsTools.ReturnColliderOverlapAmount(cast.collider, collider_RightSide) < overlapTolerance
                        && PhysicsTools.ReturnColliderOverlapAmount(cast.collider, collider_TopSide) < overlapTolerance
                        && PhysicsTools.ReturnColliderOverlapAmount(cast.collider, collider_BottomSide) < overlapTolerance)
                    {
                        Contained = true;
                    }

                }

            if (Intersects) IntersectsPlayer.Add(cast.collider.gameObject);
            if (Contained) ContainedInStomach.Add(cast.collider.gameObject);

        }
    }

    public GameObject FindEdibleBehind()
    {
        foreach (Eye eye in new List<Eye>() { Eye_BackBottomLeft, Eye_BackBottomMid, Eye_BackBottomRight, Eye_BackMiddleLeft, Eye_BackMiddleMid, Eye_BackMiddleRight, Eye_BackTopLeft, Eye_BackTopMid, Eye_BackTopRight })
        {
            if (eye.edibleHit != null)
            {
                return ((RaycastHit)eye.edibleHit).collider.gameObject;
            }

        }
        return null;
    }


}
