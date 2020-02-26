using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using BharathARFoundation.ImageTargets;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Networking;
using System;

public class MemoryProfilingTestCase : NormalImageTarget


{
    public Texture2D tex1, tex2, tex3;
    public string t1Url, t2Url, t3Url;
    Texture2D currentTex;

    protected override void Awake()
    {
        base.Awake();
        trackImageManager.enabled = false;
        trackImageManager.referenceLibrary = trackImageManager.CreateRuntimeLibrary(runtimeImageLibrary);
        trackImageManager.enabled = true;
    }
        public void AddTex(int i)
    {
        switch(i)
        {
            case 0:
                StartCoroutine(DownloadImageFromServer(t1Url));
                break;

            case 1:
                StartCoroutine(DownloadImageFromServer(t2Url));
                break;

            case 2:
                StartCoroutine(DownloadImageFromServer(t3Url));
                break;
        }
    }
        
    int addedImageCount;

    IEnumerator DownloadImageFromServer(string imageUrl)
    {

        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("network error");
            webRequest.Dispose();
        }
        else
        {
            var downloadedTexture = DownloadHandlerTexture.GetContent(webRequest);
            Debug.Log("image downloaded");
            webRequest.Dispose();
            StartCoroutine(AddImageJob(downloadedTexture));

        }
        

    }
    public IEnumerator AddImageJob(Texture2D texture2D)
    {
        yield return null;

        Debug.Log("tex name " + texture2D.name);
        var firstGuid = new SerializableGuid(0, 0);
        var secondGuid = new SerializableGuid(0, 0);

        //   XRReferenceImage newImage = new XRReferenceImage(firstGuid, secondGuid, new Vector2(0.1f, 0.1f), Guid.NewGuid().ToString(), texture2D);
        addedImageCount++;
        XRReferenceImage newImage = new XRReferenceImage(firstGuid, secondGuid, new Vector2(0.1f, 0.1f), addedImageCount.ToString(), texture2D);

        // loadingCanvas.SetActive(true);
        try
        {
            Debug.Log(newImage.ToString());

            MutableRuntimeReferenceImageLibrary mutableRuntimeReferenceImageLibrary = trackImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;



            var jobHandle = mutableRuntimeReferenceImageLibrary.ScheduleAddImageJob(texture2D, addedImageCount.ToString(), 0.1f);

            while (!jobHandle.IsCompleted)
            {
                print("Job Running...");
            }
            //loadingCanvas.SetActive(false);

            Debug.Log("job finsihed added to dictionary");
           // referenceImageAddedEvent?.Invoke(newImage, t);
            //image downloaded and added to library
            //  imageTargetUrlGetter.OnUnityEntered();
            print("image downloaded and added to library");

            //print("name da mame " + newImage.name);
            Destroy(texture2D);
            GC.Collect();
        }
        catch ( Exception e)
        {
            print(e.ToString());
        }
    }
}
