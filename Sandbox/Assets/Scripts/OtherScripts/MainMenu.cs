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

    private float targetProgress = 0.0f;
    private float progress = 0.0f;
    private float Speed = 1f;

    private void Awake()
    {
        loadProgress.gameObject.SetActive(false);
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
