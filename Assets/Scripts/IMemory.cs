using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public interface IMemory : IEquatable<IMemory>
{
    public string MemName { get; set; }
    public int Cost { get; set; }
    public Color Color {get; set; }
    public int TotalCost { get; set; }
    public int Priority { get; set; }

    public void SetMemory(string name, int cost, Color color, int priority);
    public int GetHashCode();
    public void Unload();
}
