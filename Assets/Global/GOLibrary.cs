using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GOLibrary : MonoBehaviour
{

    public static GOLibrary instance { get; private set; }

    private Dictionary<string, Type> _derivedTypeCache = null;

    public Dictionary<string, Type> DerivedTypeCache
    {
        get
        {
            return _derivedTypeCache;
        }
    }


    private void InitializeDerivedTypeCache()
    {
        if (_derivedTypeCache == null)
        {
            _derivedTypeCache = new Dictionary<string, Type>();
        }

        Type[] baseTypes = { typeof(Brain), typeof(Body), typeof(Locomotion), typeof(Senses) };

        foreach (Type baseType in baseTypes)
        {
            var derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract);

            foreach (Type derivedType in derivedTypes)
            {
                if (!_derivedTypeCache.ContainsKey(derivedType.FullName))
                {
                    _derivedTypeCache.Add(derivedType.FullName, derivedType);
                }
            }
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        InitializeDerivedTypeCache();
    }

    public Component AddComponentByTypeName(GameObject target, string typeName)
    {
        if (_derivedTypeCache.TryGetValue(typeName, out Type componentType))
        {
            return target.AddComponent(componentType);
        }
        return null;
    }

}
