using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStatsBase : ScriptableObject
{
    [Serialize] public float RotateSpeed;
    [Serialize] public float MoveSpeed;
    [Serialize] public float Mass;
    [Serialize] public float HitPoints;
    [Serialize] public float Nutrition;
}
