using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IAbility
{
    public GameObject Parent { get; set; }
    public PlayerController Info { get; set; }
    public Type GetType();
    
    public float InputCooldown { get; set; }
    public float CurrentCooldown { get; set; }

    public void Do();
    public void SetParent(GameObject parent);

    public void Tick();
}
