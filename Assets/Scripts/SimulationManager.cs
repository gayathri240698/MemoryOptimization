using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BharathARFoundation.ImageTargets;

public class SimulationManager : MonoBehaviour
{
    public List<GameObject> objectsToBeTurnedOn = new List<GameObject>();
    public List<GameObject> objectsToBeTurnedOff = new List<GameObject>();
    public Texture2D destinationTexture;
    public Texture2D sourceTexture;
   public ARFoundationYoutubePlayer t;
    public List<ImageTargetVideoUrlTemplate> template = new List<ImageTargetVideoUrlTemplate>();
    public GameObject redirectOjb;
    public void InjectDataToVideoPlayer(int i)
    {
    
                t.VideoURL = template[i].targetVideoUrl;
                t.description = template[i].adsInfo;
                t.buttonName = template[i].adsButtonName;
                t.redirectUrl = template[i].adsRedirectUrl;
                t.Init();
        
    }
    public void InjectDataToRedirectTemplate()
    {
        redirectOjb.GetComponent<VideoUI>().TurnOnRedirectTemplate("hellow how are you cat", "www.google.com");

    }

    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR

        /*foreach (GameObject g in objectsToBeTurnedOff)
        {
            g.SetActive(false);
        }
        foreach (GameObject g in objectsToBeTurnedOn)
        {
            g.SetActive(true);
        }
        if(destinationTexture ==null)
        {
            destinationTexture = new Texture2D(sourceTexture.width, sourceTexture.height);
        }
        else
        {
            if(destinationTexture.width != sourceTexture.width || destinationTexture.height != sourceTexture.height)
            {
                // height or width is not same
                destinationTexture.width = sourceTexture.width;
                destinationTexture.height = sourceTexture.height;
            }
        }
        //Graphics.CopyTexture(sourceTexture, destinationTexture);
        destinationTexture.SetPixels(sourceTexture.GetPixels());
        */
        
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(redirectOjb);
            InjectDataToRedirectTemplate();
                }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            InjectDataToVideoPlayer(1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            InjectDataToVideoPlayer(2);
        }
    }
}
