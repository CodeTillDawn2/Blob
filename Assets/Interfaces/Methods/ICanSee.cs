/// <summary>
/// Describes entities capable of seeing.
/// </summary>
public interface ICanSee
{
    /// <summary>
    /// Gets or sets the sight distance.
    /// </summary>
    FloatVariable SightDistance { get; set; }

    /// <summary>
    /// Gets or sets the layer mask for objects this entity can see.
    /// </summary>
    Shortcuts.LayerMasks OnlySeeMask { get; set; }

    /// <summary>
    /// Gets or sets the things this entity has seen.
    /// </summary>
    Dict_GameObjectToLastSeen ThingsSeen { get; set; }
}