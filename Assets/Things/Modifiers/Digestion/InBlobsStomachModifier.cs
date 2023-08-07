using Unity.VisualScripting;
using UnityEngine;

public class InBlobsStomachModifier : GameObjectInListModifierCondition<InBlobsStomachModifier>, IAmImmuneToTargetingByTentacles
{
    [SerializeField] public override bool IsStackable { get { return false; } }
    public override bool Inverse { get; set; }
    [Serialize] public GameObject Stomach;

    private Rigidbody rb;
    private bool isInsideStomach = false;
    private Vector3 stomachCenter;
    public Vector3Variable BlobDims;

    public float stayInsideThreshold = 0.5f;
    public float floatingForce = 1.0f;
    public float buoyancyForce = 1.0f;

    private Shortcuts.UnityLayers previousLayer;

    private void Start()
    {
        UpdateStomachCenter();
    }

    private void UpdateStomachCenter()
    {
        Vector3 halfBlobDims = BlobDims.Value * 0.5f;
        stomachCenter = Stomach.transform.position + Stomach.transform.TransformDirection(halfBlobDims);
    }

    public override void BeforeEffect()
    {

        previousLayer = (Shortcuts.UnityLayers)gameObject.layer;

    }

    public override void ExecuteEffect()
    {
        gameObject.layer = (int)Shortcuts.UnityLayers.BeingEaten;

        UpdateStomachCenter();

        //if (!isInsideStomach)
        //{
        //    // Check if the object is inside the stomach
        //    float distanceToStomach = Vector3.Distance(transform.position, stomachCenter);
        //    if (distanceToStomach <= stayInsideThreshold)
        //    {
        //        isInsideStomach = true;
        //    }
        //}

        //if (isInsideStomach)
        //{
        //    // Apply a force to the pig to simulate buoyancy
        //    rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Force);

        //    // Apply a force to the pig to keep it floating inside the stomach
        //    Vector3 targetPosition = stomachCenter + (Random.insideUnitSphere * stayInsideThreshold);
        //    Vector3 forceDirection = (targetPosition - transform.position).normalized;
        //    rb.AddForce(forceDirection * floatingForce, ForceMode.Force);
        //}
    }

    public override void AfterEffect()
    {
        // No need to do anything after the effect
        gameObject.layer = (int)previousLayer;
    }

    protected override void DebugEffect()
    {
        string test = "";
    }
}
