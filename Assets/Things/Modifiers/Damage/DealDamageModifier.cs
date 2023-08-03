using Unity.VisualScripting;
using UnityEngine;

public class DealDamageModifier : OneTimeModifierCondition<DealDamageModifier>
{
    [SerializeField] public override bool IsStackable { get { return false; } }

    [Serialize] public float DamageAmount;
    [Serialize] public DamageTypeEnum DamageType;

    public override void BeforeEffect()
    {

    }

    public override void ExecuteEffect()
    {
        if (TryGetComponent<IAmDamageable>(out IAmDamageable gameObject))
        {
            gameObject.TakeDamage(DamageAmount, DamageType);
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
