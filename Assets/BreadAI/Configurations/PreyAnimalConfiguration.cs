using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PreyAnimalConfiguration", menuName = "AIConfiguration/PreyAnimal")]
public class PreyAnimalConfiguration : ConfigurationBase, IHaveMoveSpeed, IHaveRotateSpeed, IHaveMass, IHaveHitPoints, IHaveNutrition, ICanSee,
    IAmAlive, IUseSightBox, IHaveEyes, IUseMeshRenderer, ICanBeDamaged, ICanBeDigested, ICanDie
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _mass;
    [SerializeField] private float _hitPoints;
    [SerializeField] private float _nutrition;
    [SerializeField] private float _sightDistance;
    [SerializeField] private Shortcuts.UnityLayers _thingNearbyFilter;
    [SerializeField] private bool _isAlive;
    [SerializeField] private Shortcuts.LayerMasks _onlySeeMask;


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

    /// <summary>
    /// Provide brand new dictionary
    /// </summary>
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

    public BooleanVariable IsAlive
    {
        get
        {
            BooleanVariable so = SOLibrary.Create<BooleanVariable>();
            so.Value = _isAlive;
            return so;
        }
        set { _isAlive = value.Value; }
    }

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

    public FloatVariable HitPoints
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _hitPoints;
            return so;
        }
        set { _hitPoints = value.Value; }
    }

    public FloatVariable Nutrition
    {
        get
        {
            FloatVariable so = SOLibrary.Create<FloatVariable>();
            so.Value = _nutrition;
            return so;
        }
        set { _nutrition = value.Value; }
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
    public GameObjectRuntimeSet ThingsNearby
    {
        get
        {
            return null;
        }
        set {  }
    }

    public BoxCollider sightBox { get; set; }
    public GameObjectRuntimeSet Eyes
    {
        get
        {
            return null;
        }
        set { }
    }
    public MeshRenderer meshRenderer { get; set; }

    public void BeDamaged(float amount, DamageTypeEnum DamageType){}

    public float BeDigested(float digestDamage){ return 0; }

    public void Die() {}
}
