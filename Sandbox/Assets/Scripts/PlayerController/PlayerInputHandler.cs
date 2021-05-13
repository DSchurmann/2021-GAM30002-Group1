using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // movement direction input
    public Vector2 RawMovementInput { get; private set; }
    public int InputXNormal { get; private set; }
    public int InputYNormal { get; private set; }
    // abilities input
    public bool InputJump { get; private set; }
    public bool InputJumpStopped { get; private set; }
    public bool InputInteract { get; private set; }
    // input varialbles
    [SerializeField]
    private float inputHoldTime = 0.1f;
    private float jumpTimer;
    [SerializeField]
    private float interactDelay = 1f;
    private float interactTimer;

    private void Update()
    {
        CheckJumpDelay();
        //CheckInteractDelay();
    }

    //Get Movement Input
    public void GetMovementInput(InputAction.CallbackContext ctx)
    {
        RawMovementInput = ctx.ReadValue<Vector2>();
        InputXNormal = (int)(RawMovementInput * Vector2.right).normalized.x;
        InputYNormal = (int)(RawMovementInput * Vector2.up).normalized.y;
    }

    //Get Jump Input
    public void GetJumpInput(InputAction.CallbackContext ctx)
    {
        // Jump pressed
        if (ctx.started)
        {
            InputJump = true;
            InputJumpStopped = false;
            jumpTimer = Time.time;
        }
       // jump released
        if(ctx.canceled)
        {
            InputJumpStopped = true;
        }
    }

    // Set jump to false
    public void SetJumpFalse() => InputJump = false;
    // Check jump delay
    private void CheckJumpDelay()
    {
        if(Time.time >= jumpTimer + inputHoldTime)
        {
            InputJump = false;
        }
    }

    //Get Interact Input
    public void GetInteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            InputInteract = true;
        }

        if (ctx.canceled)
        {
            InputInteract = false;
        }

    }
    // Set interact to false
    //public void SetInteractFalse() => InputInteract = false;
    // Check interact delay
    private void CheckInteractDelay()
    {
        if (Time.time >= interactTimer + interactDelay)
        {
            InputInteract = false;
        }
    }

    //Get Wait Input
    public void GetWaitInput(InputAction.CallbackContext ctx)
    {

    }

    // Get Switch Input
    public void GetSwitchInput(InputAction.CallbackContext ctx)
    {

    }

    // Get Moveset Inputs
    public void GetNorthInput(InputAction.CallbackContext ctx)
    {

    }
    public void GetSouthInput(InputAction.CallbackContext ctx)
    {

    }
    public void GetEastRune(InputAction.CallbackContext ctx)
    {

    }
    public void GetWestInput(InputAction.CallbackContext ctx)
    { 

    }
}
