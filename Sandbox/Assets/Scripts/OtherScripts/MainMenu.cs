using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int nextScene;
    [SerializeField] private Slider loadProgress;
    [SerializeField] private Button play;
    [SerializeField] private Button settings;
    [SerializeField] private GameObject settingsUI;
    private bool inSettings = false;
    [SerializeField] private Button quit;
    [SerializeField] private List<Button> options;
    private int selection = 0;
    [SerializeField] private GameObject cursor;

    [SerializeField] private PlayerInputHandler InputHandler;

    private float targetProgress = 0.0f;
    private float progress = 0.0f;
    private float Speed = 1f;

    private void Awake()
    {
        loadProgress.gameObject.SetActive(false);
        cursor.GetComponent<Transform>().localPosition = new Vector3(-275, options[selection].GetComponent<Transform>().localPosition.y);
    }

    public void Play()
    {
        play.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        loadProgress.gameObject.SetActive(true);
        StartCoroutine(LoadAsyncOperation());
    }

    public void Settings()
    {
        play.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);

        inSettings = true;
        settingsUI.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Show()
    {
        play.gameObject.SetActive(true);
        settings.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);

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
        inSettings = false;
        cursor.GetComponent<Transform>().localPosition = new Vector3(-275, options[selection].GetComponent<Transform>().localPosition.y);
    }

    public void Update()
    {
        progress = Mathf.Lerp(targetProgress, progress, Speed * Time.deltaTime);
        loadProgress.value = progress;

        if (!inSettings)
        {
            if (InputHandler.menuY < 0)
            {
                InputHandler.SetMenuInputFalse();
                selection++;
                if (selection >= options.Count)
                {
                    selection = options.Count - 1;
                }
            }
            else if (InputHandler.menuY > 0)
            {
                InputHandler.SetMenuInputFalse();
                selection--;
                if (selection < 0)
                {
                    selection++;
                }
            }
            //move cursor
            cursor.GetComponent<Transform>().localPosition = new Vector3(-275, options[selection].GetComponent<Transform>().localPosition.y);

            if (InputHandler.InputMenuAccept)
            {
                InputHandler.SetMenuAcceptFalse();
                options[selection].onClick.Invoke();
                if (selection == 0)
                {
                    cursor.SetActive(false);
                }
            }
        }
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(nextScene);

        while (gameLevel.progress < 1)
        {
            targetProgress = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
