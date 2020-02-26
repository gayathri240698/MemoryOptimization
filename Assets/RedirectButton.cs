using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BharathARFoundation.ImageTargets;

public class RedirectButton : MonoBehaviour
{
    public ARFoundationYoutubePlayer v;
    // Start is called before the first frame update
    void OnMouseDown()
    {
        v.RedirectUrl();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
