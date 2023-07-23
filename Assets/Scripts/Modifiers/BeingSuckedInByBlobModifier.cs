using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeingSuckedInByBlobModifier : ModifierClass
{

    [Serialize] public float Duration;
    [Serialize] public float DragInsideStomach;
    [Serialize] public float AngularDragInsideStomach;

    private float originalMass;
    private bool originalGravity;
    private float originalDrag;
    private float originalAngularDrag;
    private Rigidbody rb;
    private bool MassChanged = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        Duration = 0f;
        if (!IsStackable) RemoveDuplicateEffect();

        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Destroy(this);

        originalMass = rb.mass;
        originalGravity = rb.useGravity;
        originalDrag = rb.drag;
        originalAngularDrag = rb.angularDrag;

     
        StartCoroutine(WaitForEffectToWearOff());
    }



    public IEnumerator WaitForEffectToWearOff()
    {
        while (!StartScript) yield return null;
        rb.mass = 0;
        rb.useGravity = false;
        rb.drag = DragInsideStomach;
        rb.angularDrag = AngularDragInsideStomach;
        MassChanged = true;
        yield return new WaitForSeconds(Duration);
        Destroy(this);
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

}
