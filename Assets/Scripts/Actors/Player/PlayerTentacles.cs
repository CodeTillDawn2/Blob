using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static HelperClasses;

public class Tentacle
{
    public GameObject target;
    public GameObject tentacleObject;
    public GameObject cubeSide;
    public bool HasPrey;
    public Vector3 AttachPosition;
    public Vector3 AttachOrientation;
    internal float tentacleWidth;



    public Tentacle(GameObject obj, GameObject cubeSide, Vector3 AttachPosition)
    {

        HasPrey = false;
        tentacleObject = obj;
        this.cubeSide = cubeSide;
        this.AttachPosition = AttachPosition;
        target = obj.GetComponent<TentacleController>().target;

    }



    public Rigidbody rb
    {
        get
        {
            Rigidbody returnRB = tentacleObject.GetComponentInChildren<Rigidbody>();
            return returnRB;
        }
    }



}

public class PlayerTentacles : MonoBehaviour
{

    [Header("Stat Block")]
    [Serialize] public IntegerVariable TentacleCount;
    [Serialize] public IntegerVariable MaxTentacles;
    [Serialize] public FloatVariable CurrentTentacleReach;
    [Serialize] public FloatVariable CubeWidth;


    [HideInInspector]
    public List<global::Tentacle> Tentacles = new List<global::Tentacle>();
    public GameObject TentaclePrefab;

    private int TentacleID = 1;


    public delegate string TentacleCreatedAction(object sender, EventArgs args);
    public static event TentacleCreatedAction TentacleCreatedEvent;
    private void OnTentacleCreatedEvent(EventArgs e)
    {
        if (TentacleCreatedEvent != null)
        {
            string myString = TentacleCreatedEvent(this, e);
        }


    }


    private void Start()
    {
   
    }


    // Update is called once per frame
    void Update()
    {
        CreateTentacles();
    }

    private void FixedUpdate()
    {

    }





    public bool WithinTentacleReach(ActorController actor)
    {
        return CurrentTentacleReach.Value * CurrentTentacleReach.Value >= actor.SqDistanceFromPlayer;
    }
    public GameObject FrontSide;
    public GameObject BackSide;
    public GameObject LeftSide;
    public GameObject RightSide;
    public GameObject TopSide;
    public GameObject BottomSide;
    /// <summary>
    /// Provide game object and cardinal direction to return whether a game object is a specific cube side. Use Vector3.up etc
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="CardinalDirection"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool IsCubeSide(GameObject obj, Vector3 CardinalDirection)
    {
        GameObject sideToCheck;
        if (CardinalDirection == Vector3.forward)
        {
            sideToCheck = FrontSide;
        }
        else if (CardinalDirection == -Vector3.forward)
        {
            sideToCheck = BackSide;
        }
        else if (CardinalDirection == Vector3.up)
        {
            sideToCheck = TopSide;
        }
        else if (CardinalDirection == -Vector3.up)
        {
            sideToCheck = BottomSide;
        }
        else if (CardinalDirection == Vector3.right)
        {
            sideToCheck = RightSide;
        }
        else if (CardinalDirection == -Vector3.right)
        {
            sideToCheck = LeftSide;
        }
        else
        {
            throw new NotImplementedException();
        }


        if (Shortcuts.PathMatches(obj, sideToCheck))
        {
            return true;
        }
        return false;
    }



    public Vector3 FindCubeOrientation(GameObject cubeSide)
    {
        if (IsCubeSide(cubeSide, Vector3.up)) //Top side collision
        {
            return Vector3.up;
        }
        else if (IsCubeSide(cubeSide, Vector3.down)) //Bottom side collision
        {
            return Vector3.down;
        }
        else if (IsCubeSide(cubeSide, Vector3.forward)) //Front side collision
        {
            return Vector3.forward;
        }
        else if (IsCubeSide(cubeSide, Vector3.back)) //Back side collision
        {
            return Vector3.back;
        }
        else if (IsCubeSide(cubeSide, Vector3.left)) //Left side collision
        {
            return Vector3.left;
        }
        else if (IsCubeSide(cubeSide, Vector3.right)) //Right side collision
        {
            return Vector3.right;
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public GameObject GetCubeSideObject(GameObject obj)
    {

        if (Shortcuts.PathMatches(obj, FrontSide))
        {
            return FrontSide;
        }
        else if (Shortcuts.PathMatches(obj, BackSide))
        {
            return BackSide;
        }
        else if (Shortcuts.PathMatches(obj, LeftSide))
        {
            return LeftSide;
        }
        else if (Shortcuts.PathMatches(obj, RightSide))
        {
            return RightSide;
        }
        else if (Shortcuts.PathMatches(obj, TopSide))
        {
            return TopSide;
        }
        else if (Shortcuts.PathMatches(obj, BottomSide))
        {
            return BottomSide;
        }
        else
        {
            return null;
        }
    }

    public Vector3 PlayerCenter
    {
        get { return base.transform.position + new Vector3(0, CubeWidth.Value / 2f, 0); }
    }

    public void CreateTentacles()
    {
        List<ActorController> victims = ActorController.Actors.Where(x => x.IsSeenByPlayer && !x.TargetedByTentacle && WithinTentacleReach(x)).OrderBy(x => x.SqDistanceFromPlayer).ToList();

        while (TentacleCount.Value < 0)
        //while (currentTentacles < currentMaxTentacles)
        {

            ActorController victim = victims.Where(x => x.IsSeenByPlayer && !x.TargetedByTentacle).OrderBy(x => x.SqDistanceFromPlayer).FirstOrDefault();
            if (victim != null)
            {

                //Victim raycast
                RaycastHit? raycast = PhysicsTools.RaycastAt(victim.gameObject.transform.position, PlayerCenter, Mathf.Infinity, (int)Shortcuts.LayerMasks.LayerMask_PlayerOnly);


                GameObject cubeSide = GetCubeSideObject(raycast.Value.collider.gameObject);
                if (cubeSide == null)
                {
                    string test = "";
                }

                if (cubeSide != null)
                {
                    //Make sure no two tentacles end up too close together and rotation is correct
                    Vector3 location = raycast.Value.point;

                    Vector3 NewTentacleOrientation = FindCubeOrientation(cubeSide);
                    Transform TopLeftPoint;
                    Transform TopRightPoint;
                    Transform BottomLeftPoint;
                    Transform BottomRightPoint;
                    Quaternion rotation;
                    Vector3 UpVector;
                    Vector3 DownVector;
                    Vector3 RightVector;
                    Vector3 LeftVector;
                    Vector3 OutVector; ;

                    throw new NotImplementedException();

                    if (NewTentacleOrientation == Vector3.up) //Oriented looking from the back
                    {

                        //TopLeftPoint = PlayerManager.me.Brain.Eye_FrontTopLeft.gameObject.transform;//
                        //TopRightPoint = PlayerManager.me.Brain.Eye_FrontTopRight.gameObject.transform;//
                        //BottomLeftPoint = PlayerManager.me.Brain.Eye_BackTopLeft.gameObject.transform;//
                        //BottomRightPoint = PlayerManager.me.Brain.Eye_BackTopRight.gameObject.transform;//
                        rotation = transform.rotation * Quaternion.Euler(-90, 0, 0);
                        UpVector = transform.forward;
                        DownVector = -transform.forward;
                        RightVector = transform.right;
                        LeftVector = -transform.right;
                        OutVector = transform.up;


                    }
                    else if (NewTentacleOrientation == Vector3.down) //Oriented looking from the back
                    {
                        //TopLeftPoint = PlayerManager.me.Brain.Eye_BackBottomLeft.gameObject.transform;//
                        //TopRightPoint = PlayerManager.me.Brain.Eye_BackBottomRight.gameObject.transform;//
                        //BottomLeftPoint = PlayerManager.me.Brain.Eye_FrontBottomLeft.gameObject.transform;//
                        //BottomRightPoint = PlayerManager.me.Brain.Eye_FrontBottomRight.gameObject.transform;//
                        rotation = transform.rotation * Quaternion.Euler(90, 0, 0);
                        UpVector = -transform.forward;
                        DownVector = transform.forward;
                        RightVector = -transform.right;
                        LeftVector = transform.right;
                        OutVector = -transform.up;
                    }
                    else if (NewTentacleOrientation == Vector3.right) //Oriented looking from the back
                    {
                        //TopLeftPoint = PlayerManager.me.Brain.Eye_RightTopBack.gameObject.transform;//
                        //TopRightPoint = PlayerManager.me.Brain.Eye_RightTopFront.gameObject.transform;//
                        //BottomLeftPoint = PlayerManager.me.Brain.Eye_RightBottomBack.gameObject.transform;//
                        //BottomRightPoint = PlayerManager.me.Brain.Eye_RightBottomFront.gameObject.transform;//
                        rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
                        UpVector = transform.up;
                        DownVector = -transform.up;
                        RightVector = transform.forward;
                        LeftVector = -transform.forward;
                        OutVector = transform.right;
                    }
                    else if (NewTentacleOrientation == Vector3.left) //Oriented looking from the back
                    {
                        //TopLeftPoint = PlayerManager.me.Brain.Eye_LeftTopFront.gameObject.transform;//
                        //TopRightPoint = PlayerManager.me.Brain.Eye_LeftTopBack.gameObject.transform;//
                        //BottomLeftPoint = PlayerManager.me.Brain.Eye_LeftBottomFront.gameObject.transform;//
                        //BottomRightPoint = PlayerManager.me.Brain.Eye_LeftBottomBack.gameObject.transform;//
                        rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
                        UpVector = transform.up;
                        DownVector = -transform.up;
                        RightVector = -transform.forward;
                        LeftVector = transform.forward;
                        OutVector = -transform.right;
                    }
                    else if (NewTentacleOrientation == Vector3.forward) //Oriented looking from the back
                    {
                        //TopLeftPoint = PlayerManager.me.Brain.Eye_FrontTopLeft.gameObject.transform;//
                        //TopRightPoint = PlayerManager.me.Brain.Eye_FrontTopRight.gameObject.transform;//
                        //BottomLeftPoint = PlayerManager.me.Brain.Eye_FrontBottomLeft.gameObject.transform;//
                        //BottomRightPoint = PlayerManager.me.Brain.Eye_FrontBottomRight.gameObject.transform;//
                        rotation = transform.rotation;
                        UpVector = transform.up;
                        DownVector = -transform.up;
                        RightVector = -transform.right;
                        LeftVector = transform.right;
                        OutVector = transform.forward;
                    }
                    else if (NewTentacleOrientation == Vector3.back) //Oriented looking from the back
                    {
                        //TopLeftPoint = PlayerManager.me.Brain.Eye_BackTopLeft.gameObject.transform;//
                        //TopRightPoint = PlayerManager.me.Brain.Eye_BackTopRight.gameObject.transform;//
                        //BottomLeftPoint = PlayerManager.me.Brain.Eye_BackBottomLeft.gameObject.transform;//
                        //BottomRightPoint = PlayerManager.me.Brain.Eye_BackBottomRight.gameObject.transform;//
                        rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                        UpVector = transform.up;
                        DownVector = -transform.up;
                        RightVector = transform.right;
                        LeftVector = -transform.right;
                        OutVector = -transform.forward;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    LocRoc locRoc = new LocRoc(location, rotation);


                    List<global::Tentacle> PotentialConflicts = Tentacles.Where(x => x.AttachOrientation == NewTentacleOrientation).ToList();
                    bool ConflictExists = false;
                    foreach (global::Tentacle conflictingTentacle in PotentialConflicts)
                    {
                        if (PhysicsTools.GetSqDistanceBetweenPoints(locRoc.location, conflictingTentacle.AttachPosition) < 10)
                        {
                            ConflictExists = true;
                            int resolutionAttempts = 0;

                            while (ConflictExists && resolutionAttempts < 10)
                            {
                                float WidthToWorkWith = (TopRightPoint.position - TopLeftPoint.position).magnitude;
                                float HeightToWorkWith = (TopRightPoint.position - BottomRightPoint.position).magnitude;

                                float randomX = UnityEngine.Random.Range(WidthToWorkWith / 10, WidthToWorkWith / 10 * 9);
                                float randomY = UnityEngine.Random.Range(HeightToWorkWith / 10, HeightToWorkWith / 10 * 9);

                                locRoc.location = PhysicsTools.PointAlongDirection(TopRightPoint.position, transform.rotation * RightVector, randomX);

                                ConflictExists = false;
                                foreach (global::Tentacle tentcl in PotentialConflicts)
                                {
                                    if (PhysicsTools.GetSqDistanceBetweenPoints(locRoc.location, tentcl.AttachPosition) < 10)
                                    {
                                        ConflictExists = true;
                                        break;
                                    }
                                }
                                resolutionAttempts++;
                            }
                        }
                    }



                    GameObject tentacleobj = Instantiate(TentaclePrefab, locRoc.location, locRoc.rotation, base.transform);
                    tentacleobj.name = "Tentacle" + TentacleID;
                    TentacleID++;
                    global::Tentacle tentacle = new global::Tentacle(tentacleobj, cubeSide, location);
                    //FixedJoint joint = PlayerManager.me.PlayerGameObject.AddComponent<FixedJoint>();
                    //joint.connectedBody = tentacle.rb;
                    victim.TargetedByTentacle = true;
                    tentacle.target.transform.position = victim.transform.position;
                    tentacleobj.layer = (int)Shortcuts.UnityLayers.PlayerTentacle;
                    Tentacles.Add(tentacle);
                    TentacleCount.Value++;
                    OnTentacleCreatedEvent(EventArgs.Empty);
                }
                else
                {
                    throw new NotImplementedException();
                }

            }
            else
            {
                break;
            }


        }
        if (TentacleCount.Value == 0)
        {
            TentacleID = 1;
        }
    }





}
