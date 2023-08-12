using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    [SerializeField]
    private List<T> items = new List<T>();

    public ReadOnlyCollection<T> Items => items.AsReadOnly();

    public void Add(T t)
    {
        if (!items.Contains(t))
            items.Add(t);
    }

    public void Remove(T t)
    {
        items.Remove(t);
    }

    public bool Contains(T t)
    {
        return items.Contains(t);
    }

    public void Clear()
    {
        items.Clear();
    }

    public T[] ReturnAsArray()
    {
        return items.ToArray();
    }

    public void AddUpdate(T t)
    {
        if (!items.Contains(t))
        {
            items.Add(t);
        }
        else
        {
            int tIndex = items.IndexOf(t);
            items.RemoveAt(tIndex);
            items.Insert(tIndex, t);
        }
    }

    public void MatchList(List<T> matchList)
    {
        List<T> itemsToRemove = new List<T>();
        foreach (T obj in items)
        {
            if (!matchList.Contains(obj))
                itemsToRemove.Add(obj);
        }

        foreach (T obj in itemsToRemove)
        {
            items.Remove(obj);
        }

        List<T> itemsToAdd = new List<T>();
        foreach (T obj in matchList)
        {
            if (!items.Contains(obj))
                itemsToAdd.Add(obj);
        }

        items.AddRange(itemsToAdd);
    }
}