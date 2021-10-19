using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathInstantState : DeathState
{


    public DeathInstantState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }
    public override void Enter()
    {
        base.Enter();

        if(player.alive)
        {
            player.StartCoroutine(GameController.GH.UH.GetComponent<UI_FXController>().FadeInBlack(0f, 1, 0.5f));
            player.alive = false;
            player.StartCoroutine(GameController.GH.LoadGame(1));
            HidePlayerByScale();
            //player.Anim.Play(animation);
        }
    }

    public override void Exit()
    {
        base.Exit();
       
    }

    public override void Update()
    {
        //base.Update();

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
