using UnityEngine;

public class TentacleWhapModifer : OneTimeModifierCondition<DealDamageModifier>
{
    [SerializeField] public override bool IsStackable { get { return false; } }

    public float DamageAmount;
    public Vector3 direction;
    public float acceleration;
    private DamageTypeEnum DamageType = DamageTypeEnums.PiercingDamage;

    public override void BeforeEffect()
    {

    }

    public override void ExecuteEffect()
    {
        if (TryGetComponent<IAmDamageable>(out IAmDamageable gameObject))
        {
            gameObject.TakeDamage(DamageAmount, DamageType);
        }
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(rb.mass * direction.x * acceleration, rb.mass * direction.y * acceleration, rb.mass * direction.z * acceleration, ForceMode.Impulse);
        }
    }

    public override void AfterEffect()
    {
        Destroy(this);
    }

    protected override void DebugEffect()
    {

    }
}
