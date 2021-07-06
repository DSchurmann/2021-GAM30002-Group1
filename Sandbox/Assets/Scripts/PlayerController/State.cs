using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected bool isExitingState;    
    protected float startTime;

    public State()
    {

    }

    // called when entering state
    public virtual void Enter()
    {
        // perform 
        Perform();
        // set start time
        startTime = Time.time;

        isExitingState = false;
    }

    // update state
    public virtual void Update()
    {
      
    }

    // fixed update state
    public virtual void FixedUpdate()
    {
        Perform();
        //Debug.Log(this.GetType().Name + " state updating by fixed time");
    }

    // called on exiting state
    public virtual void Exit()
    {
        isExitingState = true;
        //Debug.Log(this.GetType().Name + " state exited");
    }
    
    // state function
    public virtual void Perform()
    {

    }
}

