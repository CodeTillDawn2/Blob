using UnityEngine;

public interface IDoDigestDamage
{
    [SerializeField] public FloatVariable CurrentDigestDamage { get; set; }

    public abstract void GainNutrition(float amount);

}
