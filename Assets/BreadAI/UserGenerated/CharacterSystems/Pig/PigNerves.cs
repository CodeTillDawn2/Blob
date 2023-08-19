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

    #endregion

    #region Interface Methods

    #endregion

    #region Private methods


    #endregion

    #region Nerves


    [SerializeField]
    protected NervePlan _nervePlan;
    protected override NervePlan NervePlan { get { return _nervePlan; } }
    #endregion


}
