using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RenderGame : MonoBehaviour
{
    public GameController GM;
    public Canvas target;

    [Header("Assets")]
    public GameObject Text;

    GameObject _status;
    RectTransform _statusTrans;
    Text _statusText;

    bool ClearOverride = false;
    bool warningState = false;
    float statusDuration = 5f;
    float statusTime = 0;

    void Start()
    {
        if (GM == null)
        {
            GM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        GM.MemBar.OverloadClear += OnOverloadClear;
        GM.MemBar.OverloadWarning += OnOverloadWarning;
        GM.MemBar.OverloadTrigger += OnOverloadTrigger;

        _status = Instantiate(Text, target.transform);

        _statusTrans = _status.transform as RectTransform;
        _statusTrans.Rotate(0, 0, 90);
        _statusTrans.anchorMin = new Vector2(0, 0.5f);
        _statusTrans.anchorMax = new Vector2(0, 0.5f);
        _statusTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 220);
        _statusTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 48);
        _statusTrans.anchoredPosition = new Vector2(140, 0);

        _statusText = _status.GetComponent<Text>();
        _statusText.text = "CLEAR!";
        _statusText.color = Color.white;
    }

    private void Update()
    {
        if (statusTime > 0)
        {
            statusTime -= Time.deltaTime;
        } else
        {
            ClearOverride = false;
            if (!warningState) OnOverloadClear();
        }
    }

    void OnOverloadClear()
    {
        if (!ClearOverride)
        {
            _statusText.text = "CLEAR!";
            warningState = false;
        }
    }

    void OnOverloadWarning()
    {
        warningState = true;
        _statusText.text = "CAUTION! CAUTION! CAUTION!";
    }

    void OnOverloadTrigger()
    {
        _statusText.text = "OVERLOADED!";
        ClearOverride = true;
        warningState = false;
        statusTime = statusDuration;
    }
}
