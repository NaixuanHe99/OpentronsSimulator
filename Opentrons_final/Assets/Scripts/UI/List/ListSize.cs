using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// [ExecuteAlways]
public class ListSize : MonoBehaviour
{
    [HideInInspector]
    public float width = 0;
    float height = 0;
    RectTransform rectTransform;
    public GameObject[] button;
    public Text text;
    public Color color1;
    public Color color2;
    [HideInInspector]
    public bool isChangeWidth = false;

    public int father = -1;
    public int index = 0;
    private void Start()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        ChangeSize();

    }
    private void Update()
    {
        ChangeSize();
    }
    void ChangeSize()
    {
        // width = Mathf.Max(width, rectTransform.rect.width);
        width = rectTransform.rect.width;
        float _width = width;
        if (isChangeWidth)
            _width = _width / 2;
        height = 0;
        float newheight = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tr = transform.GetChild(i);
            if (tr.gameObject.activeSelf && tr.name != "scrollbar")
                newheight += transform.GetChild(i).GetComponent<RectTransform>().rect.height;
        }
        height = newheight;
        rectTransform.sizeDelta = new Vector2(_width, height);
        if (transform.parent.name == "functionWindow")
        {
            float Width_New = Mathf.Max(500, rectTransform.rect.width);
            // float height_New = Mathf.Min(800, rectTransform.rect.height + 10);
            float height_New = rectTransform.rect.height + 10;
            rectTransform.sizeDelta = new Vector2(Width_New, height_New);
            // rectTransform.anchoredPosition = new Vector2(0, 0);
        }
        if (transform.parent.name == "Canvas")
        {
            float Width_New = Mathf.Max(500, rectTransform.rect.width);
            float height_New = Mathf.Min(200, rectTransform.rect.height);
            rectTransform.sizeDelta = new Vector2(Width_New, height_New);
            rectTransform.anchoredPosition = new Vector2(60, -60);
        }
    }

    public void Switch()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            GameObject tr = transform.GetChild(i).gameObject;
            tr.gameObject.SetActive(!tr.activeSelf);
        }
        ChangeSize();
    }
}
