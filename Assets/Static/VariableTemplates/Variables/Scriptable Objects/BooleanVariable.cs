using UnityEngine;

[CreateAssetMenu(fileName = "BooleanVariable", menuName = "ScriptableObjects/Boolean")]
public class BooleanVariable : ScriptableObject, IConfigDefault
{
    public bool Value;
}
