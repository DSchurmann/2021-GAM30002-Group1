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

        minimumDistance = 1.5f;
    }

  
    public override void Update()
    {
        base.Update();

        if(enemy.nav.isActiveAndEnabled)
        {
            enemy.nav.SetDestination(enemy.target.transform.position);
          
        }

        if (DistanceXZ(enemy.target) < minimumDistance)
            enemy.nav.speed = 0;
        else
        {
            enemy.nav.speed = enemy.MovementSpeed;

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
