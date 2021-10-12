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
    [SerializeField] private Button quit;
    [SerializeField] private List<Button> options;
    private int selection = 0;
    [SerializeField] private Image cursor;

    [SerializeField] private PlayerInputHandler InputHandler;

    private float targetProgress = 0.0f;
    private float progress = 0.0f;
    private float Speed = 1f;

    private void Awake()
    {
        loadProgress.gameObject.SetActive(false);
        cursor.GetComponent<Transform>().localPosition = new Vector3(cursor.GetComponent<Transform>().localPosition.x, options[selection].GetComponent<Transform>().localPosition.y);
    }

    public void Play()
    {
        play.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        loadProgress.gameObject.SetActive(true);
        StartCoroutine(LoadAsyncOperation());
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Update()
    {
        progress = Mathf.Lerp(targetProgress, progress, Speed * Time.deltaTime);
        loadProgress.value = progress;

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
