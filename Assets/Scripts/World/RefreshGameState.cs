using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RefreshGameState : MonoBehaviour
{

    [Serialize] public GameObjectRuntimeSet AllEnemies;
    [Serialize] public GameObjectRuntimeSet EnemiesBeingEaten;
    [Serialize] public GameObjectRuntimeSet EnemiesInStomach;
    [Serialize] public GameObjectRuntimeSet EnemiesIntersectingPlayer;
    [Serialize] public GameObjectRuntimeSet EnemiesSeenByPlayer;
    [Serialize] public RaycastInfoRuntimeSet InsideStomachHits;
    [Serialize] public BooleanVariable SceneReady;

    private void Awake()
    {
        SceneReady.Value = false;
        AllEnemies.RemoveAll();
        EnemiesBeingEaten.RemoveAll();
        EnemiesInStomach.RemoveAll();
        EnemiesIntersectingPlayer.RemoveAll();
        InsideStomachHits.RemoveAll();
        EnemiesSeenByPlayer.RemoveAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneReady.Value = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
