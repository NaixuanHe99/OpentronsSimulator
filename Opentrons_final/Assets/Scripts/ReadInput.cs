using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;

public class ReadInput : MonoBehaviour
{
    [HideInInspector]
    public List<string> logLines = new List<string>();
    [HideInInspector]
    public List<string> PyLines = new List<string>();

    // store the level 0 commands
    List<Command> mainCommands = new List<Command>();

    // store the reference list
    Command[] referenceList;


    public Text commandLine;
    private float totalTime;
    [SerializeField]
    int majorIndex = 0;
    [HideInInspector]
    public string path_1, path_2; //path for py file and log file

    PosAnim posAnim;
    AllList allList;
    UIFunctions uIFunctions;


    // Option option = Option.READ_OPT1;

    // Start is called before the first frame update
    void Start()
    {
        // path_1 = Application.streamingAssetsPath + "/log/indentTest.log";
        // path_2 = Application.streamingAssetsPath + "/log/tutorial.py";
        path_1 = Application.streamingAssetsPath + "/log/pcr.log";
        path_2 = Application.streamingAssetsPath + "/log/pcr.py";

        if (posAnim == null)
            posAnim = GetComponent<PosAnim>();
        if (allList == null)
            allList = GetComponent<AllList>();
        if (uIFunctions == null)
            uIFunctions = GetComponent<UIFunctions>();
        // read file
        ReadLine(path_1, logLines);
        ReadLine(path_2, PyLines);
        //LogHandler();

        allList.logLines = logLines;
        allList.CreateText();
        posAnim.OnStart();

        // init
        referenceList = new Command[logLines.Count];

        CreateTree(0, logLines.Count, null);

        buildAllFamilies();


    }

    void Update()
    {
    }

    public void Run()
    {
        LogHandler();
    }

    public void ReadLine(string path, List<string> _lines)
    {
        // string[] lines = File.ReadAllLines(path, Encoding.UTF8);
        string[] lines = File.ReadAllLines(path, Encoding.UTF8);
        foreach (string line in lines)
        {
            _lines.Add(line);
            //print(line);
        }
    }

    // create the hierachy command tree
    void CreateTree(int lowBound, int highBound, Command father)
    {
        //first time:
        //lowBound = 0;
        //highBound = logLines.Count;

        int target_level;
        Command currentFather = father;

        if (highBound - lowBound > 1)
        {
            if (highBound - lowBound == logLines.Count) // for main commands
            {

                target_level = 0;
                int previous = 0;
                int next = 0;
                for (int i = lowBound; i < highBound; i++)
                {
                    if (countIndent(logLines[i]) == target_level)
                    {
                        Command offspring = new Command(0, i, logLines[i], null);


                        if (i != 0)
                        {
                            next = i;
                            CreateTree(previous, next, mainCommands.Last());
                            mainCommands.Add(offspring);

                            // insert the command to the reference list
                            referenceList[i] = mainCommands[mainCommands.Count - 1];

                            previous = i + 1;
                        }
                        else
                        {
                            //i=0
                            mainCommands.Add(offspring);

                            // insert the command to the reference list
                            referenceList[i] = mainCommands[mainCommands.Count - 1];


                            previous = 1;


                        }

                    }
                }

                if (next == 0) // only one main command
                {
                    CreateTree(previous, highBound, mainCommands.Last());
                }

            }
            else // for sub commands recursion 
            {
                target_level = countIndent(logLines[lowBound]);
                int previous = lowBound;
                int next = lowBound;
                for (int i = lowBound; i < highBound; i++)
                {
                    if (countIndent(logLines[i]) == target_level)
                    {
                        Command offspring = new Command(target_level, i, logLines[i], father);

                        if (i != lowBound)
                        {
                            next = i;
                            CreateTree(previous, next, father.getLast());
                            father.addSubCommand(offspring);


                            // insert the command to the reference list
                            referenceList[i] = father.getLast();


                            previous = i + 1;
                        }
                        else
                        {
                            // i = lowBound
                            father.addSubCommand(offspring);

                            // insert the command to the reference list
                            referenceList[i] = father.getLast();


                            previous = lowBound + 1;
                        }

                    }




                }


            }
        }

    }

    // find the ancestors and descendants
    public void buildFamily(Command contemporary)
    {
        // find descendants
        Command temp = contemporary;
        while (temp != null)
        {
            Command firstSon = temp.getSubCommand(0);
            if (firstSon != null)
            {
                contemporary.addDescendant(firstSon);
                temp = firstSon;
            }
            else
            {
                break;
            }
        }

        // find ancestors
        temp = contemporary;
        while (temp != null)
        {
            Command father = temp.getFather();
            if (father != null)
            {
                contemporary.addAncestor(father);
                temp = father;
            }
            else
            {
                break;
            }
        }


    }

    // find families for all commands
    void buildAllFamilies()
    {
        foreach (Command c in referenceList)
        {
            buildFamily(c);
        }
    }



    void LogHandler()
    {
        int lineIndex = 0;
        int maxIndex = logLines.Count - 1;

        // while (lineIndex <= maxIndex)
        // {

        //   int next = OperationDistributer(lineIndex);

        //   lineIndex = next;
        // }
        OperationDistributer(lineIndex);
    }




    // distribute the operation
    public int OperationDistributer(int lineNum)
    {
        if (lineNum >= logLines.Count)
        {
            allList.ChangeColor(-1);
            uIFunctions.isStart = false;
            return logLines.Count;
        }
        allList.ChangeColor(lineNum);
        string line = logLines[lineNum];
        line = line.Replace("\t", "");//remove indent

        string[] words = line.Split(new char[] { ' ', });//split by space
        Debug.Log(words[0]);
        int newLineNum;

        if (String.Compare(words[0] + words[1], "Touchingtip") == 0)
        {
            newLineNum = TouchTip(lineNum, words);
        }
        else if (String.Compare(words[0] + words[1] + words[2], "Pickinguptip") == 0)
        {
            newLineNum = PickUpTip(lineNum, words);
        }
        else if (String.Compare(words[0] + words[1], "Droppingtip") == 0)
        {
            newLineNum = DropTip(lineNum, words);
        }
        else if (String.Compare(words[0] + words[1], "Returningtip") == 0)
        {
            newLineNum = ReturnTip(lineNum, words);
        }
        else if (String.Compare(words[0], "Aspirating") == 0)
        {
            if (posAnim._father != -1)
            {
                string _line_2 = logLines[posAnim._father];
                _line_2 = _line_2.Replace("\t", "");//remove indent

                string[] _words_2 = _line_2.Split(new char[] { ' ', });//split by space
                Debug.Log("aspirating with father Mixing: " + posAnim._father);
                if (String.Compare(_words_2[0], "Mixing") == 0) //handle mixing
                {
                    Debug.Log("father is mixing");
                    posAnim.isMove = false;
                    posAnim.number = int.Parse(_words_2[posAnim.SelectIndex(_words_2, "Mixing")]) * 2 - (lineNum - posAnim._father);
                    Debug.Log("remain: " + posAnim.number);
                }
                posAnim._father = -1;
            }
            newLineNum = Asperate(lineNum, words);
        }
        else if (String.Compare(words[0], "Dispensing") == 0)
        {

            string _line = logLines[lineNum + 1];
            _line = _line.Replace("\t", "");

            string[] _words = _line.Split(new char[] { ' ', });
            Debug.Log("dispensing, next is mixing");
            if (String.Compare(_words[0], "Mixing") == 0)
            {
                Debug.Log("next is Mixing");
                posAnim.isMove = false;
            }

            if (posAnim._father != -1)
            {
                string _line_2 = logLines[posAnim._father];
                _line_2 = _line_2.Replace("\t", "");

                string[] _words_2 = _line_2.Split(new char[] { ' ', });
                Debug.Log("dispensing with father Mixing" + posAnim._father + "  : " + _words_2[0]);
                if (String.Compare(_words_2[0], "Mixing") == 0)
                {
                    Debug.Log("father is mixing");
                    posAnim.isMove = false;
                    posAnim.number = int.Parse(_words_2[posAnim.SelectIndex(_words_2, "Mixing")]) * 2 - (lineNum - posAnim._father);
                    Debug.Log("remain: " + posAnim.number);
                    if (posAnim.number == 0)
                    {
                        posAnim.isMove = true;
                        posAnim._isMove = true;
                    }
                }
                posAnim._father = -1;
            }
            newLineNum = Dispense(lineNum, words);
        }
        else if (String.Compare(words[0] + words[1], "Blowingout") == 0)
        {
            newLineNum = Blowout(lineNum, words);
        }
        else if (String.Compare(words[0] + words[1], "Touchingtip") == 0)
        {
            newLineNum = TouchTip(lineNum, words);
        }
        else if (String.Compare(words[0], "Mixing") == 0)
        {
            posAnim.isMove = false;
            newLineNum = Mix(lineNum, words);
        }
        else if (String.Compare(words[0] + words[1], "Airgap") == 0)
        {
            newLineNum = AirGap(lineNum, words);
        }
        else if (String.Compare(words[0], "Delaying") == 0)
        {
            newLineNum = DelayAmountOfTime(lineNum, words);
        }
        else if (String.Compare(words[0] + words[1], "Pausingrobot") == 0)
        {
            newLineNum = PauseUntilResumed(lineNum, words);
        }
        else if (String.Compare(words[0] + words[1], "Homingpipette") == 0)
        {
            newLineNum = Homing(lineNum, words);
        }
        else if (String.Compare(words[0], "Transferring") == 0)//complex operation: Transfer, sub instructions: Pick up tip + (Aspirate & Dispense) x n + Drop tip 
        {
            newLineNum = Transfer(lineNum, words);
        }
        else if (String.Compare(words[0], "Consolidating") == 0)//complex operation: Consolidate, sub instructions: Transfer (multiple aspiration combined) 
        {
            newLineNum = Consolidate(lineNum, words);
        }
        else if (String.Compare(words[0], "Distributing") == 0)//complex operation: Distribute, sub instructions: Transfer (multiple dispensation combined, with blow out)
        {
            newLineNum = Distribute(lineNum, words);
        }
        else
        {
            // just print out the line
            // newLineNum = Comment(lineNum, words);
            newLineNum = lineNum + 1;
            OperationDistributer(newLineNum);
        }



        return newLineNum;
    }

    int PickUpTip(int index, string[] words)
    {
        // do sth.
        print("Pick up");
        posAnim.PickUpTip(index, words);

        return index + 1;
    }

    int DropTip(int index, string[] words)
    {
        // do sth.
        print("Drop");
        posAnim.DropTip(index, words);

        return index + 1;
    }

    int ReturnTip(int index, string[] words)
    {
        print("Return");
        posAnim.ReturnTip(index, words);
        // return DropTip(index + 1, words);
        return index + 1;
    }

    int Asperate(int index, string[] words)
    {
        // do sth.
        print("Asperate");
        posAnim.Asperate(index, words);
        return index + 1;
    }

    int Dispense(int index, string[] words)
    {
        // do sth.
        print("Dispense");
        posAnim.Dispense(index, words);

        return index + 1;
    }

    int Blowout(int index, string[] words)
    {
        // do sth.
        print("Blow out");
        posAnim.Blowout(index, words);
        return index + 1;
    }

    int TouchTip(int index, string[] words)
    {
        // do sth.
        print("Touch");
        string _line = logLines[index - 1];
        _line = _line.Replace("\t", "");

        string[] _words = _line.Split(new char[] { ' ', });

        posAnim.TouchTip(index, _words);


        return index + 1;
    }

    int Mix(int index, string[] words)
    {
        // do sth.
        print("Mix");
        posAnim.Mix(index, words);
        OperationDistributer(index + 1);
        return index + 1;
    }

    int AirGap(int index, string[] words)
    {
        // do sth.
        print("Air gap");
        posAnim.AirGap(index, words);

        // return Asperate(index + 1, words);
        return index + 1;
    }

    int DelayAmountOfTime(int index, string[] words)
    {
        // do sth.
        print("Delay");
        posAnim.DelayAmountOfTime(index, words);

        return index + 1;
    }

    int PauseUntilResumed(int index, string[] words)
    {
        // do sth.
        print("Pause");
        posAnim.PauseUntilResumed(index, words);

        return index + 1;
    }

    int Homing(int index, string[] words)
    {
        // do sth.
        print("Homing");
        posAnim.Homing(index, words);

        return index + 1;
    }

    int Comment(int index, string[] words)
    {
        // do sth.
        print("Comment");
        posAnim.Comment(index, words);

        return index + 1;
    }

    int Transfer(int index, string[] words)
    {
        // do sth.
        print("Transfer");
        posAnim.Transfer(index, words);
        OperationDistributer(index + 1);

        return index + 1;

    }

    int Consolidate(int index, string[] words)
    {
        // do sth.
        print("Consolidate");
        posAnim.Consolidate(index, words);

        // return Transfer(index + 1, words);
        return index + 1;
    }

    int Distribute(int index, string[] words)
    {
        // do sth.
        print("Distribute");
        posAnim.Distribute(index, words);

        // return Transfer(index + 1, words);
        return index + 1;
    }

    int IndentHandler(int index, string[] words)
    {
        // do sth.
        string line = logLines[index];


        int current = index; // overall instruction
        int temp = index + 1; // sub instructions

        while (countIndent(logLines[current]) < countIndent(logLines[temp]))//if is sub instruction
        {
            temp = OperationDistributer(temp);
            if (temp >= logLines.Count)
            {
                break;
            }
        }

        int newIndex = temp;
        return newIndex;
    }

    int countIndent(string line)
    {
        int c = 0;
        while (line.Contains('\t'))
        {
            c++;
            line = line.Remove(0, 1);//remove the first
        }
        return c;
    }


}

