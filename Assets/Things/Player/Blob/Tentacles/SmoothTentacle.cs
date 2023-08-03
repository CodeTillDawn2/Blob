using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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


    [Serialize] public FloatVariable TentacleHitSpeed;

    //Animator statuses
    //private bool IsAttacking
    //{
    //    get { return animator.GetBool("IsAttacking"); }
    //}
    //private bool IsReadying
    //{
    //    get { return animator.GetBool("IsReadied"); }
    //}
    //private bool IsGrowing
    //{
    //    get { return animator.GetBool("IsGrowing"); }
    //}
    //public bool IsShrinking
    //{
    //    get { return animator.GetBool("IsShrinking"); }
    //}

    //private bool ShouldAttack
    //{
    //    get { return animator.GetBool("ShouldAttack"); }
    //    set { animator.SetBool("ShouldAttack", value); }
    //}
    //private bool ShouldGrow
    //{
    //    get { return animator.GetBool("ShouldGrow"); }
    //    set { animator.SetBool("ShouldGrow", value); }
    //}
    //private bool ShouldShrink
    //{
    //    get { return animator.GetBool("ShouldShrink"); }
    //    set { animator.SetBool("ShouldShrink", value); }
    //}
    //private bool ShouldReady
    //{
    //    get { return animator.GetBool("ShouldReady"); }
    //    set { animator.SetBool("ShouldReady", value); }
    //}

    private bool HasTarget
    {
        get { return animator.GetBool("HasTarget"); }
        set { animator.SetBool("HasTarget", value); }
    }

    public GameObjectVariable target;
    public BooleanVariable IsAlive;
    private bool IsReady = false;
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

    private Bounds? ChosenRegion;


    public void Go(GameObject parent, Rigidbody parentRB, List<Bounds> tentacleRegions, Dict_GameObjectToLastSeen ObjectsSeen)
    {

        parentObject = parent;
        this.parentRB = parentRB;
        this.tentacleRegions = tentacleRegions;
        this.ObjectsSeen = ObjectsSeen;
        transform.SetParent(parentObject.transform, true);
        if (brain != null && parentRB != null && tentacleRegions.Count >= 1 && ObjectsSeen != null)
        {
            //ShouldGrow = true;
            IsReady = true;
        }
    }




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



    ImpulseStep AttemptTentacleAttackStep;
    ImpulseStep ShrinkStep;
    ImpulseStep GrowStep;
    ImpulseStep RetargetStep;
    ImpulseStep RelocateStep;

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

    //private void InitializeBrain_Old()
    //{
    //    brain.target = target;
    //    brain.IsAlive = IsAlive;

    //    //Retarget
    //    AttemptTentacleAttackStep = new ImpulseStep(StepAction: AttemptTentacleAttack,
    //                                       OnlyDoIf: null,
    //                                       WaitUntilTrue: DoUntilTest);
    //    AttemptTentacleAttackStep.impulseStepNameDebug = "AttemptTentacleAttackStep";
    //    brain.AddImpulse(Impulse.ImpulseType.Attack, CanReachTarget, AttemptTentacleAttackStep);

    //    ShrinkStep = new ImpulseStep(StepAction: Shrink,
    //                                       OnlyDoIf: null,
    //                                       WaitUntilTrue: DoUntilNotShrinking);
    //    ShrinkStep.impulseStepNameDebug = "ShrinkStep";
    //    brain.AddImpulse(Impulse.ImpulseType.Despawn, null, ShrinkStep);

    //    RetargetStep = new ImpulseStep(StepAction: Retarget,
    //                               OnlyDoIf: null,
    //                               WaitUntilTrue: DoUntilTest);
    //    RetargetStep.impulseStepNameDebug = "RetargetStep";
    //    brain.AddImpulse(Impulse.ImpulseType.Search, null, RetargetStep);

    //    RelocateStep = new ImpulseStep(StepAction: Relocate,
    //                       OnlyDoIf: null,
    //                       WaitUntilTrue: DoUntilTest);
    //    RelocateStep.impulseStepNameDebug = "RelocateStep";
    //    brain.AddImpulse(Impulse.ImpulseType.Move, null, ShrinkStep, RelocateStep);
    //}


    //private bool DoUntilNotShrinking()
    //{
    //    if (!IsShrinking && !ShouldShrink) return true;
    //    return false;
    //}

    //private bool DoUntilNotGrowing()
    //{
    //    if (!IsGrowing && !ShouldGrow) return true;
    //    return false;
    //}

    private bool DoUntilTest()
    {
        return true;
    }

    //public List<ImpulseStep> AttemptTentacleAttack(float percentElapsed)
    //{
    //    ShouldReady = true;

    //    //if (transform.localScale.y < .95) //Need to grow
    //    //{
    //    //    animator.Play("TentacleSpawn", 0);
    //    //}

    //    if (!IsGrowing && !IsAttacking)
    //    {
    //        ShouldAttack = true;
    //    }

    //    return null;
    //}

    //private List<ImpulseStep> Shrink(float percentElapsed)
    //{
    //    if (!IsShrinking && !IsGrowing)
    //    {
    //        ShouldShrink = true;
    //    }

    //    return null;
    //}



    protected void OnGUI()
    {
        //GUI.TextArea(new Rect(10, 10, Screen.width / 10, Screen.height / 10),
        //    "Possible targets: " + ObjectsSeen.Value.Keys.Count + " Tentacle scale: " + gameObject.transform.localScale
        //    + " Is Attacking? " + animator.GetBool("IsAttacking"));
    }

    private List<ImpulseStep> Relocate(float percentElapsed)
    {

        if (animator.)
        {
            List<ImpulseStep> addSteps = new List<ImpulseStep>() { ShrinkStep, RelocateStep, GrowStep };
            return addSteps;
        }

        print("Relocating");
        if (target.Value != null && TryFindTentacleTransform(target.Value, out TransformVariable newTransform))
        {
            transform.SetParent(null);
            transform.position = newTransform.position;
            TentacleVector = newTransform.vector;
            transform.rotation = Quaternion.identity;
            transform.rotation *= newTransform.rotation;
            transform.localScale = Vector3.one;
            transform.SetParent(parentObject.transform, true);
            //ShouldShrink = false;
            //ShouldGrow = true;
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

            //foreach (GameObject possibleTarget in ObjectsSeen.Value.Keys.OrderBy(x => ObjectsSeen.Value[x].Distance).ToList())
            //{

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
        //}



        return null;

    }



    private void LateUpdate()
    {
        UpdateTargetBall();
        UpdateAnimatorState();
    }

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
                //ShouldAttack = false;
            }
            else
            {
                targetBall.transform.position += SuggestedChange;
            }



        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!CheckTarget())
        {
            //ShouldReady = false;
            target.Value = null;
        }

        UpdateAnimator();
        //if (target.Value == null) Retarget();
        //if (!IsAlive || target.Value == null) Despawn();

    }

    private void UpdateAnimator()
    {

    }

    private void FixedUpdate()
    {
        if (target.Value != null)
        {
            //ReadyTentacleStrike();
            //targetBall.transform.position = target.Value.transform.position;
        }

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


    public bool TryFindTentacleTransform(GameObject possibleTarget, out TransformVariable newTransform)
    {
        bool Success = false;
        newTransform = new TransformVariable();


        Vector3 _direction = (possibleTarget.transform.position - parentObject.transform.position).normalized;

        float FrontSideDot = Vector3.Dot(_direction, parentObject.transform.forward);
        float RightSideDot = Vector3.Dot(_direction, parentObject.transform.right);
        float BackSideDot = Vector3.Dot(_direction, -parentObject.transform.forward);
        float LeftSideDot = Vector3.Dot(_direction, -parentObject.transform.right);

        Vector3 localVector;

        if (Math.Abs(FrontSideDot) > Math.Abs(RightSideDot))
        {
            if (FrontSideDot > 0)
            {
                newTransform.vector = Vector3.forward;
                localVector = parentRB.transform.forward;

            }
            else if (BackSideDot > 0)
            {
                newTransform.vector = -Vector3.forward;
                localVector = -parentRB.transform.forward;

            }
            else
            {
                newTransform.vector = -Vector3.forward;
                localVector = -parentRB.transform.forward;
                Debug.LogError("No vector chosen!");
            }
        }
        else
        {
            if (RightSideDot > 0)
            {
                newTransform.vector = Vector3.right;
                localVector = parentRB.transform.right;

            }
            else if (LeftSideDot > 0)
            {
                newTransform.vector = -Vector3.right;
                localVector = -parentRB.transform.right;


            }
            else
            {
                newTransform.vector = -Vector3.right;
                localVector = -parentRB.transform.right;
                Debug.LogError("No vector chosen!");
            }
        }
        newTransform.rotation = Quaternion.LookRotation(localVector);
        //newTransform.scale = Vector3.zero;




        ChosenRegion = null;
        float ChosenSqMagnitude = Mathf.Infinity;
        foreach (Bounds region in tentacleRegions)
        {

            RaycastHit? hit = PhysicsTools.RaycastAt(possibleTarget.transform.position, parentRB.transform.TransformPoint(region.center),
                Mathf.Infinity, TentacleFindingMask);

            Vector3 OriginalCenter = region.center;
            Vector3 MyTransform = parentRB.transform.position;

            Vector3 WorldPositionCenter = parentRB.transform.TransformPoint(region.center);


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

            RaycastHit? hit = PhysicsTools.RaycastAt(Shortcuts.GetMidPointOfObject(possibleTarget), randomPoint, Mathf.Infinity, TentacleFindingMask);
            if (hit != null && hit.Value.collider.gameObject.layer != BlockingMask) //Let other tentacles block
            {
                return hit.Value.point;

            }

        }

        return null;


    }

    public void Die()
    {
        target.Value = null;
        Destroy(gameObject);
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
    //public List<ImpulseStep> ReadyWeapon(float percentElapsed)
    //{

    //    if (target.Value == null) return null;
    //    Vector3 TargetChange = ((target.Value.transform.position - gameObject.transform.position) / 2f
    //             + gameObject.transform.up * ChosenRegion.Value.max.y);

    //    if (target.Value != null)
    //    {
    //        targetBall.transform.position = gameObject.transform.position + TargetChange;
    //    }
    //    return null;
    //}
}
