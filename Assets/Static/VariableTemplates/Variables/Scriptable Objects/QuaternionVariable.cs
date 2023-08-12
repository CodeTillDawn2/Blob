using UnityEngine;

[CreateAssetMenu(fileName = "QuaternionVariable", menuName = "ScriptableObjects/Quaternion")]
public class QuaternionVariable : ScriptableObject, IConfigDefault
{
    public Quaternion Value;
}
