using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Dash : Ability
{
    public void Awake()
    {
        SetMemory("Dash", 300, Color.green, MemPrio.Dash);
        InputCooldown = 3f;
        CurrentCooldown = 0;
    }

    public override Type GetType() => typeof(Dash);

    public override void Do()
    {
        if (CurrentCooldown == 0)
        {
            RaycastHit2D[] hit = Physics2D.LinecastAll(Parent.transform.position, (Info.mousePos - Parent.transform.position).normalized * Info.dashDistance, Info.hitLayers);
            if (hit.Length > 0)
            {
                Parent.transform.position = hit[0].point;
            } else
            {
                Parent.transform.position += (Info.mousePos - Parent.transform.position).normalized * Info.dashDistance;
            }
            CurrentCooldown = InputCooldown;
        }
    }

    public override void SetParent(GameObject parent)
    {
        Parent = parent;
        Info = Parent.GetComponent<PlayerController>();
    }
}
