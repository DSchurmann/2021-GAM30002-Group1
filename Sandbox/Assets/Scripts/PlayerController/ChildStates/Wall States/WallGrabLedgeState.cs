using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrabLedgeState : WallState
{
    public WallGrabLedgeState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Enter()
    {
        base.Enter();
        HoldPosition(true, true);
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

        if(!isWallClimbable && !isExitingState)
        {
            player.ChangeState(player.IdleState);
        }
    }
}
