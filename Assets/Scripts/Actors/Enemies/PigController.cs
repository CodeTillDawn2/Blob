using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;

public class PigController : EnemyController, IAmEdible
{
    [Header("Stat Block")]
    [Serialize] public FloatVariable DragInsideStomach;
    [Serialize] public FloatVariable AngularDragInsideStomach;
    [Serialize] public FloatVariable SuckSpeedModifier;
    [Serialize] public FloatVariable PlayerDigestDamage;
    [Serialize] public FloatVariable PlayerMassTarget;
    [Serialize] public FloatVariable CubeVolume;
    [Serialize] public GameObjectVariable PlayerGameObject;

    public bool BeingEaten { get; set; }
    public bool BeingSuckedIn { get; set; }
    public bool BeingSpatOut { get; set; }
    public float currentNutrition { get; set; }
    [HideInInspector]
    public override float SqDistanceFromPlayer { get; set; }

    [HideInInspector]
    public MeshRenderer meshRenderer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        BeingEaten = false;
        BeingSuckedIn = false;
        BeingSpatOut = false;
        currentNutrition = enemyStats.Nutrition;
        meshRenderer = GetComponentInChildren<MeshRenderer>();

    }

    protected override void Awake()
    {
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

    public void OnPreyReleased()
    {
        currentMass = StartingMass;
        transform.parent = null;

        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.drag = 0;
        rb.angularDrag = 0;


    }

    public void OnContact()
    {

        currentMass = 0;

        rb.useGravity = true;
        rb.drag = DragInsideStomach.Value / 2;
        rb.angularDrag = AngularDragInsideStomach.Value / 2;
   

        Vector3 RelativeVector = Vector3.Slerp(Vector3.zero,
             PlayerGameObject.Value.transform.position - transform.position + new Vector3(0, CubeWidth.Value * .5f, 0), SuckSpeedModifier.Value);

        rb.velocity = new Vector3(RelativeVector.x / Time.fixedDeltaTime, RelativeVector.y / Time.fixedDeltaTime, RelativeVector.z / Time.fixedDeltaTime);
    }

    public void OnEaten(float digestDamage)
    {



        if (!BeingEaten)
        {
            rb.useGravity = false;
            rb.drag = DragInsideStomach.Value;
            rb.angularDrag = AngularDragInsideStomach.Value;
            gameObject.transform.SetParent(PlayerGameObject.Value.transform);
            gameObject.layer = (int)Shortcuts.UnityLayers.BeingEaten;
            
        }

        BeingEaten = true;
        if (transform.position.y < PlayerGameObject.Value.transform.position.y + (CubeWidth.Value * .4f))
        {
            rb.AddForce(new Vector3(0, .01f * Time.fixedDeltaTime, 0), ForceMode.Force);
        }
        


        TakeDamage(digestDamage * Time.fixedDeltaTime, new DamageTypeEnum());
        

        if (currentHitPoints < 0)
        {
            ChangeMass(-currentHitPoints);
            currentNutrition += currentHitPoints;
            currentHitPoints = 0;
        }

        if (currentNutrition <= 0 && currentHitPoints <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void ChangeMass(float mass)
    {
        PlayerMassTarget.Value += mass;
    }

    private void OnDestroy()
    {

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
