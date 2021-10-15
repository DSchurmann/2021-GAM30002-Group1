using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcherCol : MonoBehaviour
{


    //On Trigger Enter
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerControllerRB>() != null)
        {
            StartCoroutine(GameController.GH.UH.GetComponent<UI_FXController>().FadeInBlack(0.5f, 1, 1));
            StartCoroutine(SwitchSceneDelay(1.6f));
        }
    }

    public void SwitchScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public IEnumerator SwitchSceneDelay(float time)
    {
        yield return new WaitForSeconds(time);

        SwitchScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(GameController.GH.gameObject);
    }

    public void NextScene(int index)
    {

    }
}
