using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTypeEnums : MonoBehaviour
{
    protected DamageTypeEnum acidDamage;
    protected DamageTypeEnum fireDamage;
    protected DamageTypeEnum piercingDamage;

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


