using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    //Parameters -- Core
    public static GameHandler GH;
    public List<Rune> runeDatabase = new List<Rune>(); //All Runes
    public Rune[] runes = new Rune[4]; //List of currently-equipped Runes
    public List<Sprite> runeIcons = new List<Sprite>(); //Inspector
    public List<Sprite> runeEnabledIcons = new List<Sprite>(); //Inspector
    public bool switchMode; //Is the Golem controlled?
    public bool waitMode; //Is Wait Mode on?
    public GameObject childObj;
    public GameObject golemObj;

    //Parameters -- UI
    //UI Objects to be set Here -- CODE
    public GameObject waitObj;
    public List<GameObject> runeObjects = new List<GameObject>(); //Inspector
    public GameObject runeUIParent; //Inspector

    //AWAKE: Set Singleton
    private void Awake()
    {
        //Set Singleton
        if (GH != null && GH != this)
            Destroy(this);
        else if (GH == null)
            GH = (this);

        //Don't Destroy
        DontDestroyOnLoad(this.gameObject);

        //Get the Child and the Golem, set them to variables
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("PControlled"))
        {
            //Is Child?
            if (i.GetComponent<ChildHandler>() != null && childObj == null)
            {
                childObj = (i);
            }
            else if (i.GetComponent<GolemHandler>() != null && golemObj == null)
            {
                golemObj = (i);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set base runes for prototyping purposes
        InitializeRunes();
    }

    // Update is called once per frame
    void Update()
    {
        //Do Shit about Updating the UI to display runes, wait mode, etc here
        //CODE

        /* //Controlling Golem?
         if (!switchMode)
         {
             //Get all UI Elements 
             Image[] images = runeUIParent.GetComponentsInChildren<Image>();

             //Go Through
             foreach (Image i in images)
             {
                 i.color = new Color(1f, 1f, 1f, .5f);
             }
         }
         else
         {
             //Get all UI Elements 
             Image[] images = runeUIParent.GetComponentsInChildren<Image>();

             //Go Through
             foreach (Image i in images)
             {
                 i.color = new Color(1f, 1f, 1f, 1f);
             }
         }*/

        //WAIT MODE
        /*waitObj.SetActive(waitMode);

        //Display Runes
        for (int i = 0; i < 4; i++)
        {
            if (runes[i] != null)
            {
                runeObjects[i].GetComponent<Image>().enabled = (true);
                if (runes[i] == golemObj.GetComponent<GolemHandler>().activeRune)
                    runeObjects[i].GetComponent<Image>().sprite = runeEnabledIcons[runes[i].runeIcon];
                else
                    runeObjects[i].GetComponent<Image>().sprite = runeIcons[runes[i].runeIcon];
            }
            else
            {
                runeObjects[i].GetComponent<Image>().enabled = (false);
            }
        }*/
    }

    //Initialize Rune List
    public void InitializeRunes()
    {
        //Make List
        runeDatabase.Add(new Rune("Step", 0, Rune.RuneType.blue, "Step"));
        runeDatabase.Add(new Rune("Rise", 1, Rune.RuneType.blue, "Raise"));
        runeDatabase.Add(new Rune("Smash", 2, Rune.RuneType.red, "Smash"));
        runeDatabase[2].runeRedType = Rune.RedRune.smash;
        runeDatabase.Add(new Rune("Lift", 3, Rune.RuneType.red, "Lift"));
        runeDatabase[3].runeRedType = Rune.RedRune.lift;
        runeDatabase.Add(new Rune("Tee", 4, Rune.RuneType.blue, "Tee"));
        runeDatabase.Add(new Rune("Leap", 5, Rune.RuneType.red, "Leap"));
        runeDatabase[5].runeRedType = Rune.RedRune.leap;
        runeDatabase.Add(new Rune("Throw", 6, Rune.RuneType.yellow, "Raise"));

        //Set Runes
        SetRunes();
    }

    //Set Base Rune List ((Called after all runes initialized))
    public void SetRunes()
    {
        //Set Runes here
        runes[0] = (runeDatabase[1]); //Rise Rune 
        runes[1] = (runeDatabase[0]); //Step Rune
        runes[2] = (runeDatabase[6]); //Smash (TBA)
        runes[3] = (runeDatabase[3]); //Lift
    }

    //Set Switch State (If true, Golem is in control)
    public void SetSwitch(bool set)
    {
        //Do Thing
        switchMode = set;

        //Debug
        Debug.Log("Setting SwitchMode to " + set.ToString());
    }
}
