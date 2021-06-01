using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemMoveState:GolemGroundedState
{
    public GolemMoveState(GolemControllerRB player, string animation) : base(player, animation)
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

        // Check for direction flip
        player.CheckForFlip(inputX);
        // Set player movement velocity
        player.SetVelocityX(player.MovementSpeed * inputX, inputY);
        // Set player to idle state if stop moving
        if (inputX == 0f && !isExitingState)
        {
            player.ChangeState(player.IdleState);
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

