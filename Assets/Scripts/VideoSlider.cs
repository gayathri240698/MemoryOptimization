using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class VideoSlider : MonoBehaviour
{
    public VideoPlayer newVideoPlayer;
    public Slider videoLengthSlider;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Update()
    {
        if (newVideoPlayer != null)
        {
            Debug.Log("video player time" + newVideoPlayer.time);
            Debug.Log("video player lenght" + newVideoPlayer.length);
            var averageValue = System.Math.Round((double)newVideoPlayer.time / newVideoPlayer.length, 1);
            
            videoLengthSlider.value = (float)averageValue;
            
            
            
            Debug.Log("AverageValue" + averageValue);
        }
    }
}