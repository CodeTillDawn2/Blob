using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class BlobStomach : MonoBehaviour, IDoDigestDamage
{

    [Serialize] public GameObjectRuntimeSet EnemiesBeingSuckedIn;
    [Serialize] public GameObjectRuntimeSet EnemiesInsideStomach;
    [Serialize] public FloatVariable CurrentSuckSpeedModifier;
    [Serialize] public FloatVariable CubeVolume;
    [Serialize] public Vector3Variable BlobDims;
    [Serialize] public Vector3Variable BlobConstraints;
    [Serialize] [Tooltip("Rigidbody to control")] public GameObjectVariable RigidbodyObject;
    //[Serialize] public GameEvent SuckedInEvent;
    [Serialize] public FloatVariable CurrentAngularDragInsideStomach;
    [Serialize] public FloatVariable CurrentDigestionDamage;
    [Serialize] public FloatVariable CurrentDragInsideStomach;
    [Serialize] public PlayerScriptableObject StartingStats;
    [Serialize] public FloatVariable MassTarget;
    [Serialize] public FloatVariable CurrentMassPerCubicFoot;
    [Serialize] public FloatVariable CurrentGrowthSpeedModifier;
    
    [Serialize] public GameObject topSide;

    [HideInInspector]
    private BoxCollider collider_TopSide;

    private Rigidbody rb;

    [SerializeField]
    private FloatVariable currentDigestDamage;
    public FloatVariable CurrentDigestDamage
    {
        get
        { return currentDigestDamage; }
        set { value = currentDigestDamage; }
    }


    protected void Awake()
    {
        
    }

    protected void Start()
    {
        rb = RigidbodyObject.Value.GetComponent<Rigidbody>();
        ResetStats();
         collider_TopSide = topSide.GetComponent<BoxCollider>();
        
        ResetCubeDims();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        SuckInEnemies();
        DigestEnemies();
        if (rb.mass != MassTarget.Value)
        {
            StartCoroutine(ChangeSize());
        }
    }

    private void ResetStats()
    {
        CurrentDigestDamage.Value = StartingStats.DigestDamage;
        MassTarget.Value = StartingStats.Mass;
        CurrentGrowthSpeedModifier.Value = StartingStats.GrowthSpeedModifier;
        CurrentMassPerCubicFoot.Value = StartingStats.MassPerCubicFoot;
        CurrentAngularDragInsideStomach.Value = StartingStats.AngularDragInsideStomach;
        CurrentDragInsideStomach.Value = StartingStats.DragInsideStomach;
        CurrentSuckSpeedModifier.Value = StartingStats.SuckSpeedModifier;
        rb.mass = StartingStats.Mass;
    }

    public void GainNutrition(float amount)
    {
        MassTarget.Value += amount;
    }

    private Vector3 GetBoxSideSizes(Vector3 constraints, float TargetVolume)
    {
        int NumberOfConstraints = 0;
        float Xconstraint = 0;
        float Yconstraint = 0;
        float Zconstraint = 0;
        Vector3 sideLengths = Vector3.one;

        if (Xconstraint > 0)
        {
            NumberOfConstraints++;
        }
        if (Yconstraint > 0)
        {
            NumberOfConstraints++;
        }
        if (Zconstraint > 0)
        {
            NumberOfConstraints++;
        }

        if (NumberOfConstraints == 1)
        {
            if (Xconstraint > 0)
            {
                float OtherConstraints = TargetVolume / Xconstraint;
                sideLengths = new Vector3(Xconstraint, OtherConstraints, OtherConstraints);
            }
            else if (Yconstraint > 0)
            {
                float OtherConstraints = TargetVolume / Yconstraint;
                sideLengths = new Vector3(OtherConstraints, Yconstraint, OtherConstraints);
            }
            else if (Zconstraint > 0)
            {
                float OtherConstraints = TargetVolume / Zconstraint;
                sideLengths = new Vector3(OtherConstraints, OtherConstraints, Zconstraint);
            }
        }
        else if (NumberOfConstraints == 2)
        {
            if (Xconstraint == 0)
            {
                sideLengths = new Vector3(TargetVolume / Yconstraint / Zconstraint, Yconstraint, Zconstraint);
            }
            else if (Yconstraint == 0)
            {
                sideLengths = new Vector3(Xconstraint, TargetVolume / Xconstraint / Zconstraint, Zconstraint);
            }
            else if (Zconstraint == 0)
            {
                sideLengths = new Vector3(Xconstraint, Yconstraint, TargetVolume / Xconstraint / Yconstraint);
            }
        }
        else if (NumberOfConstraints == 3) throw new NotImplementedException();

        Vector3 BuildingBlock;
        float Multiplier;
        if (sideLengths.x <= sideLengths.y && sideLengths.x <= sideLengths.z)
        {
            Multiplier = sideLengths.x;
            BuildingBlock = new Vector3(1f, sideLengths.y / sideLengths.x, sideLengths.z / sideLengths.x);
        }
        else if (sideLengths.y <= sideLengths.z && sideLengths.y <= sideLengths.z)
        {
            Multiplier = sideLengths.y;
            BuildingBlock = new Vector3(sideLengths.x / sideLengths.y, 1f, sideLengths.z / sideLengths.y);
        } 
        else //z
        {
            Multiplier = sideLengths.z;
            BuildingBlock = new Vector3(sideLengths.x / sideLengths.z, sideLengths.y / sideLengths.z, 1f);
        }

        sideLengths = BuildingBlock * Multiplier;



        return sideLengths;
    }

    private IEnumerator ChangeSize()
    {
        float TargetVolume = MassTarget.Value / CurrentMassPerCubicFoot.Value;
        Vector3 SideLengths = GetBoxSideSizes(BlobConstraints.Value, TargetVolume);
        float MassDifferential;

        transform.localScale = new Vector3(transform.localScale.x * ratio, transform.localScale.y * ratio, transform.localScale.z * ratio);

        //if (MassTarget.Value > rb.mass)
        //{
        //    MassDifferential = ratio - 1;
        //}
        //else
        //{
        //    MassDifferential = (rb.mass / MassTarget.Value) - 1;
        //}

        //if (MassDifferential > 0.001)
        //{

        //    if (MassDifferential < .01)
        //    {

        //        UnparentEdibles();
        //        transform.localScale = new Vector3(transform.localScale.x * ratio, transform.localScale.y * ratio, transform.localScale.z * ratio);

        //        ParentEdibles();
        //        rb.mass = MassTarget.Value;
        //        ResetCubeDims();
        //        yield return null;
        //    }
        //    else
        //    {
        //        UnparentEdibles();
        //        float TargetScale = transform.localScale.x * ratio;
        //        transform.localScale += new Vector3(TargetScale - transform.localScale.x,
        //                TargetScale - transform.localScale.y, TargetScale - transform.localScale.z) * Time.fixedDeltaTime * CurrentGrowthSpeedModifier.Value;
        //        ParentEdibles();

        //        float CubeWidth = ResetCubeDims();
        //        rb.mass = CubeWidth * CubeWidth * CubeWidth * CurrentMassPerCubicFoot.Value;
        //        yield return new WaitForEndOfFrame();

        //    }




        //}

    }

    private float ResetCubeDims()
    {
        CubeWidth.Value = collider_TopSide.size.x * transform.localScale.x;
        CubeVolume.Value = CubeWidth.Value * CubeWidth.Value * CubeWidth.Value;
        return CubeWidth.Value;

    }

    private void UnparentEdibles()
    {
        foreach (GameObject edible in EnemiesInsideStomach.Items)
        {
            edible.transform.parent = null;
        }
    }

    private void ParentEdibles()
    {
        foreach (GameObject edible in EnemiesInsideStomach.Items)
        {
            edible.transform.parent = gameObject.transform;
        }
    }

    private void DigestEnemies()
    {

        List<GameObject> DigestList = EnemiesInsideStomach.Items.Where(x => x.gameObject != null).ToList();
        for (int i = DigestList.Count-1; i >= 0; i--)
        {
            GameObject enemy = DigestList[i];
            Collider enemyCollider = enemy.GetComponent<Collider>();
            if (enemyCollider != null)
            {
                float x = enemyCollider.bounds.size.x;
                float y = enemyCollider.bounds.size.y;
                float z = enemyCollider.bounds.size.z;

                BoxCollider box = (BoxCollider)enemyCollider;
                if (box != null)
                {
                    x = box.size.x;
                    y = box.size.y;
                    z = box.size.z;
                }

                float ColliderArea = x * y * z;

                bool CanBeSwallowed = ColliderArea < CubeVolume.Value * .5;
                if (CanBeSwallowed) //Eat
                {

                    EnemiesBeingSuckedIn.Items.Remove(enemy);
                    EnemiesInsideStomach.Items.Add(enemy);

                    enemy.transform.parent = gameObject.transform;

                    enemy.gameObject.layer = (int)Shortcuts.UnityLayers.BeingEaten;

                    StartCoroutine(ModifierLibrary.Digestion.ApplyInBlobsStomachModifier(enemy, enemy,
                        EnemiesBeingSuckedIn, CubeWidth));

                    StartCoroutine(ModifierLibrary.Digestion.ApplyDigestionModifier(enemy, CurrentDigestionDamage.Value * Time.deltaTime, gameObject));
                    StartCoroutine(ModifierLibrary.OneTime.ApplyDamageModifier(enemy, CurrentDigestionDamage.Value * Time.deltaTime, DamageTypeEnums.AcidDamage));

                }



            }




        }
    }

   
    private void SuckInEnemies()
    {
        foreach (GameObject enemy in EnemiesBeingSuckedIn.Items)
        {
            Collider enemyCollider = enemy.GetComponent<Collider>();
            Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();
            if (enemyCollider != null && enemyRB != null)
            {
                float x = enemyCollider.bounds.size.x;
                float y = enemyCollider.bounds.size.y;
                float z = enemyCollider.bounds.size.z;

                BoxCollider box = (BoxCollider)enemyCollider;
                if (box != null)
                {
                    x = box.size.x;
                    y = box.size.y;
                    z = box.size.z;
                }

                float ColliderArea = x * y * z;

                bool CanBeSwallowed = ColliderArea < CubeVolume.Value * .5;
                if (CanBeSwallowed) //Suck in
                {

                    //EnemiesInsideStomach.Remove(enemy);
                    //EnemiesBeingSuckedIn.Add(enemy);
                    enemy.transform.parent = gameObject.transform;

                    enemy.gameObject.layer = (int)Shortcuts.UnityLayers.CanBeEaten;

                    StartCoroutine(ModifierLibrary.Digestion.ApplyInContactWithBlobModifier(enemy, enemy,
                        EnemiesBeingSuckedIn, CurrentDragInsideStomach, CurrentAngularDragInsideStomach));

                    Vector3 RelativeVector = Vector3.Slerp(Vector3.zero,
                         gameObject.transform.position - enemy.transform.position + new Vector3(0, CubeWidth.Value * .5f, 0), CurrentSuckSpeedModifier.Value);

                    enemyRB.velocity = new Vector3(RelativeVector.x / Time.fixedDeltaTime, RelativeVector.y / Time.fixedDeltaTime, RelativeVector.z / Time.fixedDeltaTime);

                }



            }




        }

    }




}
