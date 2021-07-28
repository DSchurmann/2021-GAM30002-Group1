using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatChargeState : RatEnemyState
{

    public float minimumDistance { get; set; }

    public RatChargeState(RatControllerRB enemy, string animation) : base(enemy, animation)
    {
    }

    public override void Enter()
    {
        base.Enter();

        minimumDistance = 2;
    }

  
    public override void Update()
    {
        base.Update();

        if (DistanceXZ(enemy.target) < minimumDistance)
            enemy.nav.isStopped = true;
        else
        {
            enemy.nav.isStopped = false;
            enemy.nav.SetDestination(enemy.target.transform.position);
        }
          
    }

    public override void Perform()
    {
        base.Perform();
    }

    private float DistanceXZ(Transform from)
    {
        Vector2 v1 = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
        Vector2 v2 = new Vector2(from.position.x, from.position.z);

        return Vector2.Distance(v1, v2);
    }
}
