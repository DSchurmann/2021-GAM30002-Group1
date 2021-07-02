using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    //Parameters -- GameObjects
    public GameObject childPort;
    public GameObject waitState;
    public GameObject golPort;

    //Parameters -- Assets
    public Sprite followSprite;
    public Sprite waitSprite;

    //Parameters -- Other
    public Color mainCol;
    public Color subCol;

    //Parameters -- State
    public bool childMain;
    public bool waiting;

    // Update is called once per frame
    void Update()
    {
        //Set State
        childMain = (GameController.GH.CurrentPlayer() == GameController.GH.childObj);

        //Based on State, Set Thing
        if (childMain)
        {
            //Child Colour = Full
            childPort.GetComponent<Image>().color = mainCol;

            //Golem Colour = Less Full
            golPort.GetComponent<Image>().color = subCol;
        }
        else
        {
            //Child Colour = Less Full
            childPort.GetComponent<Image>().color = subCol;

            //Golem Colour = Full
            golPort.GetComponent<Image>().color = mainCol;
        }

        //Set Wait Mode Indicator
        if (waiting)
            waitState.GetComponent<Image>().sprite = waitSprite;
        else
            waitState.GetComponent<Image>().sprite = followSprite;
    }
}
