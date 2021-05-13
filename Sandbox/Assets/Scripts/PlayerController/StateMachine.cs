using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public State CurrentState { get; private set; }


    // set and enter starting state
    public void InitialState(State startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }
    // set the current state
    public void ChangeState(State newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }


}
