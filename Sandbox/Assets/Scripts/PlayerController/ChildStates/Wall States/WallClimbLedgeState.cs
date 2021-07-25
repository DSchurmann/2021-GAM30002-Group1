using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbLedgeState : WallState
{
    public WallClimbLedgeState(ChildControllerRB player, string animation) : base(player, animation)
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
        player.SetVelocityY(player.wallClimbSpeed);

        if(!isWallClimbable && !isExitingState)
        {
            player.ChangeState(player.IdleState);
        }
       
    }
}
