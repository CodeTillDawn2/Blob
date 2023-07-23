using UnityEngine;

public interface IAmDamageable
{

    public abstract float currentHitPoints { get; set; }

    public abstract void TakeDamage(float amount, DamageTypeEnum DamageType);

}
