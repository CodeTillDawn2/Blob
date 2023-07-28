using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
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

    [Serialize] public FloatVariable currentMaxTentacleReach;
    [Serialize] public FloatVariable currentMinTentacleReach;


    private GameObjectVariable target;
    public bool IsAlive = true;
    private bool IsReady = false;
    private GameObject parentObject;

    private List<Bounds> tentacleRegions = new List<Bounds>();

    
 
    private Rigidbody parentRB;
    private Dict_GameObjectToLastSeen ObjectsSeen;


    public void GoTime(GameObject parent, Rigidbody parentRB, List<Bounds> tentacleRegions, Dict_GameObjectToLastSeen ObjectsSeen)
    {
        parentObject = parent;
        this.parentRB = parentRB;
        this.tentacleRegions = tentacleRegions;
        this.ObjectsSeen = ObjectsSeen;
        if (parentRB != null && tentacleRegions.Count >= 1 && ObjectsSeen != null)
        {
            IsReady = true;
        }
    }

    private void Awake()
    {
        print("Doing this");
        target = Instantiate(SOLibrary.instance.EmptyGameObjectVariable);
        string test = "";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckTarget()) target.Value = null;
        if (target.Value == null) Retarget();
        if (!IsAlive || target.Value == null) Despawn();
        
    }

    public bool CheckTarget()
    {
        if (target.Value != null)
        {
            float sqDistance = (target.Value.transform.position - parentRB.transform.position).sqrMagnitude;
            if (sqDistance > currentMaxTentacleReach.Value || sqDistance < currentMinTentacleReach.Value)
            {
                return false;
            }

            return true;
        }
        return false;
        
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }

    private void Retarget()
    {
        if (IsReady)
        {
            foreach (GameObject possibleTarget in ObjectsSeen.Value.Keys.OrderBy(x => ObjectsSeen.Value[x].Distance).ToList()) 
            {
                TargetedByTentacleModifier mod = possibleTarget.GetComponent<TargetedByTentacleModifier>();
                if (mod == null)
                {
                    if (TryFindTentacleTransform(possibleTarget, out TransformVariable newTransform))
                    {
                        transform.SetParent(null);
                        target.Value = possibleTarget;
                        ModifierLibrary.Tentacle.ApplyTargetedByTentacleModifier(target);
                        transform.position = newTransform.position;
                        print(newTransform.rotation);
                        transform.rotation = Quaternion.identity;
                        transform.rotation *= newTransform.rotation;
                        transform.localScale = newTransform.scale;
                        transform.SetParent(parentObject.transform, true);
                        break;
                    }
                }
            }
            
        }

    }
    public bool TryFindTentacleTransform(GameObject possibleTarget, out TransformVariable newTransform)
    {
        bool Success = false;
        newTransform = new TransformVariable();


        Vector3 _direction = (possibleTarget.transform.position - transform.position).normalized;

        float FrontSideDot = Vector3.Dot(_direction, parentObject.transform.forward);
        float RightSideDot = Vector3.Dot(_direction, parentObject.transform.right);

        if (Math.Abs(FrontSideDot) > Math.Abs(RightSideDot))
        {
            if (FrontSideDot > 0)
            {
                newTransform.vector = parentRB.transform.forward;
                
            }
            else
            {
                newTransform.vector = -parentRB.transform.forward;
               
            }
        }
        else
        {
            if (RightSideDot > 0)
            {
                newTransform.vector = parentRB.transform.right;
            }
            else
            {
                newTransform.vector = -parentRB.transform.right;
                
            }
        }
        newTransform.rotation = Quaternion.LookRotation(newTransform.vector);
        newTransform.scale = Vector3.one;


        

        Bounds? ChosenRegion = null;
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
                Debug.DrawLine(possibleTarget.transform.position, RandomPointInBounds.Value, Color.red);
                newTransform.position = RandomPointInBounds.Value;
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
