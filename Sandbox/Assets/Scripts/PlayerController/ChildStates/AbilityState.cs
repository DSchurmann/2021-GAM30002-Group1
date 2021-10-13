using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState : ChildState
{
    protected bool isAbilityFinished;
    protected bool isGrounded;

    public AbilityState(ChildControllerRB player, string animation) : base(player, animation)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // reset check for if ability is finished
        isAbilityFinished = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        // when ability is finished change to grounded or in air state
        if (isAbilityFinished && !isExitingState)
        {
            if (isGrounded)
            {
                player.ChangeState(player.IdleState);
            }
            else
            {
                Debug.Log("CHILD IN AIR");
                if(!player.GetComponent<ClimbingController>().isClimbing)
                    player.ChangeState(player.InAirState);

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
        // check if player is grounded
        isGrounded = player.CheckIfGrounded();
    }
}
