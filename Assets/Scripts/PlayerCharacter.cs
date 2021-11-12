using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerCharacter : Character, IMemory
{
    PlayerController _controller;
    GameController _gm;
    public List<Sprite> sprites;
    public float dead;
    public GameObject swordSprite;

    public float HitInvincibilityTime;
    float HitInvincibility = 0;
    Rigidbody2D _rb;
    SpriteRenderer _spr;

    void Awake()
    {
        _gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _controller = gameObject.GetComponent<PlayerController>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _spr = gameObject.GetComponent<SpriteRenderer>();
        SetMemory("Pica", 400, Color.grey, MemPrio.Player);
    }

    void Start()
    {
        _gm.MemBar.AddMemory(this);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AbilityPickup"))
        {
            Type a = collision.gameObject.GetComponent<AbilityPickup>().AbilityType;
            switch(a.Name)
            {
                case "Sword":
                    if (_controller.PrimaryAction != null)
                    {
                        _gm.MemBar.DeleteMemory(_controller.PrimaryAction);
                    }
                    _controller.PrimaryAction = gameObject.AddComponent<Sword>();
                    _controller.PrimaryAction.SetParent(gameObject);
                    _gm.MemBar.AddMemory(_controller.PrimaryAction);
                    break;
                case "Shoot":
                    if (_controller.SecondaryAction != null)
                    {
                        _gm.MemBar.DeleteMemory(_controller.SecondaryAction);
                    }
                    _controller.SecondaryAction = gameObject.AddComponent<Shoot>();
                    _controller.SecondaryAction.SetParent(gameObject);
                    _gm.MemBar.AddMemory(_controller.SecondaryAction);
                    break;
                case "Clean":
                    if (_controller.PrimaryAction != null)
                    {
                        _gm.MemBar.DeleteMemory(_controller.PrimaryAction);
                    }
                    _controller.PrimaryAction = gameObject.AddComponent<Clean>();
                    _controller.PrimaryAction.SetParent(gameObject);
                    _gm.MemBar.AddMemory(_controller.PrimaryAction);
                    break;
                case "Dash":
                    if (_controller.SecondaryAction != null)
                    {
                        _gm.MemBar.DeleteMemory(_controller.SecondaryAction);
                    }
                    _controller.SecondaryAction = gameObject.AddComponent<Dash>();
                    _controller.SecondaryAction.SetParent(gameObject);
                    _gm.MemBar.AddMemory(_controller.SecondaryAction);
                    break;
                case "FBomb":
                    if (_controller.TryGetComponent<FBomb>(out FBomb b))
                    {
                        b.isPrimed = false;
                        Debug.Log("tryDeleteComponent");
                        _gm.MemBar.DeleteMemory(b);
                        b = null;
                    }
                    FBomb c = gameObject.AddComponent<FBomb>();
                    c.SetParent(gameObject);
                    c.isPrimed = true;
                    _gm.MemBar.AddMemory(c);
                    break;
                default:
                    break;
            }
        } else if (collision.gameObject.CompareTag("Warp"))
        {
            collision.GetComponent<WarpZones>().DoWarp();
        }
    }

    public void Update()
    {
        HitInvincibility -= Time.deltaTime;
        DoSprite();
    }

    void DoSprite()
    {
        if ((-dead <= _controller.HorizontalVelocity) && (_controller.HorizontalVelocity <= dead) && (-dead <= _rb.velocity.y) && (_rb.velocity.y <= dead)) {
            _spr.sprite = sprites[0];
        } else if (dead < _rb.velocity.y)
        {
            _spr.sprite = sprites[1];
        } else if (_rb.velocity.y < -dead)
        {
            _spr.sprite = sprites[2];
        } else if (dead < _controller.HorizontalVelocity)
        {
            _spr.sprite = sprites[4];
        } else if (_controller.HorizontalVelocity < -dead) {
            _spr.sprite = sprites[3];
        }
    }

    public void GetHit(int bloat)
    {
        if (HitInvincibility <= 0)
        {
            _gm.MemBar.AddMemory(new Bloat(bloat));
            HitInvincibility = HitInvincibilityTime;
        }
    }

    public override void Unload()
    {
        Destroy(gameObject);
        _gm.Lose();
    }
}
