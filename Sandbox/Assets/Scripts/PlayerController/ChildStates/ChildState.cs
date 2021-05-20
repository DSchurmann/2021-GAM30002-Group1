using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChildState:State
{
    protected new ChildControllerRB player;
    protected bool switchedPlayer;
    protected bool inputAttack;

    protected ChildState(ChildControllerRB player, string animation) : base(player, animation)
    {
        this.player = player;
        this.animation = animation;
    }

    // called when entering state
    public override void Enter()
    {
        base.Enter();
    }

    // update state
    public override void Update()
    {
        base.Update();

        //get input
        inputAttack = player.InputHandler.InputInteract;

        if (inputAttack && !player.CheckTouchingWall())
        {
            // set interact false
            //player.InputHandler.SetInteractFalse();
            // change player to  state
            player.ChangeState(player.AttackState);

        }

        Perform();
        //Debug.Log(this.GetType().Name + " state updating by delta time");
    }

    // fixed update state
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //Debug.Log(this.GetType().Name + " state updating by fixed time");
    }

    // called on exiting state
    public override void Exit()
    {
        base.Exit();
        //Debug.Log(this.GetType().Name + " state exited");
    }
    
    // state function
    public override void Perform()
    {

    }
}

