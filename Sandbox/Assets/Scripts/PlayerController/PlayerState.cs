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

    protected Vector3 holdPosition;

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
        //player.Anim.Play(animation);
        player.Anim.CrossFade(animation, 0.5f);
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

        // get input 
        InputSwitchPlayer = player.InputHandler.InputSwitch;


        // switch players
        if (InputSwitchPlayer && player.ControllerEnabled)
        {
            //Debug.Log("SWAP PLAYER");
            InputSwitchPlayer = false;
            player.InputHandler.SetSwitchFalse();
            player.Other.InputHandler.SetSwitchFalse();

            if (player.CanSwitch && GameController.GH.IsFriend)
            {
                player.DisableControls();
                player.Other.Following = false;
                player.Other.Waiting = false;
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

    public void HoldPosition(bool x, bool y)
    {
        Vector3 pos = player.transform.position;

        // set velocity for wall grab
        if (x)
        {
            pos.x = holdPosition.x;
            player.transform.position = pos;
            player.SetVelocityX(0);
        }

        if (y)
        {
            pos.y = holdPosition.y;
            player.transform.position = pos;
            player.SetVelocityY(0);
        }
    }
}

