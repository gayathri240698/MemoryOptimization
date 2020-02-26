using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace BharathARFoundation.ImageTargets
{
    public class ImageUrlGetter : MonoBehaviour

    {
        public void Start()
        {
            print("image target url getter is starting");
          //  JsonFromNative("{\"arList\": [{\"targetImageUrl\":\"https://www.dropbox.com/s/2q0uc610uouj29f/33.jpg?dl=1\",\"targetVideoUrl\":\"https://www.youtube.com/watch?v=2IJtfbcYbxc\",\"adsInfo\":\"Your chances at winning $10,000 is just a pizza and a away\",,\"adsButtonName\":\"GET GIVEAWAYS\",\"adsRedirectUrl\":\"https://fanisko.com/\",\"advertisement_id\":\"8888\",\"project_id\":\"8888\",\"promotion_id\":\"8888\"}]}");
        }
        public string JsonUrl;
        public List<string> downloadedUrls = new List<string>();

        public UnityAction<bool> UnityStateEvent;

        public void JsonFromNative(string s)
        {
            print("got json" + s);

            Debug.Log("got info from native" + s);


            ImageVideoTemplateCollection collection = new ImageVideoTemplateCollection();
            collection = JsonUtility.FromJson<ImageVideoTemplateCollection>(s);
            Debug.Log(collection);
            for (int i = 0; i < collection.arList.Length; i++)
            {
                if (downloadedUrls.Contains(collection.arList[i].targetImageUrl))
                {
                    Debug.Log("already downloaded image urls");
                    //OnUnityEntered();
                    continue;
                }
                //todo
                downloadedUrls.Add(collection.arList[i].targetImageUrl);
                GameObject.FindObjectOfType<DynamicImageTarget>().OnUrlRetrieved(collection.arList[i]);
                //urlDownloadedEvent?.Invoke(collection.arList[i]);

                //print("image url " + i + collection.arList[i].targetImageUrl);
                //// ensure projectid  and advertisements ids are not null and send to analytics with onArscreenopen event
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
                //    Firebase.Analytics.FirebaseAnalytics.LogEvent("AR_ScreenOpen", "advertisement_id", collection.arList[i].promotion_id);
                //    Debug.Log("Promotion_id " + collection.arList[i].promotion_id);
                //}
                print("video url " + i + collection.arList[i].targetVideoUrl);
            }
        }
    }

}
