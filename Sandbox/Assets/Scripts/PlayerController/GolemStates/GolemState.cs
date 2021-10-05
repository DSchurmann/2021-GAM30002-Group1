using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GolemState: PlayerState
{
    protected new GolemControllerRB player;

    protected bool isPosing;
    protected bool isPoseLocked;
    protected bool inputPoseRaise;
    protected bool inputPoseStep;
    protected bool inputPoseC;
    protected bool inputPoseT;
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

        player.Anim.CrossFade(animation, player.PoseBlendTime);
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
            inputPoseT = player.InputHandler.InputEast;
            inputPoseC = player.InputHandler.InputSouth;
        }

        // player controls input

        // wait controls, tell other player to wait
        if (!isExitingState)
        {
            // enabled player controls
            if (player.ControllerEnabled)
            {
                if (inputWait && GameController.GH.IsFriend)
                {
                    player.InputHandler.SetWaitFalse();
                    if (player.Other.Waiting)
                    {
                        if(!(player.Other as ChildControllerRB).CheckGapAhead())
                        {
                            //Debug.Log("Child Following");
                            player.Other.ChangeState((player.Other as ChildControllerRB).AIFollowState);
                            player.Other.Following = true;
                            player.Other.Waiting = false;
                            GameController.GH.UH.waiting = (false);

                        }
                    }
                    else if (player.Other.Following)
                    {
                        //Debug.Log("Child Waiting");

                        player.Other.ChangeState((player.Other as ChildControllerRB).AIWaitState);
                        player.Other.Following = false;
                        GameController.GH.UH.waiting = (true);

                    }

                    //Debug.Log("Child Following " + player.Other.Following.ToString());
                }

                if(!isPoseLocked)
                {
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
                    if (inputPoseT)
                    {
                        player.ChangeState(player.TPoseAbility);
                        player.InputHandler.SetEastFalse();
                    }
                    if (inputPoseC)
                    {
                        player.ChangeState(player.CrouchAbility);
                        player.InputHandler.SetSouthFalse();
                    }
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

