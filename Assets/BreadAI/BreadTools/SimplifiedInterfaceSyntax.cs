using System;
using UnityEngine;

public static class Class<T> where T : CharacterSystem
{
    public static U LookupCastTo<U>(T instance) where U : class
    {
        if (instance == null)
            return null;

        Type instanceType = typeof(T);

        // If the dictionary contains the type of the instance and that type is known to implement the desired interface
        if (AIBakerData.instance.BreadSystemInterfaces.ContainsKey(instanceType) && AIBakerData.instance.BreadSystemInterfaces[instanceType].Contains(typeof(U)))
        {
            return instance as U;
        }
        else
        {
            // Log or handle the scenario where the instance cannot be safely cast to the desired interface
            Debug.LogError($"Attempt to cast {instanceType.Name} to {typeof(U).Name} failed.");
            return null;
        }
    }

}
