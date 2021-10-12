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
    [SerializeField] private List<Button> options;
    private int selection = 0;
    [SerializeField] private GameObject PauseMenuUI;
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

        if (GameIsPaused)
        {
            if (InputHandler.InputYNormal < 0)
            {
                InputHandler.SetMenuInputFalse();
                selection++;
                if (selection >= options.Count)
                {
                    selection = 0;
                }
                cursor.GetComponent<Transform>().localPosition = new Vector3(cursor.GetComponent<Transform>().localPosition.x, options[selection].GetComponent<Transform>().localPosition.y);
            }
            else if (InputHandler.InputYNormal > 0)
            {
                InputHandler.SetMenuInputFalse();
                selection--;
                if (selection < 0)
                {
                    selection = options.Count - 1;
                }

                cursor.GetComponent<Transform>().localPosition = new Vector3(cursor.GetComponent<Transform>().localPosition.x, options[selection].GetComponent<Transform>().localPosition.y);
            }

            if (InputHandler.InputMenuAccept)
            {
                InputHandler.SetMenuAcceptFalse();
                options[selection].onClick.Invoke();
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
        cursor.GetComponent<Transform>().localPosition = new Vector3(cursor.GetComponent<Transform>().localPosition.x, options[selection].GetComponent<Transform>().localPosition.y);
        GameController.GH.GamePaused = true;
        GameController.GH.ShowMouse(true);
        // disable controls
        if (GameController.GH.childObj != null)
            GameController.GH.childObj.GetComponent<PlayerInput>().currentActionMap.Disable();
        
        if (GameController.GH.golemObj != null)
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
