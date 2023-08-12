using System;
using UnityEngine;

public abstract class GameObjectInListModifierCondition<T> : ModifierClass<T> where T : GameObjectInListModifierCondition<T>
{
    public GameObject Comparer1 { get; set; }
    public GameObjectRuntimeSet Comparer2 { get; set; }

    public override bool OneTimeEffect => false;

    public override Func<bool> EvalConditions
    {
        get
        {
            if (Comparer1 == null || Comparer2 == null) return () => ReturnFalse();
            return () => Comparer2.Items.Contains(Comparer1);
        }
    }

}
