using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBar : MonoBehaviour
{
    public SortedList<int, IMemory> memorySorted;
    public List<IMemory> allMemory;
    PlayerCharacter _character;
    public bool ObserverUpdate = false;
    
    public delegate void MemoryUpdateEvent();
    public event MemoryUpdateEvent MemoryUpdate;
    public event MemoryUpdateEvent OverloadClear;
    public event MemoryUpdateEvent OverloadWarning;
    public event MemoryUpdateEvent OverloadTrigger;

    public bool isOverload = false;

    private void Awake()
    {
        memorySorted = new SortedList<int, IMemory>();
        allMemory = new List<IMemory>();
    }

    public void ManStart()
    {
        _character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
    }
    int MemorySum()
    {
        int output = 0;
        foreach (KeyValuePair<int, IMemory> m in memorySorted)
            output += m.Value.TotalCost;
        return output;
    }

    public bool UpdateMemory()
    {
        MemoryUpdate();
        return true;
    }

    public void Overload()
    {
        if (isOverload)
        {
            OverloadTrigger();
            DeleteTopMemory();
        } else
        {
            isOverload = true;
            OverloadWarning();
        }
        UpdateMemory();
    }

    public bool AddMemory(IMemory mem)
    {
        if (memorySorted.ContainsValue(mem))
        {
            if (mem.Cost + MemorySum() > _character.MemoryMax)
            {
                Overload();
                mem.Unload();
                return false;
            }
            memorySorted[mem.Priority].TotalCost += mem.Cost;
        }
        else
        {
            if (mem.Cost + MemorySum() > _character.MemoryMax)
            {
                Overload();
                mem.Unload();
                return false;
            }
            memorySorted.Add(mem.Priority, mem);
            memorySorted[mem.Priority].TotalCost += mem.Cost;
        }
        allMemory.Add(mem);
        UpdateMemory();
        return true;
    }

    public bool ReplaceMemory(IMemory mem)
    {
        DeleteMemory(mem);
        AddMemory(mem);
        return true;
    }

    public bool DeleteMemory(IMemory mem)
    {
        // Subtract it's cost from the running total.
        memorySorted[mem.Priority].TotalCost -= mem.Cost;
        // Remove itself from the list of all memory
        allMemory.Remove(mem);
        Debug.Log("Solo Delete");
        mem.Unload();

        if (memorySorted[mem.Priority].TotalCost <= 0)
        {
            int i = 0;
            foreach (IMemory m in allMemory.FindAll(x => x.MemName == mem.MemName))
            {
                Debug.Log("Group Delete");
                Debug.Log(allMemory.FindAll(x => x.MemName == mem.MemName)[0]);
                m.Unload();
                allMemory.Remove(m);
                UpdateMemory();
                i += 1;
            }
            Debug.Log("Killed: " + i);
            if (isOverload)
            {
                OverloadClear();
                isOverload = false;
            }
            i = memorySorted.IndexOfValue(mem);
            memorySorted.RemoveAt(i);
        }

        UpdateMemory();
        return true;
    }

    public bool DeleteTopMemory()
    {
        IList<int> keys = memorySorted.Keys;
        if (keys.Count == 0)
            return false;
        else if (keys.Count == 1)
            DeleteMemory(memorySorted[keys[0]]);
        else
            DeleteAllMemory(memorySorted[keys[keys.Count - 1]]);
        return true;
    }

    public bool DeleteAllMemory(IMemory mem)
    {
        memorySorted[mem.Priority].TotalCost -= mem.TotalCost;
        int i = 0;
        foreach (IMemory m in allMemory.FindAll(x => x.MemName == mem.MemName))
        {
            Debug.Log("Group Delete");
            Debug.Log(allMemory.FindAll(x => x.MemName == mem.MemName)[0]);
            m.Unload();
            allMemory.Remove(m);
            UpdateMemory();
            i += 1;
        }
        Debug.Log("Killed: " + i);
        if (isOverload)
        {
            OverloadClear();
            isOverload = false;
        }
        i = memorySorted.IndexOfValue(mem);
        memorySorted.RemoveAt(i);

        UpdateMemory();
        return true;
    }

    public bool DeleteAllMemoryByName(string name)
    {
        foreach(IMemory mem in memorySorted.Values)
        {
            if (mem.MemName == name)
            {
                return DeleteAllMemory(mem);
            }
        }
        return false;
    }

    public int GetMaxMemory() => _character.MemoryMax;

    public IMemory GetMemory(string MemName)
    {
        foreach (IMemory mem in memorySorted.Values)
        {
            if (mem.MemName == MemName)
            {
                return mem;
            }
        }
        return null;
    }
}
