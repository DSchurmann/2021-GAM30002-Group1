using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : ChildState
{
    protected int inputX;
    protected bool  inputJump;
    protected bool  inputGrab;
    protected bool  inputAttack;
    protected bool  isGrounded;
    protected bool isTouchingWall;

    public GroundedState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }
    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetJumpsAllowed();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(!isExitingState)
        {
            // get input for x 
            inputX = player.InputHandler.InputXNormal;
            // get input for jump
            inputJump = player.InputHandler.InputJump;
            inputGrab = player.InputHandler.InputInteract;
            inputAttack = player.InputHandler.InputInteract;

            // get attack input
            if (inputAttack && !player.CheckTouchingWall())
            {
                // set interact false
                //player.InputHandler.SetInteractFalse();
                // change player to  state
                player.ChangeState(player.AttackState);

            } // get jump input
            else if (inputJump && player.JumpState.CanJump())
            {
                // set jump false
                player.InputHandler.SetJumpFalse();
                // change player to jump state
                player.ChangeState(player.JumpState);
            } // check in air state
            else if (!isGrounded)
            {
                // check for jump input time buff not grounded
                player.InAirState.StartJumpTimeBuff();
                // change to in air state if not grounded
                player.ChangeState(player.InAirState);
            }
            // check for wall grab input
            /* else if (isTouchingWall && inputGrab)
            {
                // change to wall grab state if grab wall
                player.ChangeState(player.WallGrabState);
            }*/
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
