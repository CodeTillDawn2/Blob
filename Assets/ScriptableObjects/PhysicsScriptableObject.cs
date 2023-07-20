using UnityEngine;

[CreateAssetMenu(fileName = "CreatureScriptableObject", menuName = "ScriptableObjects/Creature")]
public class PhysicsScriptableObject : ScriptableObject
{
    [SerializeField]
    float nutrition;
    public float Nutrition { get => nutrition; private set => nutrition = value; }

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
    float hitPoints;
    public float HitPoints { get => hitPoints; private set => hitPoints = value; }
}
