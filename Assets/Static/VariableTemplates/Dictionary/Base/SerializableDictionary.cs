using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
        Debug.LogWarning("Before Serialize.. count is now " + keys.Count + ":" + values.Count + "..." + base.Keys.Count + "/" + base.Values.Count);
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
        Debug.LogWarning("After Deserialize.. count is now " + keys.Count + ":" + values.Count + "..." + base.Keys.Count + "/" + base.Values.Count);
    }

    public Dictionary<TKey, TValue> GetBase()
    {
        return this;
    }

    public void AddUpdate(TKey key, TValue value)
    {
        if (this.ContainsKey(key))
        {
            this[key] = value;
            return;
        }
        this.Add(key, value);
        //Debug.LogWarning("AddUpdate.. count is now " + keys.Count + ":" + values.Count + "..." + base.Keys.Count + "/" + base.Values.Count);
    }

    public void MatchDictionary(Dictionary<TKey, TValue> dictionary)
    {
        List<TKey> dummyKeys = Keys.ToList();

        for (int a = dummyKeys.Count - 1; a >= 0; a--)
        {
            TKey key = dummyKeys[a];
            if (!dictionary.ContainsKey(key))
            {
                Remove(key);
            }
        }
        foreach (TKey key in dictionary.Keys)
        {
            if (!dummyKeys.Contains(key))
            {
                Add(key, dictionary[key]);
            }
            else
            {
                this[key] = dictionary[key];
            }
        }
        //Debug.LogWarning("Matching.. count is now " + keys.Count + ":" + values.Count + "..." + base.Keys.Count + "/" + base.Values.Count);
        dummyKeys = null;
    }
}