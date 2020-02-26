using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class videoPlayer : MonoBehaviour
{
    //if we using this video player for vuforia based video playing
   // VuforiaManager vuforiaManager;
    //Raw Image to Show Video Images [Assign from the Editor]
    //ublic RawImage image;
    //Video To Play [Assign from the Editor]
    MeshRenderer renderer;
    public string url;
   // public VideoClip videoToPlay;

    protected VideoPlayer _videoPlayer;
    private VideoSource videoSource;

    //Audio
    private AudioSource audioSource;


    protected void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        //Add VideoPlayer to the GameObject
        _videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        _videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        _videoPlayer.isLooping = true;
    }
    // Use this for initialization

    protected virtual void Start()
    {
        //if you were using this for vuforia based then use vuforia manager to get ontracking and onlost events
       // vuforiaManager = GetComponent<VuforiaManager>();

     
        Application.runInBackground = true;
        renderer.enabled = false;
        //StartCoroutine(playVideo());
    }
    protected void PlayVideo()
    {
        StartCoroutine(playVideo());
    }
    protected void StopVideo()
    {
        renderer.enabled = false;
        _videoPlayer.Stop();
        audioSource.Stop();
    }
    protected void PauseVideo()
    {
        _videoPlayer.Pause();
        audioSource.Pause();
    }
    IEnumerator playVideo()
    {
        //renderer.enabled = false;

        /* //Add VideoPlayer to the GameObject
         _videoPlayer = gameObject.AddComponent<VideoPlayer>();

         //Add AudioSource
         audioSource = gameObject.AddComponent<AudioSource>();

         //Disable Play on Awake for both Video and Audio
         _videoPlayer.playOnAwake = false;
         audioSource.playOnAwake = false;*/

        //We want to play from video clip not from url
        _videoPlayer.source = VideoSource.Url;
    
        Debug.Log("video url" + url);
        //Set Audio Output to AudioSource
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        _videoPlayer.EnableAudioTrack(0, true);
        _videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
       // _videoPlayer.clip = videoToPlay;
       _videoPlayer.url = url;
        _videoPlayer.Prepare();
        

        WaitForSeconds waitTime = new WaitForSeconds(3f);
        //Wait until video is prepared
        while (!_videoPlayer.isPrepared)
        {
            yield return waitTime;
            break;
        }

        Debug.Log("Done Preparing Video");
      
        //Assign the Texture from Video to RawImage to be displayed
        renderer.material.mainTexture = _videoPlayer.texture;

        //Play Video
        _videoPlayer.Play();
        renderer.enabled = true;
        //Play Sound
        audioSource.Play();

        Debug.Log("Playing Video");
        while (_videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)_videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");
    }

}
