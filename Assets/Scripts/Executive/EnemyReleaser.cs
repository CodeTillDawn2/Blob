using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyReleaser : MonoBehaviour
{

    [Serialize] public FloatVariable CurrentMass;
    [Serialize] public FloatVariable CubeVolume;
    [Serialize] public GameObjectRuntimeSet EnemiesBeingSuckedIn;
    [Serialize] public GameObjectRuntimeSet EnemiesInsideStomach;
    [Serialize] public GameEvent ReleasedEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Release();
    }

    public void Release()
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
                if (!CanBeSwallowed) //Release
                {
                    EnemiesBeingSuckedIn.Items.Remove(enemy);
                    EnemiesInsideStomach.Items.Remove(enemy);
                    if (CanBeSwallowed)
                    {
                        gameObject.layer = (int)Shortcuts.UnityLayers.CanBeEaten;
                    }
                    else
                    {
                        gameObject.layer = (int)Shortcuts.UnityLayers.Default;
                    }
                    ReleasedEvent.Raise();
                }


            }
            else
            {
                EnemiesBeingSuckedIn.Remove(enemy);
                EnemiesInsideStomach.Remove(enemy);
                ReleasedEvent.Raise();
            }



        }


    }

}
