using UnityEngine;

public class TentacleWhapModifer : OneTimeModifierCondition<DealDamageModifier>
{
    [SerializeField] public override bool IsStackable { get { return false; } }

    public Vector3 direction;
    public float acceleration;

    public override void BeforeEffect()
    {
        print("Tentacle whap added");
    }

    public override void ExecuteEffect()
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(rb.mass * direction.x * acceleration, rb.mass * direction.y * acceleration, rb.mass * direction.z * acceleration, ForceMode.Impulse);
        }
    }

    public override void AfterEffect()
    {
        Destroy(this);
    }

    protected override void DebugEffect()
    {

    }
}
