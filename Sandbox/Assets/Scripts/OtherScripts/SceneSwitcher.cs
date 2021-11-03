using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public float fadeDelay = 0.5f;
    public float fadeTime = 1f;
    public float sceneSwitchDelay = 1.6f;

    public void SwitchSceneWithFade(int index)
    {
        StartCoroutine(GameController.GH.UH.GetComponent<UI_FXController>().FadeInBlack(fadeDelay, 1, fadeTime));
        StartCoroutine(SwitchSceneDelay(sceneSwitchDelay, index));
    }

    public void SwitchScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public IEnumerator SwitchSceneDelay(float time, int index)
    {
        yield return new WaitForSeconds(time);

        SwitchScene(index);
        Destroy(GameController.GH.gameObject);
    }
}
