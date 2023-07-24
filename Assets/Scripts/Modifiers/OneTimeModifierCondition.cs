using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class OneTimeModifierCondition<T> : ModifierClass<T> where T : OneTimeModifierCondition<T>
{

    public override bool OneTimeEffect => true;
    public override bool Inverse { get; set; }
    public override bool IsStackable => true;

    public override Func<bool> EvalConditions
    {
        get
        {
            Inverse = false;
            return () => true;
        }
    }


}
