using System;
using System.Collections;
using UnityEngine;

public abstract class ModifierClass<T> : MonoBehaviour where T : ModifierClass<T>
{
    [SerializeField] public abstract bool IsStackable { get; }
    [SerializeField] public abstract bool OneTimeEffect { get; }
    [SerializeField] public abstract bool Inverse { get; set; }
    [SerializeField] public abstract Func<bool> EvalConditions { get; }

    private bool Redundant = false;
    /// <summary>
    /// Optional action that occurs on the sender object after the effect is done
    /// </summary>
    public Action AfterAction { get; set; }
    protected abstract void DebugEffect();

    public IEnumerator Evaluate()
    {
        DebugEffect();

        if (gameObject != null)
        {
            if (!Redundant)
            {
                bool RanBeforeEffect = false;
                if (EvalConditions.Invoke() != Inverse)
                {
                    BeforeEffect();
                    RanBeforeEffect = true;
                }
                while (EvalConditions.Invoke() != Inverse)
                {

                    ExecuteEffect();
                    if (OneTimeEffect) break;
                    yield return new WaitForFixedUpdate();
                }
                if (RanBeforeEffect)
                {
                    AfterEffect();
                    if (AfterAction != null) AfterAction();
                }


                Destroy(this);
            }
        }
        else
        {
            Destroy(this);
        }

    }
    /// <summary>
    /// Occurs before the effect, if conditions evaluate true
    /// </summary>
    public abstract void BeforeEffect();
    /// <summary>
    /// The effect, which will repeat until conditions evaluate false
    /// </summary>
    public abstract void ExecuteEffect();
    /// <summary>
    /// Occurs after the effect, if the before effect happened
    /// </summary>
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
