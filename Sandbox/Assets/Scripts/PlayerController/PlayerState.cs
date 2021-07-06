using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState: State
{
    protected PlayerControllerRB player;
    protected string animation;
    protected bool isAnimationComplete;
    protected bool InputSwitchPlayer;
    protected bool SwitchPressed;

    public PlayerState(PlayerControllerRB player, string animation)
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
        //Debug.Log(this.GetType().Name + " state entered");
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
            InputSwitchPlayer = false;
            player.InputHandler.SetSwitchFalse();
            player.Other.InputHandler.SetSwitchFalse();
            if (player.CanSwitch)
            {
                player.DisableControls();
                player.Other.EnableControls();
            }
        }
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

    public virtual bool AnimationComplete()
    {
        return (player.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !player.Anim.IsInTransition(0));
    }
}

