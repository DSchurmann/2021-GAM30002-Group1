using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] protected string interactText = "Press 'e' to interact";
    [SerializeField] protected Text text;
    protected bool isOpen = false;


    public virtual void Interact() { }

    public virtual void DisplayUI() { }

    public void HideUI()
    {
        text.gameObject.SetActive(false);
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
