using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeingDigestedByBlobModifier : ModifierClass
{

    [Serialize] public GameObject PlayerGameObject;
    [Serialize] public FloatVariable CubeWidth;
    [Serialize] public float Duration;

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


     
        StartCoroutine(WaitForEffectToWearOff());
    }



    public IEnumerator WaitForEffectToWearOff()
    {
        while (!StartScript) yield return null;

        if (transform.position.y < PlayerGameObject.transform.position.y + (CubeWidth.Value * .4f))
        {
            rb.AddForce(new Vector3(0, .01f * Time.fixedDeltaTime, 0), ForceMode.Force);
        }



        //TakeDamage(digestDamage * Time.fixedDeltaTime);


        //if (currentHitPoints < 0)
        //{
        //    ChangeMass(-currentHitPoints);
        //    currentNutrition += currentHitPoints;
        //    currentHitPoints = 0;
        //}

        //if (currentNutrition <= 0 && currentHitPoints <= 0)
        //{
        //    Destroy(gameObject);
        //}
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
