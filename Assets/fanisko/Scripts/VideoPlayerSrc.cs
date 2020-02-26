using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerSrc : MonoBehaviour {
    //Gobalvariable  gobal_var;
    //public VideoClip videoToPlay;

    //// private string VideoUrl = "https://s3-us-west-2.amazonaws.com/engage-ar/201812141413/Clipchamp.mp4";

    //// Use this for initialization
    //void Start () {
    //	gobal_var = GameObject.Find("GobalVariable").GetComponent<Gobalvariable>();
    //       Debug.Log("VideoPlayerSrc targetVideo_Url = " + gobal_var.targetVideo_Url);

    //	var player = gameObject.GetComponent<VideoPlayer>();
    //	// player.source = VideoSource.VideoClip;
    //	player.source = VideoSource.Url;
    //	// player.url = VideoUrl;
    //	player.url = gobal_var.targetVideo_Url;
    //	player.clip = videoToPlay;
    //	player.Play();
    //}

    //// Update is called once per frame
    //void Update () {

    //}
    //}
    Gobalvariable gobal_var;
    public GameObject obj_Indicator;

    //Video To Play [Assign from the Editor]
    public VideoClip videoToPlay;

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    //Audio
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        gobal_var = GameObject.Find("GobalVariable").GetComponent<Gobalvariable>();
        Debug.Log("VideoPlayerSrc targetVideo_Url = " + gobal_var.targetVideo_Url);
        StartCoroutine(playVideo());
    }

    IEnumerator playVideo()
    {
        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = gobal_var.targetVideo_Url;

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.clip = videoToPlay;
        videoPlayer.isLooping = true;
        videoPlayer.Prepare();

        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            yield return null;
        }

        Debug.Log("Done Preparing Video");

        //Assign the Texture from Video to RawImage to be displayed
        //  image.texture = videoPlayer.texture;

        ////Play Video
      //  videoPlayer.Play();

        ////Play Sound
       // audioSource.Play();

        Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");
    }

    private void Update()
    {
        if (videoPlayer.isPlaying || videoPlayer.isPaused)
        {
            obj_Indicator.SetActive(false);
        }
        else
        {
            obj_Indicator.SetActive(true);
        }

    }
}
