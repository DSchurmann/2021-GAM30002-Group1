using UnityEngine;
using UnityEngine.UI;
using System;

public class Book : InteractableItem
{
    [SerializeField][TextArea] private string content;
    [SerializeField] private GameObject bookUI;
    [SerializeField] private Text bookUIText;

    private void Update()
    {
        if(isDisplay)
        {
            switch (UIHandler.controllerType)
            {
                case UIHandler.ControllerType.mkb:
                    image.gameObject.SetActive(false);
                    break;
                case UIHandler.ControllerType.ds:
                    image.sprite = dsInteract;
                    image.gameObject.SetActive(true);
                    break;
                case UIHandler.ControllerType.xbox:
                    image.sprite = xboxInteract;
                    image.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        if(isOpen)
        {
            bookUIText.text = content;
            bookUI.SetActive(true);
        }
        else
        {
            bookUI.SetActive(false);
        }
    }

    public override void DisplayUI()
    {
        isDisplay = true;

        interactText = "Press [E] to read";
        switch(UIHandler.controllerType)
        {
            case UIHandler.ControllerType.mkb:
                image.gameObject.SetActive(false);
                break;
            case UIHandler.ControllerType.ds:
                image.sprite = dsInteract;
                image.gameObject.SetActive(true);
                break;
            case UIHandler.ControllerType.xbox:
                image.sprite = xboxInteract;
                image.gameObject.SetActive(true);
                break;
        }

        text.gameObject.SetActive(true);
        text.text = interactText;
    }
}