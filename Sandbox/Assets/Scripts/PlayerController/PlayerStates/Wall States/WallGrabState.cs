using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrabState : WallState
{
    private Vector3 holdPosition;

    public WallGrabState(PlayerControllerRB player, string animation) : base(player, animation)
    {

    }

    public override bool AnimationComplete()
    {
        return base.AnimationComplete();
    }

    public override void Enter()
    {
        base.Enter();
        holdPosition = player.transform.position;

        HoldPosition();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Perform()
    {
        base.Perform();
    }

    public override void Update()
    {
        base.Update();

        // call hold position to keep player held to wall
        HoldPosition();

        if(!isExitingState)
        {
            if (inputY > 0 || inputY < 0)
            {
                // change player to wall climb state if up input detected
                player.ChangeState(player.WallClimbState);
            }
            else if (!inputGrab)
            {
                // change player to wall slide state if down input detected ot grab input is released
                //player.ChangeState(player.WallSlideState);
            }
            else if (inputJump)
            {
                player.WallJumpState.GetJumpDirection(isTouchingWall);
                player.InputHandler.SetJumpFalse();
                // check for jump while in air
                player.ChangeState(player.WallJumpState);
            }
        }
    }
      

    private void HoldPosition()
    {
        player.transform.position = holdPosition;
        // set velocity for wall grab
        player.SetVelocityX(0);
        player.SetVelocityY(0);
    }
}
