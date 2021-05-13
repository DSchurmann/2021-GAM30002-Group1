using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState: AbilityState
{
    protected int jumpsLeft;

    public JumpState(PlayerControllerRB player, string animation) : base(player, animation)
    {
        jumpsLeft = player.jumpsAllowed;
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityY(player.JumpSpeed);
        isAbilityFinished = true;
        jumpsLeft--;

        player.InAirState.SetJumping();
    }

    public bool CanJump()
    {
        if(jumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetJumpsAllowed() => jumpsLeft = player.jumpsAllowed;
    public void DecreaseJumpsLeft() => jumpsLeft--;
}
