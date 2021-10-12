using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemRaiseState : GolemAbilityState
{

    public GolemRaiseState(GolemControllerRB player, string animation) : base(player, animation)
    {

    }


    // called when entering state
    public override void Enter()
    {
        base.Enter();
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Golem/GolemPose", GameController.GH.golemAudioPos);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // if following
        if (player.Following)
        {
            // change state to follow if too far 
            if (Mathf.Abs(player.transform.position.x - player.Other.transform.position.x) > player.closeDistance)
            {
                player.ChangeState(player.AIFollowState);
            }
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    public override void Perform()
    {
        //base.Perform();
        // set ability to finished when animation is complete;
        //isAbilityFinished = isAnimationComplete ? true : false;
    }
}

