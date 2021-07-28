using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatDeathState : RatEnemyState
{


    public RatDeathState(RatControllerRB enemy, string animation) : base(enemy, animation)
    {

    }

    public override void Enter()
    {
        base.Enter();

        Object.Destroy(enemy);
        
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
