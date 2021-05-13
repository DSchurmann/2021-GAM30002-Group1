using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : AbilityState
{
    private int jumpDirection;

    public WallJumpState(PlayerControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetJumpsAllowed();
        player.SetVelocity(player.wallJumpSpeed, player.wallJumpAngle, jumpDirection);
        player.CheckForFlip(jumpDirection);
        player.JumpState.DecreaseJumpsLeft();
    }

    public override void Update()
    {
        base.Update();

        if(!isExitingState)
        {
            if (Time.time >= startTime + player.wallJumpTime)
            {
                isAbilityFinished = true;
            }
        }
    }

    public void GetJumpDirection(bool isTouchingWall)
    {
        if(isTouchingWall)
        {
            jumpDirection = -player.FacingDirection;
        }
        else
        {
            jumpDirection = player.FacingDirection;
        }
    }
}
