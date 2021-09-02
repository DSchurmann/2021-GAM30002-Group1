using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (InputHandler.InputPause)// TODO change with pause input
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }
    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
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
