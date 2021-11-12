using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject[] rooms;
    int TargetIndex;
    public int CurrentIndex;
    PlayerController _pc;
    Rigidbody2D _pcrb;

    // Start is called before the first frame update
    void Start()
    {
        CurrentIndex = 0;
    }

    public void MoveRoom(int roomIndex, bool instant = false)
    {
        TargetIndex = roomIndex;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        _pc = p.GetComponent<PlayerController>();
        _pcrb = p.GetComponent<Rigidbody2D>();

        StartCoroutine(nameof(TranslateToRoom));
    }

    public void HardMoveRoom(int roomIndex)
    {
        TargetIndex = roomIndex;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        _pc = p.GetComponent<PlayerController>();
        _pcrb = p.GetComponent<Rigidbody2D>();

        Transform target = rooms[TargetIndex].GetComponent<Transform>();
        transform.position = target.position;
        CurrentIndex = TargetIndex;
    }

    IEnumerator TranslateToRoom()
    {
        float now = 0;
        float duration = 1;
        Transform target = rooms[TargetIndex].GetComponent<Transform>();
        Transform current = rooms[CurrentIndex].GetComponent<Transform>();
        _pc.ToggleInputLock();

        while (transform.position != target.position)
        {
            now += Time.deltaTime;
            Vector3 slerp = Vector3.Slerp(current.position, target.position, now / duration);
            Vector3 movement = (transform.position - current.position);
            _pcrb.position += 0.5f * movement.magnitude * Time.deltaTime * new Vector2(movement.normalized.x, movement.normalized.y);
            transform.position = slerp;
            yield return null;
        }

        _pc.ToggleInputLock();
        CurrentIndex = TargetIndex;
    }
}
