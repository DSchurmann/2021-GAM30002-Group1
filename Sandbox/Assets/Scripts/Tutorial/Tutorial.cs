using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class Tutorial : MonoBehaviour
{
    //message for mkb and controller is separate because the current setup requires the controller message to have a gap for the image
    [SerializeField] private string mkbMessage;
    [Tooltip("This message will display when the controller is the input option, leave space for the control image")]
    [SerializeField] private string controllerMessage;
    
    //UI components
    //[Tooltip("This is where the message will be shown")]
    private Text text;
    private Image controllerImage;

    [Tooltip("This is the UI image object")]
    [SerializeField] private Sprite dualshockInputSprite;
    [SerializeField] private Sprite xboxInputSprite;

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
        text = GameObject.Find("UI/Tutorial/Text").GetComponent<Text>();
        controllerImage = GameObject.Find("UI/Tutorial/ControllerInput").GetComponent<Image>();
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
            if (triggerableBy == RailType.Child && c is ChildControllerRB) //if only triggerable by child and child is 
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
                    text.text = mkbMessage;
                    text.gameObject.SetActive(true);
                    controllerImage.gameObject.SetActive(false);
                    break;
                case UIHandler.ControllerType.ds:
                    text.text = controllerMessage;
                    text.gameObject.SetActive(true);
                    controllerImage.sprite = dualshockInputSprite;
                    controllerImage.gameObject.SetActive(true);
                    break;
                case UIHandler.ControllerType.xbox:
                    text.text = controllerMessage;
                    text.gameObject.SetActive(true);
                    controllerImage.sprite = xboxInputSprite;
                    controllerImage.gameObject.SetActive(true);
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
        text.gameObject.SetActive(false);
        controllerImage.gameObject.SetActive(false);
        displaying = false;
    }
}
