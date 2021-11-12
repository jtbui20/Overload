using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FBomb : Ability
{
    public bool isPrimed = false;
    public void Awake()
    {
        SetMemory("FBomb", 500, Color.cyan, MemPrio.FBomb);
    }

    public override Type GetType() => typeof(FBomb);

    public override void Do()
    {
        Info.FBomb();
    }

    public override void SetParent(GameObject parent)
    {
        Parent = parent;
        Info = Parent.GetComponent<PlayerController>();
    }

    public override void Unload()
    {
        if (isPrimed) Do();
        Debug.Log(GetType());
        Info.RemoveAbility(GetType());
    }
}
