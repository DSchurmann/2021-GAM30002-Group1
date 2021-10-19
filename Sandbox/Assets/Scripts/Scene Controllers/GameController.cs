using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
    //Parameters -- Core
    public static GameController GH;
    public PlayerSave SaveSystem;
    public bool GamePaused;
    
    public ChildControllerRB childObj;
    public Vector3 childAudioPos;
    public GolemControllerRB golemObj;
    public Vector3 golemAudioPos;

    public bool IsFriend = true;

    // player variables
    // child scale
    protected Vector3 childScaleCopy;

    public UIHandler UH;
    public AudioMixer mixer;

    // checkpoint save
    private SerializablePlayerSave checkpoint = new SerializablePlayerSave();
    private bool saved;

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

        //uncomment to delete player prefs
        //PlayerPrefs.DeleteAll();

        //check for player prefs and set those that don't exist
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

    // Start is called before the first frame update
    void Start()
    {
        // save game, setting initial checkpoint
        if(CurrentPlayer()!=null)
            SaveGame();

        //Set UIHandler
        if(UH == null)
            UH = GameObject.Find("UI").GetComponent<UIHandler>();

        if (GamePaused)
        {
            GameObject.Find("UI").GetComponentInChildren<PauseMenu>().Resume();
        }

        ShowMouse(false);

        // save child object scale
        childScaleCopy = childObj.transform.GetChild(0).localScale;
        Debug.Log("SCALE: " + childScaleCopy);
    }

    // Update is called once per frame
    void Update()
    {
        // save first state
        if(!saved && CurrentPlayer() != null)
        {
            saved = true;
            SaveGame();
        }

        //Set Positions (As Audio Sources)
        if(childObj != null)
            childAudioPos = childObj.transform.position;
        if (golemObj != null)
            golemAudioPos = golemObj.transform.position;
    }

    public void ShowMouse(bool state)
    {
        if(state == true)
        {
            // show the mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // hide the mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
       
    }

    // Get current player
    public PlayerControllerRB CurrentPlayer()
    {
        if (golemObj != null && golemObj.ControllerEnabled) { return golemObj; }
        else
        if (childObj.ControllerEnabled) { return childObj; }
        else
        {
            //Debug.LogError("NO CURRENT PLAYER");
            return null;
        }
    }

    // save the game
    public void SaveGame()
    {
        checkpoint = SaveSystem.Save();
    }


    // load the game
    public IEnumerator LoadGame(float delay = 0)
    {
       
        yield return new WaitForSeconds(delay);
        // normalise child scale
        childObj.transform.GetChild(0).localScale = childScaleCopy;
        childObj.ChangeState(childObj.IdleState);
        SaveSystem.Load(checkpoint);
    }

    // quit the game
    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
