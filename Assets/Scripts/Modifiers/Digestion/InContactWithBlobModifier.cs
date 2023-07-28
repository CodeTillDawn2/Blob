using Unity.VisualScripting;
using UnityEngine;

public class InContactWithBlobModifier : GameObjectInListModifierCondition<InContactWithBlobModifier>
{

    [SerializeField] public override bool IsStackable { get { return false; } }

    public override bool Inverse { get; set; }

    [Serialize] public FloatVariable DragInsideStomach;
    [Serialize] public FloatVariable AngularDragInsideStomach;

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
    }
    public override void ExecuteEffect()
    {
        rb.mass = 0;
        rb.useGravity = false;
        rb.drag = DragInsideStomach.Value;
        rb.angularDrag = AngularDragInsideStomach.Value;
        MassChanged = true;
    }

    public override void AfterEffect()
    {

    }



    private void OnDisable()
    {
        if (MassChanged)
        {
            rb.mass = originalMass;
            rb.useGravity = originalGravity;
            rb.drag = originalDrag;
            rb.angularDrag = originalAngularDrag;
        }
    }
    protected override void DebugEffect()
    {

    }
}
