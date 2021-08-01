using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemGroundedState:GolemState
{
    protected int inputX;
    protected int inputY;
    protected bool isGrounded;
    protected bool isTouchingWall;


    public GolemGroundedState(GolemControllerRB player, string animation) : base(player, animation)
    {

    }

    // called when entering state
    public override void Enter()
    {
        base.Enter();
    }
    // called when exiting state
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!isExitingState)
        {
            // get input for x and y
            inputX = player.InputHandler.InputXNormal;
            inputY = player.InputHandler.InputYNormal;
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

