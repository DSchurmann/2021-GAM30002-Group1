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
        HoldPosition(true, false);
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
                    Debug.Log("Child Far start Following");
                    player.ChangeState(player.AIFollowState);
                }
            }
            else
            {
                if (player.GetComponent<ClimbingController>().groundAngle.x < player.GetComponent<ClimbingController>().maxSlopeAngle)
                {
                    player.GetComponent<Rigidbody>().useGravity = false;
                    Debug.Log("HOLDING POSITION ON SLOPE");
                    holdPosition.x = player.transform.position.x;
                    holdPosition.y = player.transform.position.y;
                    HoldPosition(true, true);
                }
                else
                {
                    player.GetComponent<Rigidbody>().useGravity = true;
                    HoldPosition(false, false);
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
