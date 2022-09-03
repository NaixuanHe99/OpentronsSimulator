using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Operation
{
  protected int level; // level is the hierachy of this operation
  protected int mainIndex; // index of the log

  // protected int currentSubIndex = 0;

  protected string logLine;

  protected List<Command> subOperations = new List<Command>(); // sub operation list

  // abstract method
  public abstract void animation(); // animation

  // non abstract method
  public int getMainIndex()
  {
    return mainIndex;
  }

  public int getSubOperationsLength()
  {
    return subOperations.Count;
  }

  public Command getLast()
  {
    if (subOperations.Count > 0)
    {
      return subOperations.Last();
    }
    else
    {
      return null;
    }

  }

  public string getLine()
  {
    return logLine;
  }

  public void addSubCommand(Command command)
  {
    subOperations.Add(command);
  }

  public Command getSubCommand(int subIndex)
  {
    if (subOperations.Count > 0)
    {
      return subOperations[subIndex];
    }
    else
    {
      return null;
    }

  }
}