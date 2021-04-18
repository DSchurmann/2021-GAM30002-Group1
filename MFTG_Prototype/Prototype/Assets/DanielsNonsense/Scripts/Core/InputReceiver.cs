using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    //Receive Inputs, send them to their respective places
    public void SetSwitch(InputAction.CallbackContext ctx)
    {
        //On Press, Debug
        if (ctx.started && gameObject.scene.IsValid())
            GameHandler.GH.SetSwitch(true);
        //On Release, Debug
        else if (ctx.canceled && gameObject.scene.IsValid())
            GameHandler.GH.SetSwitch(false);
    }

    //Get Movement
    public void GetMove(InputAction.CallbackContext ctx)
    {
        //Switched?
        if (!GameHandler.GH.switchMode && gameObject.scene.IsValid())
            GameHandler.GH.childObj.GetComponent<ChildHandler>().GetMove(ctx.ReadValue<Vector2>());
        else
            GameHandler.GH.golemObj.GetComponent<GolemHandler>().GetMove(ctx.ReadValue<Vector2>());
    }

    //Get Jump
    public void GetJump(InputAction.CallbackContext ctx)
    {
        //On Press
        if (!GameHandler.GH.switchMode && gameObject.scene.IsValid())
            GameHandler.GH.childObj.GetComponent<ChildHandler>().GetJump();
    }

    //Get Crouch
    public void GetCrouch(InputAction.CallbackContext ctx)
    {
        //On Press
        if (!GameHandler.GH.switchMode && gameObject.scene.IsValid())
        {
            if(ctx.performed)
                GameHandler.GH.childObj.GetComponent<ChildHandler>().GetCrouch(true);
            else if(ctx.canceled)
            {
                if(GameHandler.GH.childObj.GetComponent<ChildHandler>()!= null)
                    GameHandler.GH.childObj.GetComponent<ChildHandler>().GetCrouch(false);
            }
               
        }       
           
    }

    //Get Wait Mode
    public void GetWait(InputAction.CallbackContext ctx)
    {
        //On Press
        if (ctx.started && gameObject.scene.IsValid())
        {
            //Set
            if (GameHandler.GH.waitMode)
                GameHandler.GH.waitMode = (false);
            else
                GameHandler.GH.waitMode = (true);

            Debug.Log(GameHandler.GH.waitMode.ToString());
        }
    }

    //Get Interact
    public void GetInteract(InputAction.CallbackContext ctx)
    {
        //Do Thing
        //On Press
        if (ctx.started && gameObject.scene.IsValid())
        {
            GameHandler.GH.childObj.GetComponent<ChildHandler>().Interact();
        }
    }

    //RUNES
    public void NorthRune(InputAction.CallbackContext ctx)
    {
        //On Press (when Golem Controlled)
        if (ctx.started && gameObject.scene.IsValid() && GameHandler.GH.switchMode)
        {
            //Do the Thing
            GameHandler.GH.golemObj.GetComponentInChildren<GolemHandler>().ActivateRune(0);
        }
    }
    public void WestRune(InputAction.CallbackContext ctx)
    {
        //On Press (when Golem Controlled)
        if (ctx.started && gameObject.scene.IsValid() && GameHandler.GH.switchMode)
        {
            //Do the Thing
            GameHandler.GH.golemObj.GetComponentInChildren<GolemHandler>().ActivateRune(1);
        }
    }
    public void EastRune(InputAction.CallbackContext ctx)
    {
        //On Press (when Golem Controlled)
        if (ctx.started && gameObject.scene.IsValid() && GameHandler.GH.switchMode)
        {
            //Do the Thing
            GameHandler.GH.golemObj.GetComponentInChildren<GolemHandler>().ActivateRune(2);
        }
    }
    public void SouthRune(InputAction.CallbackContext ctx)
    {
        //On Press (when Golem Controlled)
        if (ctx.started && gameObject.scene.IsValid() && GameHandler.GH.switchMode)
        {
            //Do the Thing
            GameHandler.GH.golemObj.GetComponentInChildren<GolemHandler>().ActivateRune(3);
        }
    }
}
