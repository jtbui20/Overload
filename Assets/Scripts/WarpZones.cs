using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZones : MonoBehaviour
{
    public CameraBehaviour _cam;
    public int[] roomPair;
    public float grace = 1.5f;
    float now = 0;

    private void Start()
    {
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehaviour>();
    }

    public void Update()
    {
        now -= Time.deltaTime;
    }

    public void DoWarp()
    {
        if (now <= 0)
        {
            int index = roomPair[(_cam.CurrentIndex == roomPair[0] ? 1 : 0)];
            _cam.MoveRoom(index);
            now = grace;
        }
    }
}
