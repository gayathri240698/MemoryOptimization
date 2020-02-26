using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.Video;
using YoutubePlayer;
using System;
using UnityEngine.UI;
namespace BharathARFoundation.ImageTargets
{
    public class ARFoundationYoutubePlayer : MonoBehaviour
    {

        //url
        public string VideoURL;
        public GameObject redirectTemplate;
        public string description, buttonName, redirectUrl;
      //  YoutubePlayer player;
        public GameObject videoRender;
        bool isQuadIsActive;
        bool isVideoStarted;
        bool isFirstTime = true;
        public TextMeshPro descriptTextmesh, buttonNameTextmesh;//, redirectUrlTextmesh;
        bool isVideoFinished = false;
        //this determines whether unity is a main window or its has been hided by swift app
        bool canPlay = true;
        public GameObject scanCanvasObj;
        DynamicImgVideoSpawner videoSpawner;
        public GameObject loadingSprite;
        // Start is called before the first frame update

        //new structured project
        VideoPlayer newVideoPlayer;
        AudioSource newAudioSource;
        public YoutubePlayer.YoutubePlayer  newYoutubePlayer;

        //ui slider
        public Slider videoLengthSlider;
        public GameObject canvasPrefab;
         GameObject canvasObject;
        HUDManager hud_Manager;
        bool isVideoPlaying;
        void Start()
        {
            
           // player = GetComponent<YoutubePlayer>();
            videoSpawner = GameObject.FindObjectOfType<DynamicImgVideoSpawner>();
            canvasObject = GameObject.Instantiate<GameObject>(canvasPrefab);
            //first child of this canvas is slider which we were using to show video length progress bar
            videoLengthSlider = canvasObject.GetComponent<VideoUI>().videoProgressSlider;
            hud_Manager = GameObject.FindObjectOfType<HUDManager>();
            hud_Manager.TurnOffScanPanel();
            //  player.OnVideoFinished.AddListener( TurnOnRedirectTemplate);
            //  player.OnVideoStarted.AddListener(OffVideoLoadingIndicator);
            //  newVideoPlayer.loopPointReached += OnVideoEnded;

#if !UNITY_EDITOR
           // GameObject.FindObjectOfType<ImageTargetUrlGetter>().UnityStateEvent += SetCanPlay;

            GameObject.FindObjectOfType<ARTrackedImageManager>().trackedImagesChanged += ARFoundationVIdeoPlayer_trackedImagesChanged;

#endif
        }
        public void SetCanPlay(bool val)
        {
                    canPlay = val;
        
     }
        public void Init()
        {
            // Debug.Log("game object name " + transform.parent.gameObject.name);
            Debug.Log("hey " + gameObject.transform.parent.name + "got initialized");

           
            TurnOffRedirectTemplate();
            isVideoStarted = false;
            isFirstTime = true;
             isVideoFinished = false;
            canPlay = true;
            HideQuad();
           
        }
        public void TurnOffRedirectTemplate()
        {
            // redirectTemplate.SetActive(false);
            canvasObject.GetComponent<VideoUI>().TurnOffRedirectTemplate();
        }
        public void OffVideoLoadingIndicator()
        {
            //kuty kolaru kudutha code
           // loadingSprite.SetActive(false);
        }
        public void TurnOnRedirectTemplate()
        {
            isVideoFinished = true;
            Debug.Log("turning on redirect template");
            canvasObject.GetComponent<VideoUI>().TurnOnRedirectTemplate(description, redirectUrl);
            /*
              redirectTemplate.SetActive(true);
            descriptTextmesh.text = description;
            buttonNameTextmesh.text = buttonName;
            */
        }
        public void RedirectUrl()
        {
           


            Application.OpenURL(redirectUrl);
            //once we done with this video player destroy it from memeory


            Debug.Log("redirecting to this url" + redirectUrl);
        }
        // void onUnityExited()
        // {
        //     StopVideo();
        //     isFirstTime = true;
        // }
        private void OnDestroy()
        {
           // newVideoPlayer.loopPointReached -= OnVideoEnded;

#if !UNITY_EDITOR
            //   GameObject.FindObjectOfType<ARTrackedImageManager>().trackedImagesChanged -= ARFoundationVIdeoPlayer_trackedImagesChanged;
          //  GameObject.FindObjectOfType<ImageTargetUrlGetter>().UnityStateEvent-=SetCanPlay;
            GameObject.FindObjectOfType<ARTrackedImageManager>().trackedImagesChanged -= ARFoundationVIdeoPlayer_trackedImagesChanged;

#endif
        }

        /// <summary>
        /// it gives scale calculation to fit width 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        Vector3 calculatedScale(Vector2 val)
        {
            var median = val.x / 16;
            var newX = 16 * median;
            var newY = 9 * median;
            print("size resizer ends with x " + newX + " z" + newY);

            return new Vector3(newX, newY, 1f);
        }
        /// <summary>
        /// called by arfoundation every frame with the details of images it seen in particular frame.
        /// </summary>
        /// <param name="obj"></param>
        void ARFoundationVIdeoPlayer_trackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
        {
            if (canPlay)
            {
                foreach (ARTrackedImage image in obj.updated)
                {

                    //  Debug.Log("obj name " + videoSpawner.refImgNameWithObj[this.transform.parent.gameObject]);
                    //  if (image.referenceImage.name == videoSpawner.refImgNameWithObj[this.transform.parent.gameObject])
                    if (image.referenceImage.name == transform.parent.gameObject.name)
                    {
                        
                        //if video is not played and finished already then only we should play video

                        {

                            Debug.Log("name matched "+"image name "+ image.referenceImage.name+ "parent name "+transform.parent.gameObject.name);
                            if (image.trackingState == TrackingState.Tracking)
                            {

                                transform.parent.position = image.transform.position;
                                transform.parent.rotation = image.transform.rotation;
                                transform.localScale = calculatedScale(new Vector2(image.size.x, image.size.y));

                                if (!isVideoFinished)
                                {
                                    Debug.Log("i'm in tracking status");
                                    if (!isVideoStarted)
                                    {
                                        isVideoStarted = true;
                                        if (isFirstTime)
                                        {
                                            ////kuty kolaru kudutha code
                                            //loadingSprite.SetActive(false);
                                            PlayVideo();
                                            hud_Manager.TurnOffScanPanel();
                                            isFirstTime = false;


                                            Debug.Log("first time play");
                                        }
                                        else
                                        {
                                            ResumeVideo();
                                            Debug.Log("resume");
                                        }
                                        // Debug.Log("video is not playing so starting playing video");

                                    }
                                }
                            }
                            else
                            {

                                Debug.Log("bharath yy112233 pausig video");
                                isVideoStarted = false;
                                PauseVideo();
                            }

                        }
                    }


                }
            }
            else
            {

                isVideoStarted = false;
                StopVideo();
            }
            Debug.Log("arfoundation frames working insie youtube script");

        }
        //        void ARFoundationVIdeoPlayer_trackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
        //        {
        //         if(canPlay){
        //foreach (ARTrackedImage image in obj.updated)
        //            {

        //                    //  Debug.Log("obj name " + videoSpawner.refImgNameWithObj[this.transform.parent.gameObject]);
        //                    //  if (image.referenceImage.name == videoSpawner.refImgNameWithObj[this.transform.parent.gameObject])
        //                    if (image.referenceImage.name == transform.parent.gameObject.name)
        //                {
        //                    //if video is not played and finished already then only we should play video

        //                    {

        //                        Debug.Log("name matched ");
        //                            if (image.trackingState == TrackingState.Tracking)
        //                            {

        //                                transform.parent.position = image.transform.position;
        //                                transform.parent.rotation = image.transform.rotation;
        //                                transform.localScale = calculatedScale(new Vector2(image.size.x, image.size.y));

        //                                if (!isVideoFinished) { 
        //                                    Debug.Log("i'm in tracking status");
        //                                if (!isVideoStarted)
        //                                {
        //                                    isVideoStarted = true;
        //                                    if (isFirstTime)
        //                                    {
        //                                        ////kuty kolaru kudutha code
        //                                        //loadingSprite.SetActive(false);
        //                                        PlayVideo();
        //                                        isFirstTime = false;


        //                                        Debug.Log("first time play");
        //                                    }
        //                                    else
        //                                    {
        //                                        ResumeVideo();
        //                                        Debug.Log("resume");
        //                                    }
        //                                    // Debug.Log("video is not playing so starting playing video");

        //                                }
        //                            }   
        //                            }
        //                            else
        //                            {

        //                                Debug.Log("pause video");
        //                                isVideoStarted = false;
        //                                PauseVideo();
        //                            }

        //                        }
        //                }


        //            }
        //         }
        //         else
        //         {

        //                isVideoStarted = false;
        //             PauseVideo();
        //         }
        //Debug.Log("arfoundation frames working insie youtube script");

        //            }


        public void OnVideoEnded(VideoPlayer v)
        {
            Debug.Log("VideoEnded");

            //Destroy(v);
            TurnOnRedirectTemplate();
        }
        public void PauseVideo()
        {
            if(newVideoPlayer!=null)
            {
               newVideoPlayer.playbackSpeed = 0f;
                Debug.Log("video player not null pausing video");

            }
            Debug.Log("pausing video");
            HideQuad();
        }
        public void ResumeVideo()
        {

          ShowQuad();
            if(newVideoPlayer!=null)
          newVideoPlayer.playbackSpeed = 1f;
        }
                // play video
        public void PlayVideo()
        {
            loadingSprite.SetActive(true);
            CreateVideoAllocation();
            ShowQuad();

            newYoutubePlayer.PlayVideo(VideoURL);
            
        }

       
         public void StopVideo()
        {
            isFirstTime = true;
            if (newVideoPlayer != null)
            {
                newVideoPlayer.Stop();
            }
            ReleaseVideoAllocation();
         }

        //Show quad video player
        void ShowQuad()
        {
            //isQuadIsActive = true;
            videoRender.GetComponent<Renderer>().enabled = true;
           StartCoroutine( UpdateVideoLengthSlider());
        }

        //Hide Quad videoplayer
        void HideQuad()
        {
            // isQuadIsActive = false;
            Debug.Log("quad hided");
            videoRender.GetComponent<Renderer>().enabled = false;
            TurnOffVideoLengthSlider();
       }
        public void ReleaseVideoAllocation()
        {
            //todo used lot of codes in this single function so have to refactor them in a meaningful way
            TurnOffRedirectTemplate();
            //before releaseing we were hiding
            HideQuad();
            //and w
            if (newVideoPlayer!=null) //because  the objects that sit idle without video player also try to execute this function, to it is better to check before hand
            {
                SetCanPlay(false);
                
                newVideoPlayer.loopPointReached -= OnVideoEnded;

                newVideoPlayer.Stop();
                var temp = GetComponents<videoPlayer>();
                print(temp.Length +"num of video players in this particular object");
                Destroy(newVideoPlayer);
                Destroy(newVideoPlayer);
                //newVideoPlayer.clip = null;
                //newAudioSource.clip = null;
                newVideoPlayer = null;
                newAudioSource = null;
                //Destroy(newVideoPlayer);
                //Destroy(newAudioSource);
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
           
        

        }
        /// <summary>
        /// create new video and audio source  components
        /// </summary>
        public void CreateVideoAllocation()
        {
            newVideoPlayer = gameObject.AddComponent<VideoPlayer>();
            newAudioSource = gameObject.AddComponent<AudioSource>();

            newVideoPlayer.playOnAwake = false;
            newAudioSource.playOnAwake = false;
            newVideoPlayer.loopPointReached += OnVideoEnded;
    
        }
        public void TurnOffVideoLengthSlider()
        {

            //turn off video lenght slider
            canvasObject.SetActive(false);
        }
        public IEnumerator UpdateVideoLengthSlider()
        {
            print("HI");
            yield return null;
            /*if (newVideoPlayer != null)
            {
                 while (!newVideoPlayer.isPlaying)
                  {
                      yield return new WaitForEndOfFrame();

                  }
                canvasObject.SetActive(true);
                while (newVideoPlayer.isPlaying)
                {
                    Debug.Log("video player time" + newVideoPlayer.time);
                    Debug.Log("video player lenght" + newVideoPlayer.length);
                    var averageValue = System.Math.Round((double)newVideoPlayer.time / newVideoPlayer.length, 3);

                    //these is to indicate video playback

                    


                    videoLengthSlider.value = (float)averageValue;
                    Debug.Log("AverageValue" + averageValue);

                    yield return new WaitForEndOfFrame();
                    //yield return new WaitForEndOfFrame();

                }
              
            }*/
            
        }
        // Update is called once per frame
        public void Update()
        {
            
               
                if(newVideoPlayer!=null)
            {
                if(newVideoPlayer.isPlaying)
                {
                    canvasObject.SetActive(true);

                    isVideoPlaying = true;
                    Debug.Log("video player time" + newVideoPlayer.time);
                    Debug.Log("video player lenght" + newVideoPlayer.length);
                    var averageValue = System.Math.Round((double)newVideoPlayer.time / newVideoPlayer.length, 3);

                    //these is to indicate video playback




                    videoLengthSlider.value = (float)averageValue;
                    Debug.Log("AverageValue" + averageValue);

                }
               
            }
            else
            {
                isVideoPlaying = false;

                canvasObject.SetActive(false);
            }


#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("playing video");
                PlayVideo();
                

            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                PauseVideo();

            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                StopVideo();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                ResumeVideo();
            }
#endif
        }
    }
}
