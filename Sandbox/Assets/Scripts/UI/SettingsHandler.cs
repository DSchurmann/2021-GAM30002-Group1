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
    private UIMouseOver mo;
    private int value;
    private GameObject handle;
    private List<GameObject> resOptions = new List<GameObject>();
    [SerializeField] private Color defaultColour;
    [SerializeField] private Color selectedColour;
    [SerializeField] private Toggle fullscreen;

    private void Start()
    {
        resolutions = Screen.resolutions;
        fullscreen.isOn = Screen.fullScreen;
        InitilizeSettings();
    }

    private void Update()
    {
        if (settingsUI.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (!done) //make sure this is only run once when the menu is activated
            {
                //set the options for resolution dropdown
                if (resDropdown.options.Count == 0)
                {
                    resDropdown.ClearOptions();
                    List<string> res = new List<string>();
                    for (int i = 0; i < resolutions.Length; i++)
                    {
                        string option = resolutions[i].width + " x " + resolutions[i].height;
                        res.Add(option);
                    }
                    resDropdown.AddOptions(res);
                    //change initial value
                    mo = resDropdown.GetComponent<UIMouseOver>();
                }
                //get current resolution index then refesh value
                resDropdown.value = PlayerPrefs.GetInt("Resolution");
                resDropdown.RefreshShownValue();
                done = true;
                selection = 0;

                if (inputHandler.InputPause)
                {
                    inputHandler.SetPauseFalse();
                }

                if(inputHandler.InputMenuAccept)
                {
                    inputHandler.SetMenuAcceptFalse();
                }
            }

            TMP_Dropdown d = options[selection].GetComponentInChildren<TMP_Dropdown>();
            if (inputHandler.InputMenuDecline)
            {
                inputHandler.SetMenuDeclineFalse();
                if (d && d.IsExpanded)
                {
                    d.Hide();
                }
                else
                {
                    SaveAndExit();
                }
            }
            if (inputHandler.InputPause)
            {
                inputHandler.SetPauseFalse();
                if (d && d.IsExpanded)
                {
                    d.Hide();
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

                if(d && !d.IsExpanded)
                {
                    d.Show();
                    value = d.value;
                    handle = GameObject.Find("UI/SettingsMenu/SettingsPanel/Resolution/Dropdown/Dropdown List/Scrollbar");
                    resOptions = new List<GameObject>();
                    foreach(Transform t in GameObject.Find("UI/SettingsMenu/SettingsPanel/Resolution/Dropdown/Dropdown List/Viewport/Content").transform)
                    {
                        if(t.name != "Item")
                        {
                            resOptions.Add(t.gameObject);
                        }
                    }
                }
                else if(d && d.IsExpanded)
                {
                    d.Hide();
                    d.value = value;
                    d.RefreshShownValue();
                }
            }

            if(d && d.IsExpanded)
            {
                if(inputHandler.menuY > 0)
                {
                    inputHandler.SetMenuInputFalse();
                    value--;
                    if (value < 0)
                    {
                        value++;
                    }
                }
                else if(inputHandler.menuY < 0)
                {
                    inputHandler.SetMenuInputFalse();
                    value++;
                    if (value >= d.options.Count)
                    {
                        value = d.options.Count - 1;
                    }
                }

                if (!mo.MouseOver)
                {
                    if (handle)
                    {
                        handle.GetComponent<Scrollbar>().value = 1 - (value / ((float)d.options.Count - 1));
                    }
                    for (int i = 0; i < resOptions.Count; i++)
                    {
                        ColorBlock c = resOptions[value].GetComponent<Toggle>().colors;
                        if (i == value)
                        {
                            c.normalColor = selectedColour;
                            resOptions[value].GetComponent<Toggle>().colors = c;
                        }
                        else
                        {
                            c.normalColor = defaultColour;
                            resOptions[value].GetComponent<Toggle>().colors = c;
                        }
                    }
                }
            }
            else
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
            else if (g.GetComponentInChildren<TMP_Dropdown>()) //change resolution
            {
                Resolution r = resolutions[g.GetComponentInChildren<TMP_Dropdown>().value];
                Screen.SetResolution(r.width, r.height, Screen.fullScreen);
                PlayerPrefs.SetInt("Resolution", g.GetComponentInChildren<TMP_Dropdown>().value);
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
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (Screen.currentResolution.width == resolutions[i].width && Screen.currentResolution.height == resolutions[i].height)
                {
                    PlayerPrefs.SetInt("Resolution", i);
                    break;
                }
            }
        }
        else
        {
            Resolution r = resolutions[PlayerPrefs.GetInt("Resolution")];
            Screen.SetResolution(r.width, r.height, Screen.fullScreen);
        }
        PlayerPrefs.Save();
    }
}
