using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BlobStomach : MonoBehaviour, IDoDigestDamage
{






    [SerializeField] private Vector3Variable originalSize;
    [Serialize] public Vector3Variable OriginalSize { get { return originalSize; } set { originalSize = value; } }
    [SerializeField] private QuaternionVariable bodyRotation;
    public QuaternionVariable BodyRotation { get { return bodyRotation; } set { bodyRotation = value; } }

    public abstract Vector3Variable BodyDims { get; set; }
    [Serialize] public FloatVariable CubeVolume;
    [Serialize] public GameObjectRuntimeSet IntersectsPlayer;
    [Serialize] public GameObjectRuntimeSet ContainedInStomach;
    [Serialize] public GameObject RigidBodyObject;
    [Serialize] public FloatVariable CurrentSuckSpeedModifier;
    [Serialize] public FloatVariable CurrentGrowthSpeedModifier;
    [Serialize] public FloatVariable CurrentDragInsideStomach;
    [Serialize] public FloatVariable CurrentAngularDragInsideStomach;
    [Serialize] public PlayerScriptableObject StartingStats;
    [Serialize] public FloatVariable MassTarget;
    [Serialize] public FloatVariable CurrentMassPerCubicFoot;

    [HideInInspector]
    protected abstract Rigidbody rb { get; set; }



    [SerializeField]
    private FloatVariable currentDigestDamage;
    [HideInInspector]
    protected bool ChangingSize;
    public FloatVariable CurrentDigestDamage
    {
        get
        { return currentDigestDamage; }
        set { value = currentDigestDamage; }
    }



    protected virtual void Start()
    {
        ResetStats();
        ChangingSize = false;
        ResetCubeDims();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ResetCubeDims();

    }



    protected virtual void FixedUpdate()
    {
        SuckInEnemies();
        DigestEnemies();
        if (!ChangingSize) StartCoroutine(ChangeSize());


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
        rb = RigidBodyObject.GetComponent<Rigidbody>();
        rb.mass = StartingStats.Mass;
    }

    public void GainNutrition(float amount)
    {
        MassTarget.Value += amount;
    }

    protected abstract IEnumerator ChangeSize();

    public abstract void CalculateIntersections();

    protected abstract void ResetCubeDims();

    protected void UnparentEdibles()
    {
        foreach (GameObject edible in ContainedInStomach.Items)
        {
            edible.transform.parent = null;
        }
    }

    protected void ParentEdibles()
    {
        foreach (GameObject edible in ContainedInStomach.Items)
        {
            edible.transform.parent = gameObject.transform;
        }
    }

    protected void DigestEnemies()
    {

        List<GameObject> DigestList = ContainedInStomach.Items.Where(x => x.gameObject != null).ToList();

        for (int i = DigestList.Count - 1; i >= 0; i--)
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







                ModifierLibrary.Digestion.ApplyInBlobsStomachModifier(enemy, enemy,
                    ContainedInStomach, gameObject);

                ModifierLibrary.Digestion.ApplyDigestionModifier(enemy, CurrentDigestDamage.Value * Time.deltaTime, gameObject);
                ModifierLibrary.OneTime.ApplyDamageModifier(enemy, CurrentDigestDamage.Value * Time.deltaTime, DamageTypeEnums.AcidDamage);





            }




        }
    }


    protected void SuckInEnemies()
    {
        foreach (GameObject enemy in IntersectsPlayer.Items.Where(x => !ContainedInStomach.Items.Contains(x)))
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

                //bool CanBeSwallowed = ColliderArea < CubeVolume.Value * .5;
                //if (CanBeSwallowed) //Suck in
                //{

                //enemy.transform.parent = gameObject.transform;

                enemy.gameObject.layer = (int)Shortcuts.UnityLayers.CanBeEaten;

                ModifierLibrary.Digestion.ApplyInContactWithBlobModifier(enemy, enemy,
                    IntersectsPlayer, CurrentDragInsideStomach, CurrentAngularDragInsideStomach);

                Vector3 RelativeVector = Vector3.Slerp(Vector3.zero,
                     gameObject.transform.position - enemy.transform.position + new Vector3(0, BodyDims.Value.y / 4f, 0), CurrentSuckSpeedModifier.Value);

                enemyRB.velocity = new Vector3(RelativeVector.x / Time.fixedDeltaTime, RelativeVector.y / Time.fixedDeltaTime, RelativeVector.z / Time.fixedDeltaTime);

                //}



            }




        }

    }




}
