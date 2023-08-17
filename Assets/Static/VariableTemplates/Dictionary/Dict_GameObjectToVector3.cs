using UnityEngine;

[CreateAssetMenu(fileName = "Dict_GameObjectToVectorThree", menuName = "Dictionaries/GameObjectToVector3")]
public class Dict_GameObjectToVectorThree : ScriptableObject
{
    public DictionaryOfGameObjectAndVector3 Value;

    private void OnEnable()
    {
        if (Value == null)
            Value = new DictionaryOfGameObjectAndVector3();
    }

}
