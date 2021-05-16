using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideState : WallState
{

    public WallSlideState(PlayerControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Update()
    {
        base.Update();

        inputGrab = player.InputHandler.InputInteract;

        // change to wall grab state if grab wall while sliding
        if (!isExitingState)
        {
            if (inputY > 0 || inputY < 0)
            {
                // change player to wall climb state if up input detected
                player.ChangeState(player.WallClimbState);
            }
            else if (inputGrab)
            {
                // change player to wall slide state if down input detected ot grab input is released
                player.ChangeState(player.WallGrabState);
            }
            else if (inputJump)
            {
                player.WallJumpState.GetJumpDirection(isTouchingWall);
                player.InputHandler.SetJumpFalse();
                // check for jump while in air
                player.ChangeState(player.WallJumpState);
            }
        }

        player.SetVelocityY(-player.wallSlideSpeed);
    }

}
