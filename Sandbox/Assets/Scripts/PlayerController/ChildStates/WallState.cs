using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallState : ChildState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isWallClimbable;
    protected bool inputGrab;
    protected bool inputJump;
    protected int inputX;
    protected int inputY;

    protected Vector3 holdPosition;

    public WallState(ChildControllerRB player, string animation) : base(player, animation)
    {
    }



    public override void Enter()
    {
        base.Enter();
        holdPosition = player.transform.position;
        holdPosition.x += player.wallClimbDistance;
        holdPosition.y += player.wallClimbOffsetPosition;
        HoldPosition(true,true);
    }

    public override void Exit()
    {
        base.Exit();
        //holdPosition.x -= player.transform.position.x;
        //holdPosition.y -= player.transform.position.y;

        /*holdPosition.x -= player.wallClimbDistance;
        holdPosition.y -= player.wallClimbOffsetPosition;
        player.transform.position = holdPosition;*/
    }
    public override void Update()
    {
        base.Update();

        inputX = player.InputHandler.InputXNormal;
        inputY = player.InputHandler.InputYNormal;
        inputGrab = player.InputHandler.InputInteract;
        inputJump = player.InputHandler.InputJump;

        if(isGrounded && !inputGrab)
        {   // change to idle state if grounded
            player.ChangeState(player.IdleState);
        }
        else if(!isTouchingWall || (inputX != player.FacingDirection && !inputGrab)) 
        {
            // change to in air state if not grounded
            player.ChangeState(player.InAirState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Perform()
    {
        base.Perform();

        Debug.DrawLine(player.wallCheck.position, Vector3.right * player.FacingDirection * 1, Color.green);

        //check if grounded
        isGrounded = player.CheckIfGrounded();
        // check if touching wall
        isTouchingWall = player.CheckTouchingWall();
        // check if wall is climbable
        isWallClimbable = player.CheckgWallClimbAbility();
    }

  
    public override bool AnimationComplete()
    {
        return base.AnimationComplete();
    }

    public void HoldPosition(bool x, bool y)
    {
        Vector3 pos = player.transform.position;
       
        // set velocity for wall grab
        if(x)
        {
            pos.x = holdPosition.x;
            player.transform.position = pos;
            player.SetVelocityX(0);
        }
           
        if(y)
        {
            pos.y = holdPosition.y;
            player.transform.position = pos;
            player.SetVelocityY(0);
        }
    }
}
