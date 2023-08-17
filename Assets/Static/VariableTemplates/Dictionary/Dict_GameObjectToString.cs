using UnityEngine;

[CreateAssetMenu(fileName = "Dict_GameObjectToString", menuName = "Dictionaries/GameObjectToString")]
public class Dict_GameObjectToString : ScriptableObject
{
    public DictionaryOfStringAndInt Value;

    private void OnEnable()
    {
        if (Value == null)
            Value = new DictionaryOfStringAndInt();
    }
}
