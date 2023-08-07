using Unity.VisualScripting;
using UnityEngine;

public class PigController : EnemyController, IAmDigestable, IAmDamageable
{
    [Header("Stat Block")]
    [SerializeField] private bool amAlive;
    public override bool AmAlive { get { return amAlive; } set { amAlive = value; } }
    [SerializeField] private float currentHitPoints;
    public float CurrentHitPoints { get { return currentHitPoints; } set { currentHitPoints = value; } }
    public bool BeingEaten { get; set; }
    public bool BeingSuckedIn { get; set; }
    [SerializeField]
    private float currentNutrition;
    public float CurrentNutrition { get { return currentNutrition; } set { currentNutrition = value; } }
    [HideInInspector]
    public override float SqDistanceFromPlayer { get; set; }



    [HideInInspector]
    public MeshRenderer meshRenderer;

    // Start is called before the first frame update
    protected override void Start()
    {

        base.Start();
        CurrentHitPoints = enemyStats.HitPoints;
        AmAlive = true;
        BeingEaten = false;
        BeingSuckedIn = false;
        CurrentNutrition = enemyStats.Nutrition;
        meshRenderer = GetComponentInChildren<MeshRenderer>();

    }

    protected override void Awake()
    {
        base.Awake();
        SqDistanceFromPlayer = 0;
    }

    protected override void Update()
    {
        base.Update();
        //CheckSuckedIn();
    }



    protected override void FixedUpdate()
    {
        base.FixedUpdate();




        //StartCoroutine(SuckIn());
    }

    //public void OnPreyReleased()
    //{
    //    currentMass = StartingMass;
    //    transform.parent = null;

    //    rb.useGravity = true;
    //    rb.constraints = RigidbodyConstraints.None;
    //    rb.drag = 0;
    //    rb.angularDrag = 0;


    //}

    //public void OnContact()
    //{

    //    currentMass = 0;

    //    rb.useGravity = true;
    //    rb.drag = DragInsideStomach.Value / 2;
    //    rb.angularDrag = AngularDragInsideStomach.Value / 2;


    //    Vector3 RelativeVector = Vector3.Slerp(Vector3.zero,
    //         PlayerGameObject.Value.transform.position - transform.position + new Vector3(0, CubeWidth.Value * .5f, 0), SuckSpeedModifier.Value);

    //    rb.velocity = new Vector3(RelativeVector.x / Time.fixedDeltaTime, RelativeVector.y / Time.fixedDeltaTime, RelativeVector.z / Time.fixedDeltaTime);
    //}
    public float Digest(float digestDamage)
    {

        float NutritionGained = 0;
        if (!AmAlive)
        {
            if (CurrentNutrition > 0)
            {
                NutritionGained = digestDamage;
                if (NutritionGained > CurrentNutrition) NutritionGained = CurrentNutrition;


                CurrentNutrition = CurrentNutrition - digestDamage;
            }
            if (CurrentNutrition <= 0) Destroy(gameObject);
        }
        return NutritionGained;
    }

    public void TakeDamage(float Damage, DamageTypeEnum DamageType)
    {

        if (CurrentHitPoints > 0)
        {
            CurrentHitPoints = CurrentHitPoints - Damage;
        }
        if (CurrentHitPoints < 0) CurrentHitPoints = 0;

    }
    //public void OnEaten(float digestDamage)
    //{



    //    if (!BeingEaten)
    //    {
    //        rb.useGravity = false;
    //        rb.drag = DragInsideStomach.Value;
    //        rb.angularDrag = AngularDragInsideStomach.Value;
    //        gameObject.transform.SetParent(PlayerGameObject.Value.transform);
    //        gameObject.layer = (int)Shortcuts.UnityLayers.BeingEaten;

    //    }

    //    BeingEaten = true;
    //    if (transform.position.y < PlayerGameObject.Value.transform.position.y + (CubeWidth.Value * .4f))
    //    {
    //        rb.AddForce(new Vector3(0, .01f * Time.fixedDeltaTime, 0), ForceMode.Force);
    //    }



    //    TakeDamage(digestDamage * Time.fixedDeltaTime, DamageTypeEnums.AcidDamage);


    //    if (CurrentHitPoints < 0)
    //    {
    //        ChangeMass(-CurrentHitPoints);
    //        currentNutrition += CurrentHitPoints;
    //        CurrentHitPoints = 0;
    //    }

    //    if (currentNutrition <= 0 && CurrentHitPoints <= 0)
    //    {
    //        Destroy(gameObject);
    //    }

    //}

    public void ChangeMass(float mass)
    {
        PlayerMassTarget.Value += mass;
    }

    protected override void Die()
    {
        AmAlive = false;
    }





    //public Vector3 GetPlayerSideDirectionNormalizer(BoxCollider side)
    //{
    //    if (side == collider_TopSide)
    //    {
    //        return side.transform.up;
    //    }
    //    else if (side == collider_BottomSide)
    //    {
    //        return -side.transform.up;
    //    }
    //    else if (side == collider_FrontSide)
    //    {
    //        return side.transform.forward;
    //    }
    //    else if (side == collider_BackSide)
    //    {
    //        return -side.transform.forward;
    //    }
    //    else if (side == collider_RightSide)
    //    {
    //        return side.transform.right;
    //    }
    //    else if (side == collider_LeftSide)
    //    {
    //        return -side.transform.right;
    //    }
    //    else
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
