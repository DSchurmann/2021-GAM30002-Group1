using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputButtonMapping : MonoBehaviour
{
    //dictionary for mapping enum to InputAction
    private Dictionary<InputButton, InputAction> inputConversion = new Dictionary<InputButton, InputAction>();
    //dictionary mapping input actions to the string for the button
    private Dictionary<InputAction, string> gamepadActions = new Dictionary<InputAction, string>();
    private Dictionary<InputAction, string> keyboardActions = new Dictionary<InputAction, string>();

    //lists for mapping string for buttons to the sprite of the button
    [SerializeField] private List<InputMapping> xInputSprites;
    [SerializeField] private List<InputMapping> dsSprites;
    [SerializeField] private List<InputMapping> keyboardSprites;

    [SerializeField] private InputActionAsset input;

    void Start()
    {
        UpdateBindings();
    }

    public void UpdateBindings()
    {
        InputActionMap map = input.FindActionMap("Gameplay");
        foreach (InputAction action in map)
        {
            foreach (InputControl control in action.controls)
            {
                if (control.device is Keyboard)
                {
                    if (!keyboardActions.ContainsKey(action))
                    {
                        keyboardActions.Add(action, control.name);
                    }
                }
                else if (control.device is Gamepad)
                {
                    if (!gamepadActions.ContainsKey(action))
                    {
                        gamepadActions.Add(action, control.name);
                    }
                }
 
                foreach (InputButton b in (InputButton[])Enum.GetValues(typeof(InputButton)))
                {
                    if (action.name == b.ToString() && !inputConversion.ContainsKey(b))
                    {
                        inputConversion.Add(b, action);
                        break;
                    }
                }      
            }
        }
    }

    public Sprite GetButton(InputButton b, UIHandler.ControllerType c)
    {
        Sprite result = null;
        string s;

        if (b == InputButton.DPad && c != UIHandler.ControllerType.mkb)
        {
            s = "dpad";
        }
        else
        {
            InputAction action = inputConversion[b];
            if (c == UIHandler.ControllerType.mkb)
            {
                s = keyboardActions[action];
            }
            else
            {
                s = gamepadActions[action];
            }
        }

        if(c == UIHandler.ControllerType.mkb)
        {
            foreach (InputMapping ib in keyboardSprites)
            {
                if (ib.name == s)
                {
                    result = ib.sprite;
                    break;
                }
            }
        }
        else if (c == UIHandler.ControllerType.ds)
        {
            foreach (InputMapping ib in dsSprites)
            {
                if (ib.name == s)
                {
                    result = ib.sprite;
                    break;
                }
            }
        }
        else if (c == UIHandler.ControllerType.xbox)
        {
            foreach (InputMapping ib in xInputSprites)
            {
                if (ib.name == s)
                {
                    result = ib.sprite;
                    break;
                }
            }
        }

        return result;
    }

    public string GetButtonName(InputButton b, UIHandler.ControllerType c)
    {
        string result = "";

        if(b == InputButton.DPad)
        {
            result = "dpad";
        }
        else
        {
            InputAction a = inputConversion[b];

            if (c == UIHandler.ControllerType.mkb)
            {
                result = keyboardActions[a];
            }
            else
            {
                result = gamepadActions[a];
            }
        }

        return result;
    }

    [Serializable]
    public struct InputMapping
    {
        public string name;
        public Sprite sprite;
    }

    public enum InputButton
    {
        Movement,
        Jump,
        RuneN,
        RuneS,
        RuneW,
        RuneE,
        Interact,
        Wait,
        Switch,
        Pause,
        DPad,
    }
}
