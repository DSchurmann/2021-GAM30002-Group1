using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject child;
    [SerializeField] private GameObject golem;
    [SerializeField] private CameraControl cam;
    private GameObject currentCharacter;

    private bool toggle = true;

    private void Start()
    {
        child.GetComponent<Controller>().CanControl(true);
        golem.GetComponent<Controller>().CanControl(false);
        currentCharacter = child;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Toggle"))
        {
            SwapCharacters();
        }

        if(!toggle)
        {
            if(Input.GetButtonUp("Toggle"))
            {
                SwapCharacters();
            }
        }
    }

    private void SwapCharacters()
    {
        currentCharacter.GetComponent<Controller>().CanControl(false);

        if (currentCharacter == child)
        {
            currentCharacter = golem;
        }
        else
        {
            currentCharacter = child;
        }

        currentCharacter.GetComponent<Controller>().CanControl(true);
        cam.SetTarget(currentCharacter.transform);
    }

    public void ToggleControlType()
    {
        toggle = !toggle;
        if (currentCharacter != golem)
        {
            golem.GetComponent<GolemController>().SetToggleState(toggle);
        }
    }
}
