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
        player.Anim.CrossFade(animation, 0.05f);
        if (player.ControllerEnabled)
        {

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

    public override void Exit()
    {
        base.Exit();
        player.GetComponent<Rigidbody>().useGravity = true;
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
                    if (player.GetComponent<ClimbingController>().groundAngle.x < player.GetComponent<ClimbingController>().maxSlopeAngle)
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
            else
            {
                if(player.GetComponent<ClimbingController>().groundAngle.x < player.GetComponent<ClimbingController>().maxSlopeAngle && isGrounded)
                {
                    player.GetComponent<Rigidbody>().useGravity = false;
                    //Debug.Log("HOLDING POSITION ON SLOPE");
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
    }

}
