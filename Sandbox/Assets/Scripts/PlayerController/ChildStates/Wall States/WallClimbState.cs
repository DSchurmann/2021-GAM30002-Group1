using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbState : WallState
{
    public WallClimbState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.Anim.CrossFade(animation, 0.06f);
        HoldPosition(true, false);
    }

    public override void Exit()
    {
        base.Exit();
        holdPosition.x -= player.wallClimbDistance;
        holdPosition.y = player.transform.position.y;
        HoldPosition(true, true);
    }

    public override void Update()
    {
        base.Update();

        HoldPosition(true, false);
        // check for distance to ledge ledge, apply climbing velocity based on input value

        if (!player.GetComponent<ClimbingController>().canJumpClimb)
        {
           

        }
        player.SetVelocityY(player.wallClimbSpeed * inputX * player.FacingDirection);

        if (!isWallClimbable && !isExitingState && !player.GetComponent<ClimbingController>().isClimbing)
        {
            player.ChangeState(player.WallSlideState);
            // change to grab state if y velocy is zero
            if (inputX == 0 && !isExitingState)
            {
                player.ChangeState(player.WallGrabState);
            }
        }
    }
}
