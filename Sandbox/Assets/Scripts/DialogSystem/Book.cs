using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Book : InteractableItem
{
    [SerializeField][TextArea] private List<string> content;
    private GameObject bookUI;
    private Text bookUIText;
    private Image closeInput;

    private Button next;
    private Button previous;

    private int page = 0;

    private void Awake()
    {
        SetBookUIObjects();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if(isDisplay)
        {
            //change whether mkb or controller inputs are shown for open book prompt
            if (UIHandler.controllerType == UIHandler.ControllerType.mkb)
            {
                image.gameObject.SetActive(false);
                closeInput.gameObject.SetActive(false);
            }
            else
            {
                image.sprite = GameObject.Find("UI").GetComponent<InputButtonMapping>().GetButton(InputButtonMapping.InputButton.Interact, UIHandler.controllerType);
                image.gameObject.SetActive(true);
            }
        }
        else if(IsOpen)
        {
            //change whether mkb or controller inputs are shown for close book prompt
            if (UIHandler.controllerType == UIHandler.ControllerType.mkb)
            {
                image.gameObject.SetActive(false);
                closeInput.gameObject.SetActive(false);
            }
            else
            {
                closeInput.sprite = GameObject.Find("UI").GetComponent<InputButtonMapping>().GetButton(InputButtonMapping.InputButton.Interact, UIHandler.controllerType);
                closeInput.gameObject.SetActive(true);
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
            SetBookUIObjects();
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
        if (UIHandler.controllerType == UIHandler.ControllerType.mkb)
        {
            image.gameObject.SetActive(false);
            closeInput.gameObject.SetActive(false);
        }
        else
        {
            image.sprite = GameObject.Find("UI").GetComponent<InputButtonMapping>().GetButton(InputButtonMapping.InputButton.Interact, UIHandler.controllerType);
            image.gameObject.SetActive(true);
        }

        //set and display text
        text.text = interactText;
        text.gameObject.SetActive(true);
    }

    public void NextPage()
    {
        if(isOpen)
        {
            //change to the next page
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
            //change to the previous page
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

    private void SetBookUIObjects()
    {
        //set UI component fields
        if (bookUI == null)
        {
            bookUI = GameObject.Find("UI/Book");
        }
        if (bookUIText == null)
        {
            bookUIText = GameObject.Find("UI/Book/BookContent").GetComponent<Text>();
        }
        if (next == null)
        {
            next = GameObject.Find("UI/Book/Next").GetComponent<Button>();
        }
        if (previous == null)
        {
            previous = GameObject.Find("UI/Book/Previous").GetComponent<Button>();
        }
        if (closeInput == null)
        {
            closeInput = GameObject.Find("UI/Book/CloseInput").GetComponent<Image>();
        }

        //add listerners to the buttons so they do the right thing
        next.onClick.AddListener(NextPage);
        previous.onClick.AddListener(PreviousPage);

    }
}