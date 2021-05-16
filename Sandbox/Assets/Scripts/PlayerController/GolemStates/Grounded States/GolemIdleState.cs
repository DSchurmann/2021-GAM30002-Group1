using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemIdleState:GolemGroundedState
{
    public GolemIdleState(GolemControllerRB player, string animation) : base(player, animation)
    {

    }


    // called when entering state
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(player.transform.position, player.Other.transform.position) > 3)
        {
            player.ChangeState(player.FollowState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    public override void Perform()
    {
        base.Perform();
    }

  
}

