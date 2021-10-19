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
            player.StartCoroutine(GameController.GH.UH.GetComponent<UI_FXController>().FadeInBlack(0.5f, 1, 1));
            player.alive = false;
            player.Anim.Play(animation);
            // play splash sound
            player.GetComponent<AudioSource>().PlayOneShot(GameController.GH.GetComponent<AudioManager>().PlayWaterSplash(1));
            player.StartCoroutine(GameController.GH.LoadGame(2));
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
