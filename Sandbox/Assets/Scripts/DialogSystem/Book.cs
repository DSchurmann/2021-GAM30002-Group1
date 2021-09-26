using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Book : InteractableItem
{
    [SerializeField][TextArea] private List<string> content;
    [SerializeField] private GameObject bookUI;
    [SerializeField] private Text bookUIText;
    [SerializeField] private Image closeInput;

    [SerializeField] private Button next;
    [SerializeField] private Button previous;

    private int page = 0;

    private void Start()
    {
        next.onClick.AddListener(NextPage);
        previous.onClick.AddListener(PreviousPage);
    }

    private void Update()
    {
        if(isDisplay)
        {
            //change whether mkb or controller inputs are shown for open book prompt
            switch (UIHandler.controllerType)
            {
                case UIHandler.ControllerType.mkb:
                    image.gameObject.SetActive(false);
                    closeInput.gameObject.SetActive(false);
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
        else
        {
            //change whether mkb or controller inputs are shown for close book prompt
            switch (UIHandler.controllerType)
            {
                case UIHandler.ControllerType.mkb:
                    closeInput.gameObject.SetActive(false);
                    break;
                case UIHandler.ControllerType.ds:
                    closeInput.sprite = dsInteract;
                    closeInput.gameObject.SetActive(true);
                    break;
                case UIHandler.ControllerType.xbox:
                    closeInput.sprite = xboxInteract;
                    closeInput.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        //open book, display contents and disable game UI
        if(isOpen)
        {
            HideUI();
            bookUIText.text = content[page];
            bookUI.SetActive(true);
            PageButtons();
            UIHandler.DisableUI = true;
        }
        else //disable book interface and show game UI again
        {
            bookUI.SetActive(false);
            UIHandler.DisableUI = false;
            page = 0;
        }
    }

    public override void DisplayUI()
    {
        isDisplay = true;

        //set text to appear above book and use controller button if required
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

        //set and display text
        text.text = interactText;
        text.gameObject.SetActive(true);
    }

    public void NextPage()
    {
        if(isOpen)
        {
            if (page < content.Count - 1)
            {
                page++;
                bookUIText.text = content[page];
                PageButtons();
            }
        }
    }

    public void PreviousPage()
    {
        if(isOpen)
        {
            if (page > 0)
            {
                page--;
                bookUIText.text = content[page];
                PageButtons();
            }
        }
    }

    private void PageButtons()
    {
        //display next page button
        if (page >= 0 && page < content.Count - 1)
        {
            next.gameObject.SetActive(true);
        }
        else //hide next page button
        {
            next.gameObject.SetActive(false);
        }
        //display previous page button
        if (page != 0 && page <= content.Count)
        {
            previous.gameObject.SetActive(true);
        }
        else //hide previous page button
        {
            previous.gameObject.SetActive(false);
        }
    }
}