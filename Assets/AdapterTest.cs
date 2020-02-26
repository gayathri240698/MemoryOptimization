using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdapterTest : MonoBehaviour

{
    Button backButton;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("back button working its working");
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(OnBackButtonPressed);
    }
    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(OnBackButtonPressed);

    }
    void OnBackButtonPressed()
    {
        Debug.Log("back button pressed");
       GameObject.FindObjectOfType< LifeCycleManager>().UnLoadAllModules();
    }
}
