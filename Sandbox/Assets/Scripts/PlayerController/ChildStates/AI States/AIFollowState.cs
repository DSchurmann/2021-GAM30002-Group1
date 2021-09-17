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
        player.Anim.Play(animation);
        player.GetComponent<Rigidbody>().useGravity = true;
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
                Debug.Log("Child Close stop Following");

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
        //Debug.Log("FOLLOWING");
        //Get Our Position, Position of Golem
        Vector3 pos = player.transform.position;
        Vector3 targPos = player.Other.transform.position;
        targPos.z += 1f;

        // set move speed
        float followSpeed = player.Other.MovementSpeed;
        float dist = Mathf.Abs((pos - targPos).x);
        dist -= player.closeDistance;
        followSpeed += dist * player.followSpeedFactor;
        if (followSpeed > player.maxFollowSpeed)
        {
            followSpeed = player.maxFollowSpeed;
        }

        Debug.Log(followSpeed);

        if (player.GetComponent<ClimbingController>().isGapAhead)
        {
            Debug.Log("Stop");
        
            /*holdPosition.x = player.transform.position.x;
            HoldPosition(true, false);
            player.Anim.Play("Idle");*/
            player.Following = false;
            player.ChangeState(player.AIWaitState);
            GameController.GH.UH.waiting = (true);

        }
        else
        {
            //Move Towards Golem
            Vector3 angle = (targPos - pos).normalized;
            player.SetVelocityX(followSpeed * angle.x);
            //flip the player
            if ((player.FacingDirection != 1 && angle.x > 0) || (player.FacingDirection != -1 && angle.x < 0)) { player.Flip(); }

            ////set jump
            //if (isTouchingWall && player.CheckgWallClimbAbility())
            //{
            //    player.ChangeState(player.JumpState);
            //}

        }

        

        // set y velocity, apply jumpInputMultipler for press-depended jump height
        player.SetVelocityY(player.CurrentVelocity.y * player.jumpInputMultiplier);
    }
}
