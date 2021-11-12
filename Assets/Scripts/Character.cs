using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Character : MonoBehaviour, IMemory
{
    public int Memory {
        get => _memory;
        set {
            _memory = Mathf.Clamp(value, 0, MemoryMax);
            if (_memory == MemoryMax)
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().MemBar.DeleteMemory(this);
        }
    }

    public virtual string MemName { get; set; }
    public int Cost {get; set; }
    public Color Color { get; set; }
    public int Priority { get; set; }
    public int TotalCost { get; set; }

    public int MemoryMax;
    
    private int _memory;

    void Start()
    {
        Memory = 0;
        SetMemory("Virus", 100, Color.red, MemPrio.Enemy);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().MemBar.AddMemory(this);
    }

    public void DoHit(int val) {
        Memory += val;
        Debug.Log(val);
    }

    public void DoKill()
    {
        Destroy(gameObject);
    }

    public void SetMemory(string name, int cost, Color color, int priority)
    {
        MemName = name;
        Cost = cost;
        Color = color;
        Priority = priority;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerCharacter>().GetHit(40);
        }
    }

    public virtual void Unload()
    {
        DoKill();
    }

    public bool Equals(IMemory other)
    {
        return MemName == other.MemName;
    }
}
