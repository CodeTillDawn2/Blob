using System;
using UnityEngine;

public abstract class GameObjectIsGameObjectCondition<T> : ModifierClass<T> where T : GameObjectIsGameObjectCondition<T>
{
    public GameObjectVariable Comparer1 { get; set; }
    public GameObject Comparer2 { get; set; }

    public override bool OneTimeEffect => false;

    public override Func<bool> EvalConditions
    {
        get
        {
            if (Comparer1 == null || Comparer2 == null) return () => ReturnFalse();
            return () => Comparer1.Value == Comparer2;
        }
    }


}
