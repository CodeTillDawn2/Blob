using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PigController : CreatureController, IAmEdible
{
    [Header("Stat Block")]
    [Serialize] public FloatVariable DragInsideStomach;
    [Serialize] public FloatVariable AngularDragInsideStomach;
    [Serialize] public FloatVariable PlayerMassTarget;
    [Serialize] public FloatVariable CubeVolume;

    SpringJoint EatingJoint;

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
        currentNutrition = animalStats.Nutrition;
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

    public void BeReleased()
    {
        currentMass = StartingMass;
        transform.parent = null;
        DestroyEatingJoint();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.drag = 0;
        rb.angularDrag = 0;


    }

    public void BeSuckedIn()
    {
        
        currentMass = 0;
        DestroyEatingJoint();
        rb.useGravity = true;
        rb.drag = DragInsideStomach.Value / 2;
        rb.angularDrag = AngularDragInsideStomach.Value / 2;

    }

    public void BeEaten(FloatVariable Damage)
    {

        if (!BeingEaten)
        {
            rb.useGravity = false;
            rb.drag = DragInsideStomach.Value;
            rb.angularDrag = AngularDragInsideStomach.Value;

            gameObject.layer = (int)Shortcuts.UnityLayers.BeingEaten;
        }

        BeingEaten = true;
      

        ////If min number of frames between declip exceeded, check again
        //if (FramesSinceDeclip > 15)
        //{
        //    foreach (BoxCollider side in new List<BoxCollider>() { collider_TopSide, collider_BottomSide,
        //                                                        collider_FrontSide, collider_BackSide,
        //                                                        collider_LeftSide, collider_RightSide})
        //    {
        //        Vector3 Normalizer = GetPlayerSideDirectionNormalizer(side);
        //        PhysicsTools.NormalizedDeclip(this, myCollider, transform.position, transform.rotation,
        //            side, side.transform.position, transform.rotation * side.transform.rotation, Normalizer);

        //    }
        //    FramesSinceDeclip = 0;
        //}

        TakeDamage(Damage.Value);
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




    private void DestroyEatingJoint()
    {
        if (EatingJoint != null)
        {
            Destroy(EatingJoint);
        }
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
