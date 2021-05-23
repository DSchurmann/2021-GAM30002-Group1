using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level 
{
    public List<WorldState>WorldState;
    public WorldState goal = new WorldState("hasItem", true);

    public LevelGoal HasItem = new LevelGoal();
    public bool GoalMet()
    {
        return false;
    }

    public bool InWorldState(WorldState state)
    {
        foreach (var item in WorldState)
        {
            if(state == item)
            {
                return true;
            }
        }
        return false;
    }


    public void ActionUtility(LevelAction action)
    {
        List<WorldState> peekNextWorld = new List<WorldState>();




    }
}


public class LevelGoal
{
    public WorldState _worldState;
}

public class LevelAction
{
    // pre
    public string _name_pre;
    public bool _value_pre;
    // add
    public string _name_add;
    public bool _value_add;
    // del
    public string _name_del;
    public bool _value_del;
}

public class WorldState
{
    private string _name;
    private bool _value;

    public WorldState(string name, bool value)
    {
        _name = name;
        _value = value;
    }

}
