using UnityEngine;

public class TargetedByTentacleModifier : GameObjectIsGameObjectCondition<TargetedByTentacleModifier>
{
    [SerializeField] public override bool IsStackable { get { return false; } }

    public override bool Inverse { get; set; }

    public override void BeforeEffect()
    {

    }

    public override void ExecuteEffect()
    {

    }

    public override void AfterEffect()
    {

        Destroy(this);
    }

    protected override void DebugEffect()
    {
        string test = "";
    }
}
