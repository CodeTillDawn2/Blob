using System;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public struct LastSeenData
{
    [Serialize] public Vector3 LastSeen;
    [Serialize] public double WhenSeen;
    [Serialize] public float Distance;


}
