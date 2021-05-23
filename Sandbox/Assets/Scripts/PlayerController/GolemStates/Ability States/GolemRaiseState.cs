using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemRaiseState : GolemAbilityState
{

    public GolemRaiseState(GolemControllerRB player, string animation) : base(player, animation)
    {

    }


    // called when entering state
    public override void Enter()
    {
        base.Enter();
        isPosing = true;
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
        //base.Perform();
        // set ability to finished when animation is complete;
        //isAbilityFinished = isAnimationComplete ? true : false;
    }
}

