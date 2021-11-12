using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    Type abilityType;
    readonly float DurationLimit = 30f;
    float Elapsed = 0;

    SpriteRenderer _sprite;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Elapsed += Time.deltaTime;
        if (Elapsed >= DurationLimit) Destroy(gameObject);
    }

    public void AttachType(Type t)
    {
        abilityType = t;
        switch (abilityType.Name)
        {
            case "Sword":
            case "Clean":
                _sprite.color = Color.yellow;
                break;
            case "Shoot":
            case "Dash":
                _sprite.color = Color.green;
                break;
            case "FBomb":
                _sprite.color = Color.white;
                break;
        }
    }

    public Type AbilityType
    {
        get => abilityType;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        Debug.Log(AbilityType);
    }
}
