using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovementUi : MonoBehaviour
{
    [SerializeField]
    Image panelL;
    [SerializeField]
    Image panelR;

    private void Awake()
    {
        ObjectCameraFollower.OnOrbit += Fade;
        RectTransform rectL = panelL.GetComponent<RectTransform>();
        RectTransform rectR = panelR.GetComponent<RectTransform>();

        rectL.offsetMin = Vector2.zero;
        rectL.offsetMax = Vector2.zero;
        rectL.anchorMin = new Vector2(0, 0);
        rectL.anchorMax = new Vector2(.1f, 1);

        rectR.offsetMin = Vector2.zero;
        rectR.offsetMax = Vector2.zero;
        rectR.anchorMin = new Vector2(.9f, 0);
        rectR.anchorMax = new Vector2(1, 1);
    }

    void Fade(float input)
    {
        if(input > 0)
        {
            panelR.color = new Color(panelR.color.r, panelR.color.g, panelR.color.b, 0);
            panelL.color = new Color(panelL.color.r, panelL.color.g, panelL.color.b, input);
        }
        else if(input < 0)
        {
            panelL.color = new Color(panelL.color.r, panelL.color.g, panelL.color.b, 0);
            input = Mathf.Abs(input);
            panelR.color = new Color(panelR.color.r, panelR.color.g, panelR.color.b, input);

        }
        else
        {
            panelL.color = new Color(panelL.color.r, panelL.color.g, panelL.color.b, 0);
            panelR.color = new Color(panelR.color.r, panelR.color.g, panelR.color.b, 0);
        }
    }
}
