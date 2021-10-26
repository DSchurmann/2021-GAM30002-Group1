using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimaticController : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public Button ContinueButton;

    private double time = 0;

    // Start is called before the first frame update
    void Start()
    {
        time = VideoPlayer.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        long playerCurrentFrame = VideoPlayer.frame;
        long playerFrameCount = Convert.ToInt64(VideoPlayer.frameCount);


        if (playerCurrentFrame >= playerFrameCount)
        {
            Debug.Log("VIDEO ENDED");
            SkipVideo();
        }
            

    }

    public void ReplayVideo()
    {
        VideoPlayer.Stop();
    }
 
    public void SkipVideo()
    {
        VideoPlayer.Pause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
