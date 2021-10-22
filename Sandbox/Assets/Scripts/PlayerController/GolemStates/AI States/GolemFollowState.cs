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

        player.Following = true;
        if (isPosing)
            isPosing = false;

        if (player.Waiting)
            player.Waiting = false;
        GameController.GH.UH.waiting = (false);
        Debug.Log("Golem Follow");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(isAnimationComplete)
        {
            // change state to wait if close
            //if (Mathf.Abs(player.transform.position.x - player.Other.transform.position.x) <= player.closeDistance)
            //{
            //    Debug.Log("Golem Close stop Following");
            //    player.ChangeState(player.AIWaitState);
            //}
            //else
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
       
    }

    public void FollowProcedure()
    {
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

        Vector3 diff = pos - targPos;

        diff.y = 0;

        if(Vector3.Distance(new Vector3(pos.x, 0f, pos.z), new Vector3(targPos.x, 0f, targPos.z)) > player.closeDistance)
        {
            //Move Towards Child
            //Vector3 angle = (targPos - pos).normalized;
            player.MoveTowardsTarget(targPos, followSpeed, player.Other.Train.rail, RailType.Golem);
        }
        else
        {
            player.ChangeState(player.AIWaitState);
        }
    } 
}

