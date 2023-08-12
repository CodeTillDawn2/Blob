using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectVariable", menuName = "ScriptableObjects/GameObject")]
public class GameObjectVariable : ScriptableObject, IConfigDefault
{
    public GameObject Value;
}
