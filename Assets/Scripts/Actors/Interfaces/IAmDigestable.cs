public interface IAmDigestable
{

    public abstract float CurrentNutrition { get; set; }
    public abstract bool BeingEaten { get; set; }
    public abstract bool BeingSuckedIn { get; set; }

    /// <summary>
    /// Returns nutrition gained
    /// </summary>
    /// <param name="digestDamage"></param>
    /// <returns></returns>
    public abstract float Digest(float digestDamage);


}
