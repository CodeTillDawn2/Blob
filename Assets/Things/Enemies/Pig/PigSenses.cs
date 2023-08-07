using UnityEngine;

public class PigSenses : Senses
{
    public float visionDistance = 20f;
    public float visionAngle = 160f; // Wide FoV
    public int numberOfRays = 20;
    public EnemyStatsBase enemyStats;
    public Transform leftEye;
    public Transform rightEye;


    protected override Shortcuts.LayerMasks OnlySeeMask => Shortcuts.LayerMasks.LayerMask_NotGround;


    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        CurrentSightDistance.Value = enemyStats.SightDistance;
        ThingsNearby = Instantiate(SOLibrary.instance.EmptyGameObjectRuntimeSet);
    }

    protected override void Update()
    {
        base.Update();

    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override bool FilterThingNearby(Collider col)
    {
        if (col.gameObject.layer != (int)Shortcuts.UnityLayers.Ground)
        {
            return true;
        }
        return false;
    }
}
