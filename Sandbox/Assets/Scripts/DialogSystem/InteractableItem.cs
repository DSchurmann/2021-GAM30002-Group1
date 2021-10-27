using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractableItem : MonoBehaviour
{
    protected string interactText = "Press 'e' to interact";
    protected TextMeshProUGUI text;
    protected bool isOpen = false;
    protected bool isDisplay = false;

    protected virtual void Start()
    {
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);
    }

    public virtual void Interact() { }

    public virtual void DisplayUI() { }

    public void HideUI()
    {
        //hide relevant text for object
        text.gameObject.SetActive(false);
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
