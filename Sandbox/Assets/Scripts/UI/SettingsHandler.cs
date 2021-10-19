using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject previousMenu;
    [SerializeField] private GameObject cursor;
    private bool done = false;
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private List<GameObject> options;
    private int selection = 0;

    Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown resDropdown;
    private bool dropdownOpen = false;
    private int value;
    [SerializeField] private Toggle fullscreen;

    private void Start()
    {
        resolutions = Screen.resolutions;
        fullscreen.isOn = Screen.fullScreen;
        InitilizeSettings();
        fullscreen.interactable = false;
    }

    private void Update()
    {
        if (settingsUI.activeInHierarchy)
        {
            if (!done) //make sure this is only run once when the menu is activated
            {
                EventSystem.current.SetSelectedGameObject(null);
                //set the options for resolution dropdown
                if (resDropdown.options.Count == 0)
                {
                    resDropdown.ClearOptions();
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

                if (inputHandler.InputPause)
                {
                    inputHandler.SetPauseFalse();
                }
            }

            TMP_Dropdown d = options[selection].GetComponentInChildren<TMP_Dropdown>();
            if (inputHandler.InputMenuDecline)
            {
                inputHandler.SetMenuDeclineFalse();
                if (d && dropdownOpen)
                {
                    d.Hide();
                    dropdownOpen = false;
                }
                else
                {
                    SaveAndExit();
                }
            }
            if (inputHandler.InputPause)
            {
                inputHandler.SetPauseFalse();
                if (d && dropdownOpen)
                {
                    d.Hide();
                    dropdownOpen = false;
                }
                else
                {
                    SaveAndExit();
                }
            }

            SettingsSlider s = options[selection].GetComponent<SettingsSlider>();
            if (s)
            {
                s.GetSlider.value += inputHandler.menuX * 0.005f;
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

                if(d && !dropdownOpen)
                {
                    print("should be down");
                    dropdownOpen = true;
                    d.Show();
                    value = d.value;
                }
                else if(d && dropdownOpen)
                {
                    dropdownOpen = false;                    
                    d.Hide();
                    d.value = value;
                }
            }

            if (!dropdownOpen)
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
            }
            else
            {
                if(inputHandler.menuY < 0)
                {
                    inputHandler.SetMenuInputFalse();
                   value++;
                    if (value >= d.options.Count)
                    {
                        value = d.options.Count - 1;
                    }
                }
                else if(inputHandler.menuY < 0)
                {
                    inputHandler.SetMenuInputFalse();
                    value--;
                    if(value < 0)
                    {
                        value++;
                    }
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
        foreach (GameObject g in options)
        {
            if (g.GetComponent<SettingsSlider>()) //save all sliders
            {
                g.GetComponent<SettingsSlider>().SaveValue();
                g.GetComponent<SettingsSlider>().ChangeMixer(mixer);
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
            else if (g.GetComponentInChildren<Dropdown>()) //change resolution
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

    private void InitilizeSettings()
    {
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1);
        }
        else
        {
            mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume")) * 20);
        }

        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 1);
        }
        else
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
        }
        else
        {
            mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        }

        if (!PlayerPrefs.HasKey("Fullscreen"))
        {
            bool b = Screen.fullScreen;
            int i;
            if (b)
            {
                i = 1;
            }
            else
            {
                i = 0;
            }
            PlayerPrefs.SetInt("Fullscreen", i);
        }
        else
        {
            int i = PlayerPrefs.GetInt("Fullscreen");
            if (i == 1)
            {
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreen = false;
            }
        }
        if (!PlayerPrefs.HasKey("Resolution"))
        {
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                if (Screen.currentResolution.width == Screen.resolutions[i].width && Screen.currentResolution.height == Screen.resolutions[i].height)
                {
                    PlayerPrefs.SetInt("Resolution", i);
                    break;
                }
            }
        }
        else
        {
            Resolution r = Screen.resolutions[PlayerPrefs.GetInt("Resolution")];
            Screen.SetResolution(r.width, r.height, Screen.fullScreen);
        }
        PlayerPrefs.Save();
    }
}
