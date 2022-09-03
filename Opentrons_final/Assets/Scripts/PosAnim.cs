using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class PosAnim : MonoBehaviour
{
    public Transform[] NEST_96;
    public Transform[] NEST_384;
    public Transform[] NEST_12;
    public Transform[] NEST_24;
    public Transform[] NEST_6;
    public Transform[] NEST_1_12;
    public Transform[] Big_1;
    public Transform[] opentrons;
    public Came came;

    // asprate coordinate
    List<Dictionary<string, Transform>>[] NEST_96s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] NEST_384s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] NEST_12s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] NEST_24s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] NEST_6s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] NEST_1_12s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] Big_1s = new List<Dictionary<string, Transform>>[3];

    // pick up tip coordinate
    List<Dictionary<string, Transform>>[] PickUpTip_96s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] PickUpTip_384s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] PickUpTip_12s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] PickUpTip_24s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] PickUpTip_6s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] PickUpTip_1_12s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] PickUpTip_1s = new List<Dictionary<string, Transform>>[3];


    // touch tip coordinate
    List<Dictionary<string, Transform>>[] Touch_96s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] Touch_384s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] Touch_12s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] Touch_24s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] Touch_6s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] Touch_1_12s = new List<Dictionary<string, Transform>>[3];
    List<Dictionary<string, Transform>>[] Touch_1s = new List<Dictionary<string, Transform>>[3];

    /// <summary>
    /// get the box coordinate, List length =3, 0 -> asprate coor; 1-> pick up tip coor; 2->touch tip coor
    /// </summary>
    /// <returns></returns>
    Dictionary<int, Dictionary<string, Transform>[]> box = new Dictionary<int, Dictionary<string, Transform>[]>();

    [SerializeField]
    float width_96s;
    [SerializeField]
    float width_384s;
    [SerializeField]
    float width_12s;
    [SerializeField]
    float width_24s;
    [SerializeField]
    float width_6s;
    [SerializeField]
    float width_1_12s;

    public Transform[] trashCan;


    [HideInInspector]
    public ReadInput readInput;

    [SerializeField, Tooltip("XAxis")]
    Transform[] pipettor_X;

    [SerializeField, Tooltip("YAxis")]
    Transform[] pipettor_Y;

    [SerializeField, Tooltip("Zaxis")]
    Transform[] pipettor_Z;

    [SerializeField, Tooltip("speed")]
    float speed = 3;

    [SerializeField, Tooltip("time")]
    float time = 1.5f;

    [SerializeField, Tooltip("animation")]
    public Animation operationAnim;

    [HideInInspector]
    // playing Animation
    public AnimationClip animationClip;
    // playing coroutine
    // public Coroutine coroutine;

    Vector3[] vector_X;
    Vector3[] vector_Y;
    Vector3[] vector_Z;

    Vector3[] initialPos;

    int opentronsIndex = 0;

    [HideInInspector]
    public int index_New; //current coor
    [HideInInspector]
    public bool isMove = true;
    [HideInInspector]
    public bool _isMove = true;
    [HideInInspector]
    public bool _isOver = false; // check whether finished
    [HideInInspector]
    public int number = 0;

    Transform posCusp; //tip position
    public Button button; //stop button
    public Text text; //text for the delay in amount of time
    UIFunctions uIFunctions;
    public Button[] buttons;

    string Touch_Key = "";
    [HideInInspector]
    public int _father = -1;



    void GetChild(Transform father, List<Dictionary<string, Transform>> childs, List<Dictionary<string, Transform>> touchs, List<Dictionary<string, Transform>> pick)
    {
        for (int i = 0; i < father.childCount; i++)
        {
            Transform child = father.GetChild(i).GetChild(0);
            Dictionary<string, Transform> transforms = new Dictionary<string, Transform>();
            for (int j = 0; j < child.childCount; j++)
            {
                Transform child_c = child.GetChild(j);
                for (int k = 0; k < child_c.childCount; k++)
                {
                    string key = (char)(65 + j) + (k + 1).ToString();
                    Transform pos = child_c.GetChild(k);
                    transforms.Add(key, pos);
                    // Debug.Log("The " + (i + 1) + " th" + " key:  " + key + "   pos:" + pos.position.x + ", " + pos.position.y + ", " + pos.position.z);
                }
            }
            childs.Add(transforms);

            Transform child_Touch = father.GetChild(i).GetChild(1);
            Dictionary<string, Transform> transforms_Touch = new Dictionary<string, Transform>();
            for (int j = 0; j < child_Touch.childCount; j++)
            {
                Transform child_c = child_Touch.GetChild(j);
                for (int k = 0; k < child_c.childCount; k++)
                {
                    string key = (char)(65 + j) + (k + 1).ToString();
                    Transform pos = child_c.GetChild(k);
                    transforms_Touch.Add(key, pos);
                    // Debug.Log("The " + (i + 1) + " th " + "key:  " + key + "   pos:" + pos.position.x + ", " + pos.position.y + ", " + pos.position.z);
                }
            }
            touchs.Add(transforms_Touch);

            Transform child_Pick = father.GetChild(i).GetChild(2);
            Dictionary<string, Transform> transforms_Pick = new Dictionary<string, Transform>();
            for (int j = 0; j < child_Pick.childCount; j++)
            {
                Transform child_c = child_Pick.GetChild(j);
                for (int k = 0; k < child_c.childCount; k++)
                {
                    string key = (char)(65 + j) + (k + 1).ToString();
                    Transform pos = child_c.GetChild(k);
                    transforms_Pick.Add(key, pos);
                    // Debug.Log("The " + (i + 1) + " th " + "key:  " + key + "   pos:" + pos.position.x + ", " + pos.position.y + ", " + pos.position.z);
                }
            }
            pick.Add(transforms_Pick);
        }
    }

    public void Analysis()
    {
        List<string> PyLines = readInput.PyLines;
        Debug.Log(PyLines.Count);
        bool isType = false;
        int type = 0; //0->normal;1->temp;2->magnetic
        int index = 0;
        for (int i = 0; i < PyLines.Count; i++)
        {
            string line = PyLines[i];
            line = line.Replace("\t", ""); //remove indent
            line = line.Replace("'", "");
            // line = line.Replace(" ", "");

            string[] words = line.Split(new char[] { '=', ',', '(', ')', '.' }); //split lines
            for (int j = 0; j < words.Length; j++)
            {
                if (words[j].Contains("load_module"))
                {
                    // Debug.Log("module: " + words[j + 1]);
                    // Debug.Log("module: " + words[j + 1] + " position: " + words[words.Length - 2]);
                    if (words[j + 1].Contains("temperature module"))
                    {
                        isType = true;
                        type = 1;
                        index = int.Parse(words[words.Length - 2]) - 1;
                    }
                    else if (words[j + 1].Contains("magnetic module"))
                    {
                        isType = true;
                        type = 2;
                        index = int.Parse(words[words.Length - 2]) - 1;
                    }
                    else
                    {
                        isType = false;
                        type = 0; //no temp or magnetic module
                    }
                    break;
                }
                // Debug.Log("Python data:" + line + " string: " + words[j]);
                if (words[j].Contains("load_labware"))
                {
                    Dictionary<string, Transform>[] boxPos = new Dictionary<string, Transform>[3];
                    if (!isType)
                    {
                        // Debug.Log("position1:" + words[j + 1] + "position2: " + words[j + 2]);
                        index = int.Parse(words[j + 2].Replace(" ", "")) - 1;
                    }
                    if (words[j + 1].Contains("_1_reservoir_")) // one big pit
                    {
                        Big_1[type].GetChild(index).gameObject.SetActive(true);
                        boxPos[0] = Big_1s[type][index];
                        boxPos[1] = PickUpTip_1s[type][index];
                        boxPos[2] = Touch_1s[type][index];
                    }
                    else if (words[j + 1].Contains("_12_reservoir_")) // 1x12 strips
                    {
                        NEST_1_12[type].GetChild(index).gameObject.SetActive(true);
                        boxPos[0] = NEST_1_12s[type][index];
                        boxPos[1] = PickUpTip_1_12s[type][index];
                        boxPos[2] = Touch_1_12s[type][index];
                    }
                    else if (words[j + 1].Contains("12")) // 3x4 holes
                    {
                        NEST_12[type].GetChild(index).gameObject.SetActive(true);
                        boxPos[0] = NEST_12s[type][index];
                        boxPos[1] = PickUpTip_12s[type][index];
                        boxPos[2] = Touch_12s[type][index];
                    }
                    else if (words[j + 1].Contains("24")) // 4x6 holes
                    {
                        NEST_24[type].GetChild(index).gameObject.SetActive(true);
                        boxPos[0] = NEST_24s[type][index];
                        boxPos[1] = PickUpTip_24s[type][index];
                        boxPos[2] = Touch_24s[type][index];
                    }
                    else if (words[j + 1].Contains("96")) // 9x12 holes
                    {
                        NEST_96[type].GetChild(index).gameObject.SetActive(true);
                        boxPos[0] = NEST_96s[type][index];
                        boxPos[1] = PickUpTip_96s[type][index];
                        boxPos[2] = Touch_96s[type][index];
                    }
                    else if (words[j + 1].Contains("384")) // 16x24 holes
                    {
                        NEST_384[type].GetChild(index).gameObject.SetActive(true);
                        boxPos[0] = NEST_384s[type][index];
                        boxPos[1] = PickUpTip_384s[type][index];
                        boxPos[2] = Touch_384s[type][index];
                    }
                    else if (words[j + 1].Contains("6")) // 2x3
                    {
                        NEST_6[type].GetChild(index).gameObject.SetActive(true);
                        boxPos[0] = NEST_6s[type][index];
                        boxPos[1] = PickUpTip_6s[type][index];
                        boxPos[2] = Touch_6s[type][index];
                    }
                    // add box
                    box.Add(index, boxPos);
                    type = 0;
                    isType = false;

                    break;
                }

                if (words[j] == "load_instrument")
                {
                    string[] types = words[j + 1].Split(new char[] { '_' }); //split by _
                                                                             // Debug.Log("pipette type: " + types[1] + " position:" + index);
                    if (types[1] == "single") //1
                    {
                        opentrons[0].gameObject.SetActive(true);
                        opentrons[1].gameObject.SetActive(false);
                        opentronsIndex = 0;
                    }
                    if (types[1] == "multi") // 8
                    {
                        opentrons[0].gameObject.SetActive(false);
                        opentrons[1].gameObject.SetActive(true);
                        opentronsIndex = 1;
                    }
                    came.target = came.targets[opentronsIndex];
                }
                // Debug.Log("current looping line: " + i + ", current looping word: " + words[j]);
            }
        }
    }
    /// <summary>
    /// load point location
    /// </summary>
    public void OnStart()
    {
        ///normal, temp, magnet
        if (NEST_6 != null)
            for (int i = 0; i < NEST_96.Length; i++)
            {
                NEST_6s[i] = new List<Dictionary<string, Transform>>();
                Touch_6s[i] = new List<Dictionary<string, Transform>>();
                PickUpTip_6s[i] = new List<Dictionary<string, Transform>>();
                GetChild(NEST_6[i], NEST_6s[i], Touch_6s[i], PickUpTip_6s[i]);
            }

        if (NEST_12 != null)
            for (int i = 0; i < NEST_96.Length; i++)
            {
                NEST_12s[i] = new List<Dictionary<string, Transform>>();
                Touch_12s[i] = new List<Dictionary<string, Transform>>();
                PickUpTip_12s[i] = new List<Dictionary<string, Transform>>();
                GetChild(NEST_12[i], NEST_12s[i], Touch_12s[i], PickUpTip_12s[i]);
            }

        if (NEST_24 != null)
            for (int i = 0; i < NEST_96.Length; i++)
            {
                NEST_24s[i] = new List<Dictionary<string, Transform>>();
                Touch_24s[i] = new List<Dictionary<string, Transform>>();
                PickUpTip_24s[i] = new List<Dictionary<string, Transform>>();
                GetChild(NEST_24[i], NEST_24s[i], Touch_24s[i], PickUpTip_24s[i]);
            }

        if (NEST_96 != null)
            for (int i = 0; i < NEST_96.Length; i++)
            {
                NEST_96s[i] = new List<Dictionary<string, Transform>>();
                Touch_96s[i] = new List<Dictionary<string, Transform>>();
                PickUpTip_96s[i] = new List<Dictionary<string, Transform>>();
                GetChild(NEST_96[i], NEST_96s[i], Touch_96s[i], PickUpTip_96s[i]);
            }

        if (NEST_384 != null)
            for (int i = 0; i < NEST_96.Length; i++)
            {
                NEST_384s[i] = new List<Dictionary<string, Transform>>();
                Touch_384s[i] = new List<Dictionary<string, Transform>>();
                PickUpTip_384s[i] = new List<Dictionary<string, Transform>>();
                GetChild(NEST_384[i], NEST_384s[i], Touch_384s[i], PickUpTip_384s[i]);
            }


        if (NEST_1_12 != null)
            for (int i = 0; i < NEST_96.Length; i++)
            {
                NEST_1_12s[i] = new List<Dictionary<string, Transform>>();
                Touch_1_12s[i] = new List<Dictionary<string, Transform>>();
                PickUpTip_1_12s[i] = new List<Dictionary<string, Transform>>();
                GetChild(NEST_1_12[i], NEST_1_12s[i], Touch_1_12s[i], PickUpTip_1_12s[i]);
            }

        if (Big_1 != null)
            for (int i = 0; i < NEST_96.Length; i++)
            {
                Big_1s[i] = new List<Dictionary<string, Transform>>();
                Touch_1s[i] = new List<Dictionary<string, Transform>>();
                PickUpTip_1s[i] = new List<Dictionary<string, Transform>>();
                GetChild(Big_1[i], Big_1s[i], Touch_1s[i], PickUpTip_1s[i]);
            }

        if (readInput == null)
            readInput = GetComponent<ReadInput>();

        Analysis();
    }

    // Start is called before the first frame update
    void Start()
    {
        vector_X = new Vector3[pipettor_X.Length];
        vector_Y = new Vector3[pipettor_Y.Length];
        vector_Z = new Vector3[pipettor_Z.Length];

        initialPos = new Vector3[pipettor_Z.Length];

        for (int i = 0; i < pipettor_X.Length; i++)
        {
            vector_X[i] = pipettor_X[i].position;
            vector_Y[i] = pipettor_Y[i].position;
            vector_Z[i] = pipettor_Z[i].position;

            initialPos[i] = new Vector3(vector_X[i].x, vector_Y[i].y, vector_Z[i].z);
        }


        uIFunctions = GetComponent<UIFunctions>();
    }

    List<Dictionary<string, Transform>> NEST(string type_1, int type_2)
    {
        List<Dictionary<string, Transform>> NEST;
        switch (type_1)
        {
            case "96":
                NEST = NEST_96s[type_2];
                break;
            case "384":
                NEST = NEST_384s[type_2];
                break;
            case "12":
                NEST = NEST_12s[type_2];
                break;
            case "24":
                NEST = NEST_24s[type_2];
                break;
            case "6":
                NEST = NEST_6s[type_2];
                break;
            default:
                NEST = null;
                break;
        }
        return NEST;
    }


    List<Dictionary<string, Transform>> Touch_NEST(string type_1, int type_2)
    {
        List<Dictionary<string, Transform>> NEST;
        switch (type_1)
        {
            case "96":
                NEST = Touch_96s[type_2];
                break;
            case "384":
                NEST = Touch_384s[type_2];
                break;
            case "12":
                NEST = Touch_12s[type_2];
                break;
            case "24":
                NEST = Touch_24s[type_2];
                break;
            case "6":
                NEST = Touch_6s[type_2];
                break;
            default:
                NEST = null;
                break;
        }
        return NEST;
    }

    List<Dictionary<string, Transform>> Pick_NEST(string type_1, int type_2)
    {
        List<Dictionary<string, Transform>> NEST;
        switch (type_1)
        {
            case "96":
                NEST = PickUpTip_96s[type_2];
                break;
            case "384":
                NEST = PickUpTip_384s[type_2];
                break;
            case "12":
                NEST = PickUpTip_12s[type_2];
                break;
            case "24":
                NEST = PickUpTip_24s[type_2];
                break;
            case "6":
                NEST = PickUpTip_6s[type_2];
                break;
            default:
                NEST = null;
                break;
        }
        return NEST;
    }

    void Anim(Transform pos, int index, string tab)
    {
        if (!_isMove)
        {
            Select(index, tab);
            return;
        }
        Tween tween_1 = pipettor_X[opentronsIndex].DOMoveX(pos.position.x, time);
        Tween tween_2 = pipettor_Z[opentronsIndex].DOMoveZ(pos.position.z, time).
         OnComplete(() =>
         {
             Tween tween_1 = pipettor_Y[opentronsIndex].DOMoveY(pos.position.y, time).
          OnComplete(() =>
          {
              Select(index, tab);
          });
         });
        if (!isMove)
        {
            _isMove = false;
        }
    }

    void Select(int index, string tab)
    {
        switch (tab)
        {
            case "Pick up":
                PickUpTipAnim(index);
                break;
            case "Drop":
                DropTipAnim(index);
                break;
            case "Return":
                ReturnTipAnim(index);
                break;
            case "Asperate":
                AsperateAnim(index);
                break;
            case "Dispense":
                DispenseAnim(index);
                break;
            case "Blow out":
                BlowoutAnim(index);
                break;
            case "Touch":
                // TouchTipAnim(index);
                break;
            case "Mix":
                MixAnim(index);
                break;
            case "Air gap":
                AirGapAnim(index);
                break;
            case "Delay":
                DelayAmountOfTimeAnim(index);
                break;
            case "Pause":
                PauseUntilResumedAnim(index);
                break;
            case "Homing":
                HomingAnim(index);
                break;
            case "Comment":
                CommentAnim(index);
                break;
            case "Transfer":
                TransferAnim(index);
                break;
            case "Consolidate":
                ConsolidateAnim(index);
                break;
            case "Distribute":
                DistributeAnim(index);
                break;
            default:
                break;
        }
    }

    // get the string's next word
    public int SelectIndex(string[] words, string str)
    {
        int index = 0;
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i] == str)
            {
                index = i;
            }
        }
        return index + 1;
    }

    //public int SelectType(string str)
    //{
    //  int type = 0;
    //  switch (str)
    //  {
    //    case "Opentrons"://normal
    //      type = 0;
    //      break;
    //    case "Corning"://temp
    //      type = 1;
    //      break;
    //    case "NEST"://mag
    //      type = 2;
    //      break;
    //    default:
    //      type = -1;
    //      break;
    //  }
    //  return type;
    //}

    public void Operat(int index)
    {
        readInput.OperationDistributer(index + 1);
    }

    /// <summary>
    /// start, pick up tip
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public void PickUpTip(int index, string[] words)
    {
        // do sth.
        print("Pick up");
        // int indexOF = SelectIndex(words, "of");
        // List<Dictionary<string, Transform>> _NEST = Pick_NEST(words[indexOF + 1], SelectType(words[indexOF]));
        // Debug.Log("list  " + _NEST);
        // Dictionary<string, Transform> _Trans = _NEST[int.Parse(words[SelectIndex(words, "on")]) - 1];

        int key_1 = int.Parse(words[SelectIndex(words, "on")]) - 1;
        if (!box.ContainsKey(key_1))
        {
            Debug.Log("no box on " + key_1);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Dictionary<string, Transform> _Trans = box[key_1][1];
        string key_2 = words[SelectIndex(words, "from")];
        if (!_Trans.ContainsKey(key_2))
        {
            Debug.Log("number " + key_1 + " box has no hole in position " + key_2);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Transform pos;
        _Trans.TryGetValue(key_2, out pos);
        posCusp = pos;
        Debug.Log("   X: " + pos.position.x + "   Y: " + pos.position.y + "   Z:" + pos.position.z);
        Anim(pos, index, "Pick up");
        index_New = index;
    }
    void PickUpTipAnim(int index)
    {
        print("PickUpTipAnim");
        string animName;
        if (opentronsIndex == 0) //single
            animName = "PickUpTip";
        else
            animName = "PickUpTip2";
        operationAnim[animName].time = 0;
        operationAnim[animName].speed = 1;
        animationClip = operationAnim[animName].clip;
        Debug.Log("current animation: " + animationClip.name);
        // index_New = index;
        uIFunctions.animSpeed = operationAnim[animName].speed;
        operationAnim.Play(animName);
        // coroutine = StartCoroutine(AwaitTime(operationAnim["PickUpTip"].length, index));
    }

    public int DropTip(int index, string[] words)
    {
        // do sth.
        print("Drop");
        Anim(trashCan[opentronsIndex], index, "Drop");
        index_New = index;

        return index + 1;
    }

    void DropTipAnim(int index)
    {
        // do sth.
        print("DropTipAnim");
        string animName;
        if (opentronsIndex == 0)
            animName = "DropTip";
        else
            animName = "DropTip2";
        operationAnim[animName].time = 0;
        operationAnim[animName].speed = 1;
        animationClip = operationAnim[animName].clip;
        Debug.Log("current animation: " + animationClip.name);
        // index_New = index;
        uIFunctions.animSpeed = operationAnim[animName].speed;
        operationAnim.Play(animName);
        // coroutine = StartCoroutine(AwaitTime(operationAnim["DropTip"].length, index));

    }

    public void ReturnTip(int index, string[] words)
    {
        print("Return");
        // int indexOF = SelectIndex(words, "of");
        // List<Dictionary<string, Transform>> _NEST = Pick_NEST(words[indexOF + 1], SelectType(words[indexOF]));
        // Debug.Log("list  " + _NEST);
        // Dictionary<string, Transform> _Trans = _NEST[int.Parse(words[SelectIndex(words, "on")]) - 1];



        int key_1 = int.Parse(words[SelectIndex(words, "on")]) - 1;
        if (!box.ContainsKey(key_1))
        {
            Debug.Log("no box in " + key_1);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Dictionary<string, Transform> _Trans = box[key_1][1];
        string key_2 = words[SelectIndex(words, "into")];
        if (!_Trans.ContainsKey(key_2))
        {
            Debug.Log("number " + key_1 + " box has no hole in position " + key_2);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Transform pos;
        _Trans.TryGetValue(key_2, out pos);
        posCusp = pos;
        Debug.Log("   X: " + pos.position.x + "   Y: " + pos.position.y + "   Z: " + pos.position.z);
        Anim(pos, index, "Return");
        index_New = index;

        return;
    }

    void ReturnTipAnim(int index)
    {
        print("ReturnTipAnim");
        string animName;
        if (opentronsIndex == 0)
            animName = "PickUpTip";
        else
            animName = "PickUpTip2";
        operationAnim[animName].time = operationAnim[animName].length;
        operationAnim[animName].speed = -1f;
        animationClip = operationAnim[animName].clip;
        Debug.Log("current animation: " + animationClip.name);
        // index_New = index;
        uIFunctions.animSpeed = operationAnim[animName].speed;
        operationAnim.Play(animName);
        // coroutine = StartCoroutine(AwaitTime(operationAnim["DropTip"].length, index));
    }

    public void Asperate(int index, string[] words)
    {
        // do sth.
        print("Asperate");
        // int indexOF = SelectIndex(words, "of");
        // List<Dictionary<string, Transform>> _NEST = NEST(words[indexOF + 1], SelectType(words[indexOF]));
        // Dictionary<string, Transform> _Trans = _NEST[int.Parse(words[SelectIndex(words, "on")]) - 1];

        int key_1 = int.Parse(words[SelectIndex(words, "on")]) - 1;
        if (!box.ContainsKey(key_1))
        {
            Debug.Log("no box in " + key_1);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("speed: " + Time.timeScale);
            return;
        }

        Dictionary<string, Transform> _Trans = box[key_1][0];
        string key_2 = words[SelectIndex(words, "from")];
        if (!_Trans.ContainsKey(key_2))
        {
            Debug.Log("number " + key_1 + " box has no hole in position " + key_2);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }


        Transform pos;
        _Trans.TryGetValue(key_2, out pos);
        Debug.Log("   X: " + pos.position.x + "   Y: " + pos.position.y + "   Z: " + pos.position.z);

        string _line = readInput.logLines[index + 1];
        _line = _line.Replace("\t", "");//remove indent

        string[] _words = _line.Split(new char[] { ' ', });//split by space
        Debug.Log("it should be touching: " + _words[0]);
        if (String.Compare(_words[0] + _words[1], "Touchingtip") == 0)
        {
            isMove = false;
        }

        Anim(pos, index, "Asperate");
        index_New = index;
        return;
    }
    void AsperateAnim(int index)
    {
        // do sth.
        print("AsperateAnim");
        print("Asperate");
        string animName;
        if (opentronsIndex == 0) //single
            animName = "Asperate";
        else
            animName = "Asperate2";
        operationAnim[animName].time = 0;
        operationAnim[animName].speed = 1;
        animationClip = operationAnim[animName].clip;
        Debug.Log("current animation: " + animationClip.name);
        // index_New = index;
        uIFunctions.animSpeed = operationAnim[animName].speed;
        operationAnim.Play(animName);
        // coroutine = StartCoroutine(AwaitTime(operationAnim["Asperate"].length, index));
    }

    public void Dispense(int index, string[] words)
    {
        // do sth.
        print("Dispense");
        // int indexOF = SelectIndex(words, "of");
        // List<Dictionary<string, Transform>> _NEST = NEST(words[indexOF + 1], SelectType(words[indexOF]));
        // Dictionary<string, Transform> _Trans = _NEST[int.Parse(words[SelectIndex(words, "on")]) - 1];

        int key_1 = int.Parse(words[SelectIndex(words, "on")]) - 1;
        if (!box.ContainsKey(key_1))
        {
            Debug.Log("no box in " + key_1);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Dictionary<string, Transform> _Trans = box[key_1][0];
        string key_2 = words[SelectIndex(words, "into")];
        if (!_Trans.ContainsKey(key_2))
        {
            Debug.Log("number " + key_1 + " box has no hole in position " + key_2);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Transform pos;
        _Trans.TryGetValue(key_2, out pos);
        Debug.Log("   X: " + pos.position.x + "   Y: " + pos.position.y + "   Z: " + pos.position.z);
        Anim(pos, index, "Dispense");

        index_New = index;

        return;
    }
    void DispenseAnim(int index)
    {
        // do sth.
        print("DispenseAnim");
        string animName;
        if (opentronsIndex == 0)
            animName = "Asperate";
        else
            animName = "Asperate2";
        operationAnim[animName].time = operationAnim[animName].length;
        operationAnim[animName].speed = -1;
        animationClip = operationAnim[animName].clip;
        Debug.Log("current animation: " + animationClip.name);
        // index_New = index;
        uIFunctions.animSpeed = operationAnim[animName].speed;
        operationAnim.Play(animName);
        // coroutine = StartCoroutine(AwaitTime(operationAnim["Asperate"].length, index));
    }

    public void Blowout(int index, string[] words)
    {
        // do sth.
        print("Blow out");
        // int indexOF = SelectIndex(words, "of");
        // List<Dictionary<string, Transform>> _NEST = NEST(words[indexOF + 1], SelectType(words[indexOF]));
        // Dictionary<string, Transform> _Trans = _NEST[int.Parse(words[SelectIndex(words, "on")]) - 1];

        int key_1 = int.Parse(words[SelectIndex(words, "on")]) - 1;
        if (!box.ContainsKey(key_1))
        {
            Debug.Log("no box in " + key_1);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Dictionary<string, Transform> _Trans = box[key_1][0];
        string key_2 = words[SelectIndex(words, "at")];
        if (!_Trans.ContainsKey(key_2))
        {
            Debug.Log("number " + key_1 + " box has no hole in position " + key_2);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Transform pos;
        _Trans.TryGetValue(key_2, out pos);
        Debug.Log("   X: " + pos.position.x + "   Y: " + pos.position.y + "   Z: " + pos.position.z);
        Anim(pos, index, "Blow out");
        index_New = index;

        // float minute = 0;
        // float seconds = 1;
        // StartCoroutine(Delay_Time(index_New, minute, seconds));

        return;
    }
    void BlowoutAnim(int index)
    {
        // do sth.
        print("BlowoutAnim");
        string animName;
        if (opentronsIndex == 0)
            animName = "Asperate";
        else
            animName = "Asperate2";
        operationAnim[animName].time = operationAnim[animName].length;
        operationAnim[animName].speed = -1;
        animationClip = operationAnim[animName].clip;
        Debug.Log("current animation: " + animationClip.name);
        // index_New = index;
        uIFunctions.animSpeed = operationAnim[animName].speed;
        operationAnim.Play(animName);
        // coroutine = StartCoroutine(AwaitTime(operationAnim["Asperate"].length, index));
    }

    public void TouchTip(int index, string[] words)
    {
        // do sth.
        print("Touch");
        // int indexOF = SelectIndex(words, "of");
        // List<Dictionary<string, Transform>> _NEST = Touch_NEST(words[indexOF + 1], SelectType(words[indexOF]));
        // Dictionary<string, Transform> _Trans = _NEST[int.Parse(words[SelectIndex(words, "on")]) - 1];

        int key_1 = int.Parse(words[SelectIndex(words, "on")]) - 1;
        if (!box.ContainsKey(key_1))
        {
            Debug.Log("no box in " + key_1);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Dictionary<string, Transform> _Trans = box[key_1][0];
        string key_2 = words[SelectIndex(words, "from")];
        if (!_Trans.ContainsKey(key_2))
        {
            Debug.Log("number " + key_1 + " box has no hole in position " + key_2);
            uIFunctions.isStart = false;
            uIFunctions.popup.SetActive(true);
            Time.timeScale = 0;
            return;
        }

        Transform pos;
        _Trans.TryGetValue(key_2, out pos);

        string Touch_Width = words[SelectIndex(words, "of") + 1];
        index_New = index;
        TouchTipAnim(index_New, pos, Touch_Width);
        return;
    }
    void TouchTipAnim(int index, Transform pos, string Touch_Width)
    {
        // do sth.
        print("TouchTipAnim");
        float width;
        switch (Touch_Width)
        {
            case "96":
                width = width_96s;
                break;
            case "384":
                width = width_384s;
                break;
            case "12":
                width = width_12s;
                break;
            case "24":
                width = width_24s;
                break;
            case "6":
                width = width_6s;
                break;
            default:
                width = 0;
                break;
        }
        pipettor_Y[opentronsIndex].DOMoveY(pos.position.y, time).
          OnComplete(() =>
          {
              TouchX(width);
              // index_New += 1;
          });
    }
    void TouchX(float width)
    {
        float posX_1 = pipettor_X[opentronsIndex].position.x - width / 2;
        Debug.Log("move position posX_1: " + posX_1 + "tube diameter: " + width);
        pipettor_X[opentronsIndex].DOMoveX(posX_1, 0.12f).OnComplete(() =>
        {
            float posX_2 = pipettor_X[opentronsIndex].position.x + width;
            Debug.Log("move position posX_2: " + posX_2 + "tube diameter" + width);
            pipettor_X[opentronsIndex].DOMoveX(posX_2, 0.12f).OnComplete(() =>
            {
                float posX_3 = pipettor_X[opentronsIndex].position.x - width / 2;
                Debug.Log("move position posX_3: " + posX_3 + "tube diameter: " + width);
                pipettor_X[opentronsIndex].DOMoveX(posX_3, 0.12f).OnComplete(() =>
                {
                    TouchZ(width);
                });
            });
        });
    }
    void TouchZ(float width)
    {
        float posZ_1 = pipettor_Z[opentronsIndex].position.z - width / 2;
        Debug.Log("move position posZ_1: " + posZ_1 + "tube diameter: " + width);
        pipettor_Z[opentronsIndex].DOMoveZ(posZ_1, 0.12f).OnComplete(() =>
        {
            float posZ_2 = pipettor_Z[opentronsIndex].position.z + width;
            Debug.Log("move position posZ_2: " + posZ_2 + "tube diameter: " + width);
            pipettor_Z[opentronsIndex].DOMoveZ(posZ_2, 0.12f).OnComplete(() =>
            {
                float posZ_3 = pipettor_Z[opentronsIndex].position.z - width / 2;
                Debug.Log("move position posZ_3: " + posZ_3 + "tube diameter: " + width);
                pipettor_Z[opentronsIndex].DOMoveZ(posZ_3, 0.12f).OnComplete(() =>
                {
                    isMove = true;
                    AnimOver();
                });
            });
        });
    }


    public int Mix(int index, string[] words)
    {
        // do sth.
        print("Mix");
        number = int.Parse(words[SelectIndex(words, "Mixing")]) * 2;
        index_New = index;
        return index;
    }
    void MixAnim(int index)
    {
        // do sth.
        print("MixAnim");
    }

    public int AirGap(int index, string[] words)
    {
        // do sth.
        print("Air gap");
        index_New = index;
        float minute = 0;
        float seconds = 1;
        StartCoroutine(Delay_Time(index_New, minute, seconds));

        // return Asperate(index + 1, words);
        return index + 1;
    }
    void AirGapAnim(int index)
    {
        // do sth.
        print("AirGapAnim");

    }

    public int DelayAmountOfTime(int index, string[] words)
    {
        // do sth.
        print("Delay" + int.Parse(words[SelectIndex(words, "for")]));
        print("Delay" + (int)float.Parse(words[SelectIndex(words, "and")]));
        float minute = float.Parse(words[SelectIndex(words, "for")]);
        float seconds = float.Parse(words[SelectIndex(words, "and")]);
        index_New = index;
        StartCoroutine(Delay_Time(index_New, minute, seconds));
        return index + 1;
    }
    IEnumerator Delay_Time(int index, float minute, float seconds)
    {

        while (minute >= 0 && seconds >= 0)
        {
            text.text = minute.ToString("00") + ":" + seconds.ToString("00");
            yield return new WaitForSeconds(0.02f);
            if (uIFunctions.isPuase)
                continue;

            seconds -= 0.02f;
            if (seconds < 0 && minute > 0)
            {
                minute -= 1;
                seconds = 60 - 0.02f;
            }
        }
        text.text = "";
        Operat(index_New);
    }
    void DelayAmountOfTimeAnim(int index)
    {
        // do sth.
        print("DelayAmountOfTimeAnim");
    }

    public int PauseUntilResumed(int index, string[] words)
    {
        // do sth.
        print("Pause");
        // buttons[0].gameObject.SetActive(false);
        // buttons[1].gameObject.SetActive(true);
        index_New = index;
        // uIFunctions.isPuase = true;
        StartCoroutine(Delay_Time(index_New, 0, 1)); // set wait time =1

        return index + 1;
    }
    void PauseUntilResumedAnim(int index)
    {
        // do sth.
        print("PauseUntilResumedAnim");
    }

    public int Homing(int index, string[] words)
    {
        // do sth.
        print("Homing");
        index_New = index;
        HomingAnim(index_New);

        return index + 1;
    }
    void HomingAnim(int index)
    {
        // do sth.
        print("HomingAnim");
        Tween tween_1 = pipettor_Y[opentronsIndex].DOMoveY(initialPos[opentronsIndex].y, time).
        OnComplete(() =>
        {
            Tween tween_1 = pipettor_X[opentronsIndex].DOMoveX(initialPos[opentronsIndex].x, time);
            Tween tween_2 = pipettor_Z[opentronsIndex].DOMoveZ(initialPos[opentronsIndex].z, time).
        OnComplete(() =>
        {
            Operat(index_New);
        });
        });
    }

    // if log line not match, show the line with no actions
    public int Comment(int index, string[] words)
    {
        // do sth.
        print("Comment");
        index_New = index;

        return index + 1;
    }
    void CommentAnim(int index)
    {
        // do sth.
        print("CommentAnim");
    }

    public int Transfer(int index, string[] words)
    {
        // do sth.
        print("Transfer");
        index_New = index;

        //Use indent handler to increase the index
        // return IndentHandler(index);
        return index + 1;

    }
    void TransferAnim(int index)
    {
        // do sth.
        print("TransferAnim");


    }

    public int Consolidate(int index, string[] words)
    {
        // do sth.
        print("Consolidate");
        index_New = index;

        // return Transfer(index + 1, words);
        return index + 1;
    }
    void ConsolidateAnim(int index)
    {
        // do sth.
        print("ConsolidateAnim");
    }

    public int Distribute(int index, string[] words)
    {
        // do sth.
        print("Distribute");
        index_New = index;

        // return Transfer(index + 1, words);
        return index + 1;
    }
    void DistributeAnim(int index)
    {
        // do sth.
        print("DistributeAnim");
    }

    public void AnimOver()
    {
        Debug.Log("after animation is over, isMove: " + isMove);
        if (_isMove)
        {
            pipettor_Y[opentronsIndex].DOMoveY(vector_Y[opentronsIndex].y, time).
            OnComplete(() =>
            {
                if (!_isOver)
                    Operat(index_New);
                else
                {
                    button.interactable = true;
                    _isOver = false;
                }
            });
        }
        else
        {
            Debug.Log("after animation is over, isMove: " + isMove);
            if (!_isOver)
                Operat(index_New);
            else
            {
                button.interactable = true;
                _isOver = false;
            }
        }
        if (number > 0)
        {
            number--;
            Debug.Log("remaining times: " + number);
        }
        if (number == 0)
        {
            isMove = true;
            _isMove = true;
        }
    }

    IEnumerator AwaitTime(float _time, int index)
    {
        yield return new WaitForSeconds(_time + 0.3f);
        if (_isMove)
        {
            pipettor_Y[opentronsIndex].DOMoveY(vector_Y[opentronsIndex].y, time).
            OnComplete(() =>
            {
                Operat(index);
            });
        }
        else
        {
            Operat(index);
        }
        if (number > 0)
        {
            number--;
            Debug.Log("remaining time: " + number);
        }
        if (number == 0)
        {
            isMove = true;
            _isMove = true;
        }
        // coroutine = null;

    }


}
