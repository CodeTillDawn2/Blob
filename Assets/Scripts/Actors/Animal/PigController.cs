using System.Collections.Generic;
using UnityEngine;
using static PlayerBrain;

public class PigController : CreatureController, IAmEdible
{


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
        localBounds = meshRenderer.localBounds;
        worldBounds = meshRenderer.bounds;
    }

    protected override void Awake()
    {
        base.Awake();
        SqDistanceFromPlayer = 0;


    }

    protected override void Update()
    {
        base.Update();
        CheckBounds2();
        CheckSuckedIn();
    }



    protected override void FixedUpdate()
    {
        base.FixedUpdate();




        //StartCoroutine(SuckIn());
    }

    public void BeEaten(float digestDamage)
    {

        TakeDamage(digestDamage);
        if (currentHitPoints < 0)
        {
            if (currentHitPoints < 20)
            {
                string test = "";
            }
            PlayerController.me.ChangeMass(-currentHitPoints);
            currentNutrition += currentHitPoints;
            currentHitPoints = 0;
        }

        if (currentNutrition <= 0 && currentHitPoints <= 0)
        {
            PlayerController.me.BeingEaten.Remove(this);
            Destroy(gameObject);
        }

    }

    private void DestroyEatingJoint()
    {
        if (EatingJoint != null)
        {
            Destroy(EatingJoint);
        }
    }

    private void CreateEatingJoint()
    {
        if (EatingJoint == null)
        {
            EatingJoint = gameObject.AddComponent<SpringJoint>();
            EatingJoint.connectedBody = PlayerController.me.rb;
            EatingJoint.autoConfigureConnectedAnchor = false;
            //joint.maxDistance = .01f;
            //joint.maxDistance = myCollider.bounds.extents.x - playerCollider.bounds.extents.x;
            EatingJoint.spring = 0.01f;
            EatingJoint.damper = 5000;

            //joint.transform.position = rb.position;
            EatingJoint.connectedAnchor = new Vector3(0, 0, 0);
            //joint.connectedAnchor = rb.centerOfMass;
            //joint.connectedAnchor = transform.position;
            //EatingJoint.anchor = new Vector3(0, 0, 0);
            EatingJoint.transform.parent = PlayerController.me.transform;
            //joint.anchor = playerBase.transform.position.normalized;
            
            EatingJoint.enableCollision = true;

        }
    }

    private void CheckSuckedIn()
    {
        bool CanBeSwallowed = ColliderArea < PlayerController.me.CubeArea * .5;
        if (Intersects && CanBeSwallowed) //Suck in or eat
        {
            transform.parent = PlayerController.me.PlayerEatingObject.transform;
            currentMass = 0;

            if (Contained)
            {
                BeingSuckedIn = false;
                BeingEaten = true;
                if (!PlayerController.me.BeingEaten.Contains(this))
                {
                    PlayerController.me.BeingEaten.Add(this);
                }
                //CreateEatingJoint();
                rb.useGravity = false;
                rb.drag = PlayerController.me.currentDragInsideStomach;
                rb.angularDrag = PlayerController.me.currentAngularDragInsideStomach;
                //rb.velocity = PlayerController.Player.rb.velocity;
                //rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
                gameObject.layer = (int)UnityLayers.BeingEaten;

                //If min number of frames between declip exceeded, check again
                if (FramesSinceDeclip > 15)
                {
                    foreach (BoxCollider side in new List<BoxCollider>() { PlayerController.me.collider_TopSide, PlayerController.me.collider_BottomSide,
                                                                PlayerController.me.collider_FrontSide, PlayerController.me.collider_BackSide,
                                                                PlayerController.me.collider_LeftSide, PlayerController.me.collider_RightSide})
                    {
                        Vector3 Normalizer = PhysicsTools.GetPlayerSideDirectionNormalizer(side);
                        PhysicsTools.NormalizedDeclip(this, myCollider, transform.position, transform.rotation,
                            side, side.transform.position, PlayerController.me.transform.rotation * side.transform.rotation, Normalizer);

                    }
                    FramesSinceDeclip = 0;
                }



            }
            else
            {
                BeingSuckedIn = true;
                BeingEaten = false;
                DestroyEatingJoint();
                rb.useGravity = true;
                rb.drag = PlayerController.me.currentDragInsideStomach / 2;
                rb.angularDrag = PlayerController.me.currentAngularDragInsideStomach / 2;
                if (PlayerController.me.BeingEaten.Contains(this))
                {
                    PlayerController.me.BeingEaten.Remove(this);
                }
                //rb.constraints = RigidbodyConstraints.None;
                gameObject.layer = (int)UnityLayers.CanBeEaten;
            }
        }
        else //Release
        {
            currentMass = StartingMass;
            BeingSuckedIn = false;
            BeingEaten = false;
            transform.parent = null;
            DestroyEatingJoint();
            rb.useGravity = true;
            if (PlayerController.me.BeingEaten.Contains(this))
            {
                PlayerController.me.BeingEaten.Remove(this);
            }
            rb.constraints = RigidbodyConstraints.None;
            rb.drag = 0;
            rb.angularDrag = 0;

            if (CanBeSwallowed)
            {
                gameObject.layer = (int)UnityLayers.CanBeEaten;
            }
            else
            {
                gameObject.layer = (int)UnityLayers.Default;
            }

        }
    }

}
