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
            if (Mathf.Abs(player.transform.position.x - player.Other.transform.position.x) <= player.closeDistance)
            {
                Debug.Log("Golem Close stop Following");
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
       
    }

    public void FollowProcedure()
    {
        //Get Our Position, Position of Golem
        Vector3 pos = player.transform.position;
        Vector3 targPos = player.Other.transform.position;
        targPos.z += 1f;
        //Check Distance
        if (Mathf.Abs((pos - targPos).x) > player.closeDistance)
        {
            //Move Towards
            Vector3 angle = (targPos - pos).normalized;

            //Set Mov
            if(angle.x >0)
            {
                player.SetVelocityX(player.MovementSpeed  * angle.x);
            }else if (angle.x < 0)
            {
                player.SetVelocityX(player.MovementSpeed * angle.x);
            }
                
            //player.SetVelocityY(0);
        }
    }


}

