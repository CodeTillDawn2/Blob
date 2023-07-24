using System;
using Unity.VisualScripting;
using UnityEngine;

public class InBlobsStomachModifier : GameObjectInListModifierCondition<InBlobsStomachModifier>
{
    [SerializeField] public override bool IsStackable { get { return false; } }

    public override bool Inverse { get; set; }

    [Serialize] public FloatVariable CubeWidth;


    private Rigidbody rb;



    public override void BeforeEffect()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Destroy(this);
    }

    public override void ExecuteEffect()
    {
        if (transform.position.y < Comparer1.transform.position.y + (CubeWidth.Value * .4f))
        {
            rb.AddForce(new Vector3(0, .01f * Time.fixedDeltaTime, 0), ForceMode.Force);
        }
    }

    public override void AfterEffect()
    {
        //Destroy(this);
        //Destroy(Comparer1);
    }


}
