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
        GameIsPaused = false;
        GameController.GH.GamePaused = GameIsPaused;


        InputHandler = GetComponent<PlayerInputHandler>();

        PauseMenuUI.SetActive(GameIsPaused);


    }
       

    public void Unpause(InputAction.CallbackContext ctx)
    {
        Debug.Log("UNPAUSE PRESSED");
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
        Resume();
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
                Pause(true);
            }
           
        }

        if (InputHandler.InputInteract)// TODO change with pause input
        {
            //Debug.Log("UNPAUSE");

        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        GameController.GH.GamePaused = false;
        GameController.GH.ShowMouse(false);

        // enable controls
        GameController.GH.childObj.GetComponent<PlayerInput>().currentActionMap.Enable();
        GameController.GH.golemObj.GetComponent<PlayerInput>().currentActionMap.Enable();
    }
    public void Pause(bool showUI)
    {
        if(showUI)
            PauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
        GameController.GH.GamePaused = true;
        GameController.GH.ShowMouse(true);
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
        Destroy(GameController.GH.gameObject);
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
