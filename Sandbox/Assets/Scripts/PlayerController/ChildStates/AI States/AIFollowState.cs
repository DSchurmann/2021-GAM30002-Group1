using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFollowState : AIState
{

    public AIFollowState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }

    // called when entering state
    public override void Enter()
    {
        base.Enter();
        player.Following = true;
        if (player.Waiting)
            player.Waiting = false;
        GameController.GH.UH.waiting = (false);
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
            if(!isGrounded)
            {
                player.ChangeState(player.InAirState);
            }
            // change state to wait if close
            if (Mathf.Abs(player.transform.position.x - player.Other.transform.position.x) <= player.closeDistance)
            {
                player.ChangeState(player.AIWaitState);
            }
            else
            {
                // follow other player
                FollowProcedure();
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

    public void FollowProcedure()
    {
        Debug.Log("FOLLOWING");
        //Get Our Position, Position of Golem
        Vector3 pos = player.transform.position;
        Vector3 targPos = player.Other.transform.position;
        targPos.z += 1f;

        // check if not touching ground
        //Check Distance
        if (Mathf.Abs((pos - targPos).x) > player.closeDistance)
        {
            //Move Towards
            Vector3 angle = (targPos - pos).normalized;

            //Set Mov
            if (angle.x > 0)
            {
                // flip player and change direction
                if(player.FacingDirection!= 1) {player.Flip();}
                player.SetVelocityX(player.MovementSpeed * angle.x);
                
            }
            else if (angle.x < 0)
            {
                // flip playe rand change direction
                if (player.FacingDirection != -1) { player.Flip(); }
                player.SetVelocityX(player.MovementSpeed * angle.x);
            }

            player.SetVelocityY(0);
        }

        // set y velocity, apply jumpInputMultipler for press-depended jump height
        player.SetVelocityY(player.CurrentVelocity.y * player.jumpInputMultiplier);
    }
}
