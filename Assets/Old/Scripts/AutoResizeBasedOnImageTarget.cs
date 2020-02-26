using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
namespace BharathARFoundation.ImageTargets
{
    public class AutoResizeBasedOnImageTarget : MonoBehaviour
    {
#if !UNITY_EDITOR
        // Start is called before the first frame update
        ARTrackedImageManager manager;

        void Start()
        {
            //dynamic image target or normal images target will add this component to its game object in awake so we searching it in start method
            manager = GameObject.FindObjectOfType<ARTrackedImageManager>();
            manager.trackedImagesChanged += Manager_trackedImagesChanged;
        }
        private void OnDestroy()
        {
            manager.trackedImagesChanged -= Manager_trackedImagesChanged;
        }
        /// <summary>
        /// calcualte quad height and width based on 16:9 ratio of videos
        /// </summary>
        /// <returns></returns>
        Vector3 calculatedScale(ARTrackedImage trackedImage)
        {
            var median = trackedImage.extents.x / 16;
            var newX = 16 * median;
            var newZ = 9 * median;
            print("size resizer ends with x " + newX + " z" + newZ);

            return new Vector3(newX, 0f, newZ);
        }
        private void Manager_trackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
        {
			print("atleast we know tracking started");
			foreach (ARTrackedImage trackedImage in obj.added)
			{
				trackedImage.transform.Rotate(Vector3.up, 180);


			}
			foreach (ARTrackedImage trackedImage in obj.updated)
            {
					//print("size resizer initialized with x " + trackedImage.extents.x + " z" + trackedImage.extents.y);
					//this.transform.localScale = calculatedScale(trackedImage);
					//print("my updated scale" + transform.localScale);
					
				trackedImage.transform.Rotate(Vector3.up, 180);
            }
        }
#endif
    }
}