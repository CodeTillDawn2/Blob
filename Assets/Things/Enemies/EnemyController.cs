using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyController : MonoBehaviour
{

    [Header("Stat Block")]
    [Serialize] public FloatVariable CubeWidth;
    [SerializeField] protected float currentMass;
    [SerializeReference] public float currentMoveSpeed;
    [SerializeReference] public float currentRotateSpeed;

    public abstract bool AmAlive { get; set; }


    public EnemyStatsBase enemyStats;

    [HideInInspector]
    public float StartingMass;
    [HideInInspector]
    protected Rigidbody rb;

    public abstract float SqDistanceFromPlayer { get; set; }

    [Serialize] public GameObjectRuntimeSet AllEnemies;

    protected virtual void Awake()
    {
        if (!TryGetComponent<MomentumSensor>(out MomentumSensor sensor))
        {
            gameObject.AddComponent<MomentumSensor>();
        }

    }

    protected abstract void Die();

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
        if (TryGetComponent<IAmDamageable>(out IAmDamageable found))
        {
            if (found.CurrentHitPoints <= 0)
            {
                Die();
            }
        }


    }



}
