using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ModifierClass<T> : MonoBehaviour where T : ModifierClass<T>
{
    [SerializeField] public abstract bool IsStackable { get; }
    [SerializeField] public abstract bool OneTimeEffect { get; }
    [SerializeField] public abstract bool Inverse { get; set; }
    [SerializeField] public abstract Func<bool> EvalConditions { get; }

    private bool Redundant = false;

    public IEnumerator Evaluate()
    {
        if (!Redundant)
        {
            BeforeEffect();

            while (EvalConditions.Invoke() != Inverse)
            {
                ExecuteEffect();
                if (OneTimeEffect) break;
                yield return null;
            }

            AfterEffect();
        }
        
    }

    public abstract void BeforeEffect();
    public abstract void ExecuteEffect();
    public abstract void AfterEffect();

    protected virtual void OnEnable()
    {
        if (!IsStackable)
        {
            T[] componentsFound = gameObject.GetComponents<T>();
            foreach (T component in componentsFound)
            {
                if (component != this)
                {
                    RefreshEffect();
                    break;
                }
            }
        }

        

    }

    public virtual void RefreshEffect()
    {
        Redundant = true;
    }


}
