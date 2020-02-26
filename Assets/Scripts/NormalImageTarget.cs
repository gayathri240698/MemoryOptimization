using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace BharathARFoundation.ImageTargets
{
	public class NormalImageTarget : MonoBehaviour
	{
		[SerializeField]
		protected GameObject prefabOnTrack;

		
		[SerializeField]
		protected XRReferenceImageLibrary runtimeImageLibrary;

		protected ARTrackedImageManager trackImageManager;

        int count;
        List<ARTrackedImage> totalTrackedImages = new List<ARTrackedImage>();
        //created this list to rotate only images added in odd number wise eg element added as 1st 3rd and 5th etc
        List<ARTrackedImage> oddList = new List<ARTrackedImage>();
		
		protected virtual void Awake()
		{
			Debug.Log("Creating Runtime Mutable Image Library\n");
			trackImageManager = gameObject.AddComponent<ARTrackedImageManager>();
			trackImageManager.maxNumberOfMovingImages = 1;
            //todo not addding prefab via artrackedimagemanager
			//trackImageManager.trackedImagePrefab = prefabOnTrack;

			

			trackImageManager.trackedImagesChanged += OnTrackedImagesChanged;

			// ShowTrackerInfo();



		}


		void Destroy()
		{
			trackImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
		}

        bool test = true;

		void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
		{
            totalTrackedImages.AddRange(eventArgs.added);
            Debug.Log("total tracked image count " + totalTrackedImages.Count);
            for (int i=0;i<totalTrackedImages.Count;i++)
            {
                if(i%2 ==0)
                {
                    if(!oddList.Contains(totalTrackedImages[i]))
                    oddList.Add(totalTrackedImages[i]);

                    Debug.Log("odd list count"+oddList.Count);
                }
            }
            foreach (ARTrackedImage trackedImage in eventArgs.added)
            {
                
                //print("Image has been tracked successfully" + trackedImage.name);

                //trackedImage.transform.Rotate(Vector3.up, 180);
                if(oddList.Contains(trackedImage))
                {
                    print("image in odd list");
                    trackedImage.transform.Rotate(Vector3.up, 180);
                }

            }

            foreach (ARTrackedImage trackedImage in eventArgs.updated)
            {

                // trackedImage.transform.Rotate(Vector3.up, 180);
                if (oddList.Contains(trackedImage))
                {
                    print("image in odd list updated");

                    trackedImage.transform.Rotate(Vector3.up, 180);
                }
            }
        }
	}
}