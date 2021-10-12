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

                    //Play Landing Sound
                    FMODUnity.RuntimeManager.PlayOneShot("event:/TestFolder/ChildLand", GameController.GH.childAudioPos);
                }
                else*/ 
            if (isAnimationComplete || player.isGrounded)
            {
                player.JumpState.ResetJumpsAllowed();
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
                       
                    //Play Landing Sound
                    //FMODUnity.RuntimeManager.PlayOneShot("event:/TestFolder/ChildLand", GameController.GH.childAudioPos);

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
