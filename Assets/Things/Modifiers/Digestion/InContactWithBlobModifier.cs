using Unity.VisualScripting;
using UnityEngine;

public class InContactWithBlobModifier : GameObjectInListModifierCondition<InContactWithBlobModifier>, IAmImmuneToTargetingByTentacles
{

    [SerializeField] public override bool IsStackable { get { return false; } }

    public override bool Inverse { get; set; }

    [Serialize] public FloatVariable DragInsideStomach;
    [Serialize] public FloatVariable AngularDragInsideStomach;
    [Serialize] public GameObject Stomach;

    private float originalMass;
    private bool originalGravity;
    private float originalDrag;
    private float originalAngularDrag;
    private Rigidbody rb;
    private bool MassChanged = false;

    public override void BeforeEffect()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Destroy(this);

        originalMass = rb.mass;
        originalGravity = rb.useGravity;
        originalDrag = rb.drag;
        originalAngularDrag = rb.angularDrag;
        rb.mass = 0;
        rb.useGravity = false;
        rb.drag = DragInsideStomach.Value;
        rb.angularDrag = AngularDragInsideStomach.Value;
        MassChanged = true;
        transform.parent = Stomach.transform;
    }
    public override void ExecuteEffect()
    {

    }

    public override void AfterEffect()
    {

        rb.mass = originalMass;
        rb.useGravity = originalGravity;
        rb.drag = originalDrag;
        rb.angularDrag = originalAngularDrag;
        transform.parent = null;
    }



    private void OnDisable()
    {

    }
    protected override void DebugEffect()
    {

    }
}
