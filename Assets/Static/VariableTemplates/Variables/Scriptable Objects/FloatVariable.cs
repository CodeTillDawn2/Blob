using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "ScriptableObjects/Float")]
public class FloatVariable : ScriptableObject, IConfigDefault
{
    public float Value;
}
