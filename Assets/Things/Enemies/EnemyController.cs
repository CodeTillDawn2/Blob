using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyController : MonoBehaviour
{



    public PigConfiguration enemyStats;

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


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            StartingMass = rb.mass;
        }








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

    }

    protected virtual void FixedUpdate()
    {
        if (TryGetComponent<IHaveHitPoints>(out IHaveHitPoints found))
        {
            if (found.HitPoints.Value <= 0)
            {
                (this as ICanDie)?.Die();
            }
        }


    }



}
