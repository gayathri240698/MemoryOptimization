using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;

namespace BharathARFoundation.ImageTargets

{
    public class ImageTargetUrlGetter : MonoBehaviour
    {
        public string JsonUrl;
        public List<string> downloadedUrls = new List<string>();
        public GameObject networkPanel;
        public UnityAction<ImageTargetVideoUrlTemplate> urlDownloadedEvent;
        public  UnityAction<bool> UnityStateEvent;
        // public UnityAction unityExitedEvent;

        // Start is called before the first frame update
        void Start()
        {
            print("unity started");
           // Firebase.Analytics.FirebaseAnalytics.LogEvent("AR_ScreenOpen", "device_id", SystemInfo.deviceUniqueIdentifier);
          //  Firebase.Analytics.FirebaseAnalytics.LogEvent("AR_ScreenOpen", "date", System.DateTime.Now.ToString("dd/MM/yyyy"));
            Debug.Log("Date " + System.DateTime.Now.ToString("dd/MM/yyyy"));
            //  JsonFromNative("{\"arList\": [{\"targetImageUrl\":\"https://www.dropbox.com/s/2q0uc610uouj29f/33.jpg?dl=1\",\"targetVideoUrl\":\"https://www.youtube.com/watch?v=2IJtfbcYbxc\",\"adsInfo\":\"Your chances at winning $10,000 is just a pizza and a away\",,\"adsButtonName\":\"GET GIVEAWAYS\",\"adsRedirectUrl\":\"https://fanisko.com/\",\"advertisement_id\":\"8888\",\"project_id\":\"8888\",\"promotion_id\":\"8888\"}]}");

           //  StartCoroutine(GetJsonOnline());
        }
        //enable and disable tracking in AR


        public void OnUnityExited()
            {
                UnityStateEvent?.Invoke(false);
                Debug.Log("unity screen exited");
            }
        public void OnUnityEntered()
        {
                UnityStateEvent?.Invoke(true);
                Debug.Log("unity screen entered");
        }
        //if we get json from native
        public void JsonFromNative( string s)
        {
            //turn on scan panel because this is the entry point from native to unity
            GameObject.FindObjectOfType<HUDManager>().TurnOnScanpanel();
            print("got json" + s);
           
            Debug.Log("got info from native" + s);
            

            ImageVideoTemplateCollection collection = new ImageVideoTemplateCollection();
            collection = JsonUtility.FromJson<ImageVideoTemplateCollection>(s);
            Debug.Log(collection);
            for (int i = 0; i < collection.arList.Length; i++)
            {
                /* if(downloadedUrls.Contains(collection.arList[i].targetImageUrl))
                 {
                     Debug.Log("already downloaded image urls");
                     OnUnityEntered();
                     continue;
                 }
                 //todo
                 downloadedUrls.Add(collection.arList[i].targetImageUrl);
                 urlDownloadedEvent?.Invoke(collection.arList[i]);*/
                GameObject.FindObjectOfType<DynamicImageTarget>().DownloadImageFromServerFunc(collection.arList[i]);
                print("image url " + i + collection.arList[i].targetImageUrl);
                // ensure projectid  and advertisements ids are not null and send to analytics with onArscreenopen event
                //if (collection.arList[i].project_id != null)
                //{
                //    IdManager.Instance.projectId = collection.arList[i].project_id;
                //    Firebase.Analytics.FirebaseAnalytics.LogEvent("AR_ScreenOpen", "project_id", collection.arList[i].project_id);
                //    Debug.Log("Project_id " + collection.arList[i].project_id);
                //}
               //if(collection.arList[i].advertisement_id != null)
               // {
               //     IdManager.Instance.advertisementId = collection.arList[i].advertisement_id;
               //     Firebase.Analytics.FirebaseAnalytics.LogEvent("AR_ScreenOpen", "advertisement_id", collection.arList[i].advertisement_id);
               //     Debug.Log("Advertisement_id " + collection.arList[i].advertisement_id);
               // }
                //if (collection.arList[i].promotion_id != null)
                //{
                //    IdManager.Instance.PromotionId = collection.arList[i].promotion_id;
                //    Firebase.Analytics.FirebaseAnalytics.LogEvent("AR_ScreenOpen", "advertisement_id", collection.arList[i].promotion_id);
                //    Debug.Log("Promotion_id " + collection.arList[i].promotion_id);
                //}
                print("video url " + i + collection.arList[i].targetVideoUrl);
            }
        }
       IEnumerator GetJsonOnline()
        {
            UnityWebRequest www = UnityWebRequest.Get(JsonUrl);
            yield return www.SendWebRequest();

            if (www.isNetworkError )
            {
                Debug.Log("network error"+www.error);
                Debug.Log("asdf " + networkPanel);
                //todo 
                //networkPanel.SetActive(true);
                yield break;
            }
            else
            {
                //todo
               // networkPanel.SetActive(false);

            }

            //    ImageTargetVideoUrlTemplate ImageVideoSet1 = new ImageTargetVideoUrlTemplate();
            ImageVideoTemplateCollection collection = new ImageVideoTemplateCollection();
            collection = JsonUtility.FromJson<ImageVideoTemplateCollection>(www.downloadHandler.text);
            Debug.Log(collection);
            for(int i =0;i<collection.arList.Length;i++)
            {
                //todo uncomment following event line
                //   urlDownloadedEvent?.Invoke(collection.arList[i]);
                GameObject.FindObjectOfType<DynamicImageTarget>().DownloadImageFromServerFunc(collection.arList[i]);
                //check the ad and project id

                //if (collection.arList[i].project_id != null)
                //{
                //    IdManager.Instance.projectId = collection.arList[i].project_id;
                //    Firebase.Analytics.FirebaseAnalytics.LogEvent("AR_ScreenOpen", "project_id", collection.arList[i].project_id);
                //    Debug.Log("Project_id " + collection.arList[i].project_id);
                //}
                //if (collection.arList[i].advertisement_id != null)
                //{
                //    IdManager.Instance.advertisementId = collection.arList[i].advertisement_id;
                //    Firebase.Analytics.FirebaseAnalytics.LogEvent("AR_ScreenOpen", "advertisement_id", collection.arList[i].advertisement_id);
                //    Debug.Log("Advertisement_id " + collection.arList[i].advertisement_id);
                //}
                //if (collection.arList[i].promotion_id != null)
                //{
                //    IdManager.Instance.PromotionId = collection.arList[i].promotion_id;
                //    Firebase.Analytics.FirebaseAnalytics.LogEvent("AR_ScreenOpen", "promotion_id", collection.arList[i].promotion_id);
                //    Debug.Log("Promotion_id " + collection.arList[i].promotion_id);
                //}
                //IdManager.Instance.template = collection.arList[i];

                Debug.Log("image url "+i+collection.arList[i].targetImageUrl);
                Debug.Log("video url " + i + collection.arList[i].targetVideoUrl);
            }
            

 }
    }

}
