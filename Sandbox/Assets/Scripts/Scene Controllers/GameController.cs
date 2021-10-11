using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(UH == null)
            UH = GameObject.Find("UI").GetComponent<UIHandler>();

        if (GamePaused)
        {
            GameObject.Find("UI").GetComponentInChildren<PauseMenu>().Resume();
        }


        ShowMouse(false);
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
