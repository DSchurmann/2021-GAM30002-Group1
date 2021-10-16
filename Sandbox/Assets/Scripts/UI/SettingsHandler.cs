using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject previousMenu;
    [SerializeField] private GameObject cursor;
    private bool done = false;

    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private List<GameObject> options;
    private int selection = 0;

    Resolution[] resolutions;
    [SerializeField] private Dropdown resDropdown;
    [SerializeField] private Toggle fullscreen;

    private void Start()
    {
        resolutions = Screen.resolutions;
        fullscreen.isOn = Screen.fullScreen;
    }

    private void Update()
    {
        if (settingsUI.activeInHierarchy)
        {
            if(!done) //make sure this is only run once when the menu is activated
            {
                //set the options for resolution dropdown
                if (resDropdown.options.Count == 0)
                {
                    List<string> res = new List<string>();
                    int resI = 0;
                    for (int i = 0; i < resolutions.Length; i++)
                    {
                        string option = resolutions[i].width + " x " + resolutions[i].height;
                        res.Add(option);

                        //check for current resolution to use to set initial value
                        if (Screen.currentResolution.width == Screen.resolutions[i].width && Screen.currentResolution.height == Screen.resolutions[i].height)
                        {
                            resI = i;
                        }
                    }
                    resDropdown.AddOptions(res);
                    //change initial value
                    resDropdown.value = resI;
                }
                else
                {
                    //get current resolution index
                    resDropdown.value = PlayerPrefs.GetInt("Resolution");
                }
                //refresh shown value
                resDropdown.RefreshShownValue();
                done = true;
                selection = 0;

                if(inputHandler.InputPause)
                {
                    inputHandler.SetPauseFalse();
                }
            }

            if (inputHandler.InputMenuDecline)
            {
                inputHandler.SetMenuDeclineFalse();
                SaveAndExit();
            }
            if (inputHandler.InputPause)
            {
                inputHandler.SetPauseFalse();
                SaveAndExit();
            }

            SettingsSlider s = options[selection].GetComponent<SettingsSlider>();
            if (s)
            {
                s.GetSlider.value += inputHandler.menuX * 0.005f;
            }

            if(options[selection].GetComponentInChildren<Dropdown>())
            {

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

                //if option is a toggle change it
                if (options[selection].GetComponentInChildren<Toggle>())
                {
                    options[selection].GetComponentInChildren<Toggle>().isOn = !options[selection].GetComponentInChildren<Toggle>().isOn;
                }
            }

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
            //move cursor
            cursor.GetComponent<Transform>().localPosition = new Vector3(-525f, options[selection].GetComponent<Transform>().localPosition.y);
        }
        else
        {
            done = false;
        }
    }

    public void SaveAndExit()
    {
        selection = 0;
        foreach(GameObject g in options)
        {
            if (g.GetComponent<SettingsSlider>()) //save all sliders
            {
                g.GetComponent<SettingsSlider>().SaveValue();
            }
            else if (g.GetComponentInChildren<Toggle>()) //toggle fullscreen if required
            {
                //if fullscreen and settings aren't the same, change fullscreen and save player pref
                if (Screen.fullScreen != g.GetComponentInChildren<Toggle>().isOn)
                {
                    Screen.fullScreen = g.GetComponentInChildren<Toggle>().isOn;
                    //convert bool to int
                    int i;
                    if (Screen.fullScreen)
                    {
                        i = 1;
                    }
                    else
                    {
                        i = 0;
                    }
                    PlayerPrefs.SetInt("Fullscreen", i);
                }
            }
            else if(g.GetComponentInChildren<Dropdown>()) //change resolution
            {
                Resolution r = resolutions[g.GetComponentInChildren<Dropdown>().value];
                Screen.SetResolution(r.width, r.height, Screen.fullScreen);
                PlayerPrefs.SetInt("Resolution", g.GetComponentInChildren<Dropdown>().value);
            }
        }
        //save player prefs and drop down to pause menu
        PlayerPrefs.Save();
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            previousMenu.GetComponent<MainMenu>().Show();
        }
        else
        {
            previousMenu.GetComponentInParent<PauseMenu>().Show();
        }
        settingsUI.SetActive(false);
    }
}