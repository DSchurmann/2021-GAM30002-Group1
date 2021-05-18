using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChildState:State
{
    protected new ChildControllerRB player;
    protected bool InputSwitchPlayer;
    protected bool switchedPlayer;

    protected ChildState(ChildControllerRB player, string animation) : base(player, animation)
    {
        this.player = player;
        this.animation = animation;
    }

    // called when entering state
    public override void Enter()
    {
        // perform 
        Perform();
        // set animation on
        player.Anim.Play(animation);
        // set start time
        startTime = Time.time;
        isExitingState = false;
        //Debug.Log(animation);
        //Debug.Log(this.GetType().Name + state entered");
    }

    // update state
    public override void Update()
    {
        isAnimationComplete = AnimationComplete();

        //get input
        InputSwitchPlayer = player.InputHandler.InputSwitch;

        // switch players
        if (InputSwitchPlayer && player.ControllerEnabled)
        {
            player.InputHandler.SetSwitchFalse();
            if (player.CanSwitch)
            {
                player.CanSwitch = false;
                Debug.Log("CONTROL GIVEN TO GOLEM");
                player.ControllerEnabled = false;
                player.Other.ControllerEnabled = true;
                player.ChangeState(player.AIWaitState);
            }
        }
        // reset can switch player

        //Debug.Log(this.GetType().Name + " state updating by delta time");
    }

    // fixed update state
    public override void FixedUpdate()
    {
        Perform();
        //Debug.Log(this.GetType().Name + " state updating by fixed time");
    }

    // called on exiting state
    public override void Exit()
    {
        isExitingState = true;
        //Debug.Log(this.GetType().Name + " state exited");
    }
    
    // state function
    public override void Perform()
    {

    }
}

