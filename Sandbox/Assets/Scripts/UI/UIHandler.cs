using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

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

    public Image switchPrompt;
    public Image followPrompt;
    public Image golemNorth;
    public Image golemSouth;
    public Image golemEast;
    public Image golemWest;

    public GameObject gameplayUI;
    public InputButtonMapping bm;

    public static bool DisableUI = false;
    //used for Unity editor because static variables do no show up
    [SerializeField] public bool disableUI = false;

    public static ControllerType controllerType;

    private void Awake()
    {
        DisableUI = disableUI;
    }

    private void Start()
    {
        GameObject.Find("UI/Book").SetActive(false);
    }

    void Update()
    {
        // enable  & disble UI at runtime
        DisableUI = disableUI;

        //get which controller type is being used
        controllerType = GetInputType(controllerType);

        if (!DisableUI)
        {
            gameplayUI.SetActive(true);
            //Set State
            childMain = (GameController.GH.CurrentPlayer() == GameController.GH.childObj);
            waiting = GameController.GH.CurrentPlayer().GetComponent<PlayerControllerRB>().Other.Waiting;

            //change UI elements
            ChangeIcons(childMain, waiting, followPrompt, switchPrompt, golemNorth, golemSouth, golemEast, golemWest);

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
        else
        {
            //disable all UI
            gameplayUI.SetActive(false);
        }
    }

    private void ChangeIcons(bool child, bool wait, Image follow, Image swap, Image n, Image s, Image e, Image w)
    {
        if (child)
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
        if (wait)
            waitState.GetComponent<Image>().sprite = waitSprite;
        else
            waitState.GetComponent<Image>().sprite = followSprite;

        follow.sprite = bm.GetButton(InputButtonMapping.InputButton.Wait, controllerType);
        swap.sprite = bm.GetButton(InputButtonMapping.InputButton.Switch, controllerType);
        n.sprite = bm.GetButton(InputButtonMapping.InputButton.RuneN, controllerType);
        s.sprite = bm.GetButton(InputButtonMapping.InputButton.RuneS, controllerType);
        e.sprite = bm.GetButton(InputButtonMapping.InputButton.RuneE, controllerType);
        w.sprite = bm.GetButton(InputButtonMapping.InputButton.RuneW, controllerType);
    }

    private ControllerType GetInputType(ControllerType t)
    {
        ControllerType result = t;
        //get keyboard input
        if (Keyboard.current.anyKey.wasPressedThisFrame && controllerType != ControllerType.mkb)
        {
            result = ControllerType.mkb;
        }
        else
        {
            if(Gamepad.current != null)
            {
                //get all inputs from all controllers and check which button was pressed
                foreach (InputControl c in Gamepad.current.allControls)
                {
                    if (c.IsPressed())
                    {
                        //once button is found, check if the corrisponding gamepad is a DualShock or Xbox
                        if (Gamepad.current is DualShockGamepad)
                        {
                            result = ControllerType.ds;
                            break;
                        }
                        else
                        {
                            result = ControllerType.xbox;
                            break;
                        }
                    }
                }
            }
        }

        return result;
    }
    
    public enum ControllerType
    { 
        mkb,
        xbox,
        ds,
    }
}
