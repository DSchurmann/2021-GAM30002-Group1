using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected PlayerControllerRB player;
    private string animation;
    protected bool isAnimationComplete;
    protected bool isExitingState;
    
    protected float startTime;

    protected bool inputInteract;

    public State(PlayerControllerRB player, string animation)
    {
        this.player = player;
        this.animation = animation;
    }


    // called when entering state
    public virtual void Enter()
    {
        // perform 
        Perform();
        // set animation on
        player.Anim.Play(animation);
        // set start time
        startTime = Time.time;
        isExitingState = false;
        //Debug.Log(animation);
        //Debug.Log(this.GetType().Name + " state entered");
    }

    // update state
    public virtual void Update()
    {
        isAnimationComplete = AnimationComplete();

        // get input for interact
      /*  inputInteract = player.InputHandler.InputInteract;

        // check for interact input while grounded
        if (inputInteract)
        {
            // set interact false
            player.InputHandler.SetInteractFalse();
            // change player to  state
            //player.ChangeState(player.AttackState);
        }*/
        //Debug.Log(this.GetType().Name + " state updating by delta time");
    }

    // fixed update state
    public virtual void FixedUpdate()
    {
        Perform();
        //Debug.Log(this.GetType().Name + " state updating by fixed time");
    }

    // called on exiting state
    public virtual void Exit()
    {
        isExitingState = true;
        //Debug.Log(this.GetType().Name + " state exited");
    }
    
    // state function
    public virtual void Perform()
    {

    }

    public virtual bool AnimationComplete()
    {
        return (player.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !player.Anim.IsInTransition(0));
    }
}

