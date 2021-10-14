using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogBreakState : AnimaticState
{


    public LogBreakState(ChildControllerRB player, string animation) : base(player, animation)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.GetComponent<ClimbingController>().isEnabled = false;
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
    }
}
