using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Parameters -- Core
    public static GameController GH;
    public PlayerSave SaveSystem;
    
    public PlayerControllerRB childObj;
    public Vector3 childAudioPos;
    public PlayerControllerRB golemObj;
    public Vector3 golemAudioPos;

    public GameObject uiHandler;
    public UIHandler UH;

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

    }

    // Start is called before the first frame update
    void Start()
    {
        // save game, setting initial checkpoint
        if(CurrentPlayer()!=null)
            SaveGame();

        //Set UIHandler
        uiHandler = GameObject.Find("UI");
        UH = uiHandler.GetComponent<UIHandler>();
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
        childAudioPos = childObj.transform.position;
        golemAudioPos = golemObj.transform.position;
    }

    // Get current player
    public PlayerControllerRB CurrentPlayer()
    {
        if (golemObj.ControllerEnabled) { return golemObj; }
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
    public void LoadGame()
    {
        SaveSystem.Load(checkpoint);
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
