using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SwNavComp
{

    public abstract class RuntimeSet<T> : ScriptableObject
    {
        public List<T> items = new List<T>();

        public void Clear()
        {
            items.Clear();
        }

        public T Get(int index)
        {
            return items[index];
        }

        public void Add(T thingToAdd)
        {
            if (!items.Contains(thingToAdd)) items.Add(thingToAdd);
        }

        public void Remove(T thingToRemove)
        {
            if (items.Contains(thingToRemove)) items.Remove(thingToRemove);
        }

        public int Count()
        {
            return items.Count;
        }
    }
}