using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Clean : Ability
{
    float DurationUnit;
    string InputVal = "Fire1";
    GameController _gm;
    bool doingClean = false;

    public void Awake()
    {
        SetMemory("Clean", 400, Color.yellow, MemPrio.Clean);
        _gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        InputCooldown = 45f;
        CurrentCooldown = 0;
        DurationUnit = 5f;
    }

    public void AssignKey(string axis)
    {
        InputVal = axis;
    }

    public override Type GetType() => typeof(Clean);

    public override void Do()
    {
        if ((CurrentCooldown == 0) && !doingClean)
        {
            StartCoroutine(nameof(DoCast));
            doingClean = true;
            Info.ToggleInputLock();
        }
    }

    void DoClean()
    {
        _gm.MemBar.DeleteAllMemoryByName("Bloat");
        CurrentCooldown = InputCooldown;
    }

    public override void SetParent(GameObject parent)
    {
        Parent = parent;
        Info = Parent.GetComponent<PlayerController>();
    }

    IEnumerator DoCast()
    {
        float i = 0;
        Debug.Log("Doing Clean");
        while (i < DurationUnit)
        {
            if (!Input.GetButton(InputVal))
            {
                Debug.Log("Break Clean!");
                break;
            }
            i += Time.deltaTime;
            yield return null;
        }
        if (i >= DurationUnit)
        {
            Debug.Log("Clean Complete");
            DoClean();
        }
        Info.ToggleInputLock();
        doingClean = false;
    }
}
