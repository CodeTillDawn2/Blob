using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStomach : MonoBehaviour
{

    [Serialize] public GameObjectRuntimeSet EnemiesBeingSuckedIn;
    [Serialize] public GameObjectRuntimeSet EnemiesInsideStomach;
    [Serialize] public FloatVariable SuckSpeedModifier;
    [Serialize] public FloatVariable CubeVolume;
    [Serialize] public FloatVariable CubeWidth;
    [Serialize] public GameObject PlayerEatingObject;
    //[Serialize] public GameEvent SuckedInEvent;
    [Serialize] public FloatVariable CurrentAngularDragInsideStomach;
    [Serialize] public FloatVariable CurrentDragInsideStomach;
    [Serialize] public GameObjectVariable PlayerGameObject;
    protected void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    void FixedUpdate()
    {
        SuckInEnemies();
        DigestEnemies();
    }

    private void DigestEnemies()
    {


        foreach (GameObject enemy in EnemiesInsideStomach.Items.Where(x => x.gameObject != null))
        {
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

                    enemy.transform.parent = PlayerEatingObject.transform;

                    enemy.gameObject.layer = (int)Shortcuts.UnityLayers.BeingEaten;

                    ApplyBeingDigestedByBlobModifier(enemy);

                }



            }




        }
    }
    private void ApplyBeingSuckedInByBlobModifier(GameObject target)
    {
        BeingSuckedInByBlobModifier existingModifier = target.gameObject.GetComponent<BeingSuckedInByBlobModifier>();
        if (existingModifier == null)
        {
            BeingSuckedInByBlobModifier InStomachEffect = target.gameObject.AddComponent<BeingSuckedInByBlobModifier>();
            InStomachEffect.DragInsideStomach = CurrentDragInsideStomach.Value;
            InStomachEffect.AngularDragInsideStomach = CurrentAngularDragInsideStomach.Value;
            InStomachEffect.Duration = 20;
            InStomachEffect.StartModifier();
        }
    }
    private void ApplyBeingDigestedByBlobModifier(GameObject target)
    {
        ApplyBeingSuckedInByBlobModifier(target);
        BeingDigestedByBlobModifier existingModifier = target.gameObject.GetComponent<BeingDigestedByBlobModifier>();
        if (existingModifier == null)
        {
            BeingDigestedByBlobModifier InStomachEffect = target.gameObject.AddComponent<BeingDigestedByBlobModifier>();
            InStomachEffect.PlayerGameObject = PlayerGameObject.Value;
            InStomachEffect.Duration = 20;
            InStomachEffect.StartModifier();
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
                    EnemiesInsideStomach.Remove(enemy);
                    EnemiesBeingSuckedIn.Add(enemy);
                    enemy.transform.parent = PlayerEatingObject.transform;
                    
                    enemy.gameObject.layer = (int)Shortcuts.UnityLayers.CanBeEaten;

                    ApplyBeingSuckedInByBlobModifier(enemy);

                    Vector3 RelativeVector = Vector3.Slerp(Vector3.zero,
                         PlayerGameObject.Value.transform.position - enemy.transform.position + new Vector3(0, CubeWidth.Value * .5f, 0), SuckSpeedModifier.Value);

                    enemyRB.velocity = new Vector3(RelativeVector.x / Time.fixedDeltaTime, RelativeVector.y / Time.fixedDeltaTime, RelativeVector.z / Time.fixedDeltaTime);

                }



            }




        }

    }

}
