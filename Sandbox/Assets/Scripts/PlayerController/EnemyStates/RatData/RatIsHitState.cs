using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatIsHitState : RatEnemyState
{
    public RatIsHitState(RatControllerRB enemy, string animation) : base(enemy, animation)
    {

    }
   
    public override void Enter()
    {
        base.Enter();
        Debug.Log("ENEMY HIT");    
    }
  

    public override void Update()
    {
        base.Update();
    }

    public override void Perform()
    {
        base.Perform();
    }

    public override bool AnimationComplete()
    {
        return base.AnimationComplete();
    }

}
