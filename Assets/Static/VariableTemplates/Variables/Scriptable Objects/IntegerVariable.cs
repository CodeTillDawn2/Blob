using UnityEngine;

[CreateAssetMenu(fileName = "IntegerVariable", menuName = "ScriptableObjects/Integer")]
public class IntegerVariable : ScriptableObject, IConfigDefault
{
    public int Value;
}
