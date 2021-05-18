using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // Movement input
    public Vector2 RawMovementInput { get; private set; }
    public int InputXNormal { get; private set; }
    public int InputYNormal { get; private set; }
    // Jump input
    public bool InputJump { get; private set; }
    // Interact input
    public bool InputInteract { get; private set; }
    // Wait input
    public bool InputWait { get; private set; }
    // Switch player input
    public bool InputSwitch { get; private set; }
    // Ability A input
    public bool InputNorth{ get; private set; }
    // Ability B input
    public bool InputSouth { get; private set; }
    // Ability C input
    public bool InputEast { get; private set; }
    // Ability D input
    public bool InputWest { get; private set; }

    // input varialbles
    public bool InputInteractStopped { get; private set; }
    public bool InputJumpStopped { get; private set; }

    [SerializeField]
    private float inputDelay = 1f;
    [SerializeField]
    private float inputHoldTime = 0.1f;
    private float jumpTimer;
    private float interactTimer;
    private float switchTimer;
    private float interactDelayTimer;

    private void Update()
    {
        CheckJumpHold();
        //CheckInteractHold();
        //CheckSwitchHold();
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
    private void CheckJumpHold()
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
    public void SetInteractFalse() => InputInteract = false;
    
    // Check input hold
    private void CheckInteractHold()
    {
        if (Time.time >= interactTimer + inputHoldTime)
        {
            InputInteract = false;
        }
    }

    //Get Wait Input
    public void GetWaitInput(InputAction.CallbackContext ctx)
    {
        // Input pressed
        if (ctx.started)
        {
            InputWait = true;
        }
        // Input released
        if (ctx.canceled)
        {
            InputWait= false;
        }
    }

    // Get Switch Input
    public void GetSwitchInput(InputAction.CallbackContext ctx)
    {
        // Switch player pressed
        if (ctx.started)
        {
            InputSwitch = true;
        }
        // Switch player released
        if (ctx.canceled)
        {
            InputSwitch = false;
        }
    }
    // Set switch to false
    public void SetSwitchFalse() => InputSwitch = false;

    // Check input hold
    private void CheckSwitchHold()
    {
        if (Time.time >= interactTimer + inputHoldTime)
        {
            InputInteract = false;
        }
    }

    // Get Ability Inputs
    public void GetNorthInput(InputAction.CallbackContext ctx)
    {
        // Input pressed
        if (ctx.started)
        {
            InputNorth = true;
        }
        // Input released
        if (ctx.canceled)
        {
            InputNorth = false;
        }
    }
    public void GetSouthInput(InputAction.CallbackContext ctx)
    {
        // Input pressed
        if (ctx.started)
        {
            InputSouth = true;
        }
        // Input released
        if (ctx.canceled)
        {
            InputSouth = false;
        }
    }
    public void GetEastRune(InputAction.CallbackContext ctx)
    {
        // Input pressed
        if (ctx.started)
        {
            InputEast = true;
        }
        // Input released
        if (ctx.canceled)
        {
            InputEast = false;
        }
    }
    public void GetWestInput(InputAction.CallbackContext ctx)
    {
        // Input pressed
        if (ctx.started)
        {
            InputWest = true;
        }
        // Input released
        if (ctx.canceled)
        {
            InputWest = false;
        }
    }
}
