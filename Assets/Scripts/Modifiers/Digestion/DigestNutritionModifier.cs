using Unity.VisualScripting;
using UnityEngine;

public class DigestNutritionModifier : OneTimeModifierCondition<DigestNutritionModifier>
{
    [SerializeField] public override bool IsStackable { get { return false; } }

    [Serialize] public float DigestAmount;
    [Serialize] public GameObject Digester;

    public override void BeforeEffect()
    {

    }

    public override void ExecuteEffect()
    {
        if (TryGetComponent<IAmDigestable>(out IAmDigestable gameObject))
        {
            float NutritionAmount = gameObject.Digest(DigestAmount);

            if (Digester.TryGetComponent<IDoDigestDamage>(out IDoDigestDamage digester))
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
