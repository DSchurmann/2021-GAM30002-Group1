using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public PlayerInputHandler InputHandler { get; private set; }

    public static bool GameIsPaused = false;
    [SerializeField] private GameObject PauseMenuUI;
    [SerializeField] private int MenuSceneIndex = 0;

    
    
    private void Start()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        
        PauseMenuUI.SetActive(GameIsPaused);


      
        

       /* GetComponent<PlayerInput>().SwitchCurrentActionMap("Paused");
        GetComponent<PlayerInput>().actions.FindActionMap("Paused").FindAction("Unpause").performed += Unpause;
        GetComponent<PlayerInput>().currentActionMap.Enable();*/


        // set pause in Game Conrroller
        GameController.GH.GamePaused = GameIsPaused;
    }

    void Unpause(InputAction.CallbackContext ctx)
    {
        Debug.Log("UNPAUSE PRESSED");
    }

    // Update is called once per frame
    void Update()
    {
        if (InputHandler.InputPause)// TODO change with pause input
        {
            InputHandler.SetPauseFalse();
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
           
        }

        if (InputHandler.InputUnpause)// TODO change with pause input
        {
            Debug.Log("UNPAUSE");

        }
    }

    void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        GameController.GH.GamePaused = false;

        // enable controls
        GameController.GH.childObj.GetComponent<PlayerInput>().currentActionMap.Enable();
        GameController.GH.golemObj.GetComponent<PlayerInput>().currentActionMap.Enable();
    }
    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
        GameController.GH.GamePaused = true;

        // disable controls
        GameController.GH.childObj.GetComponent<PlayerInput>().currentActionMap.Disable();
        GameController.GH.golemObj.GetComponent<PlayerInput>().currentActionMap.Disable();
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
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
