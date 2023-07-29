using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using static Shortcuts;

public class SmoothTentacle : MonoBehaviour
{
    [Serialize] public GameObject targetBall;
    /// <summary>
    /// This should be just player and player tentacles.. it is a ray looking for the tentacle region it should be a part of (sides of the cube), but 
    /// should be blockable by tentacles
    /// </summary>
    [Tooltip("Player and tentacles only")]
    [Serialize] public LayerMask TentacleFindingMask;
    /// <summary>
    /// This should be the tentacles only. 
    /// </summary>
    [Tooltip("Tentacles only")]
    [Serialize] public LayerMask TentacleBlockingMask;
    /// <summary>
    /// What can tentacles target
    /// </summary>
    [Serialize] public LayerMask TentacleTargetsMask;

    [Serialize] public FloatVariable currentMaxTentacleReach;
    [Serialize] public FloatVariable currentMinTentacleReach;


    private GameObjectVariable target;
    public BooleanVariable IsAlive;
    private bool IsReady = false;
    private GameObject parentObject;
    private TentacleBrain brain;

    private List<Bounds> tentacleRegions = new List<Bounds>();

    private Vector3 tentacleVector;
    private Vector3 TentacleVector {
        get 
        {
            if (tentacleVector == Vector3.forward)
            {
                return parentRB.transform.forward;
            }
            else if (tentacleVector == -Vector3.forward)
            {
                return -parentRB.transform.forward;
            }
            else if (tentacleVector == Vector3.right)
            {
                return parentRB.transform.right;
            }
            else
            {
                return -parentRB.transform.right;
            }
        }
        set 
        {
            tentacleVector = value;
        }
    }
 
    private Rigidbody parentRB;
    private Dict_GameObjectToLastSeen ObjectsSeen;

    private Bounds? ChosenRegion;


    public void Go(GameObject parent, Rigidbody parentRB, List<Bounds> tentacleRegions, Dict_GameObjectToLastSeen ObjectsSeen)
    {
        
        parentObject = parent;
        this.parentRB = parentRB;
        this.tentacleRegions = tentacleRegions;
        this.ObjectsSeen = ObjectsSeen;
        if (brain != null && parentRB != null && tentacleRegions.Count >= 1 && ObjectsSeen != null)
        {
            IsReady = true;
        }
    }


 

    private void Awake()
    {
        brain = GetComponent<TentacleBrain>();
        IsAlive = Instantiate(SOLibrary.instance.EmptyBooleanVariable);
        IsAlive.Value = true;
        target = Instantiate(SOLibrary.instance.EmptyGameObjectVariable);

    }

    private void InitializeBrain()
    {
        //Retarget
        if (brain.gameObject.TryGetComponent<IAttackThings>(out IAttackThings attackBrain))
        {
            attackBrain.SetImpulse(attackBrain.AttackThings, new WrappedFunc(ReadyWeapon, 1f), new WrappedFunc(MoveTargetMarker, 1f));
        }
        if (brain.gameObject.TryGetComponent<ISpawn>(out ISpawn spawnBrain))
        {
            spawnBrain.SetImpulse(spawnBrain.Spawn, new WrappedFunc(GrowToFullSize, 1f));
        }
        if (brain.gameObject.TryGetComponent<IDespawn>(out IDespawn desspawnBrain))
        {
            desspawnBrain.SetImpulse(desspawnBrain.Despawn, new WrappedFunc(ShrinkToNothing, 1f), new WrappedFunc(Despawn, 0f));
        }
        if (brain.gameObject.TryGetComponent<IRetarget>(out IRetarget retargetBrain))
        {
            retargetBrain.SetImpulse(retargetBrain.Retarget, new WrappedFunc(Retarget, 0f));
        }
        brain.target = target;
        brain.IsAlive = IsAlive;
    }

    public List<ImpulseStep> ShrinkToNothing(float percentElapsed)
    {

        transform.localScale = Vector3.one * (1 - percentElapsed);
        return null;
    }
    public List<ImpulseStep> GrowToFullSize(float percentElapsed)
    {

        transform.localScale = Vector3.one * percentElapsed;
        return null;
    }
    public List<ImpulseStep> ReadyWeapon(float percentElapsed)
    {
        if (target.Value == null) return null;
        Vector3 TargetChange = ((target.Value.transform.position - gameObject.transform.position) / 2f
                 + gameObject.transform.up * ChosenRegion.Value.max.y);

        if (target.Value != null)
        {
            targetBall.transform.position = gameObject.transform.position + TargetChange;
        }
        return null;
    }
    public List<ImpulseStep> MoveTargetMarker(float percentElapsed)
    {
        if (target.Value == null) { return null; }
        targetBall.transform.position = GetMidPointOfObject(target.Value);
        return null;
    }
  



    // Start is called before the first frame update
    void Start()
    {
        InitializeBrain();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckTarget()) target.Value = null;
        //if (target.Value == null) Retarget();
        //if (!IsAlive || target.Value == null) Despawn();
        
    }

    private void FixedUpdate()
    {
        if (target.Value != null)
        {
            //ReadyTentacleStrike();
            //targetBall.transform.position = target.Value.transform.position;
        }
        
    }

    

    public bool CheckTarget()
    {
        if (target.Value != null)
        {
            float sqDistance = (target.Value.transform.position - parentRB.transform.position).sqrMagnitude;
            if (sqDistance > currentMaxTentacleReach.Value * currentMaxTentacleReach.Value 
                || sqDistance < currentMinTentacleReach.Value * currentMinTentacleReach.Value)
            {
                return false;
            }

            //Debug.DrawLine(target.Value.transform.position, transform.position, Color.red);
            //Debug.DrawLine(transform.position + TentacleVector * 20, transform.position, Color.black);
            float dot = Vector3.Dot((target.Value.transform.position - parentObject.transform.position).normalized,
             TentacleVector);
            Debug.DrawLine(parentObject.transform.position, parentObject.transform.position + TentacleVector * 50);

            if (dot < 0)
            {
                return false;
            }

            return true;
        }
        return false;
        
    }

    private List<ImpulseStep> Despawn(float percentElapsed)
    {
        Destroy(gameObject);
        return null;
    }

    private List<ImpulseStep> Retarget(float percentElapsed)
    {
        if (IsReady)
        {
            foreach (GameObject possibleTarget in ObjectsSeen.Value.Keys.OrderBy(x => ObjectsSeen.Value[x].Distance).ToList()) 
            {
                if (!Shortcuts.IsInLayerMask(possibleTarget.layer, TentacleTargetsMask)) continue;

                TargetedByTentacleModifier mod = possibleTarget.GetComponent<TargetedByTentacleModifier>();
                if (mod == null)
                {
                    if (TryFindTentacleTransform(possibleTarget, out TransformVariable newTransform))
                    {
                        transform.SetParent(null);
                        target.Value = possibleTarget;
                        ModifierLibrary.Tentacle.ApplyTargetedByTentacleModifier(target);
                        transform.position = newTransform.position;
                        TentacleVector = newTransform.vector;
                        transform.rotation = Quaternion.identity;
                        transform.rotation *= newTransform.rotation;
                        transform.localScale = newTransform.scale;
                        transform.SetParent(parentObject.transform, true);
                        break;
                    }
                }
            }
            
        }

        return null;

    }
    public bool TryFindTentacleTransform(GameObject possibleTarget, out TransformVariable newTransform)
    {
        bool Success = false;
        newTransform = new TransformVariable();


        Vector3 _direction = (possibleTarget.transform.position - transform.position).normalized;

        float FrontSideDot = Vector3.Dot(_direction, parentObject.transform.forward);
        float RightSideDot = Vector3.Dot(_direction, parentObject.transform.right);

        Vector3 localVector;

        if (Math.Abs(FrontSideDot) > Math.Abs(RightSideDot))
        {
            if (FrontSideDot > 0)
            {
                newTransform.vector = Vector3.forward;
                localVector = parentRB.transform.forward;

            }
            else
            {
                newTransform.vector = -Vector3.forward;
                localVector = -parentRB.transform.forward;

            }
        }
        else
        {
            if (RightSideDot > 0)
            {
                newTransform.vector = Vector3.right;
                localVector = parentRB.transform.right;
            }
            else
            {
                newTransform.vector = -Vector3.right;
                localVector = -parentRB.transform.right;

            }
        }
        newTransform.rotation = Quaternion.LookRotation(localVector);
        newTransform.scale = Vector3.zero;


        

        ChosenRegion = null;
        float ChosenSqMagnitude = Mathf.Infinity;
        foreach (Bounds region in tentacleRegions)
        {

            RaycastHit? hit = PhysicsTools.RaycastAt(possibleTarget.transform.position, parentRB.transform.TransformPoint(region.center), 
                Mathf.Infinity, TentacleFindingMask);
            if (hit != null)
            {
                float SqDistanceBetweenRegionAndHit = (hit.Value.point - region.center).sqrMagnitude;
                if (SqDistanceBetweenRegionAndHit < ChosenSqMagnitude)
                {
                    ChosenRegion = region;
                    ChosenSqMagnitude = SqDistanceBetweenRegionAndHit;
                }

            }


        }

        if (ChosenRegion != null)
        {
           
            Vector3? RandomPointInBounds = PickRandomPointInBounds(possibleTarget, ChosenRegion.Value, TentacleFindingMask, TentacleBlockingMask);
            
            if (RandomPointInBounds != null)
            {
                newTransform.position = RandomPointInBounds.Value;
                if (newTransform.position.y > ChosenRegion.Value.max.y - ChosenRegion.Value.size.y * .2f)
                {
                    return false;
                }

                Success = true;
            }
        }



        return Success;
    }

    public Vector3? PickRandomPointInBounds(GameObject possibleTarget, Bounds bounds, LayerMask? FindingLayerMask = null, LayerMask? BlockingMask = null, int NumberOfAttempts = 15)
    {
        int Attempts = 0;
        float SideMargin = .8f; //Space on side of blob minus space to not spawn tentacle in
        float Inset = (1 - SideMargin) / 2f; //Space on side of blob minus space to not spawn tentacle in

        while (Attempts < NumberOfAttempts)
        {
            Attempts++;
            Vector3 randomPoint = new Vector3(
                UnityEngine.Random.Range(0f, bounds.size.x * SideMargin) - bounds.size.x / 2f + bounds.size.x * 1.5f * Inset,
               UnityEngine.Random.Range(0f, bounds.size.y * SideMargin) - bounds.size.y / 2f + bounds.size.y * Inset,
               UnityEngine.Random.Range(0f, bounds.size.z * SideMargin) - bounds.size.z / 2f + bounds.size.z * Inset);

            randomPoint = parentRB.transform.TransformPoint(randomPoint);

            RaycastHit? hit = PhysicsTools.RaycastAt(GetMidPointOfObject(possibleTarget), randomPoint, Mathf.Infinity, TentacleFindingMask);
            if (hit != null && hit.Value.collider.gameObject.layer != BlockingMask) //Let other tentacles block
            {
                return hit.Value.point;

            }

        }

        return null;


        }

    private Vector3 GetMidPointOfObject(GameObject target)
    {
        Vector3 targetCenter;
        Renderer renderer = target.GetComponent<Renderer>();

        if (renderer != null)
        {
            targetCenter = renderer.bounds.center;

            return targetCenter;

        }
        Vector3 sumVector = new Vector3(0f, 0f, 0f);

        foreach (Transform child in target.transform)
        {
            sumVector += child.position;
        }

        targetCenter = sumVector / target.transform.childCount;
        return targetCenter;
    }
}
