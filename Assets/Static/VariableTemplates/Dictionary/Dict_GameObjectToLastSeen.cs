using UnityEngine;

[CreateAssetMenu(fileName = "Dict_GameObjectToLastSeen", menuName = "Dictionaries/GameObjectToLastSeen")]
public class Dict_GameObjectToLastSeen : ScriptableObject
{
    public DictionaryOfGameObjectAndLastSeenData Value;

    private void OnEnable()
    {
        if (Value == null)
            Value = new DictionaryOfGameObjectAndLastSeenData();
    }

}
