using System;
using System.Collections.Generic;
using UnityEngine;

public class PigSenses : Senses, ICanSee, IHaveEyes, IUseSightBox
{


    public static PigSenses instance;

    #region Unity Methods
    protected override void Awake()
    {
        instance = this;
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
    public static Type[] _expectedStatsInterfaces = { typeof(IHaveMoveSpeed) };
    public override Type[] ExpectedStatsInterfaces => _expectedStatsInterfaces;
    public FloatVariable SightDistance { get; set; }
    public LayerMaskVariable OnlySeeMask { get; set; }
    public GameObjectRuntimeSet Eyes { get; set; }
    public Dict_GameObjectToLastSeen ThingsSeen { get; set; }
    public BoxCollider MySightBox { get; set; }
    public List<GameObject> ThingsNearby { get; set; }
    public Shortcuts.UnityLayers ThingNearbyFilter { get; set; }
    GameObjectRuntimeSet IUseSightBox.ThingsNearby { get; set; }
    UnityLayerVariable IUseSightBox.ThingsNearbyFilter { get; set; }

    #endregion

    #region Interface Methods

    #endregion

    #region Private methods


    #endregion







}
