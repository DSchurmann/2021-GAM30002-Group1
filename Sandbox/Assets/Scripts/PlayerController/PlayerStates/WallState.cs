using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallState : State
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool inputGrab;
    protected bool inputJump;
    protected int inputX;
    protected int inputY;

    public WallState(PlayerControllerRB player, string animation) : base(player, animation)
    {
    }



    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        inputX = player.InputHandler.InputXNormal;
        inputY = player.InputHandler.InputYNormal;
        inputGrab = player.InputHandler.InputInteract;
        inputJump = player.InputHandler.InputJump;

        if(isGrounded && !inputGrab)
        {   // change to idle state if grounded
            player.ChangeState(player.IdleState);
        }
        else if(!isTouchingWall || (inputX != player.FacingDirection && !inputGrab)) 
        {
            // change to in air state if not grounded
            player.ChangeState(player.InAirState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Perform()
    {
        base.Perform();

        //check if grounded
        isGrounded = player.CheckIfGrounded();
        // check if touching wall
        isTouchingWall = player.CheckTouchingWall();
    }

  
    public override bool AnimationComplete()
    {
        return base.AnimationComplete();
    }
}
