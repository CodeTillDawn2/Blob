using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyController : MonoBehaviour, IAmDamageable
{

    [Header("Stat Block")]
    [Serialize] public FloatVariable CubeWidth;
    protected float currentMass;
    public float currentMoveSpeed { get; set; }
    public float currentRotateSpeed { get; set; }
    public float currentHitPoints { get; set; }

    public EnemyScriptableObject enemyStats;

    [HideInInspector]
    public float StartingMass;
    [HideInInspector]
    protected Rigidbody rb;

    public abstract float SqDistanceFromPlayer { get; set; }

    [Serialize] public GameObjectRuntimeSet AllEnemies;

    protected virtual void Awake()
    {


    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            StartingMass = rb.mass;
        }


        currentMass = enemyStats.Mass;
        currentMoveSpeed = enemyStats.MoveSpeed;
        currentRotateSpeed = enemyStats.RotateSpeed;
        currentHitPoints = enemyStats.HitPoints;

        rb.mass = currentMass;

    }

    protected virtual void OnEnable()
    {
        //AllEnemies.Add(this.gameObject);
    }

    protected virtual void OnDestroy()
    {

        //AllEnemies.Remove(this.gameObject);
    }

    protected virtual void Update()
    {
        //rb.mass = currentMass;
        SqDistanceFromPlayer = PhysicsTools.GetSqDistanceBetweenPoints(transform.position, transform.position);
    }

    protected virtual void FixedUpdate()
    {

    }

    public void TakeDamage(float Damage, DamageTypeEnum DamageType)
    {
        currentHitPoints = currentHitPoints - Damage;
    }

}
