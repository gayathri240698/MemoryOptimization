using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Gobalvariable : MonoBehaviour {

	public string targetVideo_Url = "";
	public string Video360_Url = "";

	// Use this for initialization
	void Start () {
	 DontDestroyOnLoad(gameObject);	
	//  StartCoroutine(mainmenu());
	}
	
	IEnumerator mainmenu()
    {
       yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Home");
    }
}
