using UnityEngine;

public class PigConfiguration : ConfigurationBase, IHaveMoveSpeed, IHaveRotateSpeed, IHaveMass, IHaveHitPoints, IHaveNutrition, ICanSee,
    IHaveSightDistance, IAmAlive
{

    [SerializeField] public FloatVariable MoveSpeed { get; set; }
    [SerializeField] public FloatVariable RotateSpeed { get; set; }
    [SerializeField] public FloatVariable Mass { get; set; }
    [SerializeField] public FloatVariable HitPoints { get; set; }
    [SerializeField] public FloatVariable Nutrition { get; set; }
    [SerializeField] public FloatVariable SightDistance { get; set; }
    [SerializeField] public BooleanVariable IsAlive { get; set; }
    /// <summary>
    /// Shortcuts.LayerMasks.LayerMask_NotGround
    /// </summary>
    public Shortcuts.LayerMasks OnlySeeMask { get; set; }
    public Dict_GameObjectToLastSeen ThingsSeen { get; set; }


}
