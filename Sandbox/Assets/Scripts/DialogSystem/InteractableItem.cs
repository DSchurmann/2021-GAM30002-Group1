using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    protected string interactText = "Press 'e' to interact";
    [SerializeField] protected Text text;
    [SerializeField] protected Sprite dsInteract;
    [SerializeField] protected Sprite xboxInteract;
    [SerializeField] protected Image image;
    protected bool isOpen = false;
    protected bool isDisplay = false;


    public virtual void Interact() { }

    public virtual void DisplayUI() { }

    public void HideUI()
    {
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
