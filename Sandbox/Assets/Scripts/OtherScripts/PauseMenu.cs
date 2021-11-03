using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public PlayerInputHandler InputHandler { get; private set; }

    public static bool GameIsPaused = false;
    private bool done = false;
    [SerializeField] private List<Button> options;
    private int selection = 0;
    [SerializeField] private GameObject PauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private int MenuSceneIndex = 0;

    [SerializeField] private GameObject cursor;

    private void Start()
    {
        GameIsPaused = false;
        GameController.GH.GamePaused = GameIsPaused;

        InputHandler = GetComponent<PlayerInputHandler>();

        PauseMenuUI.SetActive(GameIsPaused);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameIsPaused)// TODO change with pause input
        {
            if (InputHandler.InputPause)
            {
                InputHandler.SetPauseFalse();
                Pause(true);
            } 
        }
        else if (GameIsPaused && PauseMenuUI.activeInHierarchy)
        {

            if (!done)
            {
                //disable multiple inputs between menus
                if (InputHandler.InputMenuAccept)
                {
                    InputHandler.SetMenuAcceptFalse();
                }
                if (InputHandler.InputMenuDecline)
                {
                    InputHandler.SetMenuDeclineFalse();
                }
                if (InputHandler.InputPause)
                {
                    InputHandler.SetPauseFalse();
                }
                done = !done;
                Vector2 pos = options[selection].GetComponent<Transform>().localPosition;
                Vector2 newpos = new Vector2(pos.x - (pos.x / 2) - 250, pos.y);
                cursor.GetComponent<Transform>().localPosition = newpos;
            }
            
            if(InputHandler.InputPause)
            {
                InputHandler.SetPauseFalse();
                Resume();
            }

            if (InputHandler.menuY < 0)
            {
                InputHandler.SetMenuInputFalse();
                selection++;
                if (selection >= options.Count)
                {
                    selection = 0;
                }
                Vector2 pos = options[selection].GetComponent<Transform>().localPosition;
                Vector2 newpos = new Vector2(pos.x - (pos.x / 2) - 250, pos.y);
                cursor.GetComponent<Transform>().localPosition = newpos;
            }
            else if (InputHandler.menuY > 0)
            {
                InputHandler.SetMenuInputFalse();
                selection--;
                if (selection < 0)
                {
                    selection = options.Count - 1;
                }

                Vector2 pos = options[selection].GetComponent<Transform>().localPosition;
                Vector2 newpos = new Vector2(pos.x - (pos.x / 2) - 250, pos.y);
                cursor.GetComponent<Transform>().localPosition = newpos;
            }

            if (InputHandler.InputMenuAccept)
            {
                InputHandler.SetMenuAcceptFalse();
                options[selection].onClick.Invoke();
                selection = 0;
            }

            if(InputHandler.InputMenuDecline)
            {
                InputHandler.SetMenuDeclineFalse();
                Resume();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        cursor.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        GameController.GH.GamePaused = false;
        GameController.GH.ShowMouse(false);
        // enable controls
        if (GameController.GH.childObj != null)
            GameController.GH.childObj.GetComponent<PlayerInput>().currentActionMap.Enable();

        if (GameController.GH.golemObj != null)
            GameController.GH.golemObj.GetComponent<PlayerInput>().currentActionMap.Enable();

        selection = 0;
    }
    public void Pause(bool showUI)
    {
        if (showUI)
        {
            PauseMenuUI.SetActive(true);
            cursor.SetActive(true);
        }
        Time.timeScale = 0.0f;
        GameIsPaused = true;
        //set cursor position
        Vector2 pos = options[selection].GetComponent<Transform>().localPosition;
        Vector2 newpos = new Vector2(pos.x - (pos.x / 2) - 250, pos.y);
        cursor.GetComponent<Transform>().localPosition = newpos;

        GameController.GH.GamePaused = true;
        GameController.GH.ShowMouse(true);
        // disable controls
        if (GameController.GH.childObj != null)
            GameController.GH.childObj.GetComponent<PlayerInput>().currentActionMap.Disable();
        
        if (GameController.GH.golemObj != null)
            GameController.GH.golemObj.GetComponent<PlayerInput>().currentActionMap.Disable();
    }

    public void Show()
    {
        PauseMenuUI.SetActive(true);
        done = false;
    }

    public void ResumeButton()
    {
        Resume();
    }

    public void MenuButton()
    {
        Debug.Log("Menu");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(MenuSceneIndex);
        Destroy(GameController.GH.gameObject);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SettingsButton()
    {
        PauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }
}
