using System;
using Unity.VisualScripting;
using UnityEngine;

public class SOLibrary : MonoBehaviour
{
    public static SOLibrary instance { get; private set; }

    [SerializeField]
    ScriptableObjectRuntimeSet configurationInstances;

    private void Awake()
    {
        // Singleton pattern check
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject); // Ensure this object persists between scenes
    }

    public static T Create<T>() where T : ScriptableObject
    {
        return ScriptableObject.CreateInstance<T>();
    }

    public static ScriptableObject Create(Type type)
    {
        if (type != null && type.IsSubclassOf(typeof(ScriptableObject)))
        {
            return ScriptableObject.CreateInstance(type);
        }
        return null;
    }
}
