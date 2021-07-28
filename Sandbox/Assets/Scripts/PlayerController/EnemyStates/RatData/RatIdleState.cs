using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatIdleState : RatEnemyState
{
    public RatIdleState(RatControllerRB enemy, string animation) : base(enemy, animation)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.ChangeState(enemy.RatChargeState);

    }

    public override void Update()
    {
        base.Update();
    }

    public override void Perform()
    {
        base.Perform();
    }

   
}
