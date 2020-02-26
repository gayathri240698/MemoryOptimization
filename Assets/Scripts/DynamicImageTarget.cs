 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace BharathARFoundation.ImageTargets
{
    [RequireComponent(typeof(ImageTargetUrlGetter))]

    public class DynamicImageTarget : NormalImageTarget
    {
        //this dictionary holds image id and its appropriate video url
      //  public Dictionary<string, string> imageIdVideoUrlDic = new Dictionary<string, string>();
        public Action<XRReferenceImage,ImageTargetVideoUrlTemplate> referenceImageAddedEvent;
        public GameObject loadingCanvas;
        ImageTargetUrlGetter urlGetter;
        ImageTargetVideoUrlTemplate currentTemplate;

        public List<ImageTargetVideoUrlTemplate> templateCollections = new List<ImageTargetVideoUrlTemplate>();
        int addedImageCount;
        public ImageTargetUrlGetter imageTargetUrlGetter;
        //creating texture2d
        public Texture2D targetTexture;
        public GameObject networkPanel;
        public string videoURL;
        List<string> downloadUrls = new List<string>();
        Texture2D currentDownloadedTexture;
        public GameObject loadingGameObj;
        protected override void Awake()
        {
            base.Awake();
            trackImageManager.referenceLibrary = trackImageManager.CreateRuntimeLibrary(runtimeImageLibrary);
            trackImageManager.enabled = true;

            urlGetter = GetComponent<ImageTargetUrlGetter>();
          //  loadingGameObj.SetActive(false);
           // urlGetter.urlDownloadedEvent += OnUrlRetrieved; 
        }

        private void OnDestroy()
        {
          //  urlGetter.urlDownloadedEvent -= OnUrlRetrieved;
        }

        /// <summary>
        /// this function will be call once imageurlgetter got video and image url from server
        /// </summary>
        /// <param name="t"></param>
        public void OnUrlRetrieved(ImageTargetVideoUrlTemplate t)
        {
            Debug.Log("url " + t.targetImageUrl);
            currentTemplate = t;
            StartCoroutine(DownloadImageFromServer(t.targetImageUrl,t));
        }
        //this is for testing memory 
        public void TestButton(int i)
        {
            GameObject.FindObjectOfType<HUDManager>().TurnOnScanpanel();
            if (!downloadUrls.Contains(templateCollections[i].targetImageUrl))
            {
                Debug.Log("download initiated");
                StartCoroutine(DownloadImageFromServer(templateCollections[i].targetImageUrl, templateCollections[i]));
            }
            else
            {
                Debug.Log("Already ");
                Debug.Log(GameObject.FindObjectOfType<DynamicImgVideoSpawner>() + "val");
                GameObject.FindObjectOfType<DynamicImgVideoSpawner>().InitializeAllVideoPrefabs();

                Debug.Log("Already downloaded");
            }
           
        }
        public void DownloadImageFromServerFunc(ImageTargetVideoUrlTemplate t)
        {
            //    StartCoroutine(DownloadImageFromServer(t.targetImageUrl, t));
            if (!downloadUrls.Contains(t.targetImageUrl))
            {
                Debug.Log("download initiated");
                StartCoroutine(DownloadImageFromServer(t.targetImageUrl, t));
            }
            else
            {
                Debug.Log("Already ");
                Debug.Log(GameObject.FindObjectOfType<DynamicImgVideoSpawner>() + "val");
                GameObject.FindObjectOfType<DynamicImgVideoSpawner>().InitializeAllVideoPrefabs();

                Debug.Log("Already downloaded");
            }
        }
        /// <summary>
        /// this will download image from server and add to our dynamic image libraty using addimageJob ienum
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        IEnumerator DownloadImageFromServer(string imageUrl, ImageTargetVideoUrlTemplate t)
        {

           // loadingGameObj.SetActive(true);
                downloadUrls.Add(imageUrl);
                Debug.Log("what the hell");
                Debug.Log("image url " + imageUrl);
                UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl);
                yield return webRequest.SendWebRequest();
                Debug.Log("what the hell1");

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log("network error");
                    //   networkPanel.SetActive(true);
                    webRequest.Dispose();
                }
                else
                {
                    currentDownloadedTexture = null;
                    Resources.UnloadUnusedAssets();
                    GC.Collect();

                    currentDownloadedTexture = DownloadHandlerTexture.GetContent(webRequest);
                    // networkPanel.SetActive(false);
                    webRequest.Dispose();
                    StartCoroutine(AddImageJob(t));
                }
                webRequest = null;
            //UnityEngine.SceneManagement.SceneManager
         
        }


        /// <summary>
        /// this addes images target runtime to the xrreference library through unity job system
        /// </summary>
        /// <param name="texture2D"></param>
        /// <returns></returns>
        public IEnumerator AddImageJob( ImageTargetVideoUrlTemplate t)
        {
            yield return null;
            


            print("Adding image\n");

            print("Job Starting...");

            var firstGuid = new SerializableGuid(0, 0);
            var secondGuid = new SerializableGuid(0, 0);

            //   XRReferenceImage newImage = new XRReferenceImage(firstGuid, secondGuid, new Vector2(0.1f, 0.1f), Guid.NewGuid().ToString(), texture2D);
            addedImageCount++;
            XRReferenceImage newImage = new XRReferenceImage(firstGuid, secondGuid, new Vector2(0.1f, 0.1f), addedImageCount.ToString(), currentDownloadedTexture);

            try
            {
              loadingGameObj.SetActive(true);

                Debug.Log("new image naame"+newImage.ToString()) ;

                MutableRuntimeReferenceImageLibrary mutableRuntimeReferenceImageLibrary = trackImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;



                var jobHandle = mutableRuntimeReferenceImageLibrary.ScheduleAddImageJob(currentDownloadedTexture, addedImageCount.ToString(), 0.1f);

                while (!jobHandle.IsCompleted)
                {
                    print("Job Running...");
                }

                loadingGameObj.SetActive(false);
                Debug.Log("job finsihed added to dictionary");
                //  referenceImageAddedEvent?.Invoke(newImage,t);
                GetComponent<DynamicImgVideoSpawner>().OnReferenceImageAdded(newImage, t);

                currentDownloadedTexture = null;
                Resources.UnloadUnusedAssets();
                GC.Collect();
                //image downloaded and added to library
                //  imageTargetUrlGetter.OnUnityEntered();

                print("image downloaded and added to library");
                print("image count" + mutableRuntimeReferenceImageLibrary.count);
                //print("name da mame " + newImage.name);
            
            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
       /* public void TestingTarget()
        {
            ImageTargetVideoUrlTemplate videoTemplate = new ImageTargetVideoUrlTemplate();
            videoTemplate.targetVideoUrl = videoURL;
            StartCoroutine(AddImageJob(targetTexture, videoTemplate));
            Debug.Log("ImageTarget Added");
           
        }*/

       
    }
}
