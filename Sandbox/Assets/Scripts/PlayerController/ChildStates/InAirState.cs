using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState: ChildState
{
    private int inputX;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBehind;
    private bool inputJump;
    private bool inputJumpStopped;
    private bool jumpTimeBuff;
    private bool isJumping;
    private bool inputGrab;

    public InAirState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartJumpTimeBuff();
    }

    public override void Update()
    {
        base.Update();

        // check for controller or ai

        if(player.ControllerEnabled)
        {
            ControllerMode();
        }
        else
        {
            AIMode();
        }
       
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    public override void Exit()
    {
        base.Exit();
    }

    public void AIMode()
    {
        if (player.CheckIfGrounded() && player.CurrentVelocity.y < 0.1f)
        {
            // land if ground detected while in air
            player.ChangeState(player.LandState);
        }
    }

    public void ControllerMode()
    {
        // get x input 
        inputX = player.InputHandler.InputXNormal;
        inputJump = player.InputHandler.InputJump;
        inputJumpStopped = player.InputHandler.InputJumpStopped;
        inputGrab = player.InputHandler.InputInteract;

        // check if jump input released and shorten jump height
        CheckJumpReleased();

        // check for ground
        if (player.CheckIfGrounded() && player.CurrentVelocity.y < 0.1f)
        {
            // land if ground detected while in air
            player.ChangeState(player.LandState);
        }
        /* else if (inputJump && isTouchingWall)
         {
             player.InputHandler.SetJumpFalse();
             // check for jump while in air
             player.ChangeState(player.WallJumpState);
         }*/
        else if (inputJump && player.JumpState.CanJump())
        {
            player.InputHandler.SetJumpFalse();
            // check for jump while in air
            player.ChangeState(player.JumpState);
        }
        // change state to wall grab
        else if (inputGrab && (isTouchingWall || isTouchingWallBehind))
        {
            Debug.Log("Grab wall");
            player.ChangeState(player.WallGrabState);
        }
        //change state to wall slide if touching wall
        else if (isTouchingWall && inputX == player.FacingDirection && player.CurrentVelocity.y <= 0)
        {
            // check if touch wall in air
            player.ChangeState(player.WallSlideState);
        }
        else
        {
            // check for direction change
            player.CheckForFlip(inputX);
            // set in air movement
            float inAirMovementDampening = Mathf.Clamp(player.inAirMovementSpeed / 10, 0.1f, 10.0f);
            player.SetVelocityX(player.inAirMovementSpeed * inputX);
        }
    }


    public override void Perform()
    {
        base.Perform();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckTouchingWall();
        isTouchingWallBehind = player.CheckTouchingWallBehind();
        //Debug.Log("Grounded: " + isGrounded);
        // apply jump buff to allow jump for short time after a ledge fall
        if (jumpTimeBuff && Time.time > (startTime + player.jumpTimeBuff))
        {
            //Debug.Log("FALL-TO-JUMP TIME PASSED");
            jumpTimeBuff = false;
            player.JumpState.DecreaseJumpsLeft();
        }
    }

    // check if jump input is released and shorten jump
    private void CheckJumpReleased()
    {
        // check if jumping
        if (isJumping)
        {
            // check if jump released and reduce height with jump multiplier
            if (inputJumpStopped)
            {
                player.SetVelocityY(player.CurrentVelocity.y * player.jumpInputMultiplier);
                isJumping = false;
            }
            else if (player.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    public void StartJumpTimeBuff() => jumpTimeBuff = true;
    public void SetJumping() => isJumping = true;
}
