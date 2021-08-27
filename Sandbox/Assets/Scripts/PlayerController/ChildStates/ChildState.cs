using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChildState: PlayerState
{
    protected new ChildControllerRB player;
    protected bool switchedPlayer;
    protected bool inputAttack;
    protected bool inputWait;

    protected ChildState(ChildControllerRB player, string animation) : base(player, animation)
    {
        this.player = player;
        this.animation = animation;
    }

    // called when entering state
    public override void Enter()
    {
        base.Enter();
    }

    // update state
    public override void Update()
    {
        base.Update();

        if(player.ControllerEnabled)
        {
            //get input
            //inputAttack = player.InputHandler.InputInteract;
            inputWait = player.InputHandler.InputWait;
        }
      
        
        if(!isExitingState)
        {
            if (player.ControllerEnabled)
            {
                // call wait to other player
                if (inputWait)
                {
                    player.InputHandler.SetWaitFalse();
                    if (player.Other.Waiting)
                    {
                        Debug.Log("Golem Following");
                        player.Other.ChangeState((player.Other as GolemControllerRB).AIFollowState);
                        //player.Other.Following = true;
                        //player.Other.Waiting = false;
                    }
                    else if (player.Other.Following)
                    {
                        Debug.Log("Golem Waiting");
                        player.Other.ChangeState((player.Other as GolemControllerRB).AIWaitState);
                        //player.Other.Following = false;
                    }

                    Debug.Log("Golem Following " + player.Other.Following.ToString());
                }

                // attack controls
                if (inputAttack && !player.CheckTouchingWall())
                {
                    // change to attack state
                    //player.InputHandler.SetInteractFalse();
                    player.ChangeState(player.AttackState);

                }
            }

            // auto climbing
            if (player.GetComponent<ClimbingController>().canClimb)
            {
                player.ChangeState(player.WallClimbLedgeState);
            }

            if (player.GetComponent<ClimbingController>().canJumpClimb)
            {
                player.ChangeState(player.JumpState);
            }

        }
        
      

        Perform();
        //Debug.Log(this.GetType().Name + " state updating by delta time");
    }
 
    // fixed update state
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //Debug.Log(this.GetType().Name + " state updating by fixed time");
    }

    // called on exiting state
    public override void Exit()
    {
        base.Exit();
        //Debug.Log(this.GetType().Name + " state exited");
    }
    
    // state function
    public override void Perform()
    {

    }

    public override bool AnimationComplete()
    {
        return base.AnimationComplete();
    }
}

