using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySucker : MonoBehaviour
{

    [Serialize] public FloatVariable CurrentMass;
    [Serialize] public FloatVariable CubeVolume;
    [Serialize] public GameObjectRuntimeSet EnemiesBeingSuckedIn;
    [Serialize] public GameObjectRuntimeSet EnemiesInsideStomach;
    [Serialize] public GameObject PlayerEatingObject;
    [Serialize] public GameEvent SuckedInEvent;

    private bool IsWorking = true;

    private void OnEnable()
    {
        IsWorking = true;

    }

    private void OnDisable()
    {
        IsWorking = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SuckInEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        string test = "";
    }

    public IEnumerator SuckInEnemies()
    {
        while (IsWorking)
        {
            List<GameObject> enemyList = EnemiesBeingSuckedIn.Items.Where(x => x.gameObject != null).ToList();

            foreach (GameObject enemy in enemyList)
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
                    if (CanBeSwallowed) //Suck in
                    {

                        transform.parent = PlayerEatingObject.transform;
                        EnemiesInsideStomach.Remove(enemy);
                        EnemiesBeingSuckedIn.Add(enemy);
                        gameObject.layer = (int)Shortcuts.UnityLayers.CanBeEaten;
                        SuckedInEvent.Raise();



                    }



                }




            }
            yield return new WaitForFixedUpdate();


        }



    }
}
