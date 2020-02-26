using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoUI : MonoBehaviour

{
    public Slider videoProgressSlider;
    public TextMeshProUGUI redirectText;
    public GameObject redirectObj;

    string redirectURL;
    public void TurnOnRedirectTemplate(string message,string redirecturl)
    {
        Debug.Log("turning on redirect template");
        redirectText.text = message;
        redirectURL = redirecturl;
        redirectObj.SetActive(true);
    }
    public void TurnOffRedirectTemplate()
    {
        Debug.Log("turning off redirect template");

        redirectObj.SetActive(false);
    }
    public void OnYesPressed()
    {
        Application.OpenURL(redirectURL);
    }
    public void OnNoPressed()
    {
        redirectObj.SetActive(false);

    }
}
