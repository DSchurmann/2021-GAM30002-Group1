using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AnimaticUIController : MonoBehaviour
{
    private GameObject cursor;
    private Button button;

    private float hideTimer = 0f;
    [SerializeField] private float hideTime;
    [SerializeField] private bool hide = true;
    [SerializeField] private bool hideOnStart = true;
    private bool canSkip = false;

    private int selection;

    [SerializeField] private PlayerInputHandler inputHandler;


    private void Start()
    {
        //get cursor and button
        cursor = transform.Find("Cursor").gameObject;
        button = transform.Find("Button_Skip").GetComponent<Button>();

        //hide cursor and button
        if(hideOnStart)
        {
            cursor.SetActive(false);
            button.gameObject.SetActive(false);
            //hide mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }

    private void Update()
    {
        if(hide)
        {
            //if controller/keyboard input, show skip and cursor
            if (CheckButtonPress())
            {
                //hide mouse
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                //show button and cursor
                button.gameObject.SetActive(true);
                cursor.SetActive(true);
                //start timer
                hideTimer = hideTime;
                canSkip = true;
            }
            //if mouse input, show mouse and button
            else if (Mouse.current.wasUpdatedThisFrame)
            {
                //show mouse
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //only show button
                cursor.SetActive(false);
                button.gameObject.SetActive(true);
                //start timer
                hideTimer = hideTime;
                canSkip = true;
            }
            //run timer
            if (hideTimer <= 0)
            {
                //hide mouse
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                //hide cursor and button
                button.gameObject.SetActive(false);
                cursor.SetActive(false);
                canSkip = false;
            }
            else
            {
                //decrement timer
                hideTimer -= Time.deltaTime;
            }

            //Skip if button pressed
            if (inputHandler.InputMenuAccept && canSkip)
            {
                inputHandler.SetMenuAcceptFalse();
                button.onClick.Invoke();
            }
        }
        else
        {
            //Skip if button pressed
            if (inputHandler.InputMenuAccept)
            {
                inputHandler.SetMenuAcceptFalse();
                button.onClick.Invoke();
            }
        }
    }

    private bool CheckButtonPress()
    {
        bool result = false;

        //check if any keyboard or controller buttons were pressed
        if(Keyboard.current.wasUpdatedThisFrame)
        {
            result = true;
        }
        if (Gamepad.current != null)
        {
            //get all inputs from all controllers and if a button was pressed
            foreach (InputControl c in Gamepad.current.allControls)
            {
                if (c.IsPressed())
                {
                    result = true;
                    break;
                }
            }
        }

        return result;
    }
}