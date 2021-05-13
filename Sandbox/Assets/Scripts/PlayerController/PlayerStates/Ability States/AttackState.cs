using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState: AbilityState
{
    public AttackState(PlayerControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("ATTACK");
        
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
      
       
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Perform()
    {
        base.Perform();
        // set ability to finished when animation is complete;
        isAbilityFinished = isAnimationComplete ? true : false;
    }


}
