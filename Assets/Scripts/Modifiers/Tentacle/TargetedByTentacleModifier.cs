using UnityEngine;

public class TargetedByTentacleModifier : GameObjectIsGameObjectCondition<TargetedByTentacleModifier>
{
    [SerializeField] public override bool IsStackable { get { return false; } }

    public override bool Inverse { get; set; }

    public override void BeforeEffect()
    {
        print("Before Effect");
    }

    public override void ExecuteEffect()
    {
        print("Effect");
    }

    public override void AfterEffect()
    {
        print("After effect");
        Destroy(this);
    }

    protected override void DebugEffect()
    {
        string test = "";
    }
}
