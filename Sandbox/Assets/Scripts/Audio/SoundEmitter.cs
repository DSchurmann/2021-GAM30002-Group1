using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public void PlayOneShot(string soundtype)
    {
        // store pitch copy
        float pitchCopy = GetComponent<AudioSource>().pitch;

        // play requested sounds
        switch (soundtype)
        {
            case "golemStep":

                GetComponent<AudioSource>().pitch = (Random.Range(0.6f, 1f));
                GetComponent<AudioSource>().PlayOneShot(GameController.GH.GetComponent<AudioManager>().RandomGolemWalkSound());
                break;

            case "childStep":
                
                GetComponent<AudioSource>().pitch = (Random.Range(0.6f, 1f));
                GetComponent<AudioSource>().PlayOneShot(GameController.GH.GetComponent<AudioManager>().RandomChildStepSound(), GameController.GH.GetComponent<AudioManager>().childFootstepVolume);
               
                break;
        }

        //set pitch as saved copy
        GetComponent<AudioSource>().pitch = pitchCopy;

    }
}
