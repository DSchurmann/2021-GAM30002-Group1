using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GolemState: PlayerState
{
    protected new GolemControllerRB player;

    protected bool isPosing;
    protected bool inputPoseRaise;
    protected bool inputPoseStep;
    protected bool inputWait;

    protected GolemState(GolemControllerRB player, string animation) : base(player, animation)
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

        // change to pose on input
        if(player.ControllerEnabled)
        {
            inputPoseRaise = player.InputHandler.InputNorth;
            inputPoseStep = player.InputHandler.InputWest;
            inputWait = player.InputHandler.InputWait;
        }

        // player controls input

        // wait controls, tell other player to wait
        if (!isExitingState)
        {
            // enabled player controls
            if (player.ControllerEnabled)
            {
                if (inputWait)
                {
                    player.InputHandler.SetWaitFalse();
                    if (player.Other.Waiting)
                    {
                        Debug.Log("Child Following");
                        player.Other.ChangeState((player.Other as ChildControllerRB).AIFollowState);
                        player.Other.Following = true;
                        player.Other.Waiting = false;
                    }
                    else if (player.Other.Following)
                    {
                        Debug.Log("Child Waiting");

                        player.Other.ChangeState((player.Other as ChildControllerRB).AIWaitState);
                        player.Other.Following = false;
                    }

                    Debug.Log("Child Following " + player.Other.Following.ToString());
                }


                // simple posing
                if (inputPoseRaise)
                {
                    player.ChangeState(player.RaiseAbility);
                    player.InputHandler.SetNorthFalse();
                }
                if (inputPoseStep)
                {
                    player.ChangeState(player.StepAbility);
                    player.InputHandler.SetWestFalse();
                }
                // if not posing, enable pose ability. Handle exiting poses in their states
                /* if(!isPosing)
                 {
                     if (inputPoseRaise)
                     {
                         player.ChangeState(player.RaiseAbility);
                         player.InputHandler.SetNorthFalse();
                     }
                     if (inputPoseStep)
                     {
                         player.ChangeState(player.StepAbility);
                         player.InputHandler.SetWestFalse();
                     }
                 }
                 else
                 {

                     if (inputPoseRaise)
                     {
                         player.ChangeState(player.IdleState);
                         player.InputHandler.SetNorthFalse();
                     }
                     if (inputPoseStep)
                     {
                         player.ChangeState(player.IdleState);
                         player.InputHandler.SetWestFalse();
                     }
                 }*/
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

