using System;
using UnityEngine;

public class PigBody : Body, IUseMeshRenderer, IAmAlive, IHaveHitPoints, IHaveNutrition,
    ICanBeDamaged, ICanBeDigested, ICanDie, IHaveMass
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

    public static Type[] _expectedStatsInterfaces = { typeof(IAmAlive), typeof(IHaveHitPoints)
                                                    , typeof(IHaveMass)
    };
    public override Type[] ExpectedStatsInterfaces => _expectedStatsInterfaces;


    public MeshRenderer meshRenderer { get; set; }
    public BooleanVariable IsAlive { get; set; }
    public FloatVariable Mass { get; set; }
    public FloatVariable HitPoints { get; set; }
    public FloatVariable Nutrition { get; set; }
    #endregion

    #region Interface Methods

    [AIInvoluntary]
    [AIRequiredFieldFalse(typeof(IAmAlive))]
    [AIRequiredFieldTrue(typeof(ICanBeDigested))]
    [AIRequiredRisk(typeof(IHaveNutrition))]
    public float BeDigested(float digestDamage)
    {

        float NutritionGained = 0;
        if (!IsAlive)
        {
            if (Nutrition.Value > 0)
            {
                NutritionGained = digestDamage;
                if (NutritionGained > Nutrition.Value) NutritionGained = Nutrition.Value;


                Nutrition.Value = Nutrition.Value - digestDamage;
            }
            if (Nutrition.Value <= 0) Destroy(gameObject);
        }
        return NutritionGained;
    }


    [AIInvoluntary]
    [AIRequiredRisk(typeof(IHaveHitPoints))]
    [AIRequiredRisk(typeof(ICanDie))]
    [AIRequiredInputOr(typeof(IHaveDigestDamageDealt))] //Needs other damage types
    public void BeDamaged(float Damage, DamageTypeEnum DamageType)
    {

        if (HitPoints.Value > 0)
        {
            HitPoints.Value = HitPoints.Value - Damage;
        }
        if (HitPoints.Value < 0) HitPoints.Value = 0;

    }

    [AIInvoluntary]
    [AIRequiredRisk(typeof(IAmAlive))]
    [AIOptionalInfluencer(typeof(IHaveHitPoints))]
    public void Die()
    {
        IsAlive.Value = false;
    }
    #endregion
    #region Private methods


    #endregion
}
