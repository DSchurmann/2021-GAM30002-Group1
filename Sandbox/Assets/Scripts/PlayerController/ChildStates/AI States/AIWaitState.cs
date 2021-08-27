using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaitState: AIState
{

    public AIWaitState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }

    // called when entering state
    public override void Enter()
    {
        base.Enter();
        player.Waiting = true;
        GameController.GH.UH.waiting = (true);

        holdPosition.x = player.transform.position.x;
        holdPosition.x = player.transform.position.x;
        HoldPosition(true, true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!isExitingState)
        {
            if (player.Following)
            {
                // change state to follow if too far
                if (Mathf.Abs(player.transform.position.x - player.Other.transform.position.x) > player.closeDistance)
                {
               
                    player.ChangeState(player.AIFollowState);
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

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckTouchingWall();
    }
}
