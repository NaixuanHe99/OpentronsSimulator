using System;
using System.Collections.Generic;

public class Command : Operation
{
    Command father;
    List<Command> family = new List<Command>();
    
    public Command(int level, int mainIndex,string logLine, Command father)
	{
        this.level = level;
        this.mainIndex = mainIndex;
        this.logLine = logLine;
        this.father = father;
        family.Add(this);
    }

    // not used
    // subOperations, test, getSubOperationsLength, getCurrentSubIndex

    public override void animation()
    {

    }

    public Command getFather()
    {
        return father;
    }

    public void addAncestor(Command ancestor)
    {
        family.Insert(0, ancestor);
    }

    public void addDescendant(Command descendant)
    {
        family.Add(descendant);
    }

    public List<Command> getFamily()
    {
        return family;
    }

}

