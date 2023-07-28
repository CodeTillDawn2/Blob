using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyConsumer : MonoBehaviour
{

    [Serialize] public FloatVariable CurrentMass;
    [Serialize] public FloatVariable CubeVolume;
    [Serialize] public GameObjectRuntimeSet EnemiesBeingSuckedIn;
    [Serialize] public GameObjectRuntimeSet EnemiesInsideStomach;
    [Serialize] public GameEvent EatenEvent;

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
        StartCoroutine(Consume());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Consume()
    {
        while (IsWorking)
        {

            if (EnemiesInsideStomach == null)
            {
                string test = transform.GetPath();


            }
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
                        EatenEvent.Raise();

                    }



                }




            }

            yield return new WaitForFixedUpdate();
        }



    }
}
