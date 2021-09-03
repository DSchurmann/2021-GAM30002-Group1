using UnityEngine;
using UnityEngine.UI;
using System;

public class Book : InteractableItem
{
    [SerializeField][TextArea] private string content;
    [SerializeField] private GameObject bookUI;
    [SerializeField] private Text bookUIText;

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
        interactText = "Press 'e' to read";

        text.gameObject.SetActive(true);
        text.text = interactText;
    }
}