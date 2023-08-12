[BreadAIInterface]
public interface ICanBeDigested
{


    /// <summary>
    /// Returns nutrition gained
    /// </summary>
    /// <param name="digestDamage"></param>
    /// <returns></returns>
    public abstract float BeDigested(float digestDamage);

}
