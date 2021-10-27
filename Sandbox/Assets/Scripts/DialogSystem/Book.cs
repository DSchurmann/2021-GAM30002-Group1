using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Book : InteractableItem
{
    [SerializeField][TextArea] private List<string> content;
    [SerializeField] private PlayerInputHandler inputHandler;

    private GameObject bookUI;
    private Text bookUIText;
    private TextMeshProUGUI closePrompt;

    [SerializeField] private TMP_SpriteAsset xInput;
    [SerializeField] private TMP_SpriteAsset dualshock;
    [SerializeField] private TMP_SpriteAsset keyboard;

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
        ChangeSpriteset();
        if(isDisplay)
        {
            //change whether mkb or controller inputs are shown for open book prompt
            interactText = "Press <sprite name=" + GameObject.Find("UI").GetComponent<InputButtonMapping>().GetButtonName(InputButtonMapping.InputButton.Interact, UIHandler.controllerType) + "> to read";
            text.text = interactText;
        }
        else if(IsOpen)
        {
            //change whether mkb or controller inputs are shown for close book prompt

            next.image.sprite = GameObject.Find("UI").GetComponent<InputButtonMapping>().GetButton(InputButtonMapping.InputButton.RuneE, UIHandler.controllerType);
            previous.image.sprite = GameObject.Find("UI").GetComponent<InputButtonMapping>().GetButton(InputButtonMapping.InputButton.RuneW, UIHandler.controllerType);

            closePrompt.text = "Press <sprite name=" + GameObject.Find("UI").GetComponent<InputButtonMapping>().GetButtonName(InputButtonMapping.InputButton.Interact, UIHandler.controllerType) + "> to close";

            if(inputHandler.InputXNormal < 0)
            {
                inputHandler.SetMenuInputFalse();
                PreviousPage();
            }
            else if(inputHandler.InputXNormal > 0)
            {
                inputHandler.SetMenuInputFalse();
                NextPage();
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
        interactText = "Press <sprite name=" + GameObject.Find("UI").GetComponent<InputButtonMapping>().GetButtonName(InputButtonMapping.InputButton.Interact, UIHandler.controllerType) + "> to read";
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
        if (closePrompt == null)
        {
            closePrompt = GameObject.Find("UI/Book/ClosePrompt").GetComponent<TextMeshProUGUI>();
        }

        //add listerners to the buttons so they do the right thing
        next.onClick.AddListener(NextPage);
        previous.onClick.AddListener(PreviousPage);

    }

    private void ChangeSpriteset()
    {
        if(UIHandler.controllerType == UIHandler.ControllerType.ds)
        {
            closePrompt.spriteAsset = dualshock;
            text.spriteAsset = dualshock;
        }
        else if(UIHandler.controllerType == UIHandler.ControllerType.mkb)
        {
            closePrompt.spriteAsset = keyboard;
            text.spriteAsset = keyboard;
        }
        else if(UIHandler.controllerType == UIHandler.ControllerType.xbox)
        {
            closePrompt.spriteAsset = xInput;
            text.spriteAsset = xInput;
        }
    }
}