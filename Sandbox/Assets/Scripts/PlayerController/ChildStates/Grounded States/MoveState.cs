using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : GroundedState
{
    public MoveState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.Anim.Play(animation);
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
            //Debug.Log(isAnimationComplete);
            if (player.isTouchingWall)
            {
                //player.ChangeState(player.IdleState);
            }

            if (player.ControllerEnabled)
            {
                Move(inputX);
            }
            else
            {

            }
        }
       
    }

    public override void Perform()
    {
        base.Perform();
    }

    public void Move(int input)
    {

        // Set player movement velocity
        if (player.GetComponent<ClimbingController>().groundAngle.x < player.GetComponent<ClimbingController>().maxSlopeAngle)
            player.SetVelocityX(player.MovementSpeed * input);

        if (!player.GetComponent<ClimbingController>().isClimbing)
        {
            // Check for direction flip
            player.CheckForFlip(input);

            // Set player to idle state if stop moving
            if (input == 0f && !isExitingState)
            {
                player.ChangeState(player.IdleState);
            }
        }
    }
}
