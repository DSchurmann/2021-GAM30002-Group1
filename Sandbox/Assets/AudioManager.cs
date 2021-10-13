using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Child Sounds")]
    public AudioClip[] ChildJumpSounds;
    public AudioClip[] ChildLandSounds;
    public AudioClip[] ChildStepSounds;
    [Header("Child Sound Properties")]
    public bool canPlayLandSound = true;
    public float delaySound_land = 0.25f;

    [Header("Golem Sounds")]
    public AudioClip[] GolemPoseSounds;
    public AudioClip[] GolemStepSounds;
    //[Header("Golem Sound Properties")]
    //public bool canPlayLandSound = true;
    //public float delaySound_land = 0.25f;

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
        int index = Random.Range(0, ChildJumpSounds.Length);
        AudioClip clip = ChildJumpSounds[index];
        return clip;
    }

    // play a random landing sound from the list
    public AudioClip RandomLandSound()
    {
        if(canPlayLandSound)
        {
            int index = Random.Range(0, ChildLandSounds.Length);
            AudioClip clip = ChildLandSounds[index];
            return clip;
        }

        return null;
    }

    // play a random Child Step sound from the list
    public AudioClip RandomChildStepSound()
    {
        int index = Random.Range(0, ChildStepSounds.Length);
        AudioClip clip = ChildStepSounds[index];
        return clip;
    }

    // play a random Golem Walk sound from the list
    public AudioClip RandomGolemWalkSound()
    {
        int index = Random.Range(0, GolemStepSounds.Length);
        AudioClip clip = GolemStepSounds[index];
        return clip;
    }

    // play a random Golem Pose sound from the list
    public AudioClip RandomGolemPoseSound()
    {
        int index = Random.Range(0, GolemPoseSounds.Length);
        AudioClip clip = GolemPoseSounds[index];
        return clip;
    }


    // enable land sound after a delay
    public IEnumerator EnableLandSound(float delay)
    {
        yield return new WaitForSeconds(delay);

        canPlayLandSound = true;
    }
}
