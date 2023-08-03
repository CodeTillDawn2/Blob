using Unity.VisualScripting;
using UnityEngine;

public class InBlobsStomachModifier : GameObjectInListModifierCondition<InBlobsStomachModifier>, IAmImmuneToTargetingByTentacles
{
    [SerializeField] public override bool IsStackable { get { return false; } }

    public override bool Inverse { get; set; }

    [Serialize] public GameObject Stomach;


    private Rigidbody rb;

    bool IsFloatingUp = false;

    private Shortcuts.UnityLayers previousLayer;



    public override void BeforeEffect()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Destroy(this);
        previousLayer = (Shortcuts.UnityLayers)gameObject.layer;
    }

    public override void ExecuteEffect()
    {



        gameObject.layer = (int)Shortcuts.UnityLayers.BeingEaten;
        gameObject.transform.parent = Stomach.transform;
        if (transform.position.y < Stomach.transform.position.y)
        {
            IsFloatingUp = true;
        }
        else if (transform.position.y > Stomach.transform.position.y)
        {
            IsFloatingUp = false;
        }

        if (IsFloatingUp)
        {
            //rb.AddForce(new Vector3(0, .01f * Time.fixedDeltaTime, 0), ForceMode.Force);
        }

    }

    public override void AfterEffect()
    {
        gameObject.layer = (int)previousLayer;
        gameObject.transform.parent = null;
    }

    protected override void DebugEffect()
    {
        string test = "";
    }
}
