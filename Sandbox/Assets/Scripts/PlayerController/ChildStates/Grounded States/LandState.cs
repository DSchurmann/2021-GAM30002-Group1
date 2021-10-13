using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandState : GroundedState
{
    public LandState(ChildControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("PLAYER LANDED");

        if (player.isGrounded)
        {
            // play landing sound
            player.GetComponent<AudioSource>().PlayOneShot(GameController.GH.GetComponent<AudioManager>().RandomLandSound());
            // reset jumps allowed
            player.JumpState.ResetJumpsAllowed();
        }
    }

    public override void Update()
    {
        base.Update();

        if (!isExitingState)
        {
            //if (player.ControllerEnabled)
            //{
               /* if (inputX != 0 )
                {
                    player.ChangeState(player.MoveState);

                }
                else*/ 
       

            if (inputX != 0)
            {
                if(player.ControllerEnabled)
                    player.ChangeState(player.MoveState);
                else
                    player.ChangeState(player.AIFollowState);
            }
            else
            {
                player.ChangeState(player.IdleState);
            }

           /* }
            else
            {
                if (player.Following)
                {
                    player.ChangeState(player.AIFollowState);

                    //Play Landing Sound
                    FMODUnity.RuntimeManager.PlayOneShot("event:/TestFolder/ChildLand", GameController.GH.childAudioPos);
                }
                else
                {
                    player.ChangeState(player.AIWaitState);

                    //Play Landing Sound
                    FMODUnity.RuntimeManager.PlayOneShot("event:/TestFolder/ChildLand", GameController.GH.childAudioPos);
                }
            }*/
        }
    }
}
