public interface IHaveDigestDamageDealt
{
    public FloatVariable DigestDamageDealt { get; set; }
    DamageTypeEnum DamageType => DamageTypeLibrary.AcidDamage;



}
