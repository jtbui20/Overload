using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Sword : Ability
{
    GameObject swordSprite;
    GameObject _s;

    readonly float spawnTime = 0.2f;
    float spawn = 0;
    public void Awake()
    {
        SetMemory("Sword", 300, Color.yellow, MemPrio.Slash);
        InputCooldown = 0.4f;
        CurrentCooldown = 0;
    }

    public void Start()
    {
        swordSprite = Parent.GetComponent<PlayerCharacter>().swordSprite;
    }

    public void Update()
    {
        if (spawn > 0)
        {
            spawn -= Time.deltaTime;
        }

        if (spawn <= 0)
        {
            Destroy(_s);
        }
    }

    public override Type GetType() => typeof(Sword);
    public override void Do()
    {
        if (CurrentCooldown == 0)
        {
            Vector2 direction = (Info.mousePos - Parent.transform.position).normalized;
            Vector3 direction3 = new Vector3(direction.x, direction.y, 0);
            _s = Instantiate(swordSprite, Parent.transform.position + direction3, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction)));
            spawn = spawnTime;

            RaycastHit2D[] hit = Physics2D.LinecastAll(Parent.transform.position, direction * Info.hitDistance, Info.hitLayers);
            foreach (RaycastHit2D entity in hit)
            {
                entity.transform.GetComponent<Character>().DoHit(30);
            }
            CurrentCooldown = InputCooldown;
        }
    }

    public override void Unload()
    {
        if (_s != null)
        {
            Destroy(_s);
        }
        base.Unload();
    }

    public override void SetParent(GameObject parent)
    {
        Parent = parent;
        Info = Parent.GetComponent<PlayerController>();
    }
}
