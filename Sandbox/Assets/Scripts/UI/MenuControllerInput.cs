using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllerInput : MonoBehaviour
{
    [SerializeField] private List<GameObject> options;
    private int selection;

    [SerializeField] private PlayerInputHandler inputHandler;

    void Update()
    {
        //move selection based on player input
        if (inputHandler.menuY < 0)
        {
            inputHandler.SetMenuInputFalse();
            selection++;
            if (selection >= options.Count)
            {
                selection = options.Count - 1;
            }
        }
        else if (inputHandler.menuY > 0)
        {
            inputHandler.SetMenuInputFalse();
            selection--;
            if (selection < 0)
            {
                selection++;
            }
        }

        if (inputHandler.InputMenuAccept)
        {
            //if save and exit is selected, do so
            inputHandler.SetMenuAcceptFalse();
            Button b = options[selection].GetComponent<Button>();
            if (b)
            {
                b.onClick.Invoke();
            }
        }
    }
}
