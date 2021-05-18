using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState: ChildState
{
    protected bool isGrounded;
    protected bool isTouchingWall;

    public AIState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }

    // called when entering state
    public override void Enter()
    {
        base.Enter();

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
            //check for input
            if (player.ControllerEnabled)
            {
                player.ChangeState(player.IdleState);
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
