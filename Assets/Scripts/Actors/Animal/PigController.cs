using System.Collections.Generic;
using UnityEngine;

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
        //transform.position = Vector3.MoveTowards(transform.position, PlayerController.Player.transform.position, 10 * Time.deltaTime);



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
            PlayerController.Player.ChangeMass(-currentHitPoints);
            currentNutrition += currentHitPoints;
            currentHitPoints = 0;
        }

        if (currentNutrition <= 0 && currentHitPoints <= 0)
        {
            PlayerController.Player.BeingEaten.Remove(this);
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
            EatingJoint.connectedBody = PlayerController.Player.rb;
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
            EatingJoint.transform.parent = PlayerController.Player.transform;
            //joint.anchor = playerBase.transform.position.normalized;
            
            EatingJoint.enableCollision = true;

        }
    }

    private void CheckSuckedIn()
    {
        bool CanBeSwallowed = ColliderArea < PlayerController.Player.CubeArea * .5;
        if (Intersects && CanBeSwallowed) //Suck in or eat
        {
            transform.parent = PlayerController.Player.PlayerEatingObject.transform;
            currentMass = 0;

            if (Contained)
            {
                BeingSuckedIn = false;
                BeingEaten = true;
                if (!PlayerController.Player.BeingEaten.Contains(this))
                {
                    PlayerController.Player.BeingEaten.Add(this);
                }
                CreateEatingJoint();
                rb.useGravity = false;
                rb.drag = PlayerController.Player.currentDragInsideStomach;
                rb.angularDrag = PlayerController.Player.currentAngularDragInsideStomach;
                //rb.velocity = PlayerController.Player.rb.velocity;
                //rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
                gameObject.layer = LayerMask.NameToLayer("BeingEaten");

                //If min number of frames between declip exceeded, check again
                if (FramesSinceDeclip > 15)
                {
                    foreach (BoxCollider side in new List<BoxCollider>() { PlayerController.Player.topSideCollider, PlayerController.Player.bottomSideCollider,
                                                                PlayerController.Player.frontSideCollider, PlayerController.Player.backSideCollider,
                                                                PlayerController.Player.leftSideCollider, PlayerController.Player.rightSideCollider})
                    {
                        Vector3 Normalizer = PhysicsTools.GetPlayerSideDirectionNormalizer(side);
                        PhysicsTools.NormalizedDeclip(this, myCollider, transform.position, transform.rotation,
                            side, side.transform.position, PlayerController.Player.transform.rotation * side.transform.rotation, Normalizer);

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
                rb.drag = PlayerController.Player.currentDragInsideStomach / 2;
                rb.angularDrag = PlayerController.Player.currentAngularDragInsideStomach / 2;
                if (PlayerController.Player.BeingEaten.Contains(this))
                {
                    PlayerController.Player.BeingEaten.Remove(this);
                }
                //rb.constraints = RigidbodyConstraints.None;
                gameObject.layer = LayerMask.NameToLayer("CanBeEaten");
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
            if (PlayerController.Player.BeingEaten.Contains(this))
            {
                PlayerController.Player.BeingEaten.Remove(this);
            }
            rb.constraints = RigidbodyConstraints.None;
            rb.drag = 0;
            rb.angularDrag = 0;

            if (CanBeSwallowed)
            {
                gameObject.layer = LayerMask.NameToLayer("CanBeEaten");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
            }

        }
    }

}
