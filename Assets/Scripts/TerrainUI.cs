using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainUI : MonoBehaviour
{
    public MouseDetect mouseDetect;

    public GameObject mouseGrabber;

    public TerrainClickUI terrainClickUI;

    public void SetTerrainClickUIActive(bool active)
    {
        terrainClickUI.gameObject.SetActive(active);
    }

    public void SetMouseGrabberActive(bool active)
    {
        mouseDetect.ActivateMouseOverDetect(active);
        mouseGrabber.GetComponent<MouseGrabber>().mouseDetect = mouseDetect;
        mouseGrabber.SetActive(true);
    }

}
