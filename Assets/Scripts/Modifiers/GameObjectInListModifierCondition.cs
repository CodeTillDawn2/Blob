using System;
using Unity.VisualScripting;
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
            return () => Comparer2.Items.Contains(Comparer1);
        }
    }


}
