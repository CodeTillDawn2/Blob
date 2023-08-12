using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Represents entities that use a sight box for heuristics. Reduces the need for raycasting by providing a general 
/// idea of what might be in line of sight.
/// </summary>
[BreadAIInterface]
public interface IUseSightBox
{
    /// <summary>
    /// Gets or sets the sight box collider.
    /// </summary>
    BoxCollider sightBox { get; set; }

    /// <summary>
    /// Gets or sets a list of nearby objects that might be in the entity's line of sight.
    /// </summary>
    GameObjectRuntimeSet ThingsNearby { get; set; }

    /// <summary>
    /// Gets or sets the filters for objects to be excluded from "ThingsNearby". Essentially, objects that can be ignored for sight.
    /// </summary>
    UnityLayerVariable ThingNearbyFilter { get; set; }
}