using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcherCol : SceneSwitcher
{
    //On Trigger Enter
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerControllerRB>() != null)
        {
            SwitchSceneWithFade(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
