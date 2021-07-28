using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    protected EnemyControllerRB enemy;
    protected string animation;

    protected bool isAnimationComplete;
    protected bool isActionFinished;

    public EnemyState(EnemyControllerRB enemy, string animation)
    {
        this.enemy = enemy;
        this.animation = animation;
    }

    public override void Enter()
    {
        base.Enter();

        isActionFinished = false;
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }

    public virtual bool AnimationComplete()
    {
        return (enemy.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !enemy.Anim.IsInTransition(0));
    }


}
