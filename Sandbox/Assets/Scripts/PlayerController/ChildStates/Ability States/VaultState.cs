using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultState: AbilityState
{


    public VaultState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }


    public override void Enter()
    {
        base.Enter();

        //player.SetVelocityY(player.JumpSpeed);
        //isAbilityFinished = true;

        player.InAirState.SetJumping();
    }

    public override void Perform()
    {
        Debug.Log("Performing vault");
    }
}
