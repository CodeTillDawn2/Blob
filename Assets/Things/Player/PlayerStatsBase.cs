using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatsBase : ScriptableObject
{
    [Serialize] public float RotateSpeed;
    [Serialize] public float MoveSpeed;
    [Serialize] public float MinTentacleReach;
    [Serialize] public float MaxTentacleReach;
    [Serialize] public float TentacleHitSpeed;
    [Serialize] public float SuckSpeedModifier;
    [Serialize] public float SightDistance;
    [Serialize] public float DragInsideStomach;
    [Serialize] public float AngularDragInsideStomach;
    [Serialize] public float MassPerCubicFoot;
    [Serialize] public float DigestDamage;
    [Serialize] public float GrowthSpeedModifier;
    [Serialize] public float Mass;
    [Serialize] public int MaxTentacles;

}
