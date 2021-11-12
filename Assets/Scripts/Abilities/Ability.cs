using System;
using UnityEngine;

public abstract class Ability : MonoBehaviour, IMemory
{
    public float InputCooldown { get; set; }
    public float CurrentCooldown { get; set; }
    public string MemName { get; set; }
    public int Cost { get; set; }
    public Color Color { get; set; }
    public int Priority { get; set; }
    public GameObject Parent { get; set; }
    public PlayerController Info { get; set; }
    public int TotalCost { get; set; }

    public new virtual Type GetType() {
        return typeof(Ability);
    }

    public abstract void Do();

    public virtual void Unload() {
        Info.RemoveAbility(GetType());
    }

    public void SetMemory(string name, int cost, Color color, int priority)
    {
        MemName = name;
        Cost = cost;
        Color = color;
        Priority = priority;
    }

    public abstract void SetParent(GameObject parent);

    public void Tick()
    {
        if (CurrentCooldown > 0)
        {
            CurrentCooldown = Mathf.Clamp(CurrentCooldown - Time.deltaTime, 0, InputCooldown);
        }
    }

    public bool Equals(IMemory other)
    {
        return MemName == other.MemName;
    }
}