using System;
using Unity.VisualScripting;

public class BlobSenses : Senses
{

    public static Type[] _expectedStatsInterfaces = { typeof(IHaveMoveSpeed) };
    public override Type[] ExpectedStatsInterfaces => _expectedStatsInterfaces;

    [Serialize] public BlobConfiguration StartingStats;

    public Shortcuts.LayerMasks OnlySeeMask { get; set; }

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }


}
