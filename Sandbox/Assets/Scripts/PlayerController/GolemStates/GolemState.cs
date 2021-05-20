using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GolemState:State
{
    protected new GolemControllerRB player;

    protected bool isPosing;
    // controller input for poses
    protected bool inputPoseRaise;
    protected bool inputPoseStep;

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
        inputPoseRaise = player.InputHandler.InputNorth;
        inputPoseStep = player.InputHandler.InputWest;

        // player controled input
        if(player.ControllerEnabled)
        {
            if (inputPoseRaise)
            {
                player.ChangeState(player.RaiseAbility);
            }
            if (inputPoseStep)
            {
                player.ChangeState(player.StepAbility);
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
}

