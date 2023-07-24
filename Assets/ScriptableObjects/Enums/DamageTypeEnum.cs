using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Enums/DamageType")]
public class DamageTypeEnum : ScriptableObject
{
    [Serialize] public string Description;


}
