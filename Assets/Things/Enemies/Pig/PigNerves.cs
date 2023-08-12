using System;
using UnityEngine;

public class PigNerves : Nerves
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

    #endregion

    #region Interface Fields
    public static Type[] _expectedStatsInterfaces = null;
    public override Type[] ExpectedStatsInterfaces => _expectedStatsInterfaces;

    #endregion

    #region Interface Methods

    #endregion

    #region Private methods


    #endregion

    #region Nerves
    protected override Brain brain { get; }

    protected override Senses senses { get; }

    protected override Locomotion locomotion { get; }

    protected override Body body { get; }

    [SerializeField]
    protected NervePlan _nervePlan;
    protected override NervePlan NervePlan { get { return _nervePlan; } }
    #endregion


}
