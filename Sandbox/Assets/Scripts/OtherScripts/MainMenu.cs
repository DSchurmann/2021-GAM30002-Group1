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

    private void Awake()
    {
        loadProgress.gameObject.SetActive(false);
    }

    public void Play()
    {
        play.gameObject.SetActive(false);
        loadProgress.gameObject.SetActive(true);
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(nextScene);

        while (gameLevel.progress < 1)
        {
            loadProgress.value = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
