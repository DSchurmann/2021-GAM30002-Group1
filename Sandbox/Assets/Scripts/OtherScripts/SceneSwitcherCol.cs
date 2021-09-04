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
            SwitchScene(SceneManager.GetActiveScene().buildIndex+1);
            Destroy(GameController.GH.gameObject);
        }
    }

    public void SwitchScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void NextScene(int index)
    {

    }
}
