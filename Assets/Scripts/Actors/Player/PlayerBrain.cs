using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class Eye
{
    public GameObject gameObject;
    public RaycastHit? hit;
    public RaycastHit? edibleHit;
    public RaycastHit? groundHit;


    public GameObject hitObject
    {
        get
        {
            return hit?.collider?.gameObject;
        }
    }

    public Eye(GameObject go)
    {
        gameObject = go;

    }

}



public class PlayerBrain : MonoBehaviour
{

    public List<GameObject> Eyes;
    public float sightDistance;

    private Eye Eye_FrontBottomLeft;
    private Eye Eye_FrontBottomMid;
    private Eye Eye_FrontBottomRight;
    private Eye Eye_FrontMiddleLeft;
    private Eye Eye_FrontMiddleMid;
    private Eye Eye_FrontMiddleRight;
    private Eye Eye_FrontTopLeft;
    private Eye Eye_FrontTopMid;
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

    private Eye Eye_RightBottomBack;
    private Eye Eye_RightBottomMid;
    private Eye Eye_RightBottomFront;
    private Eye Eye_RightMiddleBack;
    private Eye Eye_RightMiddleMid;
    private Eye Eye_RightMiddleFront;
    private Eye Eye_RightTopBack;
    private Eye Eye_RightTopMid;
    private Eye Eye_RightTopFront;

    private Eye Eye_LeftBottomFront;
    private Eye Eye_LeftBottomMid;
    private Eye Eye_LeftBottomBack;
    private Eye Eye_LeftMiddleFront;
    private Eye Eye_LeftMiddleMid;
    private Eye Eye_LeftMiddleBack;
    private Eye Eye_LeftTopFront;
    private Eye Eye_LeftTopMid;
    private Eye Eye_LeftTopBack;

    private static List<Eye> AllEyes;
    private static List<Eye> CenterEyes;
    private static List<Eye> TopCornerEyes;

    public RaycastHit[] InsideHits;



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
        
    }

    // Update is called once per frame
    void Update()
    {
        SeeThings();
    }

    public bool WithinVisionRange(float SqDistance)
    {
        if (SqDistance < PlayerController.me.currentSightDistance * PlayerController.me.currentSightDistance)
        {
            return true;
        }
        return false;
    }

    private void SeeThings()
    {

        InsideHits = Physics.BoxCastAll(PlayerController.me.transform.position, PlayerController.me.Vector3_CubeHalfExtents, 
            PlayerController.me.transform.up, Quaternion.identity, PlayerController.me.CubeWidth / 2f, (int)LayerMasks.LayerMask_NotGround);

        foreach (ActorController actorController in ActorController.Actors.Where(x => WithinVisionRange(x.SqDistanceFromPlayer)))
        {
            if (WithinVisionRange(actorController.SqDistanceFromPlayer))
            {
                foreach (Eye eye in TopCornerEyes)
                {
                    RaycastHit? rayCast = PhysicsTools.RaycastAt(eye.gameObject.transform.position, actorController.gameObject.transform.position, PlayerController.me.playerStats.SightDistance);
                    if (rayCast != null)
                    {
                        if (rayCast.Value.collider.gameObject.Matches(actorController.gameObject))
                        {
                            actorController.IsSeenByPlayer = true;
                            break;
                        }
                    }


                }
            }
            else
            {

            }
            
        }



        //foreach (Eye eye in AllEyes)
        //{
        //    Ray ray = new Ray(eye.gameObject.transform.position, eye.gameObject.transform.forward);
        //    if (Physics.Raycast(ray, out RaycastHit groundHit, sightDistance, (int)LayerMasks.LayerMask_GroundOnly))
        //    {
        //        eye.groundHit = groundHit;
        //    }
        //    else
        //    {
        //        eye.groundHit = null;
        //    }
        //    if (Physics.Raycast(ray, out RaycastHit edibleHit, sightDistance, (int)LayerMasks.LayerMask_EdibleOnly))
        //    {
        //        eye.edibleHit = edibleHit;
        //    }
        //    else
        //    {
        //        eye.edibleHit = null;
        //    }
        //    if (Physics.Raycast(ray, out RaycastHit hit, sightDistance, (int)LayerMasks.LayerMask_NotPlayer))
        //    {
        //        eye.hit = hit;
        //    }
        //    else
        //    {
        //        eye.hit = null;
        //    }
        //}
    }


    public bool CanTurnRight()
    {

        foreach (Eye eye in new List<Eye>() { Eye_FrontTopLeft, Eye_FrontMiddleLeft, Eye_RightTopFront, Eye_RightMiddleFront })
        {
            if (eye.hit != null && eye.hitObject != null)
            {
                UnityLayers FoundLayer = (UnityLayers)eye.hitObject.layer;
                if (FoundLayer == UnityLayers.Ground && ((RaycastHit)eye.hit).distance < .1f)
                {
                    return false;
                }
            }
        }

        return true;

    }

    public bool CanTurnLeft()
    {

        foreach (Eye eye in new List<Eye>() { Eye_FrontTopRight, Eye_FrontMiddleRight, Eye_LeftTopFront, Eye_LeftMiddleFront })
        {
            if (eye.hit != null && eye.hitObject != null)
            {
                UnityLayers FoundLayer = (UnityLayers)eye.hitObject.layer;
                if (FoundLayer == UnityLayers.Ground && ((RaycastHit)eye.hit).distance < .1f)
                {
                    return false;
                }
            }

        }

        return true;

    }

    public float CanMoveInDirection(float distance, Vector3 castDirection)
    {
 
        RaycastHit hitResult;

        if (PlayerController.me.rb.SweepTest(castDirection, out hitResult, distance, QueryTriggerInteraction.Ignore))
        {
     
                if (hitResult.collider.gameObject.layer == (int)UnityLayers.Ground)
                {
                    return hitResult.distance;
                }



         
        }

        return distance;

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

    public enum UnityLayers
    {
        @Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Ground = 3,
        Water = 4,
        UI = 5,
        Player = 6,
        CanBeEaten = 7,
        BeingEaten = 8
    }

    public enum LayerMasks
    {
        LayerMask_GroundOnly = 1 << UnityLayers.Ground,
        LayerMask_PlayerOnly = 1 << UnityLayers.Player,
        LayerMask_EdibleOnly = 1 << UnityLayers.CanBeEaten,
        LayerMask_NotPlayer = ~(1 << UnityLayers.Player),
        LayerMask_NotGround = ~(1 << UnityLayers.Ground)
    }
}
