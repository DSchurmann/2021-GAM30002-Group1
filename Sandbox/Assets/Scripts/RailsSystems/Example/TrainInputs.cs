using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrainInputs : MonoBehaviour
{
    public Vector2 Movement { get; private set; }
    public bool Jump { get; private set; }
    public bool JustJump { get; private set; }

    //Get Movement
    public void GetMove(InputAction.CallbackContext ctx)
    {
        Movement = ctx.ReadValue<Vector2>();
    }

    //Get Jump
    public void GetJump(InputAction.CallbackContext ctx)
    {
        // Jump pressed
        if (ctx.started)
        {
            Jump = true;
            JustJump = true;
        }
        else
        {
            JustJump = false;
        }
        // jump released
        if (ctx.canceled)
        {
            Jump = true;
        }
    }
}

