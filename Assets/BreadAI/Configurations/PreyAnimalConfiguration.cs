using Newtonsoft.Json;
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

    [JsonIgnore]
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
    [JsonIgnore]
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
    [JsonIgnore]
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
    [JsonIgnore]
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
    [JsonIgnore]
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
    [JsonIgnore]
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
    [JsonIgnore]
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
    [JsonIgnore]
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
    [JsonIgnore]
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
    [JsonIgnore]
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
    [JsonIgnore]
    public GameObjectRuntimeSet ThingsNearby
    {
        get
        {
            return null;
        }
        set {  }
    }

    [JsonIgnore]
    public BoxCollider sightBox { get; set; }
    [JsonIgnore]
    public GameObjectRuntimeSet Eyes
    {
        get
        {
            return null;
        }
        set { }
    }
    [JsonIgnore]
    public MeshRenderer meshRenderer { get; set; }

    public void BeDamaged(float amount, DamageTypeEnum DamageType){}

    public float BeDigested(float digestDamage){ return 0; }

    public void Die() {}
}
