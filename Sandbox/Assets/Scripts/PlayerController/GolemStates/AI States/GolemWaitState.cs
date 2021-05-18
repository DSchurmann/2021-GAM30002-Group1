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


        // change state to follow if too far
        if (Mathf.Abs(player.transform.position.x - player.Other.transform.position.x) > 3)
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

