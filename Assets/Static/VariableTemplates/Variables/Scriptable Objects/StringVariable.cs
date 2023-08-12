using UnityEngine;

[CreateAssetMenu(fileName = "StringVariable", menuName = "ScriptableObjects/String")]
public class StringVariable : ScriptableObject, IConfigDefault
{
    public string Value;
}
