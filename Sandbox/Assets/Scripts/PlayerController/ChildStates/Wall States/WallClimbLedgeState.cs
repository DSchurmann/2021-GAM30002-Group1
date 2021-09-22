using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbLedgeState : AbilityState
{
    public WallClimbLedgeState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //HoldPosition(true, false);
        player.Anim.Play(animation);
        player.GetComponent<ClimbingController>().Climb();
        Debug.Log("ENTERED CLIMBING STATE");  
    }

    public override void Exit()
    {
        base.Exit();
        //holdPosition.x -= player.wallClimbDistance;
        //holdPosition.y = player.transform.position.y;
        //HoldPosition(true, true);
    }

    public override void Perform()
    {
        base.Perform();

        //player.GetComponent<ClimbingController>().Climb();
    }

    public override void Update()
    {
        base.Update();

        //HoldPosition(true, false);
        // apply climbing velocity based on input value
       /* player.SetVelocityY(player.wallClimbSpeed);

        if(!isWallClimbable && !isExitingState)
        {
            player.ChangeState(player.IdleState);
        }*/
       if(isAnimationComplete)
        {
            isAbilityFinished = true;
        }
    }
}
