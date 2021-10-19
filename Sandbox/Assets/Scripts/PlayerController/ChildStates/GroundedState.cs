using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : ChildState
{
    protected int inputX;
    protected bool  inputJump;
    protected bool inputGrab;
    protected bool  isGrounded;
    protected bool isTouchingWall;

    public GroundedState(ChildControllerRB player, string animation) : base(player, animation)
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

        //player.HandlePlatformLanding();


        if (!isExitingState)
        {
            // check for breakable ground below
            if(GroundBelow() != null)
            {
                //Debug.Log("Ground Below: " + GroundBelow());
                if(GroundBelow().GetComponent<FallOnContact>() != null)
                {
                    player.ChangeState(player.FallState);
                }
            }

            // get input for x 
            if (player.ControllerEnabled && !player.GetComponent<ClimbingController>().isClimbing)
            {
                inputX = player.InputHandler.InputXNormal;
            }
            else
            {
                if(player.Following)
                {
                    inputX = 1;
                }
                else
                {
                    inputX = 0;
                }
            }
                

            // get input for jump
            inputJump = player.InputHandler.InputJump;
            inputGrab = player.InputHandler.InputInteract;


            // get jump input
            if (inputJump && player.JumpState.CanJump() && !player.interacting)
            {
                // set jump false
                player.InputHandler.SetJumpFalse();
                // change player to jump state
                player.ChangeState(player.JumpState);
               
                //Play Jump Sound
                //FMODUnity.RuntimeManager.PlayOneShot("event:/TestFolder/TestSound", GameController.GH.childAudioPos);


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

    public GameObject GroundBelow() => player.GetComponent<ClimbingController>().groundObject;

    public override void Perform()
    {
        base.Perform();

        isGrounded = player.isGrounded;
        isTouchingWall = player.CheckTouchingWall();
    }

  
}
