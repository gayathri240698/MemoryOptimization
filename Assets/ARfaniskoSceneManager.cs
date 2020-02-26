using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARfaniskoSceneManager : MonoBehaviour
{
    [SerializeField]
        private GameObject mainCamera;
    
   [SerializeField]
       public GameObject arsession;

        private int scene;

        void Awake() {
            //  DontDestroyOnLoad(this);	
            //  DontDestroyOnLoad(canvas);
            scene = 0;
           // arsession.SetActive(false);
            mainCamera.SetActive(true);
        }

    public void homeBtnClick() {
         sceneLoading(0);
    }

    public void targetImageBtnClick() {
         sceneLoading(1);
    }

    public void video360BtnClick(){
         sceneLoading(2);
    }

    private void sceneLoading(int inpInt) {
      if (scene == 0) {
          if (inpInt != 0) {
               scene = inpInt;
                mainCamera.SetActive(false);
                arsession.SetActive(true);
               ARSession arses = arsession.GetComponent<ARSession>();
               arses.Reset();
          SceneManager.LoadSceneAsync(scene,LoadSceneMode.Additive);
          }
       
         Debug.Log("sceneLoading"+scene);
       }else{
           mainCamera.SetActive(true);
           sceneElementDestory(scene);
            scene = 0;
            Debug.Log("gotoHome "+scene);
       }
    }

    private void sceneElementDestory(int inpScene) {
         GameObject[] _rootGameObjectsOfSpecificScene = SceneManager.GetSceneByBuildIndex(inpScene).GetRootGameObjects();
         foreach (GameObject o in _rootGameObjectsOfSpecificScene) {
         if (o.name == "AR Session Origin" ) {
           ARSessionOrigin arorigin = o.GetComponent<ARSessionOrigin>();
           arorigin.camera.enabled = false;
            Debug.Log("AR Session Origin");
             o.gameObject.SetActive(false);
         }
          Debug.Log("Destroy"+o.name);
          DestroyImmediate(o);
         }



         _rootGameObjectsOfSpecificScene = null;
            SceneManager.UnloadSceneAsync(inpScene);
            Resources.UnloadUnusedAssets();
    }
}
