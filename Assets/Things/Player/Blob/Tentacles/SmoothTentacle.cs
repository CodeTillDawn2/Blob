using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SmoothTentacle : MonoBehaviour
{

    #region Public Variables
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
    [Serialize] public FloatVariable TentacleHitSpeed;
    public GameObjectVariable target;
    public BooleanVariable IsAlive;
    #endregion

    #region Animator Statuses
    private bool HasTarget
    {
        get { return animator.GetBool("HasTarget"); }
        set { animator.SetBool("HasTarget", value); }
    }

    #endregion

    #region Private Variables
    private GameObject parentObject;
    private TentacleBrain brain;
    private Animator animator;
    private List<Bounds> tentacleRegions = new List<Bounds>();

    private Vector3 tentacleVector;
    private Vector3 TentacleVector
    {
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
    #endregion

    #region Unity Events
    private void Awake()
    {

        animator = GetComponent<Animator>();
        brain = GetComponent<TentacleBrain>();
        IsAlive = Instantiate(SOLibrary.instance.EmptyBooleanVariable);
        IsAlive.Value = true;

        target = Instantiate(SOLibrary.instance.EmptyGameObjectVariable);


    }
    protected void Start()
    {
        InitializeBrain();
    }


    void Update()
    {
        if (!CheckTarget())
        {

            target.Value = null;
        }

        UpdateAnimator();


    }

    private void FixedUpdate()
    {
        if (target.Value != null)
        {
            //ReadyTentacleStrike();
            //targetBall.transform.position = target.Value.transform.position;
        }

    }

    private void LateUpdate()
    {
        UpdateTargetBall();
        UpdateAnimatorState();
    }

    #endregion

    #region Initial Setup
    public void Go(GameObject parent, Rigidbody parentRB, List<Bounds> tentacleRegions, Dict_GameObjectToLastSeen ObjectsSeen)
    {

        parentObject = parent;
        this.parentRB = parentRB;
        this.tentacleRegions = tentacleRegions;
        this.ObjectsSeen = ObjectsSeen;
        transform.SetParent(parentObject.transform, true);

    }

    private void InitializeBrain()
    {
        brain.target = target;
        brain.IsAlive = IsAlive;

        //Retarget
        //AttemptTentacleAttackStep = new ImpulseStep(StepAction:AttemptTentacleAttack,
        //                                   OnlyDoIf: null, 
        //                                   WaitUntilTrue: DoUntilTest);
        //AttemptTentacleAttackStep.impulseStepNameDebug = "AttemptTentacleAttackStep";
        //brain.AddImpulse(Impulse.ImpulseType.Attack, CanReachTarget, AttemptTentacleAttackStep);

        ShrinkStep = new ImpulseStep(StepAction: ShrinkToNothing,
                                           OnlyDoIf: null,
                                           WaitUntilTrue: null);
        ShrinkStep.impulseStepNameDebug = "ShrinkStep";
        brain.AddImpulse(Impulse.ImpulseType.Despawn, null, ShrinkStep);


        GrowStep = new ImpulseStep(StepAction: GrowToFullSize,
                                           OnlyDoIf: null,
                                           WaitUntilTrue: null);
        GrowStep.impulseStepNameDebug = "GrowStep";
        brain.AddImpulse(Impulse.ImpulseType.Spawn, null, GrowStep);

        RetargetStep = new ImpulseStep(StepAction: Retarget,
                                   OnlyDoIf: null,
                                   WaitUntilTrue: DoUntilTest);
        RetargetStep.impulseStepNameDebug = "RetargetStep";
        brain.AddImpulse(Impulse.ImpulseType.Search, null, RetargetStep);

        RelocateStep = new ImpulseStep(StepAction: Relocate,
                           OnlyDoIf: null,
                           WaitUntilTrue: DoUntilTest);
        RelocateStep.impulseStepNameDebug = "RelocateStep";
        brain.AddImpulse(Impulse.ImpulseType.Move, null, RelocateStep);
    }


    #endregion


    #region Impulse Steps

    private ImpulseStep AttemptTentacleAttackStep;
    private ImpulseStep ShrinkStep;
    private ImpulseStep GrowStep;
    private ImpulseStep RetargetStep;
    private ImpulseStep RelocateStep;

    #endregion

    #region Impulse Conditions
    private bool DoUntilTest()
    {
        return true;
    }


    #endregion


    #region Impulse Actions
    private List<ImpulseStep> Relocate(float percentElapsed)
    {

        if (target.Value != null)
        {
            TransformVariable newTransform = FindSpawnLocationAndOrientation(tentacleRegions, target.Value.transform.position,
                        parentRB.transform.rotation, parentRB.transform.position);
            transform.SetParent(null);
            transform.position = newTransform.position;
            TentacleVector = newTransform.vector;
            transform.rotation = Quaternion.identity;
            transform.rotation *= newTransform.rotation;
            transform.localScale = Vector3.one;
            transform.SetParent(parentObject.transform, true);

        }

        return null;
    }

    private List<ImpulseStep> Retarget(float percentElapsed)
    {

        bool FoundTarget = false;
        int Attempts = 0;
        while (ObjectsSeen.Value.Keys.Count > 0 && !FoundTarget && Attempts < 15)
        {
            Attempts++;
            GameObject possibleTarget = ObjectsSeen.Value.Keys.ToList()[UnityEngine.Random.Range(0, ObjectsSeen.Value.Keys.Count)];


            if (!CanReachTheTarget(possibleTarget.transform))
            {
                continue;
            }

            IAmImmuneToTargetingByTentacles mod = possibleTarget.GetComponent<IAmImmuneToTargetingByTentacles>();
            if (mod == null)
            {

                target.Value = possibleTarget;
                ModifierLibrary.Tentacle.ApplyTargetedByTentacleModifier(target);
                FoundTarget = true;
            }

        }


        return null;

    }
    #endregion

    #region Animator Functions
    private void UpdateAnimatorState()
    {
        HasTarget = (target.Value != null);
    }

    private void UpdateTargetBall()
    {
        if (HasTarget && target.Value != null)
        {
            Vector3 targetMidpoint = Shortcuts.GetMidPointOfObject(target.Value);
            Vector3 VDif = (targetMidpoint - targetBall.transform.position);
            Vector3 dir = VDif.normalized;
            Vector3 SuggestedChange = dir * TentacleHitSpeed.Value * Time.deltaTime;

            if (SuggestedChange.sqrMagnitude > VDif.sqrMagnitude)
            {
                targetBall.transform.position = targetMidpoint;

            }
            else
            {
                targetBall.transform.position += SuggestedChange;
            }



        }

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

    private void UpdateAnimator()
    {

    }
    #endregion

    #region Supporting Functions
    public TransformVariable FindSpawnLocationAndOrientation(List<Bounds> boundsList, Vector3 targetPoint,
       Quaternion boxRotation, Vector3 boxPosition)
    {
        List<Vector3> normals = new List<Vector3>
    {
        boxRotation * new Vector3(0, 0, 1),  // Front face
        boxRotation * new Vector3(0, 0, -1), // Back face
        boxRotation * new Vector3(-1, 0, 0), // Left face
        boxRotation * new Vector3(1, 0, 0)  // Right face
    };

        Vector3 normalFromBounds = Vector3.zero;
        Bounds closestBounds = boundsList[0];
        float highestDotProduct = float.MinValue;

        for (int i = 0; i < boundsList.Count; i++)
        {
            Bounds bounds = boundsList[i];
            Vector3 centerOfBounds = bounds.center;
            Vector3 vectorToTarget = (targetPoint - centerOfBounds).normalized;
            float dotProduct = Vector3.Dot(vectorToTarget, normals[i]);

            if (dotProduct > highestDotProduct)
            {
                highestDotProduct = dotProduct;
                closestBounds = bounds;
                normalFromBounds = normals[i]; // Use the normal vector that corresponds to the bounds
            }
        }

        TransformVariable newtransform = new TransformVariable();
        newtransform.position = RandomPointOnBounds(closestBounds, boxRotation, boxPosition,
            closestBounds.size.x * .2f, closestBounds.size.y * .2f);
        newtransform.rotation = Quaternion.LookRotation(normalFromBounds, Vector3.up); // Use the normal vector for the rotation and force the up direction to be the same as world's up direction.
        Debug.DrawLine(newtransform.position, target.Value.transform.position);
        return newtransform;
    }


    public Vector3 RandomPointOnBounds(Bounds bounds, Quaternion boxRotation, Vector3 boxPosition, float Xmargin, float Ymargin)
    {
        // Generate two random values for x and y
        float x = UnityEngine.Random.Range(bounds.min.x + Xmargin, bounds.max.x - Xmargin);
        float y = UnityEngine.Random.Range(bounds.min.y + Ymargin, bounds.max.y - Ymargin);

        // Use z = bounds.center.z because the point should lie on the face of the bounds
        float z = bounds.center.z;

        // Create a point from the random values in the local coordinate system of the bounds
        Vector3 localRandomPoint = new Vector3(x, y, z);

        // Convert the random point into the world coordinate system
        Vector3 worldRandomPoint = boxRotation * localRandomPoint + boxPosition;

        return worldRandomPoint;
    }

    public bool CanReachTheTarget(Transform theTarget)
    {

        float sqDistance = (theTarget.position - parentRB.transform.position).sqrMagnitude;
        if (sqDistance > currentMaxTentacleReach.Value * currentMaxTentacleReach.Value
                || sqDistance < currentMinTentacleReach.Value * currentMinTentacleReach.Value)
        {
            return false;
        }
        return true;
    }

    public bool CheckTarget()
    {
        if (target.Value != null)
        {
            if (!CanReachTheTarget(target.Value.transform)) return false;

            //float dot = Vector3.Dot((target.Value.transform.position - parentObject.transform.position).normalized,
            // TentacleVector);

            //if (dot < 0)
            //{
            //    return false;
            //}

            return true;
        }
        return false;

    }


    #endregion







 
}
