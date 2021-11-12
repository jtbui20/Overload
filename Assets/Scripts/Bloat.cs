using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloat : IMemory
{
    public Bloat(int value)
    {
        SetMemory("Bloat", value, Color.magenta, MemPrio.Bloat);
    }

    public string MemName { get; set; }
    public int Cost { get; set; }
    public Color Color { get; set; }
    public int Priority { get; set; }
    public int TotalCost { get; set; }

    public bool Equals(IMemory other)
    {
        return MemName == other.MemName;
    }

    public void SetMemory(string name, int cost, Color color, int priority)
    {
        MemName = name;
        Cost = cost;
        Color = color;
        Priority = priority;
    }

    public void Unload()
    {
        
    }
}
