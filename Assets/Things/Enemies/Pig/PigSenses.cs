using UnityEngine;

public class PigSenses : Senses, ICanSee, IHaveEyes, IUseSightBox
{


    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();

    }
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

    #endregion


    #region Interface Fields
    private GameObjectRuntimeSet _eyes;
    private Dict_GameObjectToLastSeen _thingsSeen;
    private GameObjectRuntimeSet _thingsNearby;

    public FloatVariable SightDistance { get; set; }
    public LayerMaskVariable OnlySeeMask { get; set; }

    [IsTrigger]
    public BoxCollider MySightBox { get; set; }
    public UnityLayerVariable ThingsNearbyFilter { get; set; }


    public GameObjectRuntimeSet Eyes
    {
        get
        {
            if (_eyes == null)
            {
                _eyes = SOLibrary.Create<GameObjectRuntimeSet>();
            }
            return _eyes;
        }
        set
        {
            _eyes = value;
        }
    }

    public Dict_GameObjectToLastSeen ThingsSeen
    {
        get
        {
            if (_thingsSeen == null)
            {
                _thingsSeen = SOLibrary.Create<Dict_GameObjectToLastSeen>();
            }
            return _thingsSeen;
        }
        set
        {
            _thingsSeen = value;
        }
    }


    public GameObjectRuntimeSet ThingsNearby
    {
        get
        {
            if (_thingsNearby == null)
            {
                _thingsNearby = SOLibrary.Create<GameObjectRuntimeSet>();
            }
            return _thingsNearby;
        }
        set
        {
            _thingsNearby = value;
        }
    }



    #endregion

    #region Interface Methods

    #endregion

    #region Private methods


    #endregion







}
