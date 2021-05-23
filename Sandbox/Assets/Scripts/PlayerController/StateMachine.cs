using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public State CurrentState { get; private set; }
    public State PreviousState { get; private set; }
    public State NextState { get; private set; }


    // set and enter starting state
    public void InitialState(State startingState)
    {
        CurrentState = startingState;
        PreviousState = CurrentState;
        CurrentState.Enter();
    }
    // set the current state
    public void ChangeState(State newState)
    {
        CurrentState.Exit();
        // store previous state
        if (PreviousState != CurrentState)
            PreviousState = CurrentState;

        CurrentState = newState;
        CurrentState.Enter();
    }

    public void QueueState(State nextState)
    {
        NextState = nextState;
    }
    public State UseNextState()
    {
        State state = NextState;
        NextState = null;
        return state;
    }

}
