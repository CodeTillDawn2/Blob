using Unity.VisualScripting;
using UnityEngine;

public class DigestNutritionModifier : OneTimeModifierCondition<DigestNutritionModifier>, IAmImmuneToTargetingByTentacles
{
    [SerializeField] public override bool IsStackable { get { return false; } }

    [Serialize] public float DigestAmount;
    [Serialize] public GameObject Digester;

    public override void BeforeEffect()
    {

    }

    public override void ExecuteEffect()
    {
        if (TryGetComponent<ICanBeDigested>(out ICanBeDigested gameObject))
        {
            float NutritionAmount = gameObject.BeDigested(DigestAmount);

            if (Digester.TryGetComponent<ICanGainNutrition>(out ICanGainNutrition digester))
            {
                digester.GainNutrition(NutritionAmount);
            }

        }
    }

    public override void AfterEffect()
    {

    }


    protected override void DebugEffect()
    {

    }

}
