using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public void PlayOneShot(string soundtype)
    {
        // play requested sounds
        switch(soundtype)
        {
            case "golemStep":
                GetComponent<AudioSource>().PlayOneShot(GameController.GH.GetComponent<AudioManager>().RandomGolemWalkSound());
                
                break;

            case "childStep":
                GetComponent<AudioSource>().PlayOneShot(GameController.GH.GetComponent<AudioManager>().RandomChildStepSound());

                break;
        }

    }
}
