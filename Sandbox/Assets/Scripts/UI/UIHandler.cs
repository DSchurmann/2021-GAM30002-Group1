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

    public GameObject mkbUI;
    public GameObject xboxUI;
    public GameObject dsUI;

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
            //change UI based on controller type
            ChangeUI(controllerType);

            //Set State
            childMain = (GameController.GH.CurrentPlayer() == GameController.GH.childObj);
            waiting = GameController.GH.CurrentPlayer().GetComponent<PlayerControllerRB>().Other.Waiting;

            //change UI elements
            ChangeIcons(childMain, waiting, GetCurrentUI(controllerType));

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
            xboxUI.SetActive(false);
            mkbUI.SetActive(false);
            dsUI.SetActive(false);
        }
    }

    private void ChangeIcons(bool child, bool wait, GameObject ui)
    {
        if (child)
        {
            //Child Colour = Full
            ui.transform.Find("ChildPort").GetComponent<Image>().color = mainCol;

            //Golem Colour = Less Full
            ui.transform.Find("GolPort").GetComponent<Image>().color = subCol;
        }
        else
        {
            //Child Colour = Less Full
            ui.transform.Find("ChildPort").GetComponent<Image>().color = subCol;

            //Golem Colour = Full
            ui.transform.Find("GolPort").GetComponent<Image>().color = mainCol;
        }

        //Set Wait Mode Indicator
        if (wait)
            ui.transform.Find("WaitIndicator").GetComponent<Image>().sprite = waitSprite;
        else
            ui.transform.Find("WaitIndicator").GetComponent<Image>().sprite = followSprite;
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

    private void ChangeUI(ControllerType t)
    {
        //change displayed UI
        if(t == ControllerType.mkb && !mkbUI.activeSelf)
        {
            mkbUI.SetActive(true);
            dsUI.SetActive(false);
            xboxUI.SetActive(false);

        }
        else if(t == ControllerType.ds && !dsUI.activeSelf)
        {
            dsUI.SetActive(true);
            mkbUI.SetActive(false);
            xboxUI.SetActive(false);
        }
        else if(t == ControllerType.xbox && !xboxUI.activeSelf)
        {
            xboxUI.SetActive(true);
            mkbUI.SetActive(false);
            dsUI.SetActive(false);
        }
    }

    private GameObject GetCurrentUI(ControllerType c)
    {
        switch(c)
        {
            case ControllerType.mkb:
                return mkbUI;
            case ControllerType.ds:
                return dsUI;
            case ControllerType.xbox:
                return xboxUI;
            default:
                Debug.LogError("Something wrong getting current UI");
                return null;
        }
    }
    
    public enum ControllerType
    { 
        mkb,
        xbox,
        ds,
    }
}
