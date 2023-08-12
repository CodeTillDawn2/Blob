using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Variable", menuName = "ScriptableObjects/Vector3")]
public class Vector3Variable : ScriptableObject, IConfigDefault
{
    public Vector3 Value;
}
