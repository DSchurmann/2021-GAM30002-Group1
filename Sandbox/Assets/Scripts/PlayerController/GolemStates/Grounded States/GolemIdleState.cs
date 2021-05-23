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

        if(!isExitingState)
        {
            // movement input
            if (inputX != 0)
            {
                if (player.ControllerEnabled)
                    player.ChangeState(player.MoveState);
            }
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

