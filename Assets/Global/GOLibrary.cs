using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GOLibrary : MonoBehaviour
{

    public static GOLibrary instance { get; private set; }

    Dictionary<string, Type> TypesDecoded = new Dictionary<string, Type>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public Component AddComponentByTypeName(GameObject target, string typeName)
    {
        if (TypesDecoded.TryGetValue(typeName, out Type foundtype))
        {
            return target.AddComponent(foundtype);
        }
        else
        {

            Type type = Type.GetType(typeName);
            TypesDecoded.Add(typeName, type);
            return target.AddComponent(type);
        }

        

    }

}
