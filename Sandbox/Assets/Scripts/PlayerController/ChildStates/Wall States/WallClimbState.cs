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
        // apply climbing velocity based on input value
        player.SetVelocityY(player.wallClimbSpeed *inputY);

        if(!isWallClimbable && !isExitingState)
        {
            player.ChangeState(player.WallSlideState);
        }
        // change to grab state if y velocy is zero
        if(inputY == 0 && !isExitingState)
        {
            player.ChangeState(player.WallGrabState);
        }
    }
}
