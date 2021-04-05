using System;
using System.Collections.Generic;

public class ObjectsPool
{
    public void Add(Type type, object obj)
    {
        if (pools.ContainsKey(type) == false)
            pools.Add(type, new List<object>());

        pools[type].Add(obj);
    }

    public bool Contains(Type type)
    {
        return pools.ContainsKey(type) && pools[type].Count > 0;
    }

    public object GetObj(Type type)
    {
        if (Contains(type))
        {
            int count = pools[type].Count;
            object last = pools[type][count - 1];
            pools[type].RemoveAt(count - 1);
            return last;
        }

        return null;
    }

    private Dictionary<Type, List<object>> pools = new Dictionary<Type, List<object>>();
}
