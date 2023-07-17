using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class CreatureController : ActorController
{

    public CreatureScriptableObject animalStats;

    protected float currentMass;
    public float currentMoveSpeed { get; set; }
    public float currentRotateSpeed { get; set; }
    public float currentHitPoints { get; set; }




    protected override void Awake()
    {
        base.Awake();


    }

    protected override void Start()
    {
        base.Start();

        currentMass = animalStats.Mass;
        currentMoveSpeed = animalStats.MoveSpeed;
        currentRotateSpeed = animalStats.RotateSpeed;
        currentHitPoints = animalStats.HitPoints;

        rb.mass = currentMass;

    }

    protected override void Update()
    {
        base.Update();
        rb.mass = currentMass;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected void TakeDamage(float Damage)
    {
        currentHitPoints = currentHitPoints - Damage;
    }

}
