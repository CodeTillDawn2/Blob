using Unity.VisualScripting;
using UnityEngine;

public class BlobSenses : Senses
{



    [Serialize] public PlayerStatsBase StartingStats;

    protected override Shortcuts.LayerMasks OnlySeeMask => Shortcuts.LayerMasks.LayerMask_NotPlayerOrTentacles;

    protected override void Awake()
    {
        base.Awake();
    }





    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        CurrentSightDistance.Value = StartingStats.SightDistance;

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
        if (col.gameObject.layer != (int)Shortcuts.UnityLayers.Player &&
            col.gameObject.layer != (int)Shortcuts.UnityLayers.PlayerTentacle &&
            col.gameObject.layer != (int)Shortcuts.UnityLayers.Ground)
        {
            return true;
        }
        return false;
    }
}
