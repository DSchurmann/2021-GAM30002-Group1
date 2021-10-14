using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimaticState : ChildState
{
    protected bool isAnimaticFinished;
    protected bool isGrounded;

    public AnimaticState(ChildControllerRB player, string animation) : base(player, animation)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // reset check for if ability is finished
        isAnimaticFinished = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        // when ability is finished change to grounded or in air state
        if (isAnimaticFinished && !isExitingState)
        {
           
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Perform()
    {
        base.Perform();
        // check if player is grounded
        isGrounded = player.CheckIfGrounded();

        if(isAnimationComplete)
        {
            isAnimaticFinished = true;
        }
    }
}
