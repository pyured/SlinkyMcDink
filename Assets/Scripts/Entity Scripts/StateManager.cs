using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager //all this for fucking enums
{
    private int stateIndex;
    private List<string> states;
    public StateManager(string[] s)
    {
        states = new List<string>();
        states.AddRange(s);
    }
    public bool SetState(string s)
    {
        int index = states.FindIndex(state => state == s);
        return index != -1;
    }
    public string GetCurrentState()
    {
        return states[stateIndex];
    }
    public void AddStates(string[] s)
    {
        states.AddRange(s);
    }
}
