public interface IAmDamageable
{

    public abstract float CurrentHitPoints { get; set; }

    public abstract void TakeDamage(float amount, DamageTypeEnum DamageType);

}
