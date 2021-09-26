using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class Tutorial : MonoBehaviour
{
    [SerializeField] private string mkbMessage;
    [Tooltip("This message will display when the controller is the input option, leave space for the control image")]
    [SerializeField] private string controllerMessage;
    [Tooltip("This is where the message will be shown")]
    [SerializeField] private Text text;
    [Tooltip("This is the UI image object")]
    [SerializeField] private Image controllerImage;
    [SerializeField] private Sprite dualshockInputSprite;
    [SerializeField] private Sprite xboxInputSprite;
    [SerializeField] private RailType triggerableBy;
    [Tooltip("If true, the tutorial will only ever show once, otherwise it will always show when triggered")]
    [SerializeField] private bool oneTime = false;
    private bool triggered = false;
    private bool displaying = false;
    private List<GameObject> inTrigger = new List<GameObject>();

    private void Update()
    {
        foreach (GameObject g in inTrigger)
        {
            PlayerControllerRB c = g.GetComponent<PlayerControllerRB>();
            switch (triggerableBy)
            {
                case RailType.Child:
                    if (c is ChildControllerRB)
                    {
                        if (displaying && (!c.ControllerEnabled || c.Other.Following))
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
                        if (displaying && (!c.ControllerEnabled || c.Other.Following))
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
                    else if (displaying && (!c.ControllerEnabled || c.Other.Following))
                    {
                        HideTutorial();
                    }
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
            if (oneTime)
            {
                triggered = true;
            }
        }
    }

    private void HideTutorial()
    {
        text.gameObject.SetActive(false);
        controllerImage.gameObject.SetActive(false);
        displaying = false;
    }
}
