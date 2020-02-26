using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public GameObject scanPanel;
    public void TurnOnScanpanel()
    {
        scanPanel.SetActive(true);
    }
    public void TurnOffScanPanel()
    {
        scanPanel.SetActive(false);
    }
}
