using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Shoot : Ability
{
    private GameObject _projectile;

    public void Awake()
    {
        SetMemory("Shoot", 300, Color.green, MemPrio.Shoot);
        InputCooldown = 0.4f;
        CurrentCooldown = 0;
    }

    public override Type GetType() => typeof(Shoot);

    public override void Do()
    {
        if (CurrentCooldown == 0)
        {
            Vector3 direction = (Info.mousePos - Parent.transform.position).normalized;
            GameObject p = Instantiate(_projectile, Parent.transform.position + direction * Info.shootOffset, Quaternion.identity);
            p.GetComponent<Rigidbody2D>().AddForce(direction * Info.shootStrength, ForceMode2D.Impulse);

            CurrentCooldown = InputCooldown;
        }
    }

    public override void SetParent(GameObject parent)
    {
        Parent = parent;
        Info = Parent.GetComponent<PlayerController>();
        _projectile = Info.projectile;
    }
}
