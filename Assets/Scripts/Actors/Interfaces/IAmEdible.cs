using UnityEngine;

public interface IAmEdible
{

    public abstract float currentNutrition { get; set; }
    public abstract bool BeingEaten { get; set; }
    public abstract bool BeingSuckedIn { get; set; }

    public abstract void OnEaten(float digestDamage);


}
