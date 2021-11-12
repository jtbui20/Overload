using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject AbilityPickups;
    public GameObject Player;
    public GameObject Enemy;
    GameObject _p;
    
    public SceneControl _sc;
    public List<GameObject> AbilitySpawnPointers;
    public List<GameObject> EnemySpawnPointers;

    [HideInInspector]
    public MemoryBar MemBar;

    readonly Type[] abilities = new Type[] { typeof(Sword), typeof(Shoot), typeof(Dash), typeof(Clean), typeof(FBomb) };
    RenderMemory _UIRender;

    System.Random rng;
    readonly float SpawnCooldown = 15f;
    float SpawnNow = 0;

    float EnemySpawnChance = 0.05f;
    float AbilitySpawnChance = 0.3f;

    private void Awake()
    {
        _UIRender = GetComponent<RenderMemory>();
        MemBar = GetComponent<MemoryBar>();
        rng = new System.Random();
    }
    void Start()
    {
        MemBar = GetComponent<MemoryBar>();
        _p = Instantiate(Player, new Vector3(-5, -3), Quaternion.identity);

        _UIRender.ManStart();
        MemBar.ManStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnNow <= 0)
        {
            SpawnAbilities();
            SpawnEnemies();
            SpawnNow = SpawnCooldown;
        }
        else
        {
            SpawnNow -= Time.deltaTime;
        }
        
    }

    void SpawnEnemies()
    {
        if (rng.NextDouble() > EnemySpawnChance)
        {
            int i = rng.Next(0, EnemySpawnPointers.Count);
            Instantiate(Enemy, EnemySpawnPointers[i].transform.position, Quaternion.identity);
            EnemySpawnChance += 0.003f;
        }

    }

    void SpawnAbilities()
    {
        foreach (GameObject asp in AbilitySpawnPointers)
        {
            if (rng.NextDouble() > AbilitySpawnChance)
            {
                int i = rng.Next(0, abilities.Length);
                Instantiate(AbilityPickups, asp.transform.position, Quaternion.identity)
                    .GetComponent<AbilityPickup>().AttachType(abilities[i]);
                AbilitySpawnChance += 0.01f;
            }
        }
    }

    public void Lose()
    {
        _sc.OnDeath();
    }

    public void Reset()
    {
        _p.transform.position = new Vector3(-5, -3);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviour>().HardMoveRoom(0);
    }
}
