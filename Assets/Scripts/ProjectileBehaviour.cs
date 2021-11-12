using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour, IMemory
{
    public string MemName { get; set; }
    public int Cost { get; set; }
    public Color Color { get; set; }
    public int Priority { get; set; }
    public int TotalCost { get; set; }

    private void Start()
    {
        SetMemory("Projectile", 20, Color.white, MemPrio.Projectiles);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>()
           .MemBar.AddMemory(this);
    }

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

    public void Unload() {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Virus"))
        {
            collision.gameObject.GetComponent<Character>().DoHit(30);
        }
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>()
            .MemBar.DeleteMemory(this);
    }
}
