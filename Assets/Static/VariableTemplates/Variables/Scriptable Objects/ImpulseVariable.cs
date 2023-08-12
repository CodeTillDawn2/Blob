using UnityEngine;

[CreateAssetMenu(fileName = "ImpulseVariable", menuName = "ScriptableObjects/Impulse")]
public class ImpulseVariable : ScriptableObject, IConfigDefault
{
    public Impulse Value;
}
