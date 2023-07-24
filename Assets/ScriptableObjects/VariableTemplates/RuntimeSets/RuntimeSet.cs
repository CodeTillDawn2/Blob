using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    [SerializeField]
    private List<T> list = new List<T>();

    public List<T> Items
    {
        get
        { return list; }
        set
        {
            list = value;
            EditorUtility.SetDirty(this);
        }
    }

    public void SetFromArray(T[] values)
    {
        if (values != null)
        {
            list = new List<T>(values);
            EditorUtility.SetDirty(this);
        }
        else
        {
            list = new List<T>();
            EditorUtility.SetDirty(this);
        }
    }

    public void RemoveAll()
    {
        while (list != null && list.Count > 0)
        {
            list.RemoveAt(0);
        }
    }

    public T[] ReturnAsArray()
    {
        return list.ToArray();
    }

    public void Add(T t)
    {
        if (!Items.Contains(t))
        {
            Items.Add(t);
            EditorUtility.SetDirty(this);

        }
    }
    public void Remove(T t)
    {
        if (Items.Contains(t))
        {
            Items.Remove(t);
            EditorUtility.SetDirty(this);
        }
    }
}




