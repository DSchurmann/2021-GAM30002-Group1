using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemWaitState:GolemAIState
{
    public GolemWaitState(GolemControllerRB player, string animation) : base(player, animation)
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


        // change state if close to stop following
        if (Vector2.Distance(player.transform.position, player.Other.transform.position) > 3)
        {
            player.ChangeState(player.AIFollowState);
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

