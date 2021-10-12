using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    protected string interactText = "Press 'e' to interact";
    protected Text text;
    protected Sprite interactSprite;
    protected Image image;
    protected bool isOpen = false;
    protected bool isDisplay = false;

    protected virtual void Start()
    {
        image = gameObject.GetComponentInChildren<Image>();
        text = gameObject.GetComponentInChildren<Text>();

        image.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public virtual void Interact() { }

    public virtual void DisplayUI() { }

    public void HideUI()
    {
        //hide relevant text for object
        text.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        isDisplay = false;
    }

    public bool isTextActive
    {
        get { return text.gameObject.activeSelf; }
    }

    public bool IsOpen
    {
        get { return isOpen; }
    }
}
