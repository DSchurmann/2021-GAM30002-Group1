using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemFollowState:GolemAIState
{
    public GolemFollowState(GolemControllerRB player, string animation) : base(player, animation)
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

        // follow other player
        FollowProcedure();

        // change state if close to stop following
        if (Vector2.Distance(player.transform.position, player.Other.transform.position) < 3)
        {
            player.ChangeState(player.AIWaitState);
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

    public void FollowProcedure()
    {
        //Get Our Position, Position of Golem
        Vector3 pos = player.transform.position;
        Vector3 targPos = player.Other.transform.position;
        targPos.z += 1f;
        //Check Distance
        if (Vector3.Distance(pos, targPos) > 3)
        {
            //Move Towards
            Vector3 angle = (targPos - pos).normalized;

            //Set Mov
            player.SetVelocity(player.MovementSpeed, angle, player.FacingDirection);
            player.SetVelocityY(0);
        }
    }


}

