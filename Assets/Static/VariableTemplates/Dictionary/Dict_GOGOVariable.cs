using UnityEngine;

[CreateAssetMenu(fileName = "Dict_GameObjectToGameObject", menuName = "Dictionaries/GameObjectToGameObject")]
public class Dict_GameObjectToGameObject : ScriptableObject
{
    public DictionaryOfGOAndGO Value;

    private void OnEnable()
    {
        if (Value == null)
            Value = new DictionaryOfGOAndGO();
    }
}
