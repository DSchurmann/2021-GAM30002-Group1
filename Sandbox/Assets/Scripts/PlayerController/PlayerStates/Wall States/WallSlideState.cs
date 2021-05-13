using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideState : WallState
{
    public WallSlideState(PlayerControllerRB player, string animation) : base(player, animation)
    {

    }

    public override void Update()
    {
        base.Update();
        player.SetVelocityY(-player.wallSlideVelocity);
    }

}
