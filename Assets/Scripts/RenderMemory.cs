using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderMemory : MonoBehaviour
{
    public GameObject drawObject;
    public GameObject drawParent;
    RectTransform parentTrans;
    public MemoryBar target;

    List<GameObject> renderList;

    public void ManStart()
    {
        target.MemoryUpdate += OnBarUpdate;
        renderList = new List<GameObject>();
        parentTrans = drawParent.GetComponent<RectTransform>();
    }

    void OnBarUpdate()
    {
        DestroyBar();
        DrawBars();
    }

    private void DrawBars()
    {
        float base_height = 0f;
        foreach (IMemory mem in target.memorySorted.Values)
        {
            GameObject bar = Instantiate(drawObject, transform);
            bar.transform.SetParent(drawParent.transform);

            RectTransform bartrans = bar.transform as RectTransform;
            bartrans.anchorMin = new Vector2(0.5f, 0);
            bartrans.anchorMax = new Vector2(0.5f, 0);
            bartrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentTrans.rect.width);
            float height = parentTrans.rect.height * ((float)mem.TotalCost / target.GetMaxMemory());

            bartrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            bartrans.anchoredPosition = new Vector2(0, base_height + height / 2f);
            base_height += height;
            Image barimg = bar.GetComponent<Image>();
            barimg.color = mem.Color;

            Transform text = bar.transform.GetChild(0);
            Text textComponent = text.GetComponent<Text>();
            textComponent.text = mem.MemName;
            text.transform.position = bar.transform.position;

            renderList.Add(bar);
        }
    }

    private void DestroyBar()
    {
        foreach (GameObject o in renderList) Destroy(o);
    }
}
