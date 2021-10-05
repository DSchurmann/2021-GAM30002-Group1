using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputButtonMapping : MonoBehaviour
{
    //dictionary for mapping enum to InputAction
    private Dictionary<InputButton, InputAction> inputConversion = new Dictionary<InputButton, InputAction>();
    //dictionary mapping input actions to the string for the button
    private Dictionary<InputAction, string> inputActions = new Dictionary<InputAction, string>();

    //lists for mapping string for buttons to the sprite of the button
    [SerializeField] private List<InputMapping> xInputSprites;
    [SerializeField] private List<InputMapping> dsSprites;

    [SerializeField] private InputActionAsset input;

    void Start()
    {
        UpdateBindings();
    }

    public void UpdateBindings()
    {
        InputActionMap map = input.FindActionMap("Gameplay");
        foreach (InputAction a in map)
        {
            foreach (InputControl c in a.controls)
            {
                if (c.device is Gamepad)
                {
                    if (!inputActions.ContainsKey(a))
                    {
                        inputActions.Add(a, c.name);
                        foreach (InputButton b in (InputButton[])Enum.GetValues(typeof(InputButton)))
                        {
                            if (a.name == b.ToString() && !inputConversion.ContainsKey(b))
                            {
                                inputConversion.Add(b, a);
                            }
                        }
                    }
                }
            }
        }
    }

    public Sprite GetButton(InputButton b, UIHandler.ControllerType c)
    {
        Sprite result = null;
        string s;

        if (b == InputButton.DPad)
        {
            s = "dpad";
        }
        else
        {
            InputAction action = inputConversion[b];
            s = inputActions[action];
        }

        if (c == UIHandler.ControllerType.ds)
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
            result = inputActions[a];
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
