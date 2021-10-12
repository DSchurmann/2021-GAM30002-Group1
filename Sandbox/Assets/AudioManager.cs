using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Child Sounds")]
    
    public AudioClip[] JumpSounds;
    public AudioClip[] LandSounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip RandomJumpSound()
    {
        int index = Random.Range(0, JumpSounds.Length);
        AudioClip clip = JumpSounds[index];
        return clip;
    }

    public AudioClip RandomLandSound()
    {
        int index = Random.Range(0, LandSounds.Length);
        AudioClip clip = LandSounds[index];
        return clip;
    }
}
