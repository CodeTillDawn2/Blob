using System;

public class PigLocomotion : Locomotion, IHaveMoveSpeed, IHaveRotateSpeed
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
    public static Type[] _expectedStatsInterfaces = { typeof(IHaveMoveSpeed), typeof(IHaveRotateSpeed) };
    public override Type[] ExpectedStatsInterfaces => _expectedStatsInterfaces;
    public FloatVariable MoveSpeed { get; set; }
    public FloatVariable RotateSpeed { get; set; }
    #endregion

    #region Interface Methods
    [AIMovement]
    [AIRequiredInfluencer(typeof(IHaveMoveSpeed))]
    [AIRequiredInfluencer(typeof(IHaveRotateSpeed))]
    public void Move()
    {

    }
    #endregion

    #region Private methods


    #endregion









}
