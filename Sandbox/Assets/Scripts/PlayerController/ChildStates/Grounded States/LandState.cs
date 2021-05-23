using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandState : GroundedState
{
    public LandState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }
    public override void Update()
    {
        base.Update();

        if(!isExitingState)
        {
            if(player.ControllerEnabled)
            {
                if (inputX != 0)
                {
                    player.ChangeState(player.MoveState);
                }
                else if (isAnimationComplete)
                {
                    player.ChangeState(player.IdleState);
                }
            }
            else
            {
                if(player.Following)
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
