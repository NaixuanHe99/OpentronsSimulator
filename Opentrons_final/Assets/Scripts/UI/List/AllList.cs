using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class AllList : MonoBehaviour
{
    [HideInInspector]
    public List<string> logLines = new List<string>();
    public GameObject prefab;
    public Transform father;
    List<Transform> fathers = new List<Transform>();
    List<Button> textObj = new List<Button>();
    float width = 0;
    int index = -1;
    public ColorBlock color_1;
    public ColorBlock color_2;
    PosAnim posAnim;
    UIFunctions uIFunctions;
    private void Start()
    {
        posAnim = GetComponent<PosAnim>();
        uIFunctions = GetComponent<UIFunctions>();
    }

    //create UI list based on the log
    //create UI for each line
    public void CreateText()
    {
        if (fathers.Count == 0)
            fathers.Add(father);
        for (int i = 0; i < logLines.Count; i++)
        {
            string line = logLines[i];
            line = line.Replace("\t", "");
            int count = logLines[i].Length - line.Length;
            if (count == fathers.Count - 1)
            {
                GameObject obj = Instantiate(prefab, fathers[fathers.Count - 1]);
                obj.name = i.ToString();
                ListSize listSize = obj.GetComponent<ListSize>();
                if (count == 0)
                {
                    listSize.father = -1;
                }
                else
                {
                    ListSize fatherList = fathers[count].GetComponent<ListSize>();
                    listSize.father = fatherList.index;
                }
                listSize.index = i;

                //calculate text width and height
                ListSize objList = obj.GetComponent<ListSize>();
                objList.text.text = line;
                TextGenerator generator = objList.text.cachedTextGenerator;
                TextGenerationSettings textSetting = objList.text.GetGenerationSettings(Vector2.zero);
                float TextWidth = generator.GetPreferredWidth(line, textSetting) + 30;
                RectTransform rectTransform = objList.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(TextWidth, rectTransform.rect.height);
                width = Mathf.Max(width, TextWidth);

                Button button = obj.transform.GetChild(0).GetChild(0).GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    Debug.Log("press: " + uIFunctions.isStart);
                    posAnim.buttons[0].gameObject.SetActive(true);
                    posAnim.buttons[1].gameObject.SetActive(false);
                    // if (posAnim.animationClip != null || DOTween.PlayingTweens() != null)
                    // if (posAnim._isOver)
                    //   return;
                    GameObject _click = EventSystem.current.currentSelectedGameObject;
                    ListSize listSize1 = _click.transform.parent.parent.GetComponent<ListSize>();
                    // int index = int.Parse(_click.transform.parent.parent.name);
                    // posAnim._isOver = true;
                    // posAnim.Operat(index - 1);
                    posAnim.index_New = listSize1.index - 1;
                    posAnim._father = listSize1.father;
                    // posAnim.index_New = index - 1;
                    if (uIFunctions.isStart)
                    {
                        uIFunctions.OverOpentron();
                        posAnim._isMove = true;
                        posAnim.isMove = true;
                    }
                    else
                    {
                        Debug.Log("press: " + uIFunctions.isStart);
                        uIFunctions.isStart = true;
                        posAnim._isMove = true;
                        posAnim.isMove = true;
                        posAnim.Operat(posAnim.index_New);
                    }

                });
                textObj.Add(button);
                fathers.Add(obj.transform);
            }
            else
            {
                GameObject obj = Instantiate(prefab, fathers[count]);
                obj.name = i.ToString();
                ListSize listSize = obj.GetComponent<ListSize>();
                if (count == 0)
                {
                    listSize.father = -1;
                }
                else
                {
                    ListSize fatherList = fathers[count].GetComponent<ListSize>();
                    listSize.father = fatherList.index;
                }
                listSize.index = i;

                //calcutate text width and height
                ListSize objList = obj.GetComponent<ListSize>();
                objList.text.text = line;
                TextGenerator generator = objList.text.cachedTextGenerator;
                TextGenerationSettings textSetting = objList.text.GetGenerationSettings(Vector2.zero);
                float TextWidth = generator.GetPreferredWidth(line, textSetting) + 30 * (count + 1);
                RectTransform rectTransform = objList.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(TextWidth, rectTransform.rect.height);
                width = Mathf.Max(width, TextWidth);

                fathers[count + 1] = obj.transform;
                ListSize ButtonList = fathers[count].GetComponent<ListSize>();
                if (ButtonList.button.Length != 0 && !ButtonList.button[0].activeSelf)
                {
                    ButtonList.button[0].SetActive(true);
                }

                Button button = obj.transform.GetChild(0).GetChild(0).GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    Debug.Log("press: " + uIFunctions.isStart);
                    posAnim.buttons[0].gameObject.SetActive(true);
                    posAnim.buttons[1].gameObject.SetActive(false);
                    // if (posAnim.animationClip != null || DOTween.PlayingTweens() != null)
                    // if (posAnim._isOver)
                    //   return;
                    GameObject _click = EventSystem.current.currentSelectedGameObject;
                    ListSize listSize1 = _click.transform.parent.parent.GetComponent<ListSize>();
                    // int index = int.Parse(_click.transform.parent.parent.name);
                    // posAnim._isOver = true;
                    // posAnim.Operat(index - 1);
                    posAnim.index_New = listSize1.index - 1;
                    posAnim._father = listSize1.father;
                    if (uIFunctions.isStart)
                    {
                        uIFunctions.OverOpentron();
                        posAnim._isMove = true;
                        posAnim.isMove = true;
                    }
                    else
                    {
                        Debug.Log("press: " + uIFunctions.isStart);
                        uIFunctions.isStart = true;
                        posAnim._isMove = true;
                        posAnim.isMove = true;
                        posAnim.Operat(posAnim.index_New);
                    }
                });
                textObj.Add(button);
            }
        }
        RectTransform rectTransform1_1 = transform.GetChild(0).GetComponent<RectTransform>();
        rectTransform1_1.sizeDelta = new Vector2(width + 20, rectTransform1_1.rect.height);

        RectTransform rectTransform1_2 = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        rectTransform1_2.sizeDelta = new Vector2(width + 20, rectTransform1_2.rect.height);
    }

    public void ChangeColor(int _index)
    {
        if (index != -1)
        {
            textObj[index].colors = color_1;
        }
        if (_index != -1)
        {
            index = _index;
            textObj[index].colors = color_2;
        }
    }


    public void bigWidth()
    {
        ListSize listSize_1 = father.GetComponent<ListSize>();
        listSize_1.isChangeWidth = false;
        ListSize listSize_2 = father.parent.GetComponent<ListSize>();
        listSize_2.isChangeWidth = false;
    }
    public void tinyWidth()
    {
        ListSize listSize_1 = father.GetComponent<ListSize>();
        listSize_1.isChangeWidth = true;
        ListSize listSize_2 = father.parent.GetComponent<ListSize>();
        listSize_2.isChangeWidth = true;
    }

}
