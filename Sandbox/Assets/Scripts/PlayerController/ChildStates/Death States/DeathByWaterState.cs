using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathByWaterState : DeathState
{


    public DeathByWaterState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }
    public override void Enter()
    {
        base.Enter();

        if(player.alive)
        {
            player.StartCoroutine(GameController.GH.UH.GetComponent<UI_FXController>().FadeInBlack(1f, 1, 1));
            player.alive = false;
            player.Anim.Play(animation);
        }
    }

    public override void Exit()
    {
        base.Exit();
       
    }

    public override void Update()
    {
        base.Update();

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
