using System;
using System.Collections;
using UnityEngine;

public abstract class ModifierClass<T> : MonoBehaviour where T : ModifierClass<T>
{
    public abstract bool IsStackable { get; }
    public abstract bool OneTimeEffect { get; }
    public abstract bool Inverse { get; set; }
    public abstract Func<bool> EvalConditions { get; }
    private bool isRedundant = false;
    public Action AfterAction { get; set; }

    protected abstract void DebugEffect();

    public IEnumerator Evaluate()
    {
        DebugEffect();

        if (gameObject)
        {
            if (!isRedundant)
            {
                bool ranBeforeEffect = false;
                bool condition = EvalConditions.Invoke();
                if (condition != Inverse)
                {
                    BeforeEffect();
                    ranBeforeEffect = true;
                }

                while (condition != Inverse)
                {
                    ExecuteEffect();
                    condition = EvalConditions.Invoke();
                    if (OneTimeEffect) break;
                    yield return new WaitForFixedUpdate();
                }

                if (ranBeforeEffect)
                {
                    AfterEffect();
                    AfterAction?.Invoke();
                }
            }
        }

        Destroy(this);
    }

    public bool ReturnFalse()
    {
        return false;
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
        isRedundant = true;
    }
}
