using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public void PlayOneShot(string path)
    {
        //Play the thing!
        FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
    }
}
