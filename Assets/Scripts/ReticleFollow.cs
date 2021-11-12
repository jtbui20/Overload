using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleFollow : MonoBehaviour
{
    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _camera.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,20);
    }
}
