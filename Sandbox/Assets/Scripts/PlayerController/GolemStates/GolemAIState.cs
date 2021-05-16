using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAIState:GolemState
{
    protected bool isAbilityFinished;
    protected bool isGrounded;
    protected bool isTouchingWall;

    public GolemAIState(GolemControllerRB player, string animation) : base(player, animation)
    {

    }


    // called when entering state
    public override void Enter()
    {
        base.Enter();
        // reset check for if ability is finished
        isAbilityFinished = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (isAbilityFinished)
        {
            player.ChangeState(player.IdleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    public override void Perform()
    {
        base.Perform();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckTouchingWall();
    }

  
}

