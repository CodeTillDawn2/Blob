using UnityEngine;

[CreateAssetMenu(fileName = "Dict_StringToInt", menuName = "Dictionaries/StringToInt")]
public class Dict_StringToInt : ScriptableObject
{
    public DictionaryOfStringAndInt Value;

    private void OnEnable()
    {
        if (Value == null)
            Value = new DictionaryOfStringAndInt();
    }
}
