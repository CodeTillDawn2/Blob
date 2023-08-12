using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlobConfiguration", menuName = "AIConfiguration/Blob")]
public class BlobConfiguration : ConfigurationBase, IHaveMoveSpeed, IHaveRotateSpeed, IHaveMass, ICanSee, IHaveTentacleReach, IHaveThingsInMyStomach, IHaveSuckSpeed, IHaveMassPerCubicFoot,
    IHaveDigestDamageDealt, IHaveGrowthSpeed, IHaveTentacleHitSpeed, IUseSightBox, IHaveEyes
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _mass;
    [SerializeField] private float _sightDistance;
    [SerializeField] private float _minTentacleReach;
    [SerializeField] private float _maxTentacleReach;
    [SerializeField] private int _maxTentacles;
    [SerializeField] private float _tentacleHitSpeed;
    [SerializeField] private float _dragInsideStomach;
    [SerializeField] private float _angularDragInsideStomach;
    [SerializeField] private float _suckSpeed;
    [SerializeField] private float _massPerCubicFoot;
    [SerializeField] private float _digestDamageDealt;
    [SerializeField] private float _growthSpeed;
    [SerializeField] private Shortcuts.LayerMasks _onlySeeMask;
    [SerializeField] private Shortcuts.UnityLayers _thingNearbyFilter;


    public FloatVariable MoveSpeed
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _moveSpeed;
            return so;
        }
        set { _moveSpeed = value.Value; }
    }

    public FloatVariable RotateSpeed
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _rotateSpeed;
            return so;
        }
        set { _rotateSpeed = value.Value; }
    }

    public FloatVariable Mass
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _mass;
            return so;
        }
        set { _mass = value.Value; }
    }

    public FloatVariable SightDistance
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _sightDistance;
            return so;
        }
        set { _sightDistance = value.Value; }
    }

    public FloatVariable MinTentacleReach
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _minTentacleReach;
            return so;
        }
        set { _minTentacleReach = value.Value; }
    }

    public FloatVariable MaxTentacleReach
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _maxTentacleReach;
            return so;
        }
        set { _maxTentacleReach = value.Value; }
    }

    public IntegerVariable MaxTentacles
    {
        get
        {
            IntegerVariable so = SOLibrary.Create<IntegerVariable>();
            so.Value = _maxTentacles;
            return so;
        }
        set { _maxTentacles = value.Value; }
    }

    public FloatVariable TentacleHitSpeed
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _tentacleHitSpeed;
            return so;
        }
        set { _tentacleHitSpeed = value.Value; }
    }

    public FloatVariable DragInsideStomach
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _dragInsideStomach;
            return so;
        }
        set { _dragInsideStomach = value.Value; }
    }

    public FloatVariable AngularDragInsideStomach
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _angularDragInsideStomach;
            return so;
        }
        set { _angularDragInsideStomach = value.Value; }
    }

    public FloatVariable SuckSpeed
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _suckSpeed;
            return so;
        }
        set { _suckSpeed = value.Value; }
    }

    public FloatVariable MassPerCubicFoot
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _massPerCubicFoot;
            return so;
        }
        set { _massPerCubicFoot = value.Value; }
    }

    public FloatVariable DigestDamageDealt
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _digestDamageDealt;
            return so;
        }
        set { _digestDamageDealt = value.Value; }
    }

    public FloatVariable GrowthSpeed
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _growthSpeed;
            return so;
        }
        set { _growthSpeed = value.Value; }
    }

    public LayerMaskVariable OnlySeeMask
    {
        get
        {
            LayerMaskVariable so = SOLibrary.Create<LayerMaskVariable>();
            so.Value = _onlySeeMask;
            return so;
        }
        set { _onlySeeMask = value.Value; }
    }

    public Dict_GameObjectToLastSeen ThingsSeen
    {
        get
        {
            return null;
        }
        set
        {
            
        }
    }


    public GameObjectRuntimeSet ThingsNearby
    {
        get
        {
            return null;
        }
        set
        {
            
        }
    }

    public UnityLayerVariable ThingNearbyFilter
    {
        get
        {
            UnityLayerVariable so = SOLibrary.Create<UnityLayerVariable>();
            so.Value = _thingNearbyFilter;
            return so;
        }
        set { _thingNearbyFilter = value.Value; }
    }



    public BoxCollider sightBox { get; set; }
    public GameObjectRuntimeSet Eyes { get; set; }
}
