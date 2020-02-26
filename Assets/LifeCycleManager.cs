using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class LifeCycleManager : MonoBehaviour
{
    public static LifeCycleManager Instance;
    string LaucherSceneName = "UnityAdapter";
    string currentSceneName = null;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(this);
        }
        //else
        //{
        //    Destroy(this.gameObject);
        //}
    }
    public void LoadModule(string s)
    {
        if(!string.IsNullOrEmpty(currentSceneName))
        {
            //Debug.Log("string : " + currentSceneName);
            DeleteEntireSceneFromMemory(currentSceneName);

        }
        GetComponent<ARSession>().Reset();
        SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);
        Debug.Log("loading this scene " + s);

        currentSceneName = s;
    }
    public void UnLoadModule(string s)
    {
        currentSceneName = null;
        DeleteEntireSceneFromMemory(s);

    }
    private void DeleteEntireSceneFromMemory(string sceneToDestroy)
    {
        GameObject[] _rootGameObjectsOfSpecificScene = SceneManager.GetSceneByName(sceneToDestroy).GetRootGameObjects();
        foreach (GameObject o in _rootGameObjectsOfSpecificScene)
        {
            if (o.name == "AR Session Origin")
            {
                ARSessionOrigin arorigin = o.GetComponent<ARSessionOrigin>();
                arorigin.camera.enabled = false;
                Debug.Log("AR Session Origin");
                o.gameObject.SetActive(false);
            }
            Debug.Log("Destroy" + o.name);
            DestroyImmediate(o);
        }

        //flushing objects from memory

        _rootGameObjectsOfSpecificScene = null;
        SceneManager.UnloadSceneAsync(sceneToDestroy);
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

    }

    public void UnLoadAllModules()
    {
        if(!string.IsNullOrEmpty( currentSceneName))
        {
            DeleteEntireSceneFromMemory(currentSceneName);
            Debug.Log("unloading this scene " + currentSceneName);
            currentSceneName = null;
        }
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        //Resources.UnloadUnusedAssets();
        //System.GC.Collect();

        //Application.Unload();
      //  DeleteEntireSceneFromMemory();

    }
}
