using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;




public class BlobTentacles : MonoBehaviour
{

    [Header("Stat Block")]
    [Serialize] public IntegerVariable TentacleCount;
    [Serialize] public IntegerVariable CurrentMaxTentacles;
    [Serialize] public FloatVariable CurrentTentacleReach;
    [Serialize] public Vector3Variable BlobDims;
    [Serialize] public PlayerScriptableObject StartingStats;
    [Serialize] public Dict_GameObjectToLastSeen ObjectsSeen;
    [Serialize] public Dict_GameObjectToGameObject TentacleTargeting;

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
        CurrentTentacleReach.Value = StartingStats.TentacleReach;
        CurrentMaxTentacles.Value = StartingStats.MaxTentacles;
        TentacleCount.Value = 0;
    }


    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        CreateTentacles();
    }





    //public bool WithinTentacleReach(ActorController actor)
    //{
    //    return CurrentTentacleReach.Value * CurrentTentacleReach.Value >= actor.SqDistanceFromPlayer;
    //}

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



    public GameObject FindClosestCubeSide(GameObject target)
    {
        bool SideFound = false;
        string side = "";
        Vector3 _direction = (target.transform.position - transform.position).normalized;

        float FrontSideDot = Vector3.Dot(_direction, transform.forward);
        float RightSideDot = Vector3.Dot(_direction, transform.right);

        if (Math.Abs(FrontSideDot) > Math.Abs(RightSideDot))
        {
            if (FrontSideDot > 0)  
            {
                side = "front";
            }
            else
            {
                side = "back";
            }
        }
        else
        {
            if (RightSideDot > 0)
            {
                side = "right";
            }
            else
            {
                side = "left";
            }
        }

        return this.gameObject;
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
        get { return base.transform.position + new Vector3(0, BlobDims.Value.y / 2f, 0); }
    }



    public void CreateTentacles()
    {
        List<GameObject> potentialvictims = ObjectsSeen.Value.Keys.Where(x => x != null)
            .OrderBy(x => (x.transform.position - gameObject.transform.position).sqrMagnitude).ToList();

        //Existing tentacles
        for (int tentacleID = TentacleTargeting.Value.Count-1; tentacleID >= 0; tentacleID--)
        {
            
            GameObject tentacle = TentacleTargeting.Value.Keys.ToList()[tentacleID];
            GameObject existingTarget = TentacleTargeting.Value[tentacle];

            if (tentacleID > CurrentMaxTentacles.Value - 1 && existingTarget == null)
            {
                TentacleTargeting.Value.Remove(tentacle);
                Destroy(tentacle);
                continue;
            }

            foreach (GameObject target in potentialvictims)
            {
                if (!TentacleTargeting.Value.Keys.ToList().Contains(target))
                {
                    GameObject CubeSide = FindClosestCubeSide(target);

                }
            }

        }

        //Remove excess
        while (TentacleTargeting.Value.Count > CurrentMaxTentacles.Value)
        {
            TentacleTargeting.Value.Remove(TentacleTargeting.Value.Keys.ToList()[0]);
        }

        //New Tentacles
        for (int tentacleID = TentacleTargeting.Value.Count; tentacleID < CurrentMaxTentacles.Value; tentacleID++)
        {
            foreach (GameObject target in potentialvictims)
            {
                if (!TentacleTargeting.Value.Values.ToList().Contains(target))
                {
                    GameObject CubeSide = FindClosestCubeSide(target);

                    break;
                }
            }
                
        }



        //    while (TentacleCount.Value < 1)
        //    //while (currentTentacles < currentMaxTentacles)
        //    {




        //        //ActorController victim = victims.Where(x => x.IsSeenByPlayer && !x.TargetedByTentacle).OrderBy(x => x.SqDistanceFromPlayer).FirstOrDefault();
        //        //if (victim != null)
        //        //{

        //            //Victim raycast
        //            RaycastHit? raycast = PhysicsTools.RaycastAt(victim.transform.position, PlayerCenter, Mathf.Infinity, (int)Shortcuts.LayerMasks.LayerMask_PlayerOnly);


        //            GameObject cubeSide = GetCubeSideObject(raycast.Value.collider.gameObject);
        //            if (cubeSide == null)
        //            {
        //                string test = "";
        //            }

        //            if (cubeSide != null)
        //            {
        //                //Make sure no two tentacles end up too close together and rotation is correct
        //                Vector3 location = raycast.Value.point;

        //                Vector3 NewTentacleOrientation = FindCubeOrientation(cubeSide);
        //                Transform TopLeftPoint;
        //                Transform TopRightPoint;
        //                Transform BottomLeftPoint;
        //                Transform BottomRightPoint;
        //                Quaternion rotation;
        //                Vector3 UpVector;
        //                Vector3 DownVector;
        //                Vector3 RightVector;
        //                Vector3 LeftVector;
        //                Vector3 OutVector; ;

        //                throw new NotImplementedException();

        //                if (NewTentacleOrientation == Vector3.up) //Oriented looking from the back
        //                {

        //                    //TopLeftPoint = PlayerManager.me.Brain.Eye_FrontTopLeft.gameObject.transform;//
        //                    //TopRightPoint = PlayerManager.me.Brain.Eye_FrontTopRight.gameObject.transform;//
        //                    //BottomLeftPoint = PlayerManager.me.Brain.Eye_BackTopLeft.gameObject.transform;//
        //                    //BottomRightPoint = PlayerManager.me.Brain.Eye_BackTopRight.gameObject.transform;//
        //                    rotation = transform.rotation * Quaternion.Euler(-90, 0, 0);
        //                    UpVector = transform.forward;
        //                    DownVector = -transform.forward;
        //                    RightVector = transform.right;
        //                    LeftVector = -transform.right;
        //                    OutVector = transform.up;


        //                }
        //                else if (NewTentacleOrientation == Vector3.down) //Oriented looking from the back
        //                {
        //                    //TopLeftPoint = PlayerManager.me.Brain.Eye_BackBottomLeft.gameObject.transform;//
        //                    //TopRightPoint = PlayerManager.me.Brain.Eye_BackBottomRight.gameObject.transform;//
        //                    //BottomLeftPoint = PlayerManager.me.Brain.Eye_FrontBottomLeft.gameObject.transform;//
        //                    //BottomRightPoint = PlayerManager.me.Brain.Eye_FrontBottomRight.gameObject.transform;//
        //                    rotation = transform.rotation * Quaternion.Euler(90, 0, 0);
        //                    UpVector = -transform.forward;
        //                    DownVector = transform.forward;
        //                    RightVector = -transform.right;
        //                    LeftVector = transform.right;
        //                    OutVector = -transform.up;
        //                }
        //                else if (NewTentacleOrientation == Vector3.right) //Oriented looking from the back
        //                {
        //                    //TopLeftPoint = PlayerManager.me.Brain.Eye_RightTopBack.gameObject.transform;//
        //                    //TopRightPoint = PlayerManager.me.Brain.Eye_RightTopFront.gameObject.transform;//
        //                    //BottomLeftPoint = PlayerManager.me.Brain.Eye_RightBottomBack.gameObject.transform;//
        //                    //BottomRightPoint = PlayerManager.me.Brain.Eye_RightBottomFront.gameObject.transform;//
        //                    rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
        //                    UpVector = transform.up;
        //                    DownVector = -transform.up;
        //                    RightVector = transform.forward;
        //                    LeftVector = -transform.forward;
        //                    OutVector = transform.right;
        //                }
        //                else if (NewTentacleOrientation == Vector3.left) //Oriented looking from the back
        //                {
        //                    //TopLeftPoint = PlayerManager.me.Brain.Eye_LeftTopFront.gameObject.transform;//
        //                    //TopRightPoint = PlayerManager.me.Brain.Eye_LeftTopBack.gameObject.transform;//
        //                    //BottomLeftPoint = PlayerManager.me.Brain.Eye_LeftBottomFront.gameObject.transform;//
        //                    //BottomRightPoint = PlayerManager.me.Brain.Eye_LeftBottomBack.gameObject.transform;//
        //                    rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
        //                    UpVector = transform.up;
        //                    DownVector = -transform.up;
        //                    RightVector = -transform.forward;
        //                    LeftVector = transform.forward;
        //                    OutVector = -transform.right;
        //                }
        //                else if (NewTentacleOrientation == Vector3.forward) //Oriented looking from the back
        //                {
        //                    //TopLeftPoint = PlayerManager.me.Brain.Eye_FrontTopLeft.gameObject.transform;//
        //                    //TopRightPoint = PlayerManager.me.Brain.Eye_FrontTopRight.gameObject.transform;//
        //                    //BottomLeftPoint = PlayerManager.me.Brain.Eye_FrontBottomLeft.gameObject.transform;//
        //                    //BottomRightPoint = PlayerManager.me.Brain.Eye_FrontBottomRight.gameObject.transform;//
        //                    rotation = transform.rotation;
        //                    UpVector = transform.up;
        //                    DownVector = -transform.up;
        //                    RightVector = -transform.right;
        //                    LeftVector = transform.right;
        //                    OutVector = transform.forward;
        //                }
        //                else if (NewTentacleOrientation == Vector3.back) //Oriented looking from the back
        //                {
        //                    //TopLeftPoint = PlayerManager.me.Brain.Eye_BackTopLeft.gameObject.transform;//
        //                    //TopRightPoint = PlayerManager.me.Brain.Eye_BackTopRight.gameObject.transform;//
        //                    //BottomLeftPoint = PlayerManager.me.Brain.Eye_BackBottomLeft.gameObject.transform;//
        //                    //BottomRightPoint = PlayerManager.me.Brain.Eye_BackBottomRight.gameObject.transform;//
        //                    rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
        //                    UpVector = transform.up;
        //                    DownVector = -transform.up;
        //                    RightVector = transform.right;
        //                    LeftVector = -transform.right;
        //                    OutVector = -transform.forward;
        //                }
        //                else
        //                {
        //                    throw new NotImplementedException();
        //                }

        //                LocRoc locRoc = new LocRoc(location, rotation);


        //                List<global::Tentacle> PotentialConflicts = Tentacles.Where(x => x.AttachOrientation == NewTentacleOrientation).ToList();
        //                bool ConflictExists = false;
        //                foreach (global::Tentacle conflictingTentacle in PotentialConflicts)
        //                {
        //                    if (PhysicsTools.GetSqDistanceBetweenPoints(locRoc.location, conflictingTentacle.AttachPosition) < 10)
        //                    {
        //                        ConflictExists = true;
        //                        int resolutionAttempts = 0;

        //                        while (ConflictExists && resolutionAttempts < 10)
        //                        {
        //                            float WidthToWorkWith = (TopRightPoint.position - TopLeftPoint.position).magnitude;
        //                            float HeightToWorkWith = (TopRightPoint.position - BottomRightPoint.position).magnitude;

        //                            float randomX = UnityEngine.Random.Range(WidthToWorkWith / 10, WidthToWorkWith / 10 * 9);
        //                            float randomY = UnityEngine.Random.Range(HeightToWorkWith / 10, HeightToWorkWith / 10 * 9);

        //                            locRoc.location = PhysicsTools.PointAlongDirection(TopRightPoint.position, transform.rotation * RightVector, randomX);

        //                            ConflictExists = false;
        //                            foreach (global::Tentacle tentcl in PotentialConflicts)
        //                            {
        //                                if (PhysicsTools.GetSqDistanceBetweenPoints(locRoc.location, tentcl.AttachPosition) < 10)
        //                                {
        //                                    ConflictExists = true;
        //                                    break;
        //                                }
        //                            }
        //                            resolutionAttempts++;
        //                        }
        //                    }
        //                }



        //                GameObject tentacleobj = Instantiate(TentaclePrefab, locRoc.location, locRoc.rotation, base.transform);
        //                tentacleobj.name = "Tentacle" + TentacleID;
        //                TentacleID++;
        //                global::Tentacle tentacle = new global::Tentacle(tentacleobj, cubeSide, location);
        //                //FixedJoint joint = PlayerManager.me.PlayerGameObject.AddComponent<FixedJoint>();
        //                //joint.connectedBody = tentacle.rb;
        //                victim.TargetedByTentacle = true;
        //                tentacle.target.transform.position = victim.transform.position;
        //                tentacleobj.layer = (int)Shortcuts.UnityLayers.PlayerTentacle;
        //                Tentacles.Add(tentacle);
        //                TentacleCount.Value++;
        //                OnTentacleCreatedEvent(EventArgs.Empty);
        //            }
        //            else
        //            {
        //                throw new NotImplementedException();
        //            }

        //        //}
        //        //else
        //        //{
        //        //    break;
        //        //}


        //    }
        //    if (TentacleCount.Value == 0)
        //    {
        //        TentacleID = 1;
        //    }
    }





}
