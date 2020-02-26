using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NavigationScript : MonoBehaviour {

    public string ScreenName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void noClick() {
        if (ScreenName == "")
        {
            SceneManager.LoadScene("Home");
        }
        else
        {
            SceneManager.LoadScene(ScreenName);
        }
	}
}


//// note vuforia class DefaultTrackableEventHandler

   // add line

   //// 1 protected virtual void OnTrackingFound()
    // // SOLUTION
    //    if (mTrackableBehaviour.gameObject.GetComponentInChildren<VideoPlayer>() != null)
    //    {
    //        mTrackableBehaviour.gameObject.GetComponentInChildren<VideoPlayer>().Play();
    //    }
    //    Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

    //// 2  protected virtual void OnTrackingLost() method 

    //// SOLUTION
    //if (mTrackableBehaviour.gameObject.GetComponentInChildren<VideoPlayer>() != null)
        //{
        //    mTrackableBehaviour.gameObject.GetComponentInChildren<VideoPlayer>().Pause();
        //}
        //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

