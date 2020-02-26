using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class ApiCall : MonoBehaviour 
{

    Gobalvariable  gobal_var;
    private ApiItem apiItem;
    private const string URL = "http://testengage.fanisko.com/ws2/?action=ar";

    void Start()
    {
        gobal_var = GameObject.Find("GobalVariable").GetComponent<Gobalvariable>();
        Debug.Log("targetVideo_Url = " + gobal_var.targetVideo_Url + "\n Video360_Url = " + gobal_var.Video360_Url);
        StartCoroutine(webServiceRequest());
    }

    IEnumerator webServiceRequest()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(URL))
        {
           // yield return www.Send();
           yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("ApiCall error " + www.error);
                SceneManager.LoadScene("Home");
            }
            else
            {
                // // Show results as text
                 //Debug.Log(www.downloadHandler.text);

                // // Or retrieve results as binary data
                // byte[] results = www.downloadHandler.data;

        apiItem = JsonUtility.FromJson<ApiItem>(www.downloadHandler.text);
        gobal_var.targetVideo_Url = apiItem.target_url;
        gobal_var.Video360_Url = apiItem.output_url;
        Debug.Log("ApiCall targetVideo_Url = " + gobal_var.targetVideo_Url + "\n Video360_Url = " + gobal_var.Video360_Url);
        SceneManager.LoadScene("Home");
            }
        }
    }
}

[Serializable]
public class ApiItem
{
    public string title;
    public string input_url;
    public string video_title;
    public string output_url;
    public string target_url;
    public int is_enabled;
}

// Example
 //   string json = "{\"title\":\"360 video\"}";
        //  apiItem = JsonUtility.FromJson<ApiItem>(json);

        //  Debug.Log("apiItem" + apiItem.title);
//         MyClass myObject = new MyClass();
// myObject.level = 1;
// myObject.timeElapsed = 47.5f;
// myObject.playerName = "Dr Charles Francis";

// string json = JsonUtility.ToJson(myObject);

// Debug.Log("Json "+json);

// myObject = JsonUtility.FromJson<MyClass>(json);
// Debug.Log("myObject "+myObject);
