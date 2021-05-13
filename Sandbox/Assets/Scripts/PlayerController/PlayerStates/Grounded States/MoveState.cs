using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : GroundedState
{
    public MoveState(PlayerControllerRB player, string animation) : base(player, animation)
    {

    }

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

        //Debug.Log(isAnimationComplete);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Check for direction flip
        player.CheckForFlip(inputX);
        // Set player movement velocity
        player.SetVelocityX(player.MovementSpeed * inputX);
        // Set player to idle state if stop moving
        if(inputX == 0f && !isExitingState)
        {
            player.ChangeState(player.IdleState);
        }
    }

    public override void Perform()
    {
        base.Perform();
    }

  
}
