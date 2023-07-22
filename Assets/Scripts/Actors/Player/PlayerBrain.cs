using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;





public class PlayerBrain : MonoBehaviour
{

    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public PlayerVision Brain;
    public float CubeWidth { get; set; }

    [Header("Designer Setup")]
    [Serialize] public FloatVariable CubeVolume;
    [Serialize] public FloatVariable MassTarget;
    [Serialize] public FloatVariable CurrentMoveSpeed;
    [Serialize] public FloatVariable CurrentRotationSpeed;
    [Serialize] public FloatVariable CurrentDigestDamage;
    [Serialize] public FloatVariable CurrentTentacleReach;
    [Serialize] public IntegerVariable CurrentMaxTentacles;
    [Serialize] public IntegerVariable TentacleCount;
    [Serialize] public FloatVariable CurrentSightDistance;
    [Serialize] public FloatVariable CurrentMassPerCubicFoot;
    [Serialize] public FloatVariable CurrentAngularDragInsideStomach;
    [Serialize] public FloatVariable CurrentDragInsideStomach;
    [Serialize] public FloatVariable CurrentGrowthSpeedModifier;
    [Serialize] public FloatVariable CurrentSuckSpeedModifier;
    [Serialize] public PlayerScriptableObject StartingStats;
    [Serialize] public GameObject frontSide;
    [Serialize] public GameObject backSide;
    [Serialize] public GameObject leftSide;
    [Serialize] public GameObject rightSide;
    [Serialize] public GameObject topSide;
    [Serialize] public GameObject bottomSide;
    [Serialize] public GameObject PlayerObject;
    [Serialize] public GameObject PlayerEatingObject;
    [Serialize] public GameObject PlayerGameMesh;
    [Serialize] public GameObjectRuntimeSet BeingEaten;

    [HideInInspector]
    public BoxCollider collider_TopSide;

    //Unity Functions
    protected void Awake()
    {

        rb = GetComponent<Rigidbody>();
        collider_TopSide = topSide.GetComponent<BoxCollider>();

    }

    protected void Start()
    {
        ResetStartingStats();

    }

    private void ResetStartingStats()
    {
        CurrentRotationSpeed.Value = StartingStats.RotateSpeed;
        CurrentMoveSpeed.Value = StartingStats.MoveSpeed;
        CurrentDigestDamage.Value = StartingStats.DigestDamage;
        CurrentMassPerCubicFoot.Value = StartingStats.MassPerCubicFoot;
        CurrentAngularDragInsideStomach.Value = StartingStats.AngularDragInsideStomach;
        CurrentDragInsideStomach.Value = StartingStats.DragInsideStomach;
        CurrentGrowthSpeedModifier.Value = StartingStats.GrowthSpeedModifier;
        CurrentSuckSpeedModifier.Value = StartingStats.SuckSpeedModifier;
        CurrentTentacleReach.Value = StartingStats.TentacleReach;
        CurrentMaxTentacles.Value = StartingStats.MaxTentacles;
        CurrentSightDistance.Value = StartingStats.SightDistance;
        rb.mass = StartingStats.Mass;
        MassTarget.Value = StartingStats.Mass;
        TentacleCount.Value = 0;
        ResetCubeWidth();
    }

    protected void Update()
    {

    }

    protected void FixedUpdate()
    {

        for (int i = 0; i < BeingEaten.Items.Count; i++)
        {
            IAmEdible m = BeingEaten.Items[i].GetComponent<IAmEdible>();
            if (m != null)
            {
                m.BeEaten(CurrentDigestDamage);
            }
        }

        for (int i = 0; i < ActorController.Actors.Count; i++)
        {
            ActorController a = (ActorController)ActorController.Actors[i];

            if (a.SqDistanceFromPlayer < CubeWidth * CubeWidth * 10)
            {
                IAmEdible m = (IAmEdible)ActorController.Actors[i];

                if (m.BeingSuckedIn)
                {
                    SuckIn(ActorController.Actors[i]);
                }
            }

        }

        if (rb.mass != MassTarget.Value)
        {
            StartCoroutine(ChangeSize());
        }

        CheckIfBelowTerrain();

    }

    private void CheckIfBelowTerrain()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);

        //If ground isn't below, look above and teleport
        Vector3 RayTraceStart = transform.position;

        if (!Physics.Raycast(RayTraceStart, down, out RaycastHit hit, 10, (int)Shortcuts.LayerMasks.LayerMask_GroundOnly)) //Ground found
        {
            Vector3 up = transform.TransformDirection(Vector3.up);
            if (Physics.Raycast(RayTraceStart, up, out RaycastHit hit2, 10, (int)Shortcuts.LayerMasks.LayerMask_GroundOnly)) //Ground found
            {
                transform.position += new Vector3(0, hit2.distance, 0);
            }
        }
    }

    private void SuckIn(ActorController victim)
    {
        victim.transform.position = Vector3.Slerp(victim.transform.position,
            transform.position + new Vector3(0, CubeWidth * .5f, 0), Time.deltaTime * CurrentSuckSpeedModifier.Value);
    }

    private float ResetCubeWidth()
    {
        CubeWidth = collider_TopSide.size.x * transform.localScale.x;
        CubeVolume.Value = CubeWidth * CubeWidth * CubeWidth;
        return CubeWidth;

    }




    private IEnumerator ChangeSize()
    {
        float MassDifferential;

        float TargetVolume = MassTarget.Value / CurrentMassPerCubicFoot.Value;

        float TargetSideSize = (float)Math.Pow(TargetVolume, 1.0 / 3.0);

        float ratio = TargetSideSize / CubeWidth;

        if (MassTarget.Value > rb.mass)
        {
            MassDifferential = ratio - 1;
        }
        else
        {
            MassDifferential = (rb.mass / MassTarget.Value) - 1;
        }

        if (MassDifferential > 0.001)
        {

            if (MassDifferential < .01)
            {

                UnparentEdibles();
                transform.localScale = new Vector3(transform.localScale.x * ratio, transform.localScale.y * ratio, transform.localScale.z * ratio);

                ParentEdibles();
                rb.mass = MassTarget.Value;
                ResetCubeWidth();
                yield return null;
            }
            else
            {
                UnparentEdibles();
                float TargetScale = transform.localScale.x * ratio;
                transform.localScale += new Vector3(TargetScale - transform.localScale.x,
                        TargetScale - transform.localScale.y, TargetScale - transform.localScale.z) * Time.deltaTime * CurrentGrowthSpeedModifier.Value;
                ParentEdibles();

                float CubeWidth = ResetCubeWidth();
                rb.mass = CubeWidth * CubeWidth * CubeWidth * CurrentMassPerCubicFoot.Value;
                yield return new WaitForEndOfFrame();

            }




        }

    }

    private void UnparentEdibles()
    {
        foreach (GameObject edible in BeingEaten.Items)
        {
            edible.transform.parent = null;
        }
    }

    private void ParentEdibles()
    {
        foreach (GameObject edible in BeingEaten.Items)
        {
            edible.transform.parent = PlayerEatingObject.transform;
        }
    }



}
