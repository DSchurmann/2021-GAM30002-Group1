using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAbilityState:GolemState
{
    protected bool isAbilityFinished;
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected int inputX;

    public GolemAbilityState(GolemControllerRB player, string animation) : base(player, animation)
    {

    }


    // called when entering state
    public override void Enter()
    {
        base.Enter();
        // reset check for if ability is finished
        isAbilityFinished = false;
        player.posing = true;
        isPosing = true;
        isPoseLocked = true;
        player.StartCoroutine(ResetPoseLock(player.PoseLockTime));

        if(!player.initialState)
            player.GetComponent<AudioSource>().PlayOneShot(GameController.GH.GetComponent<AudioManager>().RandomGolemPoseSound());

    }

    public override void Exit()
    {
        base.Exit();

        player.posing = false;
        isPosing = false;
    }

    public override void Update()
    {
        base.Update();

        inputX = player.InputHandler.InputXNormal;

        if(inputX != 0 && player.ControllerEnabled && !isPoseLocked)
        {
            player.ChangeState(player.MoveState);
        }

       /* if (isAbilityFinished)
        {
            player.ChangeState(player.IdleState);
        }*/
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    public IEnumerator ResetPoseLock(float lockTime)
    {
        yield return new WaitForSeconds(lockTime);

        isPoseLocked = false;
    }

    public override void Perform()
    {
        base.Perform();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckTouchingWall();
    }

  
}

