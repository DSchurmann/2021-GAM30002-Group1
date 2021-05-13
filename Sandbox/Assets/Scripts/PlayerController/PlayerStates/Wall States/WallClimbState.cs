using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbState : WallState
{
    public WallClimbState(PlayerControllerRB player, string animation) : base(player, animation)
    {
    }

    public override void Update()
    {
        base.Update();

        // apply climbing velocity based on input value
        player.SetVelocityY(player.wallClimbSpeed*inputY);

        // change to grab state if y velocy is zero
        if(inputY == 0 && !isExitingState)
        {
            player.ChangeState(player.WallGrabState);
        }
    }
}
