using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializeableList<T> : List<T>, ISerializationCallbackReceiver
{
    [SerializeField] private List<T> values = new List<T>();


    public void OnBeforeSerialize()
    {
        values.Clear();
        foreach (var item in this)
        {
            values.Add(item);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        foreach(var item in values)
        {
            this.Add(item);
        }
    }
}

