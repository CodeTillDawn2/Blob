using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Enums/DamageType")]
public class DamageTypeEnum : ScriptableObject
{
    [SerializeField]
    protected string description;
    public string Description { get { return description; } set { description = value; } }


}
