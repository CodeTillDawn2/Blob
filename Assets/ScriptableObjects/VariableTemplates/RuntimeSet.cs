using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    [SerializeField]
    private List<T> list = new List<T>();
    public List<T> Items
    {
        get
        { return list; }
        set { list = value; }
    }

    public void SetFromArray(T[] values)
    {
        list = new List<T>(values);
    }

    public T[] ReturnAsArray()
    {
        return list.ToArray();
    }

    public void Add(T t)
    {
        if (!Items.Contains(t)) Items.Add(t);

    }

    public void Remove(T t)
    {
        if (Items.Contains(t)) Items.Remove(t);
    }
}


[CreateAssetMenu(fileName = "GameObjectRuntimeSet", menuName = "RuntimeSets/GameObjectRuntimeSet")]
public class GameObjectRuntimeSet : RuntimeSet<GameObject> { }

[CreateAssetMenu(fileName = "RaycastRuntimeSet", menuName = "RuntimeSets/RaycastRuntimeSet")]
public class RaycastRuntimeSet : RuntimeSet<RaycastHit>
{

}