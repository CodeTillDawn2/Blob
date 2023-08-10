using UnityEngine;

public class BlobConfiguration : ConfigurationBase, IHaveMoveSpeed, IHaveRotateSpeed, IHaveMass, ICanSee, IHaveTentacleReach, IHaveThingsInMyStomach, IHaveSuckSpeed, IHaveMassPerCubicFoot,
    IHaveDigestDamageDealt, IHaveGrowthSpeed, IHaveTentacleHitSpeed, IHaveSightDistance
{



    [SerializeField] public FloatVariable MoveSpeed { get; set; }
    [SerializeField] public FloatVariable RotateSpeed { get; set; }
    [SerializeField] public FloatVariable Mass { get; set; }
    [SerializeField] public FloatVariable SightDistance { get; set; }
    [SerializeField] public FloatVariable MinTentacleReach { get; set; }
    [SerializeField] public FloatVariable MaxTentacleReach { get; set; }
    [SerializeField] public IntegerVariable MaxTentacles { get; set; }
    [SerializeField] public FloatVariable TentacleHitSpeed { get; set; }
    [SerializeField] public FloatVariable DragInsideStomach { get; set; }
    [SerializeField] public FloatVariable AngularDragInsideStomach { get; set; }
    [SerializeField] public FloatVariable SuckSpeed { get; set; }
    [SerializeField] public FloatVariable MassPerCubicFoot { get; set; }
    [SerializeField] public FloatVariable DigestDamageDealt { get; set; }
    [SerializeField] public FloatVariable GrowthSpeed { get; set; }
    /// <summary>
    /// Shortcuts.LayerMasks.LayerMask_NotPlayerOrTentacles;
    /// </summary>
    public Shortcuts.LayerMasks OnlySeeMask { get; set; }
    public Dict_GameObjectToLastSeen ThingsSeen { get; set; }
}
