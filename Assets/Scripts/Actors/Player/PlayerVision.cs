using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;





public class PlayerVision : MonoBehaviour
{

    [Header("Stat Block")]
    [Serialize] public FloatVariable CurrentSightDistance;
    [Serialize] public FloatVariable CubeWidth;
    [Serialize] public BooleanVariable PlayerIsAlive;
    [Serialize] public GameObjectVariable PlayerGameObject;
    [Serialize] public GameObjectRuntimeSet IntersectsPlayer;
    [Serialize] public GameObjectRuntimeSet ContainedInStomach;
    [Serialize] public GameObjectRuntimeSet AllEnemies;
    [Serialize] public GameObjectRuntimeSet ThingsSeenByPlayer;
    [Serialize] public List<GameObject> Eyes;

    [Serialize] public GameObject frontSide;
    [Serialize] public GameObject backSide;
    [Serialize] public GameObject leftSide;
    [Serialize] public GameObject rightSide;
    [Serialize] public GameObject topSide;
    [Serialize] public GameObject bottomSide;
    [Serialize] public RaycastInfoRuntimeSet InsideHits;

    protected BoxCollider collider_TopSide;
    protected BoxCollider collider_FrontSide;
    protected BoxCollider collider_BackSide;
    protected BoxCollider collider_LeftSide;
    protected BoxCollider collider_RightSide;
    protected BoxCollider collider_BottomSide;
    protected List<Eye> CenterEyes;
    protected List<Eye> TopCornerEyes;



    protected Rigidbody rb;

    //private Eye Eye_FrontBottomLeft;
    //private Eye Eye_FrontBottomMid;
    //private Eye Eye_FrontBottomRight;
    //private Eye Eye_FrontMiddleLeft;
    private Eye Eye_FrontMiddleMid;
    //private Eye Eye_FrontMiddleRight;
    private Eye Eye_FrontTopLeft;
    //private Eye Eye_FrontTopMid;
    private Eye Eye_FrontTopRight;

    private Eye Eye_BackBottomLeft;
    private Eye Eye_BackBottomMid;
    private Eye Eye_BackBottomRight;
    private Eye Eye_BackMiddleLeft;
    private Eye Eye_BackMiddleMid;
    private Eye Eye_BackMiddleRight;
    private Eye Eye_BackTopLeft;
    private Eye Eye_BackTopMid;
    private Eye Eye_BackTopRight;

    //private Eye Eye_RightBottomBack;
    //private Eye Eye_RightBottomMid;
    //private Eye Eye_RightBottomFront;
    //private Eye Eye_RightMiddleBack;
    private Eye Eye_RightMiddleMid;
    //private Eye Eye_RightMiddleFront;
    //private Eye Eye_RightTopBack;
    //private Eye Eye_RightTopMid;
    //private Eye Eye_RightTopFront;

    //private Eye Eye_LeftBottomFront;
    //private Eye Eye_LeftBottomMid;
    //private Eye Eye_LeftBottomBack;
    //private Eye Eye_LeftMiddleFront;
    private Eye Eye_LeftMiddleMid;
    //private Eye Eye_LeftMiddleBack;
    //private Eye Eye_LeftTopFront;
    //private Eye Eye_LeftTopMid;
    //private Eye Eye_LeftTopBack;





    private void Awake()
    {



    }

    public void SetupEyes()
    {
    //    Eye_FrontBottomLeft = new Eye(Eyes.Where(x => x.name == "FrontBottomLeft").First());
    //    Eye_FrontBottomMid = new Eye(Eyes.Where(x => x.name == "FrontBottomMid").First());
    //    Eye_FrontBottomRight = new Eye(Eyes.Where(x => x.name == "FrontBottomRight").First());
    //    Eye_FrontMiddleLeft = new Eye(Eyes.Where(x => x.name == "FrontMiddleLeft").First());
        Eye_FrontMiddleMid = new Eye(Eyes.Where(x => x.name == "FrontMiddleMid").First());
        //Eye_FrontMiddleRight = new Eye(Eyes.Where(x => x.name == "FrontMiddleRight").First());
        Eye_FrontTopLeft = new Eye(Eyes.Where(x => x.name == "FrontTopLeft").First());
        //Eye_FrontTopMid = new Eye(Eyes.Where(x => x.name == "FrontTopMid").First());
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

        //Eye_LeftBottomFront = new Eye(Eyes.Where(x => x.name == "LeftBottomFront").First());
        //Eye_LeftBottomMid = new Eye(Eyes.Where(x => x.name == "LeftBottomMid").First());
        //Eye_LeftBottomBack = new Eye(Eyes.Where(x => x.name == "LeftBottomBack").First());
        //Eye_LeftMiddleFront = new Eye(Eyes.Where(x => x.name == "LeftMiddleFront").First());
        Eye_LeftMiddleMid = new Eye(Eyes.Where(x => x.name == "LeftMiddleMid").First());
        //Eye_LeftMiddleBack = new Eye(Eyes.Where(x => x.name == "LeftMiddleBack").First());
        //Eye_LeftTopFront = new Eye(Eyes.Where(x => x.name == "LeftTopFront").First());
        //Eye_LeftTopMid = new Eye(Eyes.Where(x => x.name == "LeftTopMid").First());
        //Eye_LeftTopBack = new Eye(Eyes.Where(x => x.name == "LeftTopBack").First());

        //Eye_RightBottomBack = new Eye(Eyes.Where(x => x.name == "RightBottomBack").First());
        //Eye_RightBottomMid = new Eye(Eyes.Where(x => x.name == "RightBottomMid").First());
        //Eye_RightBottomFront = new Eye(Eyes.Where(x => x.name == "RightBottomFront").First());
        //Eye_RightMiddleBack = new Eye(Eyes.Where(x => x.name == "RightMiddleBack").First());
        Eye_RightMiddleMid = new Eye(Eyes.Where(x => x.name == "RightMiddleMid").First());
        //Eye_RightMiddleFront = new Eye(Eyes.Where(x => x.name == "RightMiddleFront").First());
        //Eye_RightTopBack = new Eye(Eyes.Where(x => x.name == "RightTopBack").First());
        //Eye_RightTopMid = new Eye(Eyes.Where(x => x.name == "RightTopMid").First());
        //Eye_RightTopFront = new Eye(Eyes.Where(x => x.name == "RightTopFront").First());

        //AllEyes = new List<Eye> { Eye_FrontBottomLeft, Eye_FrontBottomMid, Eye_FrontBottomRight, Eye_FrontMiddleLeft, Eye_FrontMiddleMid, Eye_FrontMiddleRight, Eye_FrontTopLeft, Eye_FrontTopMid, Eye_FrontTopRight,
        //                                Eye_BackBottomLeft, Eye_BackBottomMid, Eye_BackBottomRight, Eye_BackMiddleLeft, Eye_BackMiddleMid, Eye_BackMiddleRight, Eye_BackTopLeft, Eye_BackTopMid, Eye_BackTopRight,
        //                                Eye_LeftBottomFront, Eye_LeftBottomMid, Eye_LeftBottomBack, Eye_LeftMiddleFront, Eye_LeftMiddleMid, Eye_LeftMiddleBack, Eye_LeftTopFront, Eye_LeftTopMid, Eye_LeftTopBack,
        //                                Eye_RightBottomBack, Eye_RightBottomMid, Eye_RightBottomFront, Eye_RightMiddleBack, Eye_RightMiddleMid, Eye_RightMiddleFront, Eye_RightTopBack, Eye_RightTopMid, Eye_RightTopFront};

        CenterEyes = new List<Eye> { Eye_FrontMiddleMid, Eye_BackMiddleMid, Eye_LeftMiddleMid, Eye_RightMiddleMid };
        TopCornerEyes = new List<Eye> { Eye_FrontTopLeft, Eye_FrontTopRight, Eye_BackTopLeft, Eye_BackTopRight };
    }

    // Start is called before the first frame update
    void Start()
    {
        InsideHits = new RaycastInfoRuntimeSet();
        SetupEyes();
        rb = GetComponent<Rigidbody>();
        collider_TopSide = topSide.GetComponent<BoxCollider>();
        collider_BottomSide = bottomSide.GetComponent<BoxCollider>();
        collider_LeftSide = leftSide.GetComponent<BoxCollider>();
        collider_RightSide = rightSide.GetComponent<BoxCollider>();
        collider_FrontSide = frontSide.GetComponent<BoxCollider>();
        collider_BackSide = backSide.GetComponent<BoxCollider>();
        StartCoroutine(CheckCubeBounds());
    }


    void Update()
    {
        SeeThings();

    }

    private void FixedUpdate()
    {

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
        get { return new Vector3(CubeWidth.Value * .449f, CubeWidth.Value * .449f, CubeWidth.Value * .449f); }
    }

    private void SeeThings()
    {

        InsideHits.Items = Physics.BoxCastAll(PlayerGameObject.Value.transform.position, Vector3_CubeHalfExtents,
            transform.up, Quaternion.identity, CubeWidth.Value / 2f, (int)Shortcuts.LayerMasks.LayerMask_NotGround).ToList().ConvertAll(x => new RaycastHitInfo(x));

        foreach (GameObject gameObject in AllEnemies.Items)
        {
                if ((gameObject.transform.position - transform.position).sqrMagnitude <= CurrentSightDistance.Value * CurrentSightDistance.Value)
                {
                    foreach (Eye eye in TopCornerEyes)
                    {
                        RaycastHit? rayCast = PhysicsTools.RaycastAt(eye.gameObject.transform.position, gameObject.gameObject.transform.position, CurrentSightDistance.Value);
                        if (rayCast != null)
                        {
                            if (rayCast.Value.collider.gameObject.PathMatches(gameObject.gameObject))
                            {
                                ThingsSeenByPlayer.Add(gameObject);
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
