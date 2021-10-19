using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
public class Tutorial : MonoBehaviour
{
    //message for mkb and controller is separate because the current setup requires the controller message to have a gap for the image
    [SerializeField] private string mkbMessage;
    [Tooltip("This message will display when the controller is the input option, leave space for the control image")]
    [SerializeField] private string controllerMessage;
    
    //UI components
    private TextMeshProUGUI controllerText;
    [SerializeField] private TMP_SpriteAsset xInput;
    [SerializeField] private TMP_SpriteAsset dualshock;

    [SerializeField] private InputButtonMapping.InputButton button;
    private InputButtonMapping ibm;

    //Type of triggerable collider
    [SerializeField] private RailType triggerableBy;

    [Tooltip("If true, the tutorial will only ever show once, otherwise it will always show when triggered")]
    [SerializeField] private bool oneTime = false;
    private bool triggered = false;
    private bool displaying = false;
    //List containing PlayerControllerRB objects currently in the trigger
    private List<GameObject> inTrigger = new List<GameObject>();

    private void Start()
    {
        controllerText = GameObject.Find("UI/Tutorial/text").GetComponent<TextMeshProUGUI>();
        ibm = GameObject.Find("UI").GetComponent<InputButtonMapping>();
    }

    private void Update()
    {
        //for each object in trigger check if it is the right character to show or hide the tutorial display
        foreach (GameObject g in inTrigger)
        {
            PlayerControllerRB c = g.GetComponent<PlayerControllerRB>();
            switch (triggerableBy)
            {
                case RailType.Child:
                    if (c is ChildControllerRB)
                    {
                        if (displaying && !c.ControllerEnabled)
                        {
                            HideTutorial();
                        }
                        else if (c.ControllerEnabled)
                        {
                            ShowTutorial();
                        }
                    }
                    break;
                case RailType.Golem:
                    if (c is GolemControllerRB)
                    {
                        if (displaying && !c.ControllerEnabled)
                        {
                            HideTutorial();
                        }
                        else if (c.ControllerEnabled)
                        {
                            ShowTutorial();
                        }
                    }
                    break;
                case RailType.Both:
                    if (c.ControllerEnabled || inTrigger.Count == 2)
                    {
                        ShowTutorial();
                    }
                    else if (displaying && !c.ControllerEnabled )
                    {
                        HideTutorial();
                    }
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if the object entering trigger has a PlayerControllerRB component, display tutorial if its the right character type and add to list of objects in trigger
        PlayerControllerRB c = other.GetComponent<PlayerControllerRB>();
        if (c != null)
        {
            if (triggerableBy == RailType.Child && c is ChildControllerRB)
            {
                ShowTutorial();
            }
            else if (triggerableBy == RailType.Golem && c is GolemControllerRB)
            {
                ShowTutorial();
            }
            else if (triggerableBy == RailType.Both && (c is ChildControllerRB || c is GolemControllerRB))
            {
                ShowTutorial();
            }
            if(!inTrigger.Contains(c.gameObject))
            {
                inTrigger.Add(c.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if the object entering trigger has a PlayerControllerRB component, hide tutorial if its the right character type and remove the object from the list
        PlayerControllerRB c = other.GetComponent<PlayerControllerRB>();
        if (c != null)
        {
            if (triggerableBy == RailType.Child && c is ChildControllerRB) //if only triggerable by child
            {
                HideTutorial();
            }
            else if (triggerableBy == RailType.Golem && c is GolemControllerRB)
            {
                HideTutorial();
            }
            else if(triggerableBy == RailType.Both && (c is ChildControllerRB || c is GolemControllerRB))
            {
                HideTutorial();
            }
            inTrigger.Remove(c.gameObject);
        }
    }

    private void ShowTutorial()
    {
        //if tutorial can be triggered (isn't a one time) show the relevant tutorial message (mkb or controller)
        if (!triggered)
        {
            switch (UIHandler.controllerType)
            {
                case UIHandler.ControllerType.mkb:
                    controllerText.text = mkbMessage;
                    controllerText.gameObject.SetActive(true);
                    break;
                default:
                    controllerText.gameObject.SetActive(true);
                    string spriteName = ibm.GetButtonName(button, UIHandler.controllerType);
                    //set sprite sheet for textmesh depending in controller type
                    if(UIHandler.controllerType == UIHandler.ControllerType.ds)
                    {
                        controllerText.spriteAsset = dualshock;
                    }
                    else if(UIHandler.controllerType == UIHandler.ControllerType.xbox)
                    {
                        controllerText.spriteAsset = xInput;
                    }
                    //prepare message to contain controller button
                    string message = String.Format(controllerMessage, "<sprite name=" + spriteName + ">");
                    controllerText.text = message;
                    break;
            }

            displaying = true;
            //if the tutorial is a one-time, change triggered and the tutorial will never display again... or until next playthrough or load or something
            if (oneTime)
            {
                triggered = true;
            }
        }
    }

    private void HideTutorial()
    {
        //hide everything
        controllerText.gameObject.SetActive(false);
        displaying = false;
    }
}
