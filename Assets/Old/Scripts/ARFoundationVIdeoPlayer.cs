using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace BharathARFoundation.ImageTargets
{

    public class ARFoundationVIdeoPlayer : videoPlayer
    {
        string currentName;
        bool isVideoStarted;
       // videoPlayer vidPlayer;
        DynamicImgVideoSpawner videoSpawner;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            videoSpawner = GameObject.FindObjectOfType<DynamicImgVideoSpawner>();
            GameObject.FindObjectOfType<ARTrackedImageManager>().trackedImagesChanged += ARFoundationVIdeoPlayer_trackedImagesChanged;
        }

        Vector3 calculatedScale(Vector2 val)
        {
            var median = val.x / 16;
            var newX = 16 * median;
            var newY = 9 * median;
            print("size resizer ends with x " + newX + " z" + newY);

            return new Vector3(newX, newY, 1f);
        }
        void ARFoundationVIdeoPlayer_trackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
        {

            foreach (ARTrackedImage image in obj.updated)
            {
                
                Debug.Log("obj name " + videoSpawner.refImgNameWithObj[this.transform.parent.gameObject]);
              //  if (image.referenceImage.name == videoSpawner.refImgNameWithObj[this.transform.parent.gameObject])
              if(image.referenceImage.name == transform.parent.gameObject.name)
                {
                    Debug.Log("name matched ");
                    if (image.trackingState == TrackingState.Tracking)
                    {
                        transform.parent.position = image.transform.position;
                        transform.parent.rotation = image.transform.rotation;
                        transform.localScale = calculatedScale(new Vector2(image.size.x, image.size.y));


                        Debug.Log("i'm in tracking status");
                        if(!isVideoStarted)
                        {
                            isVideoStarted = true;
                            Debug.Log("video is not playing so starting playing video");
                            PlayVideo();
                            
                        }
                    }
                    else
                    {
                        isVideoStarted = false;
                        StopVideo();
                    }
                }
                   

            }
         }

        // Update is called once per frame
        void Update()
        {

        }

    }
}