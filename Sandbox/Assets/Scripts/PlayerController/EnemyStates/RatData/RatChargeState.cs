using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatChargeState : RatEnemyState
{
    public RatChargeState(RatControllerRB enemy, string animation) : base(enemy, animation)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

  
    public override void Update()
    {
        base.Update();

        enemy.nav.SetDestination(GameObject.Find("ChildObj").transform.position);
    }

    public override void Perform()
    {
        base.Perform();
    }

}
