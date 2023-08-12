using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BlobBody : Body, IHaveDigestDamageDealt
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
    [Serialize] public FloatVariable TentacleHitSpeed;
    [Serialize] public BlobConfiguration StartingStats;
    [Serialize] public FloatVariable MassTarget;
    [Serialize] public FloatVariable CurrentMassPerCubicFoot;

    [HideInInspector]
    protected abstract Rigidbody rb { get; set; }



    //[SerializeField]
    //private FloatVariable currentDigestDamage;
    [HideInInspector]
    protected bool ChangingSize;
    //public FloatVariable DigestDamageDealt
    //{
    //    get
    //    { return currentDigestDamage; }
    //    set { value = currentDigestDamage; }
    //}

    [SerializeField] public FloatVariable DigestDamageDealt { get; set; }

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
        TentacleHitSpeed.Value = StartingStats.TentacleHitSpeed.Value;
        DigestDamageDealt.Value = StartingStats.DigestDamageDealt.Value;
        MassTarget.Value = StartingStats.Mass.Value;
        CurrentGrowthSpeedModifier.Value = StartingStats.GrowthSpeed.Value;
        CurrentMassPerCubicFoot.Value = StartingStats.MassPerCubicFoot.Value;
        CurrentAngularDragInsideStomach.Value = StartingStats.AngularDragInsideStomach.Value;
        CurrentDragInsideStomach.Value = StartingStats.DragInsideStomach.Value;
        CurrentSuckSpeedModifier.Value = StartingStats.SuckSpeed.Value;
        rb = RigidBodyObject.GetComponent<Rigidbody>();
        rb.mass = StartingStats.Mass.Value;
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
            if (edible != null)
            {
                edible.transform.parent = null;
            }

        }
    }

    protected void ParentEdibles()
    {
        foreach (GameObject edible in ContainedInStomach.Items)
        {
            if (edible != null)
            {
                edible.transform.parent = gameObject.transform;
            }

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


                BoxCollider box = enemyCollider as BoxCollider;
                if (box != null)
                {
                    x = box.size.x;
                    y = box.size.y;
                    z = box.size.z;
                }

                float ColliderArea = x * y * z;







                ModifierLibrary.Digestion.ApplyInBlobsStomachModifier(enemy, enemy,
                    ContainedInStomach, gameObject, BodyDims);

                ModifierLibrary.Digestion.ApplyDigestionModifier(enemy, DigestDamageDealt.Value * Time.deltaTime, gameObject);
                ModifierLibrary.OneTime.ApplyDamageModifier(enemy, DigestDamageDealt.Value * Time.deltaTime, DamageTypeLibrary.AcidDamage);





            }




        }
    }


    protected void SuckInEnemies()
    {
        foreach (GameObject enemy in IntersectsPlayer.Items.Where(x => !ContainedInStomach.Items.Contains(x)))
        {
            if (enemy != null)
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

                    ModifierLibrary.Digestion.ApplyInContactWithBlobModifier(enemy, enemy, gameObject,
                        IntersectsPlayer, CurrentDragInsideStomach, CurrentAngularDragInsideStomach);

                    //Vector3 RelativeVector = Vector3.Slerp(Vector3.zero,
                    //     gameObject.transform.position - enemy.transform.position + new Vector3(0, BodyDims.Value.y / 4f, 0), CurrentSuckSpeedModifier.Value);


                    //enemyRB.velocity = new Vector3(RelativeVector.x / Time.fixedDeltaTime, RelativeVector.y / Time.fixedDeltaTime, RelativeVector.z / Time.fixedDeltaTime);
                    float bobbingForce = 0.00001f;

                    // Check if the Rigidbody's vertical velocity is nearly zero or moving downwards
                    //if (Mathf.Approximately(Mathf.Abs(enemyRB.velocity.y), 0f) || Mathf.Abs(enemyRB.velocity.y) < 0f)
                    //{
                    // Calculate the direction towards the center of the current game object
                    Vector3 directionToCenter = (this.transform.position - enemyRB.position).normalized;

                    enemyRB.AddForce(directionToCenter * bobbingForce, ForceMode.Force);
                    //}

                }
            }





        }

    }




}
