using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModifierClass : MonoBehaviour
{
    [SerializeField] public bool IsStackable;
    protected bool StartScript;


    protected virtual void OnEnable()
    {
        StartScript = false;
        if (!IsStackable) RemoveDuplicateEffect();
        

    }

    public void StartModifier()
    {
        StartScript = true;
    }

    public void RemoveDuplicateEffect()
    {

        ModifierClass[] existing = GetComponents<ModifierClass>();
        if (existing.Length > 1)
        {
            foreach (ModifierClass existingMod in existing)
            {
                if (existingMod != this)
                {
                    existingMod.enabled = false;
                    Destroy(existingMod);
                }
            }
        }
    }


}
