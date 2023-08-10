using Unity.VisualScripting;
using UnityEngine;

public class RefreshGameState : MonoBehaviour
{

    [Serialize] public GameObjectRuntimeSet AllEnemies;
    [Serialize] public GameObjectRuntimeSet EnemiesInStomach;
    [Serialize] public GameObjectRuntimeSet EnemiesIntersectingPlayer;
    [Serialize] public GameObjectRuntimeSet ThingsNearby;
    [Serialize] public BooleanVariable SceneReady;

    [Serialize] public Dict_GameObjectToGameObject TentacleTargeting;
    [Serialize] public Dict_GameObjectToLastSeen EnemiesSeenByPlayer;

    private void Awake()
    {

        SceneReady.Value = false;
        AllEnemies.RemoveAll();
        EnemiesInStomach.RemoveAll();
        EnemiesIntersectingPlayer.RemoveAll();
        ThingsNearby.RemoveAll();

        TentacleTargeting.Value.Clear();
        EnemiesSeenByPlayer.Value.Clear();
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
