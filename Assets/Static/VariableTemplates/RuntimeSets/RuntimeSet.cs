using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    private List<T> list = new List<T>();

    public List<T> Items
    {
        get { return list; }
        set { list = value; }
    }

    public void SetFromArray(T[] values)
    {
        if (values != null)
            list = new List<T>(values);
        else
            list = new List<T>();
    }

    public void RemoveAll()
    {
        if (list == null)
        {
            Debug.LogWarning("List is null while attempting to remove all");
        }
        else
        {
            list.Clear();
        }


    }

    public T[] ReturnAsArray()
    {
        return list.ToArray();
    }

    public void Add(T t)
    {
        if (!Items.Contains(t))
            Items.Add(t);
    }

    public void AddUpdate(T t)
    {
        if (!Items.Contains(t))
        {
            Items.Add(t);
        }
        else
        {
            int tIndex = Items.IndexOf(t);
            Items.Remove(t);
            Items.Insert(tIndex, t);
        }

    }

    public void Remove(T t)
    {
        if (Items.Contains(t))
            Items.Remove(t);
    }

    public void MatchList(List<T> matchList)
    {
        List<T> itemsToRemove = new List<T>();
        foreach (T obj in list)
        {
            if (!matchList.Contains(obj))
                itemsToRemove.Add(obj);
        }

        foreach (T obj in itemsToRemove)
        {
            list.Remove(obj);
        }

        List<T> itemsToAdd = new List<T>();
        foreach (T obj in matchList)
        {
            if (!list.Contains(obj))
                itemsToAdd.Add(obj);
        }

        list.AddRange(itemsToAdd);
    }
}
