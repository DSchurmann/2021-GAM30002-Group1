using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : ChildState
{
    protected int inputX;
    protected bool  inputJump;
    protected bool inputGrab;
    protected bool  isGrounded;
    protected bool isTouchingWall;

    public DeathState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }
    public override void Enter()
    {
        base.Enter();


    }

    public override void Exit()
    {
        base.Exit();
       
    }

    public override void Update()
    {
        base.Update();

        if (isAnimationComplete)
        {
            player.StartCoroutine(GameController.GH.LoadGame(1));
            
        }


    }

    public void HidePlayerByScale()
    {
        player.transform.GetChild(0).localScale = Vector3.zero;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
      
    }

    public override void Perform()
    {
        base.Perform();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckTouchingWall();
    }

}
