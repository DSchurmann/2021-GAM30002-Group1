using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatEnemyState : EnemyState
{
    protected new RatControllerRB enemy;

    public RatEnemyState(RatControllerRB enemy, string animation) : base(enemy, animation)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        isActionFinished = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }


}
