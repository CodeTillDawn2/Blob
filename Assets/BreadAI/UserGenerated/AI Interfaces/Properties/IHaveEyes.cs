/// <summary>
/// Represents entities equipped with physical eyes. Important for determining sight based on specific locations in space.
/// </summary>
[BreadInterface]
public interface IHaveEyes
{
    /// <summary>
    /// Gets or sets the collection of eyes.
    /// </summary>
    GameObjectRuntimeSet Eyes { get; set; }
}