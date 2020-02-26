using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace BharathARFoundation.ImageTargets

{
   [RequireComponent(typeof(DynamicImageTarget))]
    public class DynamicImgVideoSpawner : MonoBehaviour
    {
        public GameObject videoPrefab;
        List<GameObject> videoPrefabList = new List<GameObject>();

        //we were going to reuse this prefab
       // GameObject instantiatedGameObject;
        public Dictionary<GameObject, string> refImgNameWithObj = new Dictionary<GameObject, string>();
        private void Start()
        {
            GetComponent<DynamicImageTarget>().referenceImageAddedEvent += OnReferenceImageAdded;
        }
        private void OnDestroy()
        {
            GetComponent<DynamicImageTarget>().referenceImageAddedEvent -= OnReferenceImageAdded;
        }
        public void ReleaseAllVideoAllocation()
        {
            print("Releasing videos from memory");

            foreach (GameObject g in videoPrefabList)
            {
                g.GetComponentInChildren<ARFoundationYoutubePlayer>().ReleaseVideoAllocation();
               
            }
            print("Videos released from memory");
        }
        public void InitializeAllVideoPrefabs()
        {
            foreach(GameObject g in videoPrefabList)
            {

                g.GetComponentInChildren<ARFoundationYoutubePlayer>().Init();
            }
        }
            
        public void OnReferenceImageAdded(XRReferenceImage img,ImageTargetVideoUrlTemplate template)
        {
            Debug.Log("on reference image " + template.targetVideoUrl);
           
             var instantiatedGameObject = Instantiate(videoPrefab);

            
            instantiatedGameObject.name = img.name;
            //todo
            videoPrefabList.Add(instantiatedGameObject);
            //instantiatedGameObject.GetComponentInChildren<ARFoundationVIdeoPlayer>().url = template.targetVideoUrl;
			//todo bharath uncomment the following all lines
			var t = instantiatedGameObject.GetComponentInChildren<ARFoundationYoutubePlayer>();
			t.VideoURL = template.targetVideoUrl;
			t.description = template.adsInfo;
			t.buttonName = template.adsButtonName;
			t.redirectUrl = template.adsRedirectUrl;

            //t.Init();
            //we already added t to the list so initialiatize all includes t
            InitializeAllVideoPrefabs();

			//refImgNameWithObj.Add(instantiatedGameObject, img.name);
			//Debug.Log("dictionary count" + refImgNameWithObj.Count);
		}

    }
}