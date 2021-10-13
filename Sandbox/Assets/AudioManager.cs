using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Child Sounds")]
    
    public AudioClip[] JumpSounds;
    public AudioClip[] LandSounds;


    public bool canPlayLandSound = true;
    public float delaySound_land = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        canPlayLandSound = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // play a random jump sound from the list
    public AudioClip RandomJumpSound()
    {
        int index = Random.Range(0, JumpSounds.Length);
        AudioClip clip = JumpSounds[index];
        return clip;
    }

    // play a random landing sound from the list
    public AudioClip RandomLandSound()
    {
        if(canPlayLandSound)
        {
            int index = Random.Range(0, LandSounds.Length);
            AudioClip clip = LandSounds[index];
            return clip;
        }

        return null;
        
    }


    // enable land sound after a delay
    public IEnumerator EnableLandSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        canPlayLandSound = true;
    }
}
