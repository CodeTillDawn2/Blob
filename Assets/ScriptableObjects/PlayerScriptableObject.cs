using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/Player")]
public class PlayerScriptableObject : ScriptableObject
{
    [SerializeField]
    float mass;
    public float Mass { get => mass; private set => mass = value; }

    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }

    [SerializeField]
    float rotateSpeed;
    public float RotateSpeed { get => rotateSpeed; private set => rotateSpeed = value; }

    [SerializeField]
    float digestDamage;
    public float DigestDamage { get => digestDamage; private set => digestDamage = value; }

    [SerializeField]
    float massPerCubicFoot;
    public float MassPerCubicFoot { get => massPerCubicFoot; private set => massPerCubicFoot = value; }

    [SerializeField]
    float angularDragInsideStomach;
    public float AngularDragInsideStomach { get => angularDragInsideStomach; private set => angularDragInsideStomach = value; }

    [SerializeField]
    float dragInsideStomach;
    public float DragInsideStomach { get => dragInsideStomach; private set => dragInsideStomach = value; }

    [SerializeField]
    float suckSpeedModifier;
    public float SuckSpeedModifier { get => suckSpeedModifier; private set => suckSpeedModifier = value; }

    [SerializeField]
    float growthSpeedModifier;
    public float GrowthSpeedModifier { get => growthSpeedModifier; private set => growthSpeedModifier = value; }

    [SerializeField]
    float sightDistance;
    public float SightDistance { get => sightDistance; private set => sightDistance = value; }

    [SerializeField]
    float tentacleReach;
    public float TentacleReach { get => tentacleReach; private set => tentacleReach = value; }

    [SerializeField]
    int maxTentacles;
    public int MaxTentacles { get => maxTentacles; private set => maxTentacles = value; }

}
