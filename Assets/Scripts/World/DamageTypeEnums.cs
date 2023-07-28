using Unity.VisualScripting;
using UnityEngine;

public class DamageTypeEnums : MonoBehaviour
{
    [Serialize] public DamageTypeEnum acidDamage;
    [Serialize] public DamageTypeEnum fireDamage;
    [Serialize] public DamageTypeEnum piercingDamage;

    public static DamageTypeEnums instance;

    private void Start()
    {
        instance = this;
    }

    public static DamageTypeEnum AcidDamage
    {
        get { return instance.acidDamage; }
    }
    public static DamageTypeEnum FireDamage
    {
        get { return instance.fireDamage; }
    }
    public static DamageTypeEnum PiercingDamage
    {
        get { return instance.piercingDamage; }
    }
}


