using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState: GroundedState
{
    public IdleState(ChildControllerRB player, string animation) : base(player, animation)
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
  
    public override void Update()
    {
        base.Update();

        if(!isExitingState)
        {
            //movement input
            if (inputX != 0)
            {
                if (player.ControllerEnabled)
                {
                    player.ChangeState(player.MoveState);
                }
                else
                {
                    if (player.Following)
                    {
                        player.ChangeState(player.AIFollowState);
                    }
                    else
                    {
                        player.ChangeState(player.AIWaitState);

                    }
                }
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
