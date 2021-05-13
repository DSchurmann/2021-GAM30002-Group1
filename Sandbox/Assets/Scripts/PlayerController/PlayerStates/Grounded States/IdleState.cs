using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState: GroundedState
{
    public IdleState(PlayerControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if(inputX != 0 && !isExitingState)
        {
            player.ChangeState(player.MoveState);
        }
    }

    public override void Perform()
    {
        base.Perform();
    }

    public override void Update()
    {
        base.Update();
    }
}
